using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    public interface ISignalProcessor
    {
        bool IsPrimary { get; }
        Dictionary<string, string[]> Settings { get; set; } // FIELD_NAME, { Value, Type, Display Name, Min, Max }
        object InputBuffer { get; set; }
        Dictionary<string, object> InputArgs { get; set; }
        object OutputBuffer { get; set; }
        Dictionary<string, object> OutputArgs { get; set; }

        void OnSettingsChange();
        void Process();
    }
}
