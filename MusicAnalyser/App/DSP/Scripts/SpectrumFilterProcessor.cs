/*
 * Music Analyser - Secondary Processor Script - SpectrumFilter
 * Author: Sean King
 * Performs a band pass filter operation on the frequency spectrum.
 * Can be used with both linearly scaled (eg. FFT) and non-linearly scaled (eg. CQT) spectrums.
 * Properties:
 * InputBuffer: type double[] (Spectrum)
 * OutputBuffer: type double[]
 * InputArgs: SCALE - Frequency scale value/function - type double (linear) or Func<int, double> (non-linear)
 * OutputArgs: None
 * Settings:
 * - ENABLED: Specifies if filter is active or bypassed - type enum (values: Yes, No)
 * - LOW_CUT: Specifies the low cutoff frequency (Hz) of the band pass filter - type double (values: 0 - 2000)
 * - HIGH_CUT: Specifies the high cutoff frequency (Hz) of the band pass filter - type double (values: 0 - 2000)
 * - ATT_FACTOR: Specifies the sharpness of the attenuated bands - type double (0 - 1)
 */
using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;

class SpectrumFilterProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return false; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object OutputBuffer { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    public SpectrumFilterProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "ENABLED", new string[] { "Yes", "enum", "Enabled", "Yes|No", "" } },
            { "LOW_CUT", new string[] { "200", "double", "Low Cutoff (Hz)", "0", "2000" } },
            { "HIGH_CUT", new string[] { "2000", "double", "High Cutoff (Hz)", "0", "2000" } },
            { "ATT_FACTOR", new string[] { "1", "double", "Attenuation Factor", "0", "1" } },
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
        if (InputArgs.ContainsKey("SCALE"))
        {
            if (InputArgs["SCALE"] == null)
                return;
        }
        else
            return;

        if (Settings["ENABLED"][0] == "Yes")
        {
            double[] output = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                if (InputArgs["SCALE"].GetType().Name == "Double")
                {
                    output[i] = GetFilteredSample(i * (double)InputArgs["SCALE"], input[i]);
                }
                else if (InputArgs["SCALE"].GetType().Name == "Func`2")
                {
                    var scaleFunc = (Func<int, double>)InputArgs["SCALE"];
                    output[i] = GetFilteredSample(scaleFunc(i), input[i]);
                }
            }
            OutputBuffer = output;
        }
        else
            OutputBuffer = input;
    }

    private double GetFilteredSample(double freq, double mag)
    {
        double output = mag;
        if (freq < double.Parse(Settings["LOW_CUT"][0]))
            output = mag * Math.Pow(freq / double.Parse(Settings["LOW_CUT"][0]), 20 * double.Parse(Settings["ATT_FACTOR"][0]));
        else if (freq > double.Parse(Settings["HIGH_CUT"][0]))
            output = mag * Math.Pow(double.Parse(Settings["HIGH_CUT"][0]) / freq, 20 * double.Parse(Settings["ATT_FACTOR"][0]));
        return output;
    }
}
