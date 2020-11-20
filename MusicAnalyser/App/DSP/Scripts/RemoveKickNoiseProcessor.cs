using System;
using System.Collections.Generic;
using System.Linq;
using MusicAnalyser.App.DSP;

class RemoveKickNoiseProcessor : ISignalProcessor
{
    public bool IsPrimary { get { return false; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputBuffer { get; set; }
    public int SampleRate { get; set; }
    public object OutputBuffer { get; set; }
    public object OutputScale { get; set; }

    public RemoveKickNoiseProcessor()
    {
        Settings = new Dictionary<string, string[]> 
        {
            { "CUTOFF_FREQ", new string[] { "200", "double", "Cutoff Frequency (Hz)", "0", "1000" } },
            { "MAX_FREQ_CHANGE", new string[] { "2.5", "double", "Max Frequency Change (Hz)", "0", "50" } },
            { "SIMILAR_GAIN_THRESHOLD", new string[] { "5", "double", "Similar Gain Threshold (dB)", "0", "50" } },
        };
    }

    public void OnSettingsChange() { }

    public void Process()
    {
        Dictionary<double, double> input = null;
        if (InputBuffer.GetType().Name == "Dictionary`2")
            input = (Dictionary<double, double>)InputBuffer;
        if (input == null)
            return;

        List<double> discardFreqs = new List<double>();
        double prevFreq = 0;
        input = input.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high

        foreach (double freq in input.Keys)
        {
            if (freq > double.Parse(Settings["CUTOFF_FREQ"][0]))
                break;
            if (prevFreq == 0)
            {
                prevFreq = freq;
                continue;
            }
            if ((freq - prevFreq) <= freq / 100 * (2.5 * double.Parse(Settings["MAX_FREQ_CHANGE"][0]))) // Checking for consecutive, closely packed peaks - noise
            {
                if (Math.Abs(input[freq] - input[prevFreq]) <= double.Parse(Settings["SIMILAR_GAIN_THRESHOLD"][0]))
                {
                    if (!discardFreqs.Contains(prevFreq))
                        discardFreqs.Add(prevFreq);
                    discardFreqs.Add(freq);
                }
            }
            prevFreq = freq;
        }
        foreach (double frequency in discardFreqs)
            input.Remove(frequency);

        OutputBuffer = input;
    }
}

