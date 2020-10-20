using System;

namespace MusicAnalyser.App.DSP.Scripts
{
    class BasicFFTProcessor : ISignalProcessor
    {
        public object InputBuffer { get; set; }
        public int SampleRate { get; set; }
        public object OutputBuffer { get; set; }
        public double OutputScale { get; set; }

        public void Process()
        {
            short[] input = (short[])InputBuffer;
            int fftPoints = 2;
            while (fftPoints * 2 <= input.Length) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
                fftPoints *= 2;
            double[] output = new double[fftPoints / 2];

            // FFT Process
            NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
            for (int i = 0; i < fftPoints; i++)
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

            for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
            {
                double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
                double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
                output[i] = 20 * Math.Log10(fft + fftMirror) - Prefs.PEAK_FFT_POWER; // Estimates gain of FFT bin
            }
            OutputBuffer = output;
            OutputScale = (double)fftPoints / SampleRate;
        }
    }
}
