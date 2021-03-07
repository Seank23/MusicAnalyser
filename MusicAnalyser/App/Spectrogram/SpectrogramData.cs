using System;
using System.Collections.Generic;

namespace MusicAnalyser.App.Spectrogram
{
    [Serializable]
    public class SpectrogramData
    {
        public List<SpectrogramFrame> Frames { get; set; }
        public object FrequencyScale { get; set; }
        public int FrequencyBins { get; set; }
        public string AudioFilename { get; set; }
        public Dictionary<string, string[]> ScriptProperties { get; set; }

        public SpectrogramData()
        {
            Frames = new List<SpectrogramFrame>();
        }
    }
}
