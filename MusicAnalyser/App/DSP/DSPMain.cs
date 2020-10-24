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
        public double MaxGain { get; set; }

        private AppController app;
        private ScriptManager scriptManager;
        private ISignalProcessor processor;
        private ISignalDetector detector;

        private double[] processedData;
        public List<double[]> prevProcessedData = new List<double[]>();
        private double scale;
        public Dictionary<double, double> fftPeaks;

        public DSPMain(AppController appController)
        {
            Analyser = new Analyser();
            app = appController;
            scriptManager = new ScriptManager();
            processor = new BasicFFTProcessor();
            detector = new ByMagnitudeDetector();
        }

        /*
         * Master method for calculating realtime frequency domain data from audio playback
         */
        public bool GetFrequencyAnalysis()
        {
            byte[] bytesBuffer;
            short[] audioBuffer;

            bytesBuffer = new byte[Prefs.BUFFERSIZE];
            double posScaleFactor = (double)app.AudioSource.Audio.WaveFormat.SampleRate / (double)app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            app.AudioSource.AudioFFT.Position = (long)(app.AudioSource.AudioStream.Position / posScaleFactor / app.AudioSource.AudioStream.WaveFormat.Channels); // Syncs position of FFT WaveStream to current playback position
            app.AudioSource.AudioFFT.Read(bytesBuffer, 0, Prefs.BUFFERSIZE); // Reads PCM data at synced position to bytesBuffer
            audioBuffer = new short[Prefs.BUFFERSIZE];
            Buffer.BlockCopy(bytesBuffer, 0, audioBuffer, 0, bytesBuffer.Length); // Bytes to shorts

            processor.InputBuffer = audioBuffer;
            processor.SampleRate = app.AudioSource.AudioFFT.WaveFormat.SampleRate;
            processor.Process();
            processedData = (double[])processor.OutputBuffer;
            scale = processor.OutputScale;

            if (!Double.IsInfinity(processedData[0]))
                processedData = SmoothSignal(processedData, Prefs.SMOOTH_FACTOR);

            MaxGain = processedData.Max();
            app.DrawSpectrum(processedData, scale, processedData.Average(), MaxGain);

            return true;
        }

        public void GetDetectedPitches()
        {
            detector.InputData = processedData;
            detector.InputScale = scale;
            detector.Detect();
            RemoveKickNoiseProcessor denoise = new RemoveKickNoiseProcessor();
            denoise.InputBuffer = detector.Output;
            denoise.Process();
            fftPeaks = (Dictionary<double, double>)denoise.OutputBuffer;
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
