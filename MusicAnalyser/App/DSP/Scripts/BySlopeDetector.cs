﻿using System;
using System.Collections.Generic;
using System.Linq;
using MusicAnalyser.App.DSP;

class BySlopeDetector : ISignalDetector
{
    public Dictionary<string, string[]> Settings { get; set; }
    public double[] InputData { get; set; }
    public double InputScale { get; set; }
    public Dictionary<double, double> Output { get; set; }

    public BySlopeDetector()
    {
        Settings = new Dictionary<string, string[]>
        {
            { "MIN_FREQ", new string[] { "30", "int", "Min Frequency (Hz)", "0", "20000" } },
            { "MAX_FREQ", new string[] { "2000", "int", "Max Frequency (Hz)", "0", "20000" } },
        };
    }

    public void Detect()
    {
        double[] derivative = GetSlope(InputData);
        Output = new Dictionary<double, double>();
        double gainThreshold = InputData.Average() + 25;

        for (int i = (int)(InputScale * int.Parse(Settings["MIN_FREQ"][0])); i < Math.Min(InputData.Length, (int)(InputScale * int.Parse(Settings["MAX_FREQ"][0]))); i++)
        {
            if (InputData[i] < gainThreshold)
                continue;

            if (derivative[i] > 0 && derivative[i + 1] < 0)
            {
                double freq = (i + 1) / InputScale;
                double avgGainChange = (derivative[i] + derivative[i - 1] + derivative[i - 2]) / 3;
                if (avgGainChange > 3)
                    Output.Add(freq, InputData[i]);
            }
        }

        Output = Output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
    }

    private double[] GetSlope(double[] source)
    {
        double[] derivative = new double[source.Length];
        derivative[0] = 0;
        for (int i = 1; i < source.Length - 1; i++)
        {
            double deltaX = ((i + 2) / InputScale) - (i / InputScale);
            derivative[i] = (source[i + 1] - source[i - 1]) / deltaX; // P[i] = y[i + 1] - y[i - 1] / x[i + 1] - x[i - 1]
        }
        derivative[source.Length - 1] = 0;
        return derivative;
    }
}
