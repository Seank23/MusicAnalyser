using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicAnalyser.App.DSP.Scripts
{
    class ByMagnitudeDetector : ISignalDetector
    {
        public double[] InputData { get; set; }
        public double InputScale { get; set; }
        public Dictionary<double, double> Output { get; set; }

        public void Detect()
        {
            GetPeaksByMagnitude();
        }

        public void GetPeaksByMagnitude()
        {
            Output = new Dictionary<double, double>();
            double freq;
            double gainThreshold = InputData.Average() + 25;

            // Iterates through frequency data, storing the frequency and gain of the largest frequency bins 
            for (int i = (int)(InputScale * Prefs.MIN_FREQ); i < Math.Min(InputData.Length, (int)(InputScale * Prefs.MAX_FREQ)); i++)
            {
                if (InputData[i] > gainThreshold)
                {
                    freq = (i + 1) / InputScale; // Frequency value of bin

                    Output.Add(freq, InputData[i]);
                    Output = Output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low

                    if (Output.Count > Prefs.PEAK_BUFFER) // When fftPeaks overflows, remove smallest frequency bin
                    {
                        double keyToRemove = GetDictKey(Output, Output.Count - 1);
                        Output.Remove(keyToRemove);
                    }
                }
            }

            Output = Output.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<KeyValuePair<double, double>> cluster = null;
            KeyValuePair<double, double> largestGain = new KeyValuePair<double, double>();
            int peakIndex = 0;
            while (peakIndex < Output.Count) // Removes unwanted and redundant peaks
            {
                double myFreq = GetDictKey(Output, peakIndex);

                if (cluster == null)
                {
                    cluster = new List<KeyValuePair<double, double>>();
                    largestGain = new KeyValuePair<double, double>(myFreq, Output[myFreq]);
                    cluster.Add(largestGain);
                    peakIndex++;
                    continue;
                }
                else if ((myFreq - largestGain.Key) <= largestGain.Key / 100 * Prefs.MAX_FREQ_CHANGE) // Finds clusters of points that represent the same peak
                {
                    cluster.Add(new KeyValuePair<double, double>(myFreq, Output[myFreq]));

                    if (Output[myFreq] > largestGain.Value)
                        largestGain = new KeyValuePair<double, double>(myFreq, Output[myFreq]);

                    if (peakIndex < Output.Count - 1)
                    {
                        peakIndex++;
                        continue;
                    }
                }

                if (cluster.Count > 1) // Keeps only the largest value in the cluster
                {
                    cluster.Remove(largestGain);
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        Output.Remove(cluster[j].Key);
                    }
                    peakIndex -= cluster.Count;
                }
                cluster = null;
            }

            Output = Output.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high
            List<double> discardFreqs = new List<double>();

            for (int i = 0; i < Output.Count - 1; i++) // Removes any unwanted residual peaks after a large peak
            {
                double freqA = GetDictKey(Output, i);
                double freqB = GetDictKey(Output, i + 1);
                if (Math.Abs(Output[freqA] - Output[freqB]) >= Prefs.MAX_GAIN_CHANGE)
                {
                    if (Output[freqA] > Output[freqB]) // Discard lowest value
                        discardFreqs.Add(freqB);
                    else
                        discardFreqs.Add(freqA);
                }
            }

            foreach (double frequency in discardFreqs)
                Output.Remove(frequency);

            Output = Output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
        }

        private double GetDictKey(Dictionary<double, double> dict, int index)
        {
            int i = 0;
            foreach (var key in dict.Keys)
            {
                if (i == index)
                    return key;
                i++;
            }
            return 0;
        }
    }
}
