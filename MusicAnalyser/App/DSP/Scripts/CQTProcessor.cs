/*
 * Music Analyser - Primary Processor Script - CQT
 * Author: Sean King
 * Constant-Q Transform (CQT) implementation with optimised kernel calculation, set up for 12 tone equal temperment analysis.
 * Based on the efficient CQT algorithm by Benjamin Blankertz:
 * http://doc.ml.tu-berlin.de/bbci/material/publications/Bla_constQ.pdf
 * Properties:
 * InputBuffer: type short[]
 * OutputBuffer: type double[]
 * InputArgs: SAMPLE_RATE - sample rate (Hz) of the input signal - type int
 * OutputArgs: SCALE - Non-linear scale function to map each frequency bin to a frequency value - type Func<int, double>
 * Settings:
 * - OCTAVES: Number of octaves to analyse  - type int (1 - 10)
 * - BINS_PER_OCTAVE: Number of frequency bins per octave - type enum (values: 12, 24, 36, 48, 60, 72, 84, 96)
 * - MIN_FREQ: Starting frequency (Hz), for analysis in standard tuning use appropriate note frequency (eg. C1 = 32.7 Hz) - type double (1 - 1000)
 * - N_WEIGHTING: Frequency weighting factor, lower values emphasise the magnitude of low frequencies and vice versa - type double (0 - 1)
 * - OUTPUT_MODE: Specifies how the output magnitude should be scaled - type enum (values: Magnitude, dB)
 * - SQUARE: Specifies whether output magnitudes should be squared - type enum (values: Yes, No)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MusicAnalyser.App.DSP;

class CQTProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object OutputBuffer { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    private Matrix<Complex> kernel;

    public CQTProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "OCTAVES", new string[] { "5", "int", "Octaves", "1", "10" } },
            { "BINS_PER_OCTAVE", new string[] { "48", "enum", "Bins Per Octave", "12|24|36|48|60|72|84|96", "" } },
            { "MIN_FREQ", new string[] { "32.7", "double", "Minimum Frequency (Hz)", "1", "1000" } },
            { "N_WEIGHTING", new string[] { "0.5", "double", "Frequency Weighting Factor", "0", "1" } },
            { "OUTPUT_MODE", new string[] { "Magnitude", "enum", "Output Mode", "Magnitude|dB", "" } },
            { "SQUARE", new string[] { "No", "enum", "Square Output", "Yes|No", "" } },
        };
    }

    public void OnSettingsChange() 
    {
        if (InputBuffer != null || InputArgs != null)
            kernel = null;
    }

    public void Process()
    {
        short[] input = null;
        if (InputBuffer.GetType().Name == "Int16[]")
            input = (short[])InputBuffer;
        if (input == null)
            return;

        if (kernel == null)
            GetSparseKernel();

        if (input.Length != kernel.RowCount)
            Array.Resize(ref input, kernel.RowCount);

        int fftPoints = 2;
        while (fftPoints * 2 <= input.Length)
            fftPoints *= 2;

        NAudio.Dsp.Complex[] fftFull = new NAudio.Dsp.Complex[fftPoints];
        for (int i = 0; i < fftPoints; i++)
            fftFull[i].X = (float)(input[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftPoints));

        NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(kernel.RowCount, 2.0), fftFull);
        Complex[] fftComp = new Complex[fftFull.Length];

        for (int i = 0; i < fftComp.Length; i++)
            fftComp[i] = new Complex(fftFull[i].X, fftFull[i].Y);

        Matrix<Complex> fftVec = CreateVector.DenseOfArray(fftComp).ToRowMatrix();
        Complex[] product = (fftVec * kernel).Row(0).AsArray();
        double[] mag = new double[product.Length];

        for (int i = 0; i < mag.Length; i++)
            mag[i] = product[i].Magnitude;

        if (Settings["OUTPUT_MODE"][0] == "dB")
        {
            double maxDB = 20 * Math.Log10(mag.Max());
            for (int i = 0; i < mag.Length; i++)
                mag[i] = 20 * Math.Log10(product[i].Magnitude) - maxDB;
        }
        else if (Settings["SQUARE"][0] == "Yes")
        {
            for (int i = 0; i < mag.Length; i++)
                mag[i] = Math.Pow(mag[i], 2) / 100;
        }

        OutputBuffer = mag;
        Func<double, double> scale = i => double.Parse(Settings["MIN_FREQ"][0]) * Math.Pow(2, (double)i / int.Parse(Settings["BINS_PER_OCTAVE"][0]));
        OutputArgs.Add("SCALE", scale);
    }

    private void GetSparseKernel()
    {
        int sampleRate = 1;
        if (InputArgs.ContainsKey("SAMPLE_RATE"))
        {
            if (InputArgs["SAMPLE_RATE"].GetType().Name == "Int32")
                sampleRate = (int)InputArgs["SAMPLE_RATE"];
        }

        float threshold = 0.000054f;
        int numOctaves = int.Parse(Settings["OCTAVES"][0]);
        int binsPerOctave = int.Parse(Settings["BINS_PER_OCTAVE"][0]);
        double minFreq = double.Parse(Settings["MIN_FREQ"][0]);
        int numBins = numOctaves * binsPerOctave;
        double Q = 1 / (Math.Pow(2, 1 / (double)binsPerOctave) - 1);

        int fftLen = 1;
        while (fftLen < Q * sampleRate / minFreq)
            fftLen *= 2;

        NAudio.Dsp.Complex[] tempKernel = new NAudio.Dsp.Complex[fftLen];
        List<Complex[]> sparKernel = new List<Complex[]>();

        for (int k = 0; k < numBins; k++)
        {
            int N = (int)Math.Ceiling(Q * sampleRate / (minFreq * Math.Pow(2, k / (double)binsPerOctave)));
            for (int n = 0; n < N; n++)
            {
                Complex temp = NAudio.Dsp.FastFourierTransform.HammingWindow(n, N) / (N * (1 + (double.Parse(Settings["N_WEIGHTING"][0]) * N)))
                    * Complex.Exp(2 * Math.PI * Complex.ImaginaryOne * n * (Q / N)) * (1000 * (1 + (double.Parse(Settings["N_WEIGHTING"][0]) * 1000)));

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
        kernelMat.Multiply(1000);
        kernel = kernelMat.ConjugateTranspose();
    }
}