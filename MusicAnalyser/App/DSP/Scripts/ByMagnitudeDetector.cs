using System;
using System.Collections.Generic;
using System.Linq;
using MusicAnalyser.App.DSP;

class ByMagnitudeDetector : ISignalDetector
{
    public Dictionary<string, string[]> Settings { get; set; }
    public double[] InputData { get; set; }
    public double InputScale { get; set; }
    public Dictionary<double, double> Output { get; set; }
    
    public ByMagnitudeDetector()
    {
        Settings = new Dictionary<string, string[]>
        {
            { "MIN_FREQ", new string[] { "30", "int", "Min Frequency (Hz)", "0", "20000" } },
            { "MAX_FREQ", new string[] { "2000", "int", "Max Frequency (Hz)", "0", "20000" } },
            { "THOLD_FROM_AVG", new string[] { "25", "int", "Gain Threshold (from Avg) (dB)", "-50", "50" } },
            { "PEAK_BUFFER", new string[] { "90", "int", "Spectrum Peak Buffer Size", "0", "500" } },
            { "MAX_GAIN_CHANGE", new string[] { "8", "double", "Max Gain Change (dB)", "0", "50" } },
            { "MAX_FREQ_CHANGE", new string[] { "2.8", "double", "Max Frequency Change (Hz)", "0", "50" } },
        };
    }

    public void Detect()
    {
        Output = new Dictionary<double, double>();
        double freq;
        double gainThreshold = InputData.Average() + int.Parse(Settings["THOLD_FROM_AVG"][0]);

        // Iterates through frequency data, storing the frequency and gain of the largest frequency bins 
        for (int i = (int)(InputScale * int.Parse(Settings["MIN_FREQ"][0])); i < Math.Min(InputData.Length, (int)(InputScale * int.Parse(Settings["MAX_FREQ"][0]))); i++)
        {
            if (InputData[i] > gainThreshold)
            {
                freq = (i + 1) / InputScale; // Frequency value of bin

                Output.Add(freq, InputData[i]);

                if (Output.Count > int.Parse(Settings["PEAK_BUFFER"][0])) // When fftPeaks overflows, remove smallest frequency bin
                {
                    Output = Output.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); // Order: Gain - high to low
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
            else if ((myFreq - largestGain.Key) <= largestGain.Key / 100 * double.Parse(Settings["MAX_FREQ_CHANGE"][0])) // Finds clusters of points that represent the same peak
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
            if (Math.Abs(Output[freqA] - Output[freqB]) >= double.Parse(Settings["MAX_GAIN_CHANGE"][0]))
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

