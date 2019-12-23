using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser
{
    public class AppController
    {
        private Form1 ui;
        private AudioSource source;
        private Analyser analyser;

        private double[] dataFft;
        private List<double[]> dataFftPrev = new List<double[]>();
        private Dictionary<double, double> fftPeaks;
        private List<int> executionTime = new List<int>();
        private double fftScale;
        private int analysisUpdates = 0;
        private double avgGain;
        private double maxGain;
        private bool started = false;
        private long startSample = 0;

        public bool Opened { get; set; }

        public AppController(Form1 form)
        {
            ui = form;
            analyser = new Analyser(ui, this);
            LoadPrefs();
            ui.UpdateUI();
        }

        public bool IsStarted() { return started; }
        public void SetStarted(bool state) { started = state; }
        public AudioSource GetSource() { return source; }

        public static void LoadPrefs()
        {
            if (File.Exists("Prefs.ini"))
            {
                string[] loadedPrefs = FileHandler.ReadFile("prefs.ini");
                Dictionary<string, string> prefsDict = new Dictionary<string, string>();
                foreach(string line in loadedPrefs)
                {
                    string[] lineSplit = line.Split('=');
                    if(lineSplit.Length == 2)
                        prefsDict.Add(lineSplit[0], lineSplit[1]);
                }
                Prefs.LoadPrefs(prefsDict);
            }
        }

        /*
         * Master method for opening file
         */
        public void TriggerOpenFile()
        {
            OpenFileDialog open = null;

            if(ui.SelectFile(out open))
            {
                if (open.FileName.EndsWith(".wav"))
                {
                    FileHandler.OpenWav(open.FileName, out source);
                }
                else if (open.FileName.EndsWith(".mp3"))
                {
                    FileHandler.OpenMP3(open.FileName, out source);
                }
                else return;

                ui.SetupPlaybackUI(source.AudioGraph);
                Opened = true;
            }
        }

        /*
         * Master method for playing/pausing audio
         */
        public void TriggerPlayPause()
        {
            if (ui.output == null)
            {
                Console.WriteLine("Error: No output provider exists");
                return;
            }

            ui.output.Init(source.SpeedControl); // Using SpeedControl SampleProvider to allow tempo changes

            if (ui.output.PlaybackState == PlaybackState.Playing) // Pause audio
            {
                ui.output.Pause();
                startSample = source.AudioStream.Position;
                ui.DrawPauseUI();
            }
            else if (ui.output.PlaybackState == PlaybackState.Paused || ui.output.PlaybackState == PlaybackState.Stopped) // Play audio
            {
                source.AudioStream.Seek(startSample, SeekOrigin.Begin);
                ui.output.Play();
                ui.DrawPlayUI();

                if (!started)
                {
                    var ts = new ThreadStart(ui.UpdatePlayPosition); // New thread to handle playback position indicator
                    var backgroundThread = new Thread(ts);
                    backgroundThread.Start();
                }
            }
        }

        public void TriggerStop()
        {
            ui.output.Stop();
            ui.EnableTimer(false);
            startSample = ui.cwvViewer.SelectSample * ui.cwvViewer.BytesPerSample * ui.cwvViewer.WaveStream.WaveFormat.Channels;
            ui.SetPlayBtnText("Play from " + TimeSpan.FromSeconds((double)ui.cwvViewer.SelectSample / ui.cwvViewer.GetSampleRate()).ToString(@"m\:ss\:fff"));
        }

        /*
         * Master method for clearing session
         */
        public void TriggerClose()
        {
            Opened = false;
            ui.EnableTimer(false);
            ui.output.Stop();
            Thread.Sleep(100);
            DisposeAudio();
            analysisUpdates = 0;
            startSample = 0;
            executionTime.Clear();
            ui.ClearUI();
            analyser.DisposeAnalyser();
            GC.Collect();
        }

        /*
         * Master method for calculating realtime frequency domain data from audio playback
         */
        private void PerformFFT()
        {
            byte[] bytesBuffer = new byte[Prefs.BUFFERSIZE];
            double posScaleFactor = (double)source.Audio.WaveFormat.SampleRate / (double)source.AudioFFT.WaveFormat.SampleRate;
            source.AudioFFT.Position = (long)(source.AudioStream.Position / posScaleFactor / source.AudioStream.WaveFormat.Channels); // Syncs position of FFT WaveStream to current playback position
            source.AudioFFT.Read(bytesBuffer, 0, Prefs.BUFFERSIZE); // Reads PCM data at synced position to bytesBuffer
            short[] audioBuffer = new short[Prefs.BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length); // Bytes to shorts

            int fftPoints = 2;
            while (fftPoints * 2 <= Prefs.BUFFERSIZE) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
                fftPoints *= 2;

            // FFT Process
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(audioBuffer[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);


            if (dataFft == null)
                dataFft = new double[fftPoints / 2];
            for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
            {
                double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                dataFft[i] = 20 * Math.Log10(fft + fftMirror) - Prefs.PEAK_FFT_POWER; // Estimates gain of FFT bin
            }
            fftScale = (double)fftPoints / source.AudioFFT.WaveFormat.SampleRate;

            SmoothSignal();
            avgGain = dataFft.Average();
            maxGain = dataFft.Max();
            ui.DisplayFFT(dataFft, fftScale, avgGain, maxGain);

            analysisUpdates++;
            ui.UpdateFFTDrawsUI(analysisUpdates);

            Application.DoEvents();
        }

        /*
         * Performs smoothing on frequency domain data by averaging several frames
         */
        private void SmoothSignal()
        {
            double[] signal = new double[dataFft.Length];
            Array.Copy(dataFft, signal, dataFft.Length);
            dataFftPrev.Add(signal);

            if (dataFftPrev.Count > Prefs.SMOOTH_FACTOR)
                dataFftPrev.RemoveAt(0);

            for (int i = 0; i < signal.Length; i++)
            {
                double smoothedValue = 0;
                for (int j = 0; j < dataFftPrev.Count; j++)
                {
                    smoothedValue += dataFftPrev[j][i];
                }
                smoothedValue /= dataFftPrev.Count;
                dataFft[i] = smoothedValue;
            }
        }

        /*
         * Returns the slope at each point of the signal passed in (By Slope Algorithm)
         */
        private double[] GetSlope(double[] source)
        {
            double[] derivative = new double[source.Length];
            derivative[0] = 0;
            for(int i = 1; i < source.Length - 1; i++)
            {
                double deltaX = ((i + 2) / fftScale) - (i / fftScale);
                derivative[i] = (source[i + 1] - source[i - 1]) / deltaX; // P[i] = y[i + 1] - y[i - 1] / x[i + 1] - x[i - 1]
            }
            derivative[source.Length - 1] = 0;
            return derivative;
        }

        /*
         * First step of analysis, identifies the most prominent frequency bins in the spectrum by using the slope of the signal (By Slope Algorithm)
         */
        private void GetPeaksBySlope()
        {
            double[] derivative = GetSlope(dataFft);
            fftPeaks = new Dictionary<double, double>();
            double gainThreshold = avgGain + 25;

            for (int i = (int)(fftScale * Prefs.MIN_FREQ); i < Math.Min(dataFft.Length, (int)(fftScale * Prefs.MAX_FREQ)); i++)
            {
                if (dataFft[i] < gainThreshold)
                    continue;

                if (derivative[i] > 0 && derivative[i + 1] < 0)
                {
                    double freq = (i + 1) / fftScale;
                    double avgGainChange = (derivative[i] + derivative[i - 1] + derivative[i - 2]) / 3;
                    if(avgGainChange > 3)
                        fftPeaks.Add(freq, dataFft[i]);
                }
            }
            RemoveKickNoise();

            fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
    }

        /*
         * First step of analysis, identifies the most prominent frequency bins in the spectrum - these represent possible notes (By Magnitude Algorithm)
         */
        private void GetPeaksByMagnitude()
        {
            fftPeaks = new Dictionary<double, double>();
            double freq;
            double gainThreshold = avgGain + 25;

            // Iterates through frequency data, storing the frequency and gain of the largest frequency bins 
            for (int i = (int)(fftScale * Prefs.MIN_FREQ); i < Math.Min(dataFft.Length, (int)(fftScale * Prefs.MAX_FREQ)); i++) 
            {
                if (dataFft[i] > gainThreshold)
                {
                    freq = (i + 1) / fftScale; // Frequency value of bin

                    fftPeaks.Add(freq, dataFft[i]);
                    fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low

                    if (fftPeaks.Count > Prefs.PEAK_BUFFER) // When fftPeaks overflows, remove smallest frequency bin
                    {
                        double keyToRemove = GetDictKey(fftPeaks, fftPeaks.Count - 1);
                        fftPeaks.Remove(keyToRemove);
                    }
                }
            }

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<KeyValuePair<double, double>> cluster = null;
            KeyValuePair<double, double> largestGain = new KeyValuePair<double, double>();
            int peakIndex = 0;
            while (peakIndex < fftPeaks.Count) // Removes unwanted and redundant peaks
            {
                double myFreq = GetDictKey(fftPeaks, peakIndex);

                if (cluster == null)
                {
                    cluster = new List<KeyValuePair<double, double>>();
                    largestGain = new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]);
                    cluster.Add(largestGain);
                    peakIndex++;
                    continue;
                }
                else if ((myFreq - largestGain.Key) <= largestGain.Key / 100 * Prefs.MAX_FREQ_CHANGE) // Finds clusters of points that represent the same peak
                {
                    cluster.Add(new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]));

                    if (fftPeaks[myFreq] > largestGain.Value)
                        largestGain = new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]);

                    if (peakIndex < fftPeaks.Count - 1)
                    {
                        peakIndex++;
                        continue;
                    }
                }

                if (cluster.Count > 1) // Keeps only the largest value in the cluster
                {
                    cluster.Remove(largestGain);
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        fftPeaks.Remove(cluster[j].Key);
                    }
                    peakIndex -= cluster.Count;
                }
                cluster = null;
            }

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<double> discardFreqs = new List<double>();

            for (int i = 0; i < fftPeaks.Count - 1; i++) // Removes any unwanted residual peaks after a large peak
            {
                double freqA = GetDictKey(fftPeaks, i);
                double freqB = GetDictKey(fftPeaks, i + 1);
                if (Math.Abs(fftPeaks[freqA] - fftPeaks[freqB]) >= Prefs.MAX_GAIN_CHANGE)
                {
                    if (fftPeaks[freqA] > fftPeaks[freqB]) // Discard lowest value
                        discardFreqs.Add(freqB);
                    else
                        discardFreqs.Add(freqA);
                }
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);

            RemoveKickNoise();

            fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
        }

        /*
         * Performs further refinement of peaks, removing noise mostly attributed to the kick drum
         */
        private void RemoveKickNoise()
        {
            List<double> discardFreqs = new List<double>();
            double prevFreq = 0;
            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high

            foreach (double freq in fftPeaks.Keys)
            {
                if (freq > 200)
                    break;
                if (prevFreq == 0)
                {
                    prevFreq = freq;
                    continue;
                }
                if ((freq - prevFreq) <= freq / 100 * (2.5 * Prefs.MAX_FREQ_CHANGE)) // Checking for consecutive, closely packed peaks - noise
                {
                    if (Math.Abs(fftPeaks[freq] - fftPeaks[prevFreq]) <= Prefs.SIMILAR_GAIN_THRESHOLD)
                    {
                        if (!discardFreqs.Contains(prevFreq))
                            discardFreqs.Add(prevFreq);
                        discardFreqs.Add(freq);
                    }
                }
                prevFreq = freq;
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);
        }

        /*
         * Returns the key value of 'dict' at 'index'
         */
        private double GetDictKey(Dictionary<double, double> dict, int index)
        {
            int i = 0;
            foreach (var key in dict.Keys)
            {
                if (i == index)
                    return key;
                i++;
            }
            return 0;
        }

        /*
         * Calculates a color dynamically based on the actualValue in relation to the specified range of values
         */
        public Color GetNoteColor(int rangeStart, int rangeEnd, int actualValue)
        {
            if (rangeStart >= rangeEnd) return Color.Black;

            actualValue = Math.Min(actualValue, rangeEnd);
            int max = rangeEnd - rangeStart;
            int value = actualValue - rangeStart;

            int blue = 0;
            int green = Math.Min(255 * value / (max / 2), 255);
            int red = 0;
            if (value > max / 2)
            {
                blue = Math.Min(value - (max / 2), 255);
                green = 255 - blue;
                red = 0;
            }
            else
                red = 255 - green;

            return Color.FromArgb((byte)red, (byte)green, (byte)blue);
        }

        /*
         * Master method for performing analysis
         */
        public async void RunAnalysis()
        {
            if (ui.output.PlaybackState == PlaybackState.Playing)
            {
                ui.EnableTimer(false);
                var watch = System.Diagnostics.Stopwatch.StartNew();

                PerformFFT();

                if(Prefs.NOTE_ALGORITHM == 0) // By Magnitude
                    GetPeaksByMagnitude();
                else if(Prefs.NOTE_ALGORITHM == 1) // By Slope
                    GetPeaksBySlope();

                analyser.GetNotes(fftPeaks, analysisUpdates);
                Task asyncAnalysis = RunAnalysisAsync();
                DisplayAnalysisUI();
                ui.RenderSpectrum();

                if (analyser.GetAvgError().Count == Prefs.ERROR_DURATION) // Calculate average note error
                {
                    int error = (int)analyser.GetAvgError().Average();
                    if (error >= 0)
                        ui.SetErrorText("+ " + Math.Abs(error) + " Cents");
                    else
                        ui.SetErrorText("- " + Math.Abs(error) + " Cents");
                    analyser.ResetError();
                }

                await asyncAnalysis;
                watch.Stop();

                if (Prefs.UPDATE_MODE == 0) // Dynamic update mode
                {
                    executionTime.Add((int)watch.ElapsedMilliseconds);
                    if (executionTime.Count == Prefs.AVG_EXECUTIONS) // Calculate average execution time
                    {
                        int executionTime = AverageExecutionTime();
                        if (executionTime >= Prefs.MIN_UPDATE_TIME)
                            ui.SetTimerInterval(executionTime);
                        ui.SetExecTimeText(executionTime);
                    }
                }
                else if(Prefs.UPDATE_MODE == 1) // Manual update mode
                {
                    ui.SetTimerInterval(Prefs.MIN_UPDATE_TIME);
                    ui.SetExecTimeText((int)watch.ElapsedMilliseconds);
                }
                ui.EnableTimer(true);
            }
        }

        public Task RunAnalysisAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                analyser.FindKey();
                if (analysisUpdates % Prefs.CHORD_DETECTION_INTERVAL == 0)
                {
                    int completed = analyser.FindChordsNotes();
                    if (completed == 1)
                    {
                        analyser.FindChords();
                        DisplayChords();
                    }
                }
            });
        }

        public void DisplayAnalysisUI()
        {
            List<Note> notes = analyser.GetNotes();
            List<Note>[] chordNotes = analyser.GetChordNotes();
            Color[] noteColors = analyser.GetNoteColors();
            double[] notePercents = analyser.GetNotePercents();
            analyser.GetChords(out List<Chord> chords);
            string key = analyser.GetCurrentKey();
            string mode = analyser.GetCurrentMode();

            if (notes != null)
            {
                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i] != null)
                        ui.PlotNote(notes[i].Name + notes[i].Octave, notes[i].Frequency, notes[i].Magnitude, noteColors[notes[i].NoteIndex], false);

                    if (chordNotes != null)
                    {
                        for (int j = 0; j < chordNotes.Length; j++)
                        {
                            for (int k = 0; k < chordNotes[j].Count; k++)
                            {
                                if (notes[i].Name == chordNotes[j][k].Name && notes[i].Octave == chordNotes[j][k].Octave)
                                    ui.PlotNote("*", notes[i].Frequency, notes[i].Magnitude + 5, noteColors[notes[i].NoteIndex], true);
                            }
                        }
                    }
                }
            }

            if (chords != null)
            {
                float X = 0;
                for (int i = 0; i < chords.Count; i++)
                {
                    if (chords[i].Name != "N/A" && chords[i] != null)
                    {
                        if(!ui.IsShowAllChordsChecked())
                        {
                            if (chords[i].Name.Contains('('))
                                ui.PlotNote(chords[0].Name, X, maxGain + 10, Color.Black, false);
                            else
                                ui.PlotNote(chords[0].Name, X, maxGain + 10, Color.Blue, false);
                            break;
                        }
                        if (chords[i].Name.Contains('('))
                            ui.PlotNote(chords[i].Name, X, maxGain + 10, Color.Black, false);
                        else
                            ui.PlotNote(chords[i].Name, X, maxGain + 10, Color.Blue, false);

                        X += (chords[i].Name.Length * 7 + 20) * (ui.fftZoom / 1000f);
                    }
                }
            }

            if (notePercents != null && noteColors != null)
            {
                for (int i = 0; i < notePercents.Length; i++)
                {
                    if (noteColors[i] != null && notePercents != null)
                        ui.UpdateNoteOccurencesUI(Music.Scales[i * 7], (int)(notePercents[i] / 100 * Prefs.NOTE_BUFFER_SIZE), notePercents[i], noteColors[i]);
                }
            }

            if(key != null)
            {
                ui.InvokeUI(() => ui.SetKeyText("Predicted Key: " + key));
                if (mode != null)
                {
                    if(mode != "" && key != "N/A")
                        ui.InvokeUI(() => ui.SetModeText("(" + mode + ")"));
                    else
                        ui.InvokeUI(() => ui.SetModeText(""));
                }
            }
        }

        public void DisplayChords()
        {
            ui.InvokeUI(() => ui.ClearNotesList());
            List<Note>[] chordNotes = analyser.GetChordNotes();
            if (chordNotes == null)
                return;
            analyser.GetChords(out List<Chord> chords);

            for (int i = 0; i < chords.Count; i++)
            {
                Chord chord = chords[i];
                ui.InvokeUI(() => ui.PrintChord(chord.Name + " (" + chord.Probability.ToString("0.00") + "%)"));
            }

            for (int i = 0; i < chordNotes.Length; i++)
            {
                for (int j = 0; j < chordNotes[i].Count; j++)
                {
                    Console.Write(chordNotes[i][j].Name + chordNotes[i][j].Octave + " ");
                }
                for(int j = 0; j < chords.Count; j++)
                {
                    if(chords[j].Name.Contains(chordNotes[i][0].Name))
                        Console.Write(" - " + chords[j].Name + " (" + chords[j].Probability.ToString("0.00") + "%)");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        /*
         * Returns the mean execution time of analysis taken over several executions
         */
        private int AverageExecutionTime()
        {
            int avg = (int)executionTime.Average();
            executionTime.RemoveAt(0);
            return avg;
        }

        /*
         * Volume change handler
         */
        public void VolumeChange(int value)
        {
            if (source != null)
                source.AudioStream.Volume = value / 20f;
        }

        /*
         * Tempo change handler
         */
        public void TempoChange(int value)
        {
            if (source != null)
                source.SpeedControl.PlaybackRate = 0.5f + value / 20f;
        }

        /*
         * Pitch change handler
         */
        public void PitchChange(int value)
        {
            int centDifference = 50 - value;
            analyser.GetMusic().GetPercentChange(centDifference);
            analyser.GetMusic().ResetNoteCount();
        }

        public void DisposeAudio()
        {
            started = false;
            if (ui.output != null)
            {
                ui.output.Stop();
                ui.output.Dispose();
                ui.output = null;
            }
            if (source != null)
            {
                if (source.Audio != null)
                {
                    source.Dispose();
                }
            }
        }
    }
}
