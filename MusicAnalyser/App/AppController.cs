using Accord;
using MusicAnalyser.App.Analysis;
using MusicAnalyser.App.DSP;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser.App
{
    public class AppController
    {
        public AudioSource AudioSource { get; set; }

        private Form1 ui;
        private DSPMain dsp;
        private LiveInputRecorder liveRecorder;

        private List<int> executionTime = new List<int>();
        private int analysisUpdates = 0;
        private bool started = false;
        private long startSample = 0;

        public bool LiveMode { get; set; }
        public bool IsRecording { get; set; }
        public bool Opened { get; set; }

        public AppController(Form1 form)
        {
            ui = form;
            dsp = new DSPMain(this);
            liveRecorder = new LiveInputRecorder(ui);
            LiveMode = false;
            LoadPrefs();
            ui.UpdateUI();
        }

        public bool IsStarted() { return started; }
        public void SetStarted(bool state) { started = state; }

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
            if (IsRecording || Opened)
                return;

            OpenFileDialog open;
            if (ui.SelectFile(out open))
            {
                AudioSource source;
                if (open.FileName.EndsWith(".wav"))
                {
                    FileHandler.OpenWav(open.FileName, out source);
                    AudioSource = source;
                }
                else if (open.FileName.EndsWith(".mp3"))
                {
                    FileHandler.OpenMP3(open.FileName, out source);
                    AudioSource = source;
                }
                else return;

                ui.SetupPlaybackUI(source.AudioGraph, open.FileName, false);
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

            ui.output.Init(AudioSource.SpeedControl); // Using SpeedControl SampleProvider to allow tempo changes

            if (ui.output.PlaybackState == PlaybackState.Playing) // Pause audio
            {
                ui.output.Pause();
                startSample = AudioSource.AudioStream.Position;
                ui.DrawPauseUI();
            }
            else if (ui.output.PlaybackState == PlaybackState.Paused || ui.output.PlaybackState == PlaybackState.Stopped) // Play audio
            {
                AudioSource.AudioStream.Seek(startSample, SeekOrigin.Begin);
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
            GC.Collect();
        }

        public void LoopPlayback()
        {
            AudioSource.AudioStream.Seek(ui.cwvViewer.SelectSample * ui.cwvViewer.BytesPerSample * ui.cwvViewer.WaveStream.WaveFormat.Channels, SeekOrigin.Begin);
        }

        public void DrawSpectrum(double[] freqData, double scale, double avgGain, double maxGain)
        {
            ui.DisplayFFT(freqData, scale, avgGain, maxGain);
            Application.DoEvents();
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

                if (dsp.FFTMain())
                {
                    if (Prefs.NOTE_ALGORITHM == 0) // By Magnitude
                        dsp.GetPeaksByMagnitude();
                    else if (Prefs.NOTE_ALGORITHM == 1) // By Slope
                        dsp.GetPeaksBySlope();

                    dsp.Analyser.GetNotes(dsp.fftPeaks, analysisUpdates);
                    Task asyncAnalysis = RunAnalysisAsync();
                    DisplayAnalysisUI();
                    ui.RenderSpectrum();

                    if (dsp.Analyser.GetAvgError().Count == Prefs.ERROR_DURATION) // Calculate average note error
                    {
                        int error = (int)dsp.Analyser.GetAvgError().Average();
                        if (error >= 0)
                            ui.SetErrorText("+ " + Math.Abs(error) + " Cents");
                        else
                            ui.SetErrorText("- " + Math.Abs(error) + " Cents");
                        dsp.Analyser.ResetError();
                    }

                    await asyncAnalysis;
                }
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
                else if (Prefs.UPDATE_MODE == 1) // Manual update mode
                {
                    ui.SetTimerInterval(Prefs.MIN_UPDATE_TIME);
                    ui.SetExecTimeText((int)watch.ElapsedMilliseconds);
                }
                ui.EnableTimer(true);
                analysisUpdates++;
            }
        }

        public Task RunAnalysisAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                dsp.Analyser.FindKey();
                if (analysisUpdates % Prefs.CHORD_DETECTION_INTERVAL == 0)
                {
                    ui.InvokeUI(() => ui.ClearNotesList());
                    int completed = dsp.Analyser.FindChordsNotes();
                    if (completed == 1)
                    {
                        dsp.Analyser.FindChords();
                        DisplayChords();
                    }
                }
            });
        }

        public void DisplayAnalysisUI()
        {
            List<Note> notes = dsp.Analyser.GetNotes();
            List<Note>[] chordNotes = dsp.Analyser.GetChordNotes();
            Color[] noteColors = dsp.Analyser.GetNoteColors();
            double[] notePercents = dsp.Analyser.GetNotePercents();
            dsp.Analyser.GetChords(out List<Chord> chords);
            string key = dsp.Analyser.GetCurrentKey();
            string mode = dsp.Analyser.GetCurrentMode();

            if (notes != null)
            {
                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i] != null)
                        ui.PlotNote(notes[i].Name + notes[i].Octave, notes[i].Frequency, notes[i].Magnitude, noteColors[notes[i].NoteIndex], false);
                    else
                        return;

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
                    if (chords[i] != null && chords[i].Name != "N/A")
                    {
                        if(!ui.IsShowAllChordsChecked())
                        {
                            if (chords[i].Name.Contains('('))
                                ui.PlotNote(chords[0].Name, X, dsp.maxGain + 7.5, Color.Black, false);
                            else
                                ui.PlotNote(chords[0].Name, X, dsp.maxGain + 7.5, Color.Blue, false);
                            break;
                        }
                        if (chords[i].Name.Contains('('))
                            ui.PlotNote(chords[i].Name, X, dsp.maxGain + 7.5, Color.Black, false);
                        else
                            ui.PlotNote(chords[i].Name, X, dsp.maxGain + 7.5, Color.Blue, false);

                        X += (chords[i].Name.Length * 7 + 20) * (ui.fftZoom / 1000f);
                    }
                }
            }

            if (notePercents != null && noteColors != null)
            {
                for (int i = 0; i < notePercents.Length; i++)
                {
                    if (noteColors[i] != null)
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
            List<Note>[] chordNotes = dsp.Analyser.GetChordNotes();
            if (chordNotes == null)
                return;
            dsp.Analyser.GetChords(out List<Chord> chords);

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
            if (AudioSource != null)
                AudioSource.AudioStream.Volume = value / 20f;
        }

        /*
         * Tempo change handler
         */
        public void TempoChange(int value)
        {
            if (AudioSource != null)
                AudioSource.SpeedControl.PlaybackRate = 0.5f + value / 20f;
        }

        /*
         * Pitch change handler
         */
        public void PitchChange(int value)
        {
            int centDifference = 50 - value;
            dsp.Analyser.GetMusic().GetPercentChange(centDifference);
            dsp.Analyser.GetMusic().ResetNoteCount();
        }
    
        public void EnableLiveMode()
        {
            if(ui.output != null)
               TriggerClose();
            liveRecorder = new LiveInputRecorder(ui);
            LiveMode = true;
            ui.SetupLiveModeUI();
        }

        public void ExitLiveMode()
        {
            if(liveRecorder.Recording)
                liveRecorder.StopRecording();
            LiveMode = false;
            IsRecording = false;
            ui.ClearUI();
        }

        public void TriggerLiveModeStartStop()
        {
            if (!liveRecorder.Recording)
            {
                if (liveRecorder.StartRecording())
                {
                    IsRecording = true;
                    ui.SetPlayBtnText("Stop Recording");
                }
            }
            else
            { 
                liveRecorder.StopRecording();
                IsRecording = false;
                LiveMode = false;
                AudioSource source;
                FileHandler.OpenWav(Path.Combine(Path.GetTempPath(), "recording.wav"), out source);
                AudioSource = source;
                ui.SetupPlaybackUI(AudioSource.AudioGraph, "", true);
                Opened = true;
            }
        }

        public void SaveRecording(string filename)
        {
            if (!FileHandler.WriteMp3(filename, AudioSource.Audio.WaveFormat))
                MessageBox.Show("Error: Recording could not be saved");
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
            if (AudioSource != null)
            {
                if (AudioSource.Audio != null)
                {
                    AudioSource.Dispose();
                }
            }
        }
    }
}
