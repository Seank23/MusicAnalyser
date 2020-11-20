using System;
using System.Collections.Generic;
using System.Linq;
using MusicAnalyser.App.DSP;

class BySlopeDetector : ISignalDetector
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputData { get; set; }
    public object InputScale { get; set; }
    public object Output { get; set; }

    public BySlopeDetector()
    {
        Settings = new Dictionary<string, string[]>
        {
            { "MIN_FREQ", new string[] { "30", "int", "Min Frequency (Hz)", "0", "20000" } },
            { "MAX_FREQ", new string[] { "2000", "int", "Max Frequency (Hz)", "0", "20000" } },
        };
    }

    public void OnSettingsChange() { }

    public void Detect()
    {
        double[] input = null;
        double scale = 0;
        if (InputData.GetType().Name == "Double[]")
            input = (double[])InputData;
        if (InputScale.GetType().Name == "Double")
            scale = (double)InputScale;
        if (input == null || scale == 0)
            return;

        Dictionary<double, double> output = new Dictionary<double, double>();
        double[] derivative = GetSlope(input, scale);
        double gainThreshold = input.Average() + 25;

        for (int i = (int)(scale * int.Parse(Settings["MIN_FREQ"][0])); i < Math.Min(input.Length, (int)(scale * int.Parse(Settings["MAX_FREQ"][0]))); i++)
        {
            if (input[i] < gainThreshold)
                continue;

            if (derivative[i] > 0 && derivative[i + 1] < 0)
            {
                double freq = (i + 1) / scale;
                double avgGainChange = (derivative[i] + derivative[i - 1] + derivative[i - 2]) / 3;
                if (avgGainChange > 3)
                    output.Add(freq, input[i]);
            }
        }

        Output = output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
    }

    private double[] GetSlope(double[] source, double scale)
    {
        double[] derivative = new double[source.Length];
        derivative[0] = 0;
        for (int i = 1; i < source.Length - 1; i++)
        {
            double deltaX = ((i + 2) / scale) - (i / scale);
            derivative[i] = (source[i + 1] - source[i - 1]) / deltaX; // P[i] = y[i + 1] - y[i - 1] / x[i + 1] - x[i - 1]
        }
        derivative[source.Length - 1] = 0;
        return derivative;
    }
}

