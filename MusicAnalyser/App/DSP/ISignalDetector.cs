using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    public interface ISignalDetector
    {
        double[] InputData { get; set; }
        double InputScale { get; set; }
        Dictionary<double, double> Output { get; set; }

        void Detect();
    }
}
