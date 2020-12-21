using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.Linq;

class CQTByMagnitudeDetector : ISignalDetector
{
    public bool IsPrimary { get { return true; } }
    public Dictionary<string, string[]> Settings { get; set; }
    public object InputData { get; set; }
    public Dictionary<string, object> InputArgs { get; set; }
    public object Output { get; set; }
    public Dictionary<string, object> OutputArgs { get; set; }

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
        if (InputArgs.ContainsKey("SCALE"))
        {
            if (InputArgs["SCALE"].GetType().Name == "Func`2")
                scale = (Func<int, double>)InputArgs["SCALE"];
        }
        if (input == null || scale == null)
            return;

        double threshold = input.Average() + (input.Max() - input.Average()) * double.Parse(Settings["THRESHOLD_FACTOR"][0]);
        double minFreq = scale(0);
        int binsPerNote = 1;
        while (scale(binsPerNote) < minFreq * 2)
            binsPerNote++;
        binsPerNote /= 12;

        int binsToRead = (int)Math.Floor((double)(binsPerNote - 1) / 2);
        Dictionary<double, double> output = new Dictionary<double, double>();
        List<double> positions = new List<double>();

        for(int i = 0; i < input.Length; i += binsPerNote)
        {
            double freq = scale(i);
            double avgComponent = 0;
            for(int j = -binsToRead; j <= binsToRead; j++)
            {
                if (i + j >= 0)
                    avgComponent += input[i + j];
            }
            avgComponent /= binsToRead * 2 + 1;
            if (avgComponent > threshold)
            {
                output.Add(freq, avgComponent);
                positions.Add(i);
            } 
        }
        Output = output;
        OutputArgs.Add("POSITIONS", positions.ToArray());
    }
    
}

