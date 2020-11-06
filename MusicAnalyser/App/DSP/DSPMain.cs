using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP
{
    class DSPMain
    {
        public Analyser Analyser { get; set; }
        public ScriptManager ScriptManager { get; set; }
        public double MaxGain { get; set; }

        private AppController app;
        private Dictionary<int, ISignalProcessor> processors = new Dictionary<int, ISignalProcessor>();
        private Dictionary<int, ISignalDetector> detectors = new Dictionary<int, ISignalDetector>();

        private double[] processedData;
        public List<double[]> prevProcessedData = new List<double[]>();
        private double scale;
        public Dictionary<double, double> fftPeaks;
        private int detectorIndex = 0;

        public DSPMain(AppController appController)
        {
            Analyser = new Analyser();
            app = appController;
            ScriptManager = new ScriptManager();
            LoadScripts();
        }

        public async void LoadScripts()
        {
            await Task.Factory.StartNew(() => ScriptManager.LoadScripts());
            LoadScriptSettings();
            app.SetScriptSelectorUI(ScriptManager.GetAllScriptNames(), false);
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
                            processors.Add(j, ScriptManager.ProcessorScripts[j]);
                            break;
                        }
                        else if (ScriptManager.DetectorScripts.ContainsKey(j))
                        {
                            detectors.Add(j, ScriptManager.DetectorScripts[j]);
                            if (ScriptManager.DetectorScripts[j].IsPrimary && detectorIndex == 0)
                                detectorIndex = j;
                            break;
                        }
                    }
                }
            }
            app.ScriptSelectionApplied = true;
        }

        public bool GetFrequencyAnalysis()
        {
            object audio = ReadAudioStream();

            foreach (int key in processors.Keys)
            {
                if (key < detectorIndex)
                {
                    processors[key].InputBuffer = audio;
                    processors[key].SampleRate = app.AudioSource.AudioFFT.WaveFormat.SampleRate;
                    processors[key].Process();
                    audio = processors[key].OutputBuffer;
                    scale = processors[key].OutputScale;
                }
                else break;
            }

            processedData = (double[])audio;
            if (!Double.IsInfinity(processedData[0]))
                processedData = SmoothSignal(processedData, Prefs.SMOOTH_FACTOR);

            MaxGain = processedData.Max();
            app.DrawSpectrum(processedData, scale, processedData.Average(), MaxGain);

            return true;
        }

        public void GetDetectedPitches()
        {
            object data = processedData;
            for(int i = detectorIndex; i < ScriptManager.GetScriptCount(); i++)
            {
                if(processors.ContainsKey(i))
                {
                    processors[i].InputBuffer = data;
                    processors[i].SampleRate = app.AudioSource.AudioFFT.WaveFormat.SampleRate;
                    processors[i].Process();
                    data = processors[i].OutputBuffer;
                }
                else if(detectors.ContainsKey(i))
                {
                    detectors[i].InputData = data;
                    detectors[i].InputScale = scale;
                    detectors[i].Detect();
                    data = detectors[i].Output;
                }
            }
            fftPeaks = (Dictionary<double, double>)data;
        }

        private short[] ReadAudioStream()
        {
            byte[] bytesBuffer;
            short[] audioBuffer;

            bytesBuffer = new byte[Prefs.BUFFERSIZE];
            double posScaleFactor = (double)app.AudioSource.Audio.WaveFormat.SampleRate / (double)app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            app.AudioSource.AudioFFT.Position = (long)(app.AudioSource.AudioStream.Position / posScaleFactor / app.AudioSource.AudioStream.WaveFormat.Channels); // Syncs position of FFT WaveStream to current playback position
            app.AudioSource.AudioFFT.Read(bytesBuffer, 0, Prefs.BUFFERSIZE); // Reads PCM data at synced position to bytesBuffer
            audioBuffer = new short[Prefs.BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length); // Bytes to shorts
            return audioBuffer;
        }

        /*
         * Performs smoothing on frequency domain data by averaging several frames
         */
        public double[] SmoothSignal(double[] signal, int smoothDepth)
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
    }
}
