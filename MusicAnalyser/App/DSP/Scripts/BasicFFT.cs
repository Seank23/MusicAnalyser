using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.DSP.Scripts
{
    class BasicFFT : ISignalProcessor
    {
        public short[] InputBuffer { get; set; }
        public int SampleRate { get; set; }
        public double[] OutputBuffer { get; set; }
        public double OutputScale { get; set; }

        public void Process()
        {
            int fftPoints = 2;
            while (fftPoints * 2 <= InputBuffer.Length) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
                fftPoints *= 2;
            OutputBuffer = new double[fftPoints / 2];

            // FFT Process
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(InputBuffer[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
            {
                double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                OutputBuffer[i] = 20 * Math.Log10(fft + fftMirror) - Prefs.PEAK_FFT_POWER; // Estimates gain of FFT bin
            }
            OutputScale = (double)fftPoints / SampleRate;
        }
    }
}
