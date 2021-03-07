/*
 * Music Analyser - Secondary Processor Script - SpectrumQuantiser
 * Author: Sean King
 * Quantises the magnitude values of the frequency spectrum to a specied number of levels.
 * Can be used with both linearly scaled (eg. FFT) and non-linearly scaled (eg. CQT) spectrums.
 * Properties:
 * InputBuffer: type double[] (Spectrum)
 * OutputBuffer: type double[]
 * InputArgs: None
 * OutputArgs: None
 * Settings:
 * - ENABLED: Specifies if filter is active or bypassed - type enum (values: Yes, No)
 * - LEVELS: Specifies the number of quantisation levels to be used - type enum (values: 16, 32, 64, 128, 256, 512, 1024)
 */
using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
class SpectrumQuantiserProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return false; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object OutputBuffer { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

    public SpectrumQuantiserProcessor()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "ENABLED", new string[] { "Yes", "enum", "Enabled", "Yes|No", "" } },
            { "LEVELS", new string[] { "256", "enum", "Quantization Levels", "16|32|64|128|256|512|1024", "" } },
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

        if (Settings["ENABLED"][0] == "Yes")
        {
            double bandSize = input.Max() / int.Parse(Settings["LEVELS"][0]) - 1;
            double[] output = new double[input.Length];

            for (int i = 0; i < input.Length; i++)
                output[i] = Math.Floor(input[i] / bandSize);

            OutputArgs.Add("QUANT_BIT_DEPTH", (int)Math.Log(int.Parse(Settings["LEVELS"][0]), 2));
            OutputBuffer = output;
        }
        else
            OutputBuffer = input;
    }
}
