using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP
{
    class DSPMain
    {
        public Analyser Analyser { get; set; }
        public ScriptManager ScriptManager { get; set; }
        public SpectrogramHandler Spectrogram { get; set; }
        public double MaxGain { get; set; }
        public Dictionary<double, double> FreqPeaks { get; set; }
        public double CurTimestamp { get; set; }

        private AppController app;
        private Dictionary<int, ISignalProcessor> processors = new Dictionary<int, ISignalProcessor>();
        private Dictionary<int, ISignalDetector> detectors = new Dictionary<int, ISignalDetector>();
        private List<double[]> prevProcessedData = new List<double[]>();
        private Dictionary<string, object> scriptVals = new Dictionary<string, object>();
        private double[] processedData;
        private int detectorIndex = 0;
        private double largestTimestamp = -1;

        public DSPMain(AppController appController)
        {
            Analyser = new Analyser();
            app = appController;
            ScriptManager = new ScriptManager();
            Spectrogram = new SpectrogramHandler();
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

            for(int i = 0; i < selectionDict.Count; i++)
            {
                for(int j = 0; j < ScriptManager.GetScriptCount(); j++)
                {
                    if(selectionDict[i] == j)
                    {
                        if (ScriptManager.ProcessorScripts.ContainsKey(j))
                        {
                            processors.Add(i, ScriptManager.ProcessorScripts[j]);
                            break;
                        }
                        else if (ScriptManager.DetectorScripts.ContainsKey(j))
                        {
                            detectors.Add(i, ScriptManager.DetectorScripts[j]);
                            if (ScriptManager.DetectorScripts[j].IsPrimary && detectorIndex == 0)
                                detectorIndex = i;
                            break;
                        }
                    }
                }
            }
            app.ScriptSelectionApplied = true;
            prevProcessedData.Clear();
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
            scriptVals["SAMPLE_RATE"] = app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            scriptVals["TUNING_PERCENT"] = Analyser.GetMusic().GetTuningPercent();
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

            processedData = (double[])audio;
            if (!Double.IsInfinity(processedData[0]) && !Double.IsNaN(processedData[0]) && app.Mode != 1)
                processedData = SmoothSignal(processedData, Prefs.SMOOTH_FACTOR);

            HandleSpectrogram();
        }

        // Creates a spectrogram frame at specified update rate and initialises with spectrum data and timestamp
        private void HandleSpectrogram()
        {
            double curAudioPos = app.AudioSource.AudioFFT.CurrentTime.TotalMilliseconds;
            if (Prefs.STORE_SPEC_DATA && curAudioPos >= largestTimestamp)
            {
                if (Spectrogram.Frames.Count / (curAudioPos / 1000) <= Prefs.SPEC_UPDATE_RATE)
                {
                    byte[] specData = SpectrogramQuantiser(processedData);
                    specData = FilterSpectrogramData(specData);

                    if (Spectrogram.FrequencyScale == null)
                    {
                        object scale = GetScriptVal("SCALE", "Double");
                        if (scale == null)
                            scale = GetScriptVal("SCALE", "Func`2");
                        Spectrogram.FrequencyScale = scale;
                    }

                    Spectrogram.CreateFrame(curAudioPos, specData);
                    CurTimestamp = curAudioPos;
                    largestTimestamp = curAudioPos;
                }
                else
                    CurTimestamp = 0;
            }
            else
                CurTimestamp = 0;
        }

        public void FrequencyAnalysisToSpectrum()
        {
            MaxGain = processedData.Max();
            double avgGain = processedData.Average();
            if (scriptVals["SCALE"].GetType().Name == "Double")
                app.DrawSpectrum(processedData, (double)scriptVals["SCALE"], avgGain, MaxGain);
            else
                app.DrawSpectrum(processedData, 1, avgGain, MaxGain);
        }

        public void RunPitchDetection()
        {
            object data = processedData;
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

        private short[] ReadAudioStream()
        {
            byte[] bytesBuffer;
            short[] audioBuffer;

            bytesBuffer = new byte[Prefs.BUFFERSIZE];
            double posScaleFactor = (double)app.AudioSource.Audio.WaveFormat.SampleRate / (double)app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            if (app.Mode == 1)
            {
                if(app.StepBack && app.AudioSource.AudioFFT.Position > 0)
                    app.AudioSource.AudioFFT.Position -= app.AudioSource.AudioFFT.WaveFormat.SampleRate * app.StepMilliseconds / 1000 * app.AudioSource.AudioFFT.WaveFormat.BitsPerSample / 4;
                else
                    app.AudioSource.AudioFFT.Position += app.AudioSource.AudioFFT.WaveFormat.SampleRate * app.StepMilliseconds / 1000 * app.AudioSource.AudioFFT.WaveFormat.BitsPerSample / 4;

                app.AudioSource.AudioStream.Position = (long)(app.AudioSource.AudioFFT.Position * posScaleFactor * app.AudioSource.AudioStream.WaveFormat.Channels);
            }
            else
            {
                app.AudioSource.AudioFFT.Position = (long)(app.AudioSource.AudioStream.Position / posScaleFactor / app.AudioSource.AudioStream.WaveFormat.Channels); // Syncs position of FFT WaveStream to current playback position
            }
            app.AudioSource.AudioFFT.Read(bytesBuffer, 0, Prefs.BUFFERSIZE); // Reads PCM data at synced position to bytesBuffer
            app.AudioSource.AudioFFT.Position -= Prefs.BUFFERSIZE;
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
            prevProcessedData.Add(newSignal);

            if (prevProcessedData.Count > smoothDepth)
                prevProcessedData.RemoveAt(0);

            for (int i = 0; i < newSignal.Length; i++)
            {
                double smoothedValue = 0;
                for (int j = 0; j < prevProcessedData.Count; j++)
                {
                    smoothedValue += prevProcessedData[j][i];
                }
                smoothedValue /= prevProcessedData.Count;
                newSignal[i] = smoothedValue;
            }
            return newSignal;
        }

        private byte[] SpectrogramQuantiser(double[] data)
        {
            byte[] output = new byte[data.Length];

            if (GetScriptVal("QUANT_BIT_DEPTH", "Double") != null)
            {
                if ((double)GetScriptVal("QUANT_BIT_DEPTH", "Double") == 8.0)
                {
                    for (int i = 0; i < data.Length; i++)
                        output[i] = (byte)data[i];
                    return output;
                }
            }

            double bandSize = data.Max() / 256;
            for (int i = 0; i < data.Length; i++)
                output[i] = (byte)Math.Floor(data[i] / bandSize);
            return output;
        }

        private byte[] FilterSpectrogramData(byte[] data)
        {
            object scale = GetScriptVal("SCALE", "Double");
            if (scale == null)
                scale = GetScriptVal("SCALE", "Func`2");
            int size = 0;
            if (scale.GetType().Name == "Func`2")
            {
                Func<int, double> scaleFunc = (Func<int, double>)scale;
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

        public object GetScriptVal(string name, string type)
        {
            if(scriptVals.ContainsKey(name))
            {
                if (scriptVals[name].GetType().Name == type)
                    return scriptVals[name];
            }
            return null;
        }

        public void Dispose()
        {
            largestTimestamp = -1;
            CurTimestamp = -1;
            Spectrogram.Dispose();
        }
    }
}
