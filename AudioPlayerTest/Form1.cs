using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MathNet.Numerics;
using NAudio.Wave;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAnalyser
{
    public partial class Form1 : Form
    {
        private static int BUFFERSIZE = (int)Math.Pow(2, 13);
        private static int UI_DELAY_FACTOR = 100000;
        private static double PEAK_FFT_POWER = 20* Math.Log10(12020);
        private static int PEAK_BUFFER = 90;
        private static int MAX_FREQ = 4000;
        private static int MIN_FREQ = 30;
        private static int MAX_GAIN_CHANGE = 8;
        private static float MAX_FREQ_CHANGE = 2.8f;
        private static int FOLLOW_SECS = 5;
        private static int AVG_EXECUTIONS = 10;
        private static int MIN_UPDATE_TIME = 12;
        //private static int SLOPE_THRESHOLD = 4;
        private static int SMOOTH_FACTOR = 3;
        private static int SIMILAR_GAIN_THRESHOLD = 5;
        private static int NOTE_BUFFER_SIZE = 10000;
        private static int ERROR_DURATION = 5;

        private AudioSource source;
        private Music music;
        private DirectSoundOut output = null;
        private bool started = false;
        private int fftDraws = 0;
        private double[] dataFft;
        private List<double[]> dataFftPrev = new List<double[]>();
        //private double[] fftDerivative;
        private Dictionary<double, double> fftPeaks;
        private Dictionary<double, double> notes;
        private double fftScale;
        //private int totalNotes = 0;
        private double[] notePercent = new double[12];
        private List<int> executionTime = new List<int>();
        private double avgGain;
        private double maxGain;
        private List<int> avgError = new List<int>();

        public Form1()
        {
            InitializeComponent();
            source = new AudioSource();
            music = new Music();
            SetupFFTPlot();
            barVolume.Value = 10;
        }

        private void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Audio Files (*.wav; *.mp3)|*.wav; *.mp3;";

            if (open.ShowDialog() != DialogResult.OK)
                return;

            DisposeAudio();
            output = new DirectSoundOut();

            if (open.FileName.EndsWith(".wav"))
            {
                FileHandler.OpenWav(open.FileName, out source);
            }
            else if (open.FileName.EndsWith(".mp3"))
            {
                FileHandler.OpenMP3(open.FileName, out source);
            }
            else return;

            cwvViewer.WaveStream = source.AudioGraph;
            cwvViewer.FitToScreen();
            btnPlay.Enabled = true;
            btnOpen.Enabled = false;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (output != null)
            {
                if(chbTempo.Enabled)
                {
                    if (chbTempo.Checked)
                        output.Init(source.SpeedControl);
                    else
                        output.Init(source.AudioStream);
                }
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    output.Pause();
                    btnPlay.Text = "Play";
                    timerFFT.Enabled = false;
                    btnClose.Enabled = true;
                }
                else if (output.PlaybackState == PlaybackState.Paused || output.PlaybackState == PlaybackState.Stopped)
                {
                    output.Play();
                    btnClose.Enabled = false;
                    btnPlay.Text = "Pause";
                    timerFFT.Enabled = true;
                    if (!started)
                    {
                        chbTempo.Enabled = false;
                        if (!chbTempo.Checked)
                            barTempo.Enabled = false;

                        var ts = new ThreadStart(UpdatePlayPosition);
                        var backgroundThread = new Thread(ts);
                        backgroundThread.Start();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            timerFFT.Enabled = false;
            output.Stop();
            Thread.Sleep(100);
            DisposeAudio();
            ClearUI();
            GC.Collect();
        }

        private void ClearUI()
        {
            lstNotes.Items.Clear();
            spFFT.plt.Clear();
            spFFT.Render();
            cwvViewer.WaveStream = null;
            btnClose.Enabled = false;
            btnOpen.Enabled = true;
            btnPlay.Enabled = false;
            btnPlay.Text = "Play";
            fftDraws = 0;
            fftPeaks = null;
            notes = null;
            music = new Music();
            chbTempo.Enabled = true;
            barTempo.Enabled = true;
            barVolume.Value = 10;
            barTempo.Value = 10;
            barPitch.Value = 50;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeAudio();
        }

        private void DisposeAudio()
        {
            started = false;
            if (output != null)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                    output.Stop();
                output.Dispose();
                output = null;
            }
            if (source.Audio != null)
            {
                source.DisposeAudio();
            }
        }

        private void UpdatePlayPosition()
        {
            int currentPos = 0;
            int previousPos = 0;
            long currentSample;
            int index = 0;

            while (output.PlaybackState != PlaybackState.Stopped)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    if(!started && chbFollow.Checked)
                    {
                        cwvViewer.LeftSample = 0;
                        cwvViewer.RightSample = cwvViewer.LeftSample + (FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                        cwvViewer.Zoom();
                    }

                    started = true;
                    index++;
                    index = index % UI_DELAY_FACTOR;

                    if (index == UI_DELAY_FACTOR - 1)
                    {
                        InvokeUI(() => txtTime.Text =  source.AudioStream.CurrentTime.ToString());
                        currentSample = source.AudioStream.Position / 8;

                        if (currentSample > cwvViewer.LeftSample && currentSample < cwvViewer.RightSample)
                        {
                            currentPos = (int)(currentSample - cwvViewer.LeftSample) / cwvViewer.SamplesPerPixel;

                            if (currentPos != previousPos)
                            {
                                InvokeUI(() => cwvViewer.Overlay.MovePosIndicator(currentPos));
                                previousPos = currentPos;
                            }
                        }
                        else if(currentSample >= cwvViewer.RightSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = cwvViewer.RightSample;
                            cwvViewer.RightSample = cwvViewer.LeftSample + (FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                            cwvViewer.Zoom();
                        }
                    }
                }
            }
        }

        private void PerformFFT()
        {
            byte[] bytesBuffer = new byte[BUFFERSIZE];
            double posScaleFactor = (double)source.Audio.WaveFormat.SampleRate / (double)source.AudioFFT.WaveFormat.SampleRate;
            source.AudioFFT.Position = (long)(source.AudioStream.Position / posScaleFactor / 2);
            source.AudioFFT.Read(bytesBuffer, 0, BUFFERSIZE);
            short[] audioBuffer = new short[BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length);

            int fftPoints = 2;
            while (fftPoints * 2 <= BUFFERSIZE)
                fftPoints *= 2;

            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(audioBuffer[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));

            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            if (dataFft == null)
                dataFft = new double[fftPoints / 2];
            for (int i = 0; i < fftPoints / 2; i++)
            {
                double fftLeft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftRight = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                dataFft[i] = 20 * Math.Log10(fftLeft + fftRight) - PEAK_FFT_POWER;
            }
            fftScale = (double)fftPoints / source.AudioFFT.WaveFormat.SampleRate;

            SmoothSignal();
            //fftDerivative = GetDerivative(dataFft);
            avgGain = dataFft.Average();
            maxGain = dataFft.Max();
            DisplayFFT();

            fftDraws++;
            lblFFTDraws.Text = "FFT Updates: " + fftDraws;

            Application.DoEvents();
        }

        private void DisplayFFT()
        {
            spFFT.plt.Clear();
            spFFT.plt.PlotSignal(dataFft, fftScale, markerSize: 0);
            int zoom = 0;
            switch(barZoom.Value)
            {
                case 0:
                    zoom = 500;
                    break;
                case 1:
                    zoom = 1000;
                    break;
                case 2:
                    zoom = 2000;
                    break;
                case 3:
                    zoom = 4000;
                    break;
            }
            spFFT.plt.Axis(0, zoom, avgGain - 10, maxGain + 10);
        }

        private void SmoothSignal()
        {
            double[] signal = new double[dataFft.Length];
            Array.Copy(dataFft, signal, dataFft.Length);
            dataFftPrev.Add(signal);

            if (dataFftPrev.Count > SMOOTH_FACTOR)
                dataFftPrev.RemoveAt(0);

            for(int i = 0; i < signal.Length; i++)
            {
                double smoothedValue = 0;
                for(int j = 0; j < dataFftPrev.Count; j++)
                {
                    smoothedValue += dataFftPrev[j][i];
                }
                smoothedValue /= dataFftPrev.Count;
                dataFft[i] = smoothedValue;
            }
        }

        private double[] GetDerivative(double[] source)
        {
            double[] derivative = new double[source.Length];
            derivative[0] = 0;
            double prevDifference, nextDifference;

            for(int i = 1; i < source.Length - 1; i++)
            {
                prevDifference = source[i] - source[i - 1];
                nextDifference = source[i + 1] - source[i];
                derivative[i] = (prevDifference + nextDifference) / 2;
            }

            derivative[source.Length - 1] = 0;
            return derivative;
        }

        private void GetPeaks()
        {
            fftPeaks = new Dictionary<double, double>();
            double freq;
            double gainThreshold = avgGain + 25;

            for (int i = 0; i < dataFft.Length; i++)
            {
                if (dataFft[i] > gainThreshold)
                {
                    freq = (i + 1) / fftScale;
                    if (freq > MAX_FREQ)
                        break;
                    if (freq < MIN_FREQ)
                        continue;

                    fftPeaks.Add(freq, dataFft[i]);
                    fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    if (fftPeaks.Count > PEAK_BUFFER)
                    {
                        double keyToRemove = 0;
                        int keyIndex = 0;
                        foreach (double key in fftPeaks.Keys)
                        {
                            keyIndex++;
                            if (keyIndex == fftPeaks.Count)
                                keyToRemove = key;
                        }
                        fftPeaks.Remove(keyToRemove);
                    }
                }
            }

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            List<KeyValuePair<double, double>> cluster = null;
            KeyValuePair<double, double> largestGain = new KeyValuePair<double, double>();
            int peakIndex = 0;
            while (peakIndex < fftPeaks.Count)
            {
                double myFreq = GetKey(fftPeaks, peakIndex);

                if (cluster == null)
                {
                    cluster = new List<KeyValuePair<double, double>>();
                    largestGain = new KeyValuePair<double, double>(myFreq, fftPeaks[myFreq]);
                    cluster.Add(largestGain);
                    peakIndex++;
                    continue;
                }
                else if ((myFreq - largestGain.Key) <= largestGain.Key / 100 * MAX_FREQ_CHANGE)
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

                if (cluster.Count > 1)
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

            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            List<double> discardFreqs = new List<double>();

            for (int i = 0; i < fftPeaks.Count - 1; i++)
            {
                double freqA = GetKey(fftPeaks, i);
                double freqB = GetKey(fftPeaks, i + 1);
                if (Math.Abs(fftPeaks[freqA] - fftPeaks[freqB]) >= MAX_GAIN_CHANGE)
                {
                    if (fftPeaks[freqA] > fftPeaks[freqB])
                        discardFreqs.Add(freqB);
                    else
                        discardFreqs.Add(freqA);
                }
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);

            RemoveNoise();

            if (chbOrder.Checked)
                fftPeaks = fftPeaks.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            else
                fftPeaks = fftPeaks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private void RemoveNoise()
        {
            List<double> discardFreqs = new List<double>();
            double prevFreq = 0;
            fftPeaks = fftPeaks.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (double freq in fftPeaks.Keys)
            {
                if (freq > 200)
                    break;
                if(prevFreq == 0)
                {
                    prevFreq = freq;
                    continue;
                }
                if((freq - prevFreq) <= freq / 100 * (2.5 * MAX_FREQ_CHANGE))
                {
                    if(Math.Abs(fftPeaks[freq] - fftPeaks[prevFreq]) <= SIMILAR_GAIN_THRESHOLD)
                    {
                        if(!discardFreqs.Contains(prevFreq))
                            discardFreqs.Add(prevFreq);
                        discardFreqs.Add(freq);
                    }
                }
                prevFreq = freq;
            }

            foreach (double frequency in discardFreqs)
                fftPeaks.Remove(frequency);
        }

        private double GetKey(Dictionary<double, double> dict, int index)
        {
            int i = 0;
            foreach(var key in dict.Keys)
            {
                if (i == index)
                    return key;
                i++;
            }
            return 0;
        }

        private void GetNotes()
        {
            notes = new Dictionary<double, double>();
            lstNotes.Items.Clear();
            music.NoteError = new List<int>();

            foreach(double freq in fftPeaks.Keys)
            {
                string note = music.GetNote(freq);
                if (note != "N/A")
                {
                    notes.Add(freq, fftPeaks[freq]);
                    lstNotes.Items.Add(note + " (" + String.Format("{0:0.00}", freq) + " Hz) @ " + String.Format("{0:0.00}", fftPeaks[freq]) + " dB");
                    music.CountNote(note);
                }
            }
            foreach (double freq in notes.Keys)
            {
                string noteName = music.GetNote(freq);
                int noteIndex = Music.GetNoteIndex(noteName);
                BufferNote(noteIndex);
                int occurences = music.NoteOccurences[noteIndex];
                double percent = ((double)occurences / (double)music.NoteBuffer.Count) * 100;
                Color noteColor = GetNoteColor(0, (int)(10000 / 7), (int)(percent * 100));
                notePercent[noteIndex] = percent;
                UpdateNoteOccurencesUI(noteName, occurences, percent, noteColor);
                spFFT.plt.PlotText(noteName, freq, notes[freq], noteColor, fontSize: 11);
            }
            if (music.NoteError.Count > 0)
            {
                avgError.Add((int)music.NoteError.Average());
            }
        }

        private void BufferNote(int noteIndex)
        {
            music.NoteBuffer.Add(noteIndex);
            if(music.NoteBuffer.Count > NOTE_BUFFER_SIZE)
            {
                int noteToRemove = music.NoteBuffer[0];
                music.NoteOccurences[noteToRemove]--;
                music.NoteBuffer.RemoveAt(0);
            }
        }

        private void UpdateNoteOccurencesUI(string noteName, int occurences, double percent, Color noteColor)
        {
            string note = noteName.Substring(0, noteName.Length - 1);
            switch (note)
            {
                case "C":
                    lblC.Text = "C: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblC.ForeColor = noteColor;
                    return;
                case "Db":
                    lblDb.Text = "Db: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblDb.ForeColor = noteColor;
                    return;
                case "D":
                    lblD.Text = "D: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblD.ForeColor = noteColor;
                    return;
                case "Eb":
                    lblEb.Text = "Eb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblEb.ForeColor = noteColor;
                    return;
                case "E":
                    lblE.Text = "E: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblE.ForeColor = noteColor;
                    return;
                case "F":
                    lblF.Text = "F: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblF.ForeColor = noteColor;
                    return;
                case "Gb":
                    lblGb.Text = "Gb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblGb.ForeColor = noteColor;
                    return;
                case "G":
                    lblG.Text = "G: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblG.ForeColor = noteColor;
                    return;
                case "Ab":
                    lblAb.Text = "Ab: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblAb.ForeColor = noteColor;
                    return;
                case "A":
                    lblA.Text = "A: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblA.ForeColor = noteColor;
                    return;
                case "Bb":
                    lblBb.Text = "Bb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblBb.ForeColor = noteColor;
                    return;
                case "B":
                    lblB.Text = "B: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblB.ForeColor = noteColor;
                    return;
            }
        }

        Color GetNoteColor(int rangeStart, int rangeEnd, int actualValue)
        {
            if (rangeStart >= rangeEnd) return Color.Black;

            actualValue = Math.Min(actualValue, rangeEnd);
            int max = rangeEnd - rangeStart;
            int value = actualValue - rangeStart;

            int blue = 0;
            int green = Math.Min(255 * value / (max / 2), 255);
            int red = 0;
            if(value > max / 2)
            {
                blue = Math.Min(value - (max / 2), 255);
                green = 255 - blue;
                red = 0;
            }
            else
                red = 255 - green;

            return Color.FromArgb((byte)red, (byte)green, (byte)blue);
        }

        private void FindKey()
        {
            double[] percents = new double[notePercent.Length];
            Array.Copy(notePercent, percents, percents.Length);
            string[] dominantNotes = new string[7];
            double largestPercent; 
             int largestIndex;

            for(int i = 0; i < dominantNotes.Length; i++)
            {
                largestPercent = percents[0];
                largestIndex = 0;
                for (int j = 1; j < percents.Length; j++)
                {
                    if (percents[j] > largestPercent)
                    {
                        largestPercent = percents[j];
                        largestIndex = j;
                    }
                }
                dominantNotes[i] = Music.GetNoteName(largestIndex);
                percents[largestIndex] = 0;
            }

            int[] keyProbability = Music.FindScale(dominantNotes);

            largestIndex = 0;
            bool confident = false;
            List<int> possibleKeys = new List<int>();
            for (int i = 0; i < keyProbability.Length; i++)
            {
                if (keyProbability[i] == 7)
                {
                    largestIndex = i;
                    possibleKeys.Clear();
                    confident = true;
                    break;
                }
                else if(keyProbability[i] == 6)
                {
                    possibleKeys.Add(i);
                }
            }
            if(possibleKeys.Count > 0)
            {
                List<double> lowNotes = new List<double>();
                foreach(int keyIndex in possibleKeys)
                {
                    string[] scale = new string[7];
                    Array.Copy(Music.Scales, keyIndex * 7, scale, 0, scale.Length);
                    double lowest = notePercent[Music.GetNoteIndex(scale[0] + "0")];
                    for (int i = 1; i < scale.Length; i++)
                    {
                        double percentage = notePercent[Music.GetNoteIndex(scale[i] + "0")];
                        if (percentage < lowest)
                            lowest = percentage;
                    }
                    lowNotes.Add(lowest);
                }
                int highOfLowIndex = lowNotes.IndexOf(lowNotes.Max());
                largestIndex = possibleKeys[highOfLowIndex];
            }
            else if(!confident)
            {
                InvokeUI(() => lblKey.Text = "Predicted Key: N/A");
                for (int i = 0; i < keyProbability.Length; i++)
                {
                    Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
                }
                Console.WriteLine("");
                return;
            }
            string keyRoot = Music.GetNoteName(largestIndex);
            if (music.IsMinor(keyRoot, out string minorRoot))
                InvokeUI(() => lblKey.Text = "Predicted Key: " + minorRoot + " Minor");
            else
                InvokeUI(() => lblKey.Text = "Predicted Key: " + keyRoot + " Major");

            for (int i = 0; i < keyProbability.Length; i++)
            {
                Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
            }
            Console.WriteLine("");
        }

        private void timerFFT_Tick(object sender, EventArgs e)
        {
            if (output.PlaybackState == PlaybackState.Playing)
            {
                timerFFT.Enabled = false;
                var watch = System.Diagnostics.Stopwatch.StartNew();

                PerformFFT();
                GetPeaks();
                GetNotes();
                new Task(FindKey).Start();
                spFFT.Render();
                watch.Stop();

                if(avgError.Count == ERROR_DURATION)
                {
                    int error = (int)avgError.Average();
                    if (error >= 0)
                        lblError.Text = "+ " + Math.Abs(error) + " Cents";
                    else
                        lblError.Text = "- " + Math.Abs(error) + " Cents";
                    avgError = new List<int>();
                }

                executionTime.Add((int)watch.ElapsedMilliseconds);
                if (executionTime.Count == AVG_EXECUTIONS)
                {
                    int executionTime = AverageExecutionTime();
                    if (executionTime >= MIN_UPDATE_TIME)
                        timerFFT.Interval = executionTime;
                    lblExeTime.Text = "Execution Time: " + executionTime + " ms";
                    Console.WriteLine(timerFFT.Interval);
                }
                timerFFT.Enabled = true;
            }
        }

        private void ResetNoteCount()
        {
            music.ResetNoteCount();
        }

        private int AverageExecutionTime()
        {
            int avg = (int)executionTime.Average();
            executionTime.RemoveAt(0);
            return avg;
        }

        private void SetupFFTPlot()
        {
            spFFT.plt.Title("Frequency Spectrum");
            spFFT.plt.YLabel("Gain (dB)", fontSize: 12);
            spFFT.plt.XLabel("Frequency (Hz)", fontSize: 12);
            spFFT.Render();
        }

        private void barVolume_Scroll(object sender, EventArgs e)
        {
            if(source.AudioStream != null)
                source.AudioStream.Volume = barVolume.Value / 20f;
        }

        private void barTempo_Scroll(object sender, EventArgs e)
        {
            if (source.SpeedControl != null)
            {
                source.SpeedControl.PlaybackRate = 0.5f + barTempo.Value / 20f;
                ResetNoteCount();
            }
        }

        private void barPitch_Scroll(object sender, EventArgs e)
        {
            int centDifference = 50 - barPitch.Value;
            music.GetPercentChange(centDifference);
            ResetNoteCount();
        }
    }
}
