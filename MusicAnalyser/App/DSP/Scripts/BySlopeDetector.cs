using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicAnalyser.App.DSP.Scripts
{
    class BySlopeDetector : ISignalDetector
    {
        public double[] InputData { get; set; }
        public double InputScale { get; set; }
        public Dictionary<double, double> Output { get; set; }

        public void Detect()
        {
            double[] derivative = GetSlope(InputData);
            Output = new Dictionary<double, double>();
            double gainThreshold = InputData.Average() + 25;

            for (int i = (int)(InputScale * Prefs.MIN_FREQ); i < Math.Min(InputData.Length, (int)(InputScale * Prefs.MAX_FREQ)); i++)
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
}
