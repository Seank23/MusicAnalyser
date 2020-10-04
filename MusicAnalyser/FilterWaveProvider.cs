using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser
{
    class FilterWaveProvider : ISampleProvider, IDisposable
    {
        ISampleProvider sourceProvider;
        private BiQuadFilter lowPass;
        private BiQuadFilter highPass;

        public FilterWaveProvider(ISampleProvider sourceProvider, BiQuadFilter lowPass, BiQuadFilter highPass)
        {
            this.sourceProvider = sourceProvider;
            this.lowPass = lowPass;
            this.highPass = highPass;
        }

        public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

        public void Dispose()
        {
            sourceProvider = null;
            lowPass = null;
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

