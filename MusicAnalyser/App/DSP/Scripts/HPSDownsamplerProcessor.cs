/*
 * Music Analyser - Secondary Processor Script - HPSDownsampler
 * Author: Sean King
 * Implementation of the Harmonic Product Spectrum (HPS) algorithm, to be used in conjunction with a FFT primary processor script.
 * Based on the HPS method described here:
 * https://cnx.org/contents/aY7_vV4-@5.8:i5AAkZCP@2/Pitch-Detection-Algorithms
 * Properties:
 * InputBuffer: type double[] (Output of FFT)
 * OutputBuffer: type double[]
 * InputArgs: None
 * OutputArgs: None
 * Settings:
 * - HARMONICS: Specifies the number of harmonics to consolidate via downsampling - type int (values: 0 - 5)
 * - INTERP: Specifies an interpolation factor to upsample output spectrum - type int (values: 1 - 5)
 * - MAG_SCALE: Specifies a value to scale down the output magnitude exponentially - type double (1 - 5)
 * - SQUARE: Specifies whether output magnitudes should be squared - type enum (values: Yes, No)
 * - FLOOR: Specifies the minimum output spectrum value - type double (0 - 10)
 */
using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;

class HPSDownsamplerProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return false; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object OutputBuffer { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    public HPSDownsamplerProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "HARMONICS", new string[] { "2", "int", "Number of Harmonics", "0", "5" } },
            { "INTERP", new string[] { "2", "int", "Interpolation Factor", "1", "5" } },
            { "MAG_SCALE", new string[] { "2", "double", "Magnitude Scale Factor", "1", "5" } },
            { "SQUARE", new string[] { "No", "enum", "Square Output", "Yes|No", "" } },
            { "FLOOR", new string[] { "1", "double", "Spectrum Floor", "0", "10" } },
        };
    }

    public void OnSettingsChange() { }

    public void Process()
    {
        double[] input = null;
        if (InputBuffer.GetType().Name == "Double[]")
            input = (double[])InputBuffer;
        if (input == null)
            return;

        for (int i = 0; i < input.Length; i++)
            input[i] += double.Parse(Settings["FLOOR"][0]);

        List<double[]> spectrums = new List<double[]>();
        spectrums.Add(input);

        for(int i = 0; i < int.Parse(Settings["HARMONICS"][0]); i++)
        {
            double[] downsampledSpectrum = Downsample(input, i + 2);
            spectrums.Add(downsampledSpectrum);
        }

        double[] productSpectrum = new double[input.Length];
        for(int i = 0; i < spectrums[spectrums.Count - 1].Length; i++)
        {
            productSpectrum[i] = 1;
            for (int j = 0; j < spectrums.Count; j++)
                productSpectrum[i] *= spectrums[j][i];
            productSpectrum[i] = Math.Max(Math.Pow(productSpectrum[i], 1.0 / double.Parse(Settings["MAG_SCALE"][0])) - double.Parse(Settings["FLOOR"][0]), 0);
            if (Settings["SQUARE"][0] == "Yes")
                productSpectrum[i] *= productSpectrum[i];
        }
        if (int.Parse(Settings["INTERP"][0]) > 1)
            OutputBuffer = Interpolate(productSpectrum, int.Parse(Settings["INTERP"][0]));
        else
            OutputBuffer = productSpectrum;
    }

    private double[] Downsample(double[] source, int sampleFactor)
    {
        if (sampleFactor > 0)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < source.Length - sampleFactor + 1; i += sampleFactor)
            {
                double avgVal = 0;
                for (int j = 0; j < sampleFactor; j++)
                    avgVal += source[i + j];
                avgVal /= sampleFactor;
                output.Add(avgVal);
            }
            return output.ToArray();
        }
        return null;
    }

    private double[] Interpolate(double[] source, int interpolationFactor)
    {
        if (interpolationFactor > 0)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < source.Length - 1; i++)
            {
                output.Add(source[i]);
                double delta = source[i + 1] - source[i];
                for (int j = 1; j < interpolationFactor; j++)
                    output.Add(source[i] + delta * ((double)j / interpolationFactor));
            }
            return output.ToArray();
        }
        return null;
    }
}
