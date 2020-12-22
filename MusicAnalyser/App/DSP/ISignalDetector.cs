﻿using System.Collections.Generic;

namespace MusicAnalyser.App.DSP
{
    public interface ISignalDetector
    {
        bool IsPrimary { get; }
        Dictionary<string, string[]> Settings { get; set; } // FIELD_NAME, { Value, Type, Display Name, Min, Max }
        object InputData { get; set; }
        Dictionary<string, object> InputArgs { get; set; }
        object Output { get; set; }
        Dictionary<string, object> OutputArgs { get; set; }

        void OnSettingsChange();
        void Detect();
    }
}
