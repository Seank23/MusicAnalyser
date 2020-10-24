using System;
using System.Collections.Generic;
using System.Linq;
using MusicAnalyser.App.DSP;

class RemoveKickNoiseProcessor : ISignalProcessor
{
    static class Settings
    {
        public static float MAX_FREQ_CHANGE = 2.8f;
        public static float SIMILAR_GAIN_THRESHOLD = 5.0f;
    }

    public object InputBuffer { get; set; }
    public int SampleRate { get; set; }
    public object OutputBuffer { get; set; }
    public double OutputScale { get; set; }

    public void Process()
    {
        Dictionary<double, double> input = (Dictionary<double, double>)InputBuffer;
        List<double> discardFreqs = new List<double>();
        double prevFreq = 0;
        input = input.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value); // Order: Frequency - low to high

        foreach (double freq in input.Keys)
        {
            if (freq > 200)
                break;
            if (prevFreq == 0)
            {
                prevFreq = freq;
                continue;
            }
            if ((freq - prevFreq) <= freq / 100 * (2.5 * Settings.MAX_FREQ_CHANGE)) // Checking for consecutive, closely packed peaks - noise
            {
                if (Math.Abs(input[freq] - input[prevFreq]) <= Settings.SIMILAR_GAIN_THRESHOLD)
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

