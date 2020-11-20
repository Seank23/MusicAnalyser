using System;
using System.Collections.Generic;
using MusicAnalyser.App.DSP;

class BasicFFTProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public int SampleRate { get; set; }
    public object OutputBuffer { get; set; }
    public object OutputScale { get; set; }

    public BasicFFTProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "WINDOW", new string[] { "Hamming", "enum", "Window Function", "Rectangle|Hamming|Hann|BlackmannHarris", "" } }
        };
    }

    public void Process()
    {
        short[] input = null;
        if (InputBuffer.GetType().Name == "Int16[]")
            input = (short[])InputBuffer;
        if (input == null)
            return;

        int fftPoints = 2;
        while (fftPoints * 2 <= input.Length) // Sets fftPoints to largest multiple of 2 in BUFFERSIZE
            fftPoints *= 2;
        double[] output = new double[fftPoints / 2];

        // FFT Process
        NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
        for (int i = 0; i < fftPoints; i++)
        {
            if (Settings["WINDOW"][0] == "Hamming")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));
            else if (Settings["WINDOW"][0] == "Hann")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HannWindow(i, fftPoints));
            else if (Settings["WINDOW"][0] == "BlackmannHarris")
                fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.BlackmannHarrisWindow(i, fftPoints));
            else
                fftFull[i].X = input[i];
        }    
        NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftPoints, 2.0), fftFull);

        for (int i = 0; i < fftPoints / 2; i++) // Since FFT output is mirrored above Nyquist limit (fftPoints / 2), these bins are summed with those in base band
        {
            double fft = Math.Abs(fftFull[i].X + fftFull[i].Y);
            double fftMirror = Math.Abs(fftFull[fftPoints - i - 1].X + fftFull[fftPoints - i - 1].Y);
            output[i] = 20 * Math.Log10(fft + fftMirror) - 20 * Math.Log10(input.Length); // Estimates gain of FFT bin
        }
        OutputBuffer = output;
        OutputScale = (double)fftPoints / SampleRate;
    }
}

