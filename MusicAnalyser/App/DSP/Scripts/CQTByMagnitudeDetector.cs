using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;

class CQTByMagnitudeDetector : ISignalDetector
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputData { get; set; }
    public object InputScale { get; set; }
    public object Output { get; set; }
    public double[] OutputPosition { get; set; }

    public CQTByMagnitudeDetector()
    {
        Settings = new Dictionary<string, string[]>()
        {
            { "THRESHOLD_FACTOR", new string[] { "0.3", "double", "Threshold Factor", "0", "1" } },
        };
    }

    public void OnSettingsChange() { }

    public void Detect()
    {
        double[] input = null;
        Func<int, double> scale = null;
        if (InputData.GetType().Name == "Double[]")
            input = (double[])InputData;
        if (InputScale.GetType().Name == "Func`2")
            scale = (Func<int, double>)InputScale;
        if (input == null || scale == null)
            return;

        double threshold = input.Average() + (input.Max() - input.Average()) * double.Parse(Settings["THRESHOLD_FACTOR"][0]);
        double minFreq = scale(0);
        int binsPerNote = 1;
        while (scale(binsPerNote) < minFreq * 2)
            binsPerNote++;
        binsPerNote /= 12;

        Dictionary<double, double> output = new Dictionary<double, double>();
        List<double> positions = new List<double>();

        for(int i = 0; i < input.Length; i += binsPerNote)
        {
            double freq = scale(i);
            if (input[i] > threshold)
            {
                output.Add(freq, input[i]);
                positions.Add(i);
            } 
        }
        Output = output;
        OutputPosition = positions.ToArray();
    }
    
}

