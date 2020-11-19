using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MusicAnalyser.App.DSP;

class CQTProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public int SampleRate { get; set; }
    public object OutputBuffer { get; set; }
    public double OutputScale { get; set; }

    private Matrix<Complex> kernel;

    public CQTProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "WINDOW", new string[] { "Hamming", "enum", "Window Function", "Rectangle|Hamming|Hann|BlackmannHarris", "" } },
            { "OCTAVES", new string[] { "7", "int", "Octaves", "1", "10" } },
            { "BINS_PER_OCTAVE", new string[] { "24", "enum", "Bins Per Octave", "12|24|48|96", "" } },
        };
    }
    public void Process()
    {
        if (kernel == null)
            GetSparseKernel();

        short[] input = (short[])InputBuffer;
        int fftPoints = 2;
        while (fftPoints * 2 <= input.Length)
            fftPoints *= 2;

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
        if (fftFull.Length < kernel.RowCount)
            Array.Resize(ref fftFull, kernel.RowCount);
        NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(kernel.RowCount, 2.0), fftFull);
        Complex[] fftComp = new Complex[fftFull.Length];
        for(int i = 0; i < fftComp.Length; i++)
            fftComp[i] = new Complex(fftFull[i].X, fftFull[i].Y);

        Matrix<Complex> fftVec = CreateVector.DenseOfArray(fftComp).ToRowMatrix();
        Complex[] product = (fftVec * kernel).Row(0).AsArray();
        double[] mag = new double[product.Length];
        for (int i = 0; i < mag.Length; i++)
            mag[i] = product[i].Magnitude;
        OutputBuffer = mag;
        OutputScale = 1;
    }

    private void GetSparseKernel()
    {
        float threshold = 0.00000054f;
        float minFreq = 32.7f;
        int numOctaves = int.Parse(Settings["OCTAVES"][0]);
        int binsPerOctave = int.Parse(Settings["BINS_PER_OCTAVE"][0]);
        int numBins = numOctaves * binsPerOctave;
        double Q = 1 / (Math.Pow(2, 1 / (double)binsPerOctave) - 1);

        int fftLen = 1;
        while (fftLen < Q * SampleRate / minFreq)
            fftLen *= 2;

        NAudio.Dsp.Complex[] tempKernel = new NAudio.Dsp.Complex[fftLen];
        List<Complex[]> sparKernel = new List<Complex[]>();

        for (int k = 0; k < numBins; k++)
        {
            int N = (int)Math.Ceiling(Q * SampleRate / (minFreq * Math.Pow(2, k / (double)binsPerOctave)));
            for (int n = 0; n < N; n++)
            {
                Complex temp = NAudio.Dsp.FastFourierTransform.HammingWindow(n, N) / N * Complex.Exp(-2 * Math.PI * Complex.ImaginaryOne * n * (Q / N));
                tempKernel[n].X = (float)temp.Real;
                tempKernel[n].Y = (float)temp.Imaginary;
            }
            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftLen, 2.0), tempKernel);
            Complex[] compKernel = new Complex[tempKernel.Length];
            for (int i = 0; i < tempKernel.Length; i++)
            {
                if (tempKernel[i].X < threshold && tempKernel[i].Y < threshold)
                    compKernel[i] = new Complex(0, 0);
                else
                    compKernel[i] = new Complex(tempKernel[i].X, tempKernel[i].Y);
            }
            sparKernel.Add(compKernel);
        }
        Matrix<Complex> kernelMat = CreateMatrix.SparseOfRowArrays(sparKernel.ToArray());
        kernel = kernelMat.ConjugateTranspose();
    }
}

