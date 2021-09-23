using MusicAnalyser.App.Analysis;
using MusicAnalyser.App.Spectrogram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP
{
    public class DSPMain
    {
        public Analyser Analyser { get; set; }
        public ScriptManager ScriptManager { get; set; }
        public SpectrogramHandler SpectrogramHandler { get; set; }
        public double MaxGain { get; set; }
        public Dictionary<double, double> FreqPeaks { get; set; }

        private AppController app;
        private Dictionary<int, ISignalProcessor> processors = new Dictionary<int, ISignalProcessor>();
        private Dictionary<int, ISignalDetector> detectors = new Dictionary<int, ISignalDetector>();
        private List<double[]> prevSpectrumData = new List<double[]>();
        private Dictionary<string, object> scriptVals = new Dictionary<string, object>();
        private string scriptSet, startingScriptSet;
        private double[] spectrumData;
        private int detectorIndex = 0;
        private double largestTimestamp = -1;
        private double lastGC = 0;

        public DSPMain(AppController appController)
        {
            Analyser = new Analyser();
            app = appController;
            ScriptManager = new ScriptManager();
            SpectrogramHandler = new SpectrogramHandler();
            LoadScripts();
            LoadPresets();
        }

        public async void LoadScripts()
        {
            await Task.Factory.StartNew(() => ScriptManager.LoadScripts());

            LoadScriptSettings();
            app.SetScriptSelectorUI(ScriptManager.GetAllScriptNames(), false);
        }

        public void LoadPresets()
        {
            ScriptManager.LoadPresets();
            app.SetPresetSelectorUI(ScriptManager.GetPresetNames());
        }

        public void LoadScriptSettings()
        {
            for(int i = 0; i < ScriptManager.GetScriptCount(); i++)
            {
                ScriptManager.LoadScriptSettings(i);
            }
        }

        public void ApplyScripts(Dictionary<int, int> selectionDict)
        {
            processors.Clear();
            detectors.Clear();
            detectorIndex = 0;
            scriptSet = "";

            for(int i = 0; i < selectionDict.Count; i++)
            {
                for(int j = 0; j < ScriptManager.GetScriptCount(); j++)
                {
                    if(selectionDict[i] == j)
                    {
                        if (ScriptManager.ProcessorScripts.ContainsKey(j))
                        {
                            processors.Add(i, ScriptManager.ProcessorScripts[j]);
                            scriptSet += j;
                            break;
                        }
                        else if (ScriptManager.DetectorScripts.ContainsKey(j))
                        {
                            detectors.Add(i, ScriptManager.DetectorScripts[j]);
                            scriptSet += j;
                            if (ScriptManager.DetectorScripts[j].IsPrimary && detectorIndex == 0)
                                detectorIndex = i;
                            break;
                        }
                    }
                }
            }
            app.ScriptSelectionApplied = true;
            prevSpectrumData.Clear();
            scriptVals.Clear();
        }

        public void ApplyPreset(string presetName)
        {
            var preset = ScriptManager.Presets[presetName];
            if (preset == null)
                return;
        }

        public void RunFrequencyAnalysis()
        {
            scriptVals["SAMPLE_RATE"] = app.AudioSource.AudioAnalysis.WaveFormat.SampleRate;
            scriptVals["TUNING"] = Analyser.GetMusic().GetTuningPercent();
            object audio = ReadAudioStream();

            foreach (int key in processors.Keys)
            {
                if (key < detectorIndex)
                {
                    processors[key].InputBuffer = audio;
                    processors[key].InputArgs = scriptVals;
                    processors[key].OutputArgs = new Dictionary<string, object>();
                    processors[key].Process();
                    audio = processors[key].OutputBuffer;
                    foreach (var arg in processors[key].OutputArgs)
                        scriptVals[arg.Key] = arg.Value;
                }
                else break;
            }

            spectrumData = (double[])audio;
            if (!Double.IsInfinity(spectrumData[0]) && !Double.IsNaN(spectrumData[0]) && app.Mode != 1)
                spectrumData = SmoothSignal(spectrumData, Prefs.SMOOTH_FACTOR);
        }

        // Creates a spectrogram frame at specified update rate containing the current spectrum data, timestamp and analysis
        public void WriteToSpectrogram()
        {
            double curAudioPos = app.AudioSource.AudioAnalysis.CurrentTime.TotalMilliseconds;
            if (curAudioPos >= largestTimestamp)
            {
                if (SpectrogramHandler.Spectrogram.Frames.Count / (curAudioPos / 1000) <= Prefs.SPEC_UPDATE_RATE / app.AudioSource.SpeedControl.PlaybackRate)
                {
                    byte[] specData = SpectrogramQuantiser(spectrumData, out double quantScale);
                    specData = FilterSpectrogramData(specData);

                    if (SpectrogramHandler.Spectrogram.FrequencyScale == null)
                        SpectrogramHandler.Spectrogram.FrequencyScale = GetScriptVal("SCALE");

                    if (scriptSet != startingScriptSet || specData.Length != SpectrogramHandler.Spectrogram.FrequencyBins || curAudioPos - largestTimestamp > 1000)
                    {
                        SpectrogramHandler.Clear(); // Clears previous spectrogram frames if scripts are changed
                        SpectrogramHandler.Spectrogram.AudioFilename = app.AudioSource.Filename;
                        SpectrogramHandler.Spectrogram.ScriptProperties = app.GetScriptSettingValues(app.GetScriptChainData());
                        startingScriptSet = scriptSet;
                    }

                    SpectrogramHandler.CreateFrame(curAudioPos, specData, Analyser.Notes.ToArray(), Analyser.Chords.ToArray(), Analyser.CurrentKey, quantScale);
                    largestTimestamp = curAudioPos;

                    if (curAudioPos - lastGC >= 1000) // Garbage collection every second
                    {
                        GC.Collect();
                        lastGC = curAudioPos;
                    }
                }
            }
        }

        public void FrequencyAnalysisToSpectrum(object scale)
        {
            MaxGain = spectrumData.Max();
            double avgGain = spectrumData.Average();
            if (scale.GetType().Name == "Double")
                app.DrawSpectrum(spectrumData, (double)scale, avgGain, MaxGain);
            else
                app.DrawSpectrum(spectrumData, 1, avgGain, MaxGain);
        }

        public void RunPitchDetection()
        {
            object data = spectrumData;
            for(int i = detectorIndex; i < ScriptManager.GetScriptCount(); i++)
            {
                if(processors.ContainsKey(i))
                {
                    processors[i].InputBuffer = data;
                    processors[i].InputArgs = scriptVals;
                    processors[i].OutputArgs = new Dictionary<string, object>();
                    processors[i].Process();
                    data = processors[i].OutputBuffer;
                    foreach (var arg in processors[i].OutputArgs)
                        scriptVals[arg.Key] = arg.Value;
                }
                else if(detectors.ContainsKey(i))
                {
                    detectors[i].InputData = data;
                    detectors[i].InputArgs = scriptVals;
                    detectors[i].OutputArgs = new Dictionary<string, object>();
                    detectors[i].Detect();
                    data = detectors[i].Output;
                    foreach (var arg in detectors[i].OutputArgs)
                        scriptVals[arg.Key] = arg.Value;
                }
            }
            FreqPeaks = (Dictionary<double, double>)data;
        }

        public void RunScriptPostProcessing()
        {
            foreach(string valName in scriptVals.Keys)
            {
                if (valName == "TUNING_OUT")
                    Analyser.GetMusic().SetTuningPercent(app.PitchSyncVal + (int)scriptVals[valName]);
            }
        }

        public void ReadSpectrogramFrame()
        {
            double curAudioPos = app.AudioSource.AudioStream.CurrentTime.TotalMilliseconds;
            // Gets the frame with a timestamp closest to curAudioPos
            SpectrogramFrame curFrame = SpectrogramHandler.Spectrogram.Frames.Aggregate(
                (x, y) => Math.Abs(x.Timestamp - curAudioPos) < Math.Abs(y.Timestamp - curAudioPos) ? x : y); 
            
            // Converts byte valued spectrogram data to double valued spectrum data 
            double[] doubleData = new double[curFrame.SpectrumData.Length];
            for (int i = 0; i < doubleData.Length; i++)
                doubleData[i] = curFrame.SpectrumData[i];
            spectrumData = doubleData;

            foreach(Note note in curFrame.Notes)
            {
                Analyser.BufferNote(note.NoteIndex);
                Analyser.GetMusic().CountNote(note.Name + "0");
            }

            // Directly assigns analyser properties from current spectrogram frame
            Analyser.Notes = curFrame.Notes.ToList();
            Analyser.Chords = curFrame.Chords.ToList();
            Analyser.CurrentKey = curFrame.KeySignature;
            Analyser.CalculateNotePercentages();
            FrequencyAnalysisToSpectrum(SpectrogramHandler.Spectrogram.FrequencyScale);

            curFrame = null;
            doubleData = null;
            spectrumData = null;
            GC.Collect();
        }

        private short[] ReadAudioStream()
        {
            byte[] bytesBuffer;
            short[] audioBuffer;

            bytesBuffer = new byte[Prefs.BUFFERSIZE * 2];
            double posScaleFactor = (double)app.AudioSource.Audio.WaveFormat.SampleRate * ((app.AudioSource.AudioStream.WaveFormat.Channels + 1 - Prefs.RESAMP_CHANNELS)) / (double)app.AudioSource.AudioAnalysis.WaveFormat.SampleRate;
            if (app.Mode == 1)
            {
                if(app.StepBack && app.AudioSource.AudioAnalysis.Position > 0)
                    app.AudioSource.AudioAnalysis.Position -= app.AudioSource.AudioAnalysis.WaveFormat.SampleRate * app.StepMilliseconds / 1000 * app.AudioSource.AudioAnalysis.WaveFormat.BitsPerSample / 4;
                else
                    app.AudioSource.AudioAnalysis.Position += app.AudioSource.AudioAnalysis.WaveFormat.SampleRate * app.StepMilliseconds / 1000 * app.AudioSource.AudioAnalysis.WaveFormat.BitsPerSample / 4;

                app.AudioSource.AudioStream.Position = (long)(app.AudioSource.AudioAnalysis.Position * posScaleFactor * app.AudioSource.AudioStream.WaveFormat.Channels);
            }
            else
            {
                app.AudioSource.AudioAnalysis.Position = (long)(app.AudioSource.AudioStream.Position / (posScaleFactor * app.AudioSource.AudioStream.WaveFormat.Channels)); // Syncs position of FFT WaveStream to current playback position
            }
            app.AudioSource.AudioAnalysis.Read(bytesBuffer, 0, Prefs.BUFFERSIZE * 2); // Reads PCM data at synced position to bytesBuffer
            app.AudioSource.AudioAnalysis.Position -= Prefs.BUFFERSIZE * 2;
            audioBuffer = new short[Prefs.BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length); // Bytes to shorts
            return audioBuffer;
        }

        /*
         * Performs smoothing on frequency domain data by averaging several frames
         */
        private double[] SmoothSignal(double[] signal, int smoothDepth)
        {
            double[] newSignal = new double[signal.Length];
            Array.Copy(signal, newSignal, signal.Length);
            prevSpectrumData.Add(newSignal);

            if (prevSpectrumData.Count > smoothDepth)
                prevSpectrumData.RemoveAt(0);

            for (int i = 0; i < newSignal.Length; i++)
            {
                double smoothedValue = 0;
                for (int j = 0; j < prevSpectrumData.Count; j++)
                {
                    smoothedValue += prevSpectrumData[j][i];
                }
                smoothedValue /= prevSpectrumData.Count;
                newSignal[i] = smoothedValue;
            }
            return newSignal;
        }

        private byte[] SpectrogramQuantiser(double[] data, out double quantScale)
        {
            byte[] output = new byte[data.Length];
            quantScale = 1;

            if (GetScriptVal("QUANT_BIT_DEPTH") != null)
            {
                if ((double)GetScriptVal("QUANT_BIT_DEPTH") == 8.0)
                {
                    for (int i = 0; i < data.Length; i++)
                        output[i] = (byte)data[i];
                    return output;
                }
            }

            double bandSize = data.Max() / 255;
            for (int i = 0; i < data.Length; i++)
                output[i] = (byte)Math.Floor(data[i] / bandSize);
            quantScale = bandSize;
            return output;
        }

        private byte[] FilterSpectrogramData(byte[] data)
        {
            object scale = GetScriptVal("SCALE");
            int size = 0;
            if (scale.GetType().Name == "Func`2")
            {
                Func<double, double> scaleFunc = (Func<double, double>)scale;
                if (scaleFunc(data.Length - 1) <= Prefs.SPEC_MAX_FREQ)
                    return data;
                
                for(int i = data.Length - 2; i > 0; i--)
                {
                    if(scaleFunc(i) <= Prefs.SPEC_MAX_FREQ)
                    {
                        size = i;
                        break;
                    }
                }
            }
            else
                size = (int)Math.Floor(Prefs.SPEC_MAX_FREQ / (double)scale);

            byte[] output = new byte[size];
            Array.Copy(data, output, output.Length);
            return output;
        }

        public object GetScriptVal(string name)
        {
            if(scriptVals.ContainsKey(name))
                return scriptVals[name];
            return null;
        }

        public void ClearSpectrogramData()
        {
            SpectrogramHandler.Clear();
            largestTimestamp = -1;
        }

        public void Dispose()
        {
            ClearSpectrogramData();
            lastGC = 0;
            Analyser.DisposeAnalyser();
        }
    }
}
