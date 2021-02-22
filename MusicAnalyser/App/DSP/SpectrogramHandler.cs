using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicAnalyser.App.DSP
{
    public class SpectrogramHandler
    {
        public List<SpectrogramFrame> Frames { get; set; }
        public object FrequencyScale { get; set; }
        public int FrequencyBins { get; set; }

        public SpectrogramHandler()
        {
            Frames = new List<SpectrogramFrame>();
        }

        public void CreateFrame(double timestamp, byte[] data)
        {
            Frames.Add(new SpectrogramFrame(timestamp, data));
            if (FrequencyBins == 0)
                FrequencyBins = data.Length;
        }

        public void AddAnalysis(double timestamp, Note[] notes, Chord[] chords, string key)
        {
            SpectrogramFrame myFrame = Frames.Where(frame => frame.Timestamp == timestamp).First();
            myFrame.Notes = notes;
            myFrame.Chords = chords;
            myFrame.KeySignature = key;
        }

        public void Clear()
        {
            Frames.Clear();
            FrequencyScale = null;
        }
    }
}
