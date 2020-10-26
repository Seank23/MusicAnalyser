using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    public interface ISignalDetector
    {
        Dictionary<string, string[]> Settings { get; set; } // FIELD_NAME, { Value, Type, Display Name, Min, Max }
        double[] InputData { get; set; }
        double InputScale { get; set; }
        Dictionary<double, double> Output { get; set; }

        void Detect();
    }
}
