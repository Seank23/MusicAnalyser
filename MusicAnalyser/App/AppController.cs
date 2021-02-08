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

        public int Mode { get; set; }
        public bool IsRecording { get; set; }
        public bool Opened { get; set; }
        public bool ScriptSelectionApplied { get; set; }
        public bool ScriptSelectionValid { get; set; }
        public int StepMilliseconds { get; set; }
        public bool StepBack { get; set; }

        public AppController(Form1 form)
        {
            ui = form;
            dsp = new DSPMain(this);
            liveRecorder = new LiveInputRecorder(ui);
            Mode = 0;
            StepMilliseconds = 50;
            LoadPrefs();
            ui.UpdateUI();
        }

        public bool IsStarted() { return started; }
        public void SetStarted(bool state) { started = state; }

        public static void LoadPrefs()
        {
            string[] loadedPrefs = FileHandler.ReadFile("prefs.ini");
            if (loadedPrefs != null)
            {
                Dictionary<string, string> prefsDict = new Dictionary<string, string>();
                foreach (string line in loadedPrefs)
                {
                    string[] lineSplit = line.Split('=');
                    if (lineSplit.Length == 2)
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

                Opened = true;
                ui.SetupPlaybackUI(source.AudioGraph, open.FileName, false);
            }
        }

        /*
         * Master method for playing/pausing audio
         */
        public void TriggerPlayPause()
        {
            if (ui.Output == null || !Opened)
            {
                Console.WriteLine("Error: No output provider exists");
                return;
            }

            if (!ScriptSelectionApplied)
                return;

            ui.Output.Init(AudioSource.FilteredSource); // Using SpeedControl SampleProvider to allow tempo changes

            if (ui.Output.PlaybackState == PlaybackState.Playing) // Pause audio
            {
                ui.Output.Pause();
                startSample = AudioSource.AudioStream.Position;
                ui.DrawPauseUI();
            }
            else if (ui.Output.PlaybackState == PlaybackState.Paused || ui.Output.PlaybackState == PlaybackState.Stopped) // Play audio
            {
                AudioSource.AudioStream.Seek(startSample, SeekOrigin.Begin);
                ui.Output.Play();
                ui.DrawPlayUI();

                if (!started)
                {
                    var ts = new ThreadStart(ui.UpdatePlayPosition); // New thread to handle playback position indicator
                    var backgroundThread = new Thread(ts);
                    backgroundThread.Start();
                }
            }
            ui.CheckAppState();
        }

        public void TriggerStop()
        {
            ui.Output.Stop();
            ui.EnableTimer(false);
            startSample = ui.cwvViewer.SelectSample * ui.cwvViewer.BytesPerSample * ui.cwvViewer.WaveStream.WaveFormat.Channels;
            ui.SetPlayBtnText("Play from " + TimeSpan.FromSeconds((double)ui.cwvViewer.SelectSample / ui.cwvViewer.GetSampleRate()).ToString(@"m\:ss\:fff"));
            ui.CheckAppState();
        }

        /*
         * Master method for clearing session
         */
        public void TriggerClose()
        {
            Opened = false;
            ui.EnableTimer(false);
            ui.Output.Stop();
            Thread.Sleep(100);
            DisposeAudio();
            analysisUpdates = 0;
            startSample = 0;
            executionTime.Clear();
            ui.ClearUI();
            GC.Collect();
            ui.CheckAppState();
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

        public void SetScriptSelectorUI(Dictionary<int, string> scripts, bool add)
        {
            ui.SetScriptSelection(scripts, add);
        }

        public void SetPresetSelectorUI(string[] presets)
        {
            ui.SetPresetSelection(presets);
        }

        public void AddScript()
        {
            SetScriptSelectorUI(dsp.ScriptManager.GetAllScriptNames(), true);
        }

        public void ApplyScripts(Dictionary<int, int> selectionDict)
        {
            dsp.ApplyScripts(selectionDict);
        }

        public void ApplyScriptSettings(int scriptIndex, string[] settings)
        {
            dsp.ScriptManager.SetScriptSettings(scriptIndex, settings);
        }

        public void ApplyPreset(string presetName)
        {
            Dictionary<int, int> selectionDict = dsp.ScriptManager.GetPresetSelectionDict(presetName);
            if(CheckSelectionValidity(selectionDict, out string message))
            {
                ApplyScripts(selectionDict);
                Dictionary<string, string[]> preset = dsp.ScriptManager.Presets[presetName];
                for (int i = 0; i < selectionDict.Count; i++)
                    ApplyScriptSettings(selectionDict.Values.ElementAt(i), preset.Values.ElementAt(i));
                ui.SetAppliedScripts(selectionDict);
            }
            else
                MessageBox.Show("Error: Preset could not be applied.");
        }

        public bool CheckSelectionValidity(Dictionary<int, int> selectionDict, out string message)
        {
            ScriptSelectionValid = false;
            message = "";
            int primaryProcessor = -1;
            int primaryDetector = -1;
            if(selectionDict.Values.Contains(-1) || selectionDict.Count == 0)
            {
                message = "One or more scripts are not selected";
                return false;
            }
            for(int i = 0; i < selectionDict.Count; i++)
            {
                if(dsp.ScriptManager.ProcessorScripts.ContainsKey(selectionDict[i]))
                {
                    bool isPrimary = dsp.ScriptManager.ProcessorScripts[selectionDict[i]].IsPrimary;
                    if (isPrimary && primaryProcessor == -1)
                    {
                        primaryProcessor = i;
                        continue;
                    }
                    if(isPrimary || primaryProcessor == -1 || isPrimary && primaryDetector != -1)
                    {
                        message = "Script selection is invalid";
                        return false;
                    }
                }
                else if(dsp.ScriptManager.DetectorScripts.ContainsKey(selectionDict[i]))
                {
                    bool isPrimary = dsp.ScriptManager.DetectorScripts[selectionDict[i]].IsPrimary;
                    if (isPrimary && primaryDetector == -1 && primaryProcessor != -1)
                    {
                        primaryDetector = i;
                        continue;
                    }
                    if(isPrimary || primaryDetector == -1 || isPrimary && primaryProcessor != -1)
                    {
                        message = "Script selection is invalid";
                        return false;
                    }
                }
            }
            if(primaryProcessor == -1 || primaryDetector == -1)
            {
                message = "Selection must contain one Primary Processor and one Primary Detector";
                return false;
            }
            ScriptSelectionValid = true;
            return true;
        }

        public Dictionary<string, string[]> GetScriptSettings(int scriptIndex)
        {
            return dsp.ScriptManager.GetScriptSettings(scriptIndex);
        }

        public void SaveScriptSettings(int scriptIndex)
        {
            ApplyScriptSettings(scriptIndex, ui.GetSettingValues());
            dsp.ScriptManager.SaveScriptSettings(scriptIndex);
        }

        public void SetDefaultSettingValues(int scriptIndex)
        {
            dsp.ScriptManager.SetScriptSettings(scriptIndex, dsp.ScriptManager.SettingDefaults[scriptIndex]);
            ui.DisplayScriptSettings(scriptIndex);
        }

        public void SavePreset(string name)
        {
            if (CheckSelectionValidity(ui.GetSelectionDict(), out string message))
            {
                int[] scripts = ui.GetSelectionDict().Values.ToArray();
                Dictionary<string, Dictionary<string, string[]>> presetData = new Dictionary<string, Dictionary<string, string[]>>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    string scriptName = dsp.ScriptManager.GetScriptName(scripts[i]);
                    Dictionary<string, string[]> settings = GetScriptSettings(scripts[i]);
                    presetData[scriptName] = settings;
                }
                dsp.ScriptManager.SavePreset(name, presetData);
                dsp.LoadPresets();
                ApplyPreset(name);
            }
            else
                MessageBox.Show("Error: Preset could not be saved, script selection is invalid.");
        }

        /*
         * Master method for performing analysis
         */
        public async void RunAnalysis()
        {
            if (ui.Output.PlaybackState == PlaybackState.Playing || Mode == 1)
            {
                ui.EnableTimer(false);
                var watch = System.Diagnostics.Stopwatch.StartNew();

                dsp.RunFrequencyAnalysis();
                dsp.FrequencyAnalysisToSpectrum();
                dsp.RunPitchDetection();
                dsp.Analyser.GetNotes(dsp.FreqPeaks, (double[])dsp.GetScriptVal("POSITIONS", "Double[]"), analysisUpdates);
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
                if(Mode != 1)
                    ui.EnableTimer(true);
                analysisUpdates++;
            }
        }

        public Task RunAnalysisAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                if (analysisUpdates % Prefs.CHORD_DETECTION_INTERVAL == 0)
                {
                    dsp.Analyser.FindKey();
                    ui.InvokeUI(() => ui.ClearNotesList());
                    if (dsp.Analyser.FindChordsNotes())
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
                        ui.PlotNote(notes[i].Name + notes[i].Octave, notes[i].Position, notes[i].Magnitude, noteColors[notes[i].NoteIndex], false);
                    else
                        return;

                    if (chordNotes != null)
                    {
                        for (int j = 0; j < chordNotes.Length; j++)
                        {
                            for (int k = 0; k < chordNotes[j].Count; k++)
                            {
                                if (notes[i].Name == chordNotes[j][k].Name && notes[i].Octave == chordNotes[j][k].Octave)
                                    ui.PlotNote("*", notes[i].Position, notes[i].Magnitude + Math.Abs(notes[i].Magnitude * 0.1), noteColors[notes[i].NoteIndex], true);
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
                                ui.PlotNote(chords[0].Name, X, dsp.MaxGain + Math.Abs(dsp.MaxGain * 0.07), Color.Black, false);
                            else
                                ui.PlotNote(chords[0].Name, X, dsp.MaxGain + Math.Abs(dsp.MaxGain * 0.07), Color.Blue, false);
                            break;
                        }
                        if (chords[i].Name.Contains('('))
                            ui.PlotNote(chords[i].Name, X, dsp.MaxGain + Math.Abs(dsp.MaxGain * 0.07), Color.Black, false);
                        else
                            ui.PlotNote(chords[i].Name, X, dsp.MaxGain + Math.Abs(dsp.MaxGain * 0.07), Color.Blue, false);

                        X += (chords[i].Name.Length * 7 + 20) * (ui.fftZoom / 1000f);
                    }
                }
            }

            if (notePercents != null && noteColors != null)
            {
                for (int i = 0; i < notePercents.Length; i++)
                {
                    if (noteColors[i] != null)
                        ui.UpdateNoteOccurencesUI(Music.Scales[i * 7], notePercents[i], noteColors[i]);
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
            dsp.Analyser.GetMusic().SetTuningPercent(centDifference);
            dsp.Analyser.GetMusic().ResetNoteCount();
        }

        public void SetFilter(float lowPassFreq, float highPassFreq, float centreFreq, float centreQ, float gain)
        {
            if (AudioSource != null)
            {
                if (AudioSource.FilteredSource != null)
                {
                    AudioSource.FilteredSource.SetBandFilter(lowPassFreq, 1, highPassFreq, 1);
                    AudioSource.FilteredSource.SetPeakFilter(centreFreq, centreQ, gain);
                }
            }
        }

        public void GetFilterRange(double x, double y)
        {
            double lowFreq = 20000;
            double highFreq = 20;
            double centreFreq = x;
            if(dsp.GetScriptVal("SCALE", "Func`2") != null)
            {
                Func<int, double> scale = (Func<int, double>)dsp.GetScriptVal("SCALE", "Func`2");
                centreFreq = scale((int)x);
            }
            if (y > 0.7)
            {
                highFreq = 20 + (centreFreq - 2.8 * (centreFreq / 100) - 20) * Math.Pow(y, 6);
                lowFreq = centreFreq + centreFreq - highFreq;
            }
            SetFilter((float)lowFreq, (float)highFreq, (float)centreFreq, (float)(16 * y), (float)(40 * y));
            string note = dsp.Analyser.GetMusic().GetNote(centreFreq);
            ui.SetFilterText(note, centreFreq);
        }

        public void Step(bool backwards)
        {
            if (Opened && ui.Output.PlaybackState != PlaybackState.Playing)
            {
                Mode = 1;
                StepBack = backwards;
                ui.EnableTimer(false);
                RunAnalysis();
                ui.SetTimeStamp(AudioSource.AudioStream.CurrentTime);
            }
        }
    
        public void EnableLiveMode()
        {
            if(ui.Output != null)
               TriggerClose();
            liveRecorder = new LiveInputRecorder(ui);
            Mode = 2;
            ui.SetUIState();
        }

        public void ExitLiveMode()
        {
            if(liveRecorder.Recording)
                liveRecorder.StopRecording();
            IsRecording = false;
            Mode = 0;
            ui.SetUIState();
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
                Opened = true;
                Mode = 0;
                AudioSource source;
                FileHandler.OpenWav(Path.Combine(Path.GetTempPath(), "recording.wav"), out source);
                AudioSource = source;
                ui.SetupPlaybackUI(AudioSource.AudioGraph, "", true);
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
            dsp.SpectrogramData.Clear();
            if (ui.Output != null)
            {
                ui.Output.Stop();
                ui.Output.Dispose();
                ui.Output = null;
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
