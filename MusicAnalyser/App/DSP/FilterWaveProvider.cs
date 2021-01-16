using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace MusicAnalyser.App.DSP
{
    public class FilterWaveProvider : ISampleProvider, IDisposable
    {
        ISampleProvider sourceProvider;
        private BiQuadFilter lowPass;
        private BiQuadFilter highPass;

        public FilterWaveProvider(ISampleProvider sourceProvider, float lowPassFreq = 20000, float lowPassQ = 1, float highPassFreq = 20, float highPassQ = 1)
        {
            this.sourceProvider = sourceProvider;
            lowPass = BiQuadFilter.LowPassFilter(WaveFormat.SampleRate, lowPassFreq, lowPassQ);
            highPass = BiQuadFilter.HighPassFilter(WaveFormat.SampleRate, highPassFreq, highPassQ);
        }

        public void SetFilter(float lowPassFreq, float lowPassQ, float highPassFreq, float highPassQ)
        {
            lowPass.SetLowPassFilter(WaveFormat.SampleRate, lowPassFreq, lowPassQ);
            highPass.SetHighPassFilter(WaveFormat.SampleRate, highPassFreq, highPassQ);
        }

        public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

        public void Dispose()
        {
            sourceProvider = null;
            lowPass = null;
            highPass = null;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sourceProvider.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
                buffer[offset + i] = lowPass.Transform(buffer[offset + i]);
            for (int i = 0; i < samplesRead; i++)
                buffer[offset + i] = highPass.Transform(buffer[offset + i]);

            return samplesRead;
        }
    }
}

