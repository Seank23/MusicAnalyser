using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    public interface ISignalProcessor
    {
        bool IsPrimary { get; }
        Dictionary<string, string[]> Settings { get; set; } // FIELD_NAME, { Value, Type, Display Name, Min, Max }
        object InputBuffer { get; set; }
        int SampleRate { get; set; }
        object OutputBuffer { get; set; }
        object OutputScale { get; set; }

        void Process();
    }
}
