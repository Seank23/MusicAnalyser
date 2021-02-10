using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicAnalyser.App.DSP
{
    class SpectrogramHandler
    {
        public List<SpectrogramFrame> Frames { get; set; }

        public SpectrogramHandler()
        {
            Frames = new List<SpectrogramFrame>();
        }

        public void CreateFrame(double timestamp, byte[] data)
        {
            Frames.Add(new SpectrogramFrame(timestamp, data));
            Console.WriteLine(Frames.Count);
        }

        public void AddAnalysis(double timestamp, Note[] notes, Chord[] chords, string key)
        {
            SpectrogramFrame myFrame = Frames.Where(frame => frame.Timestamp == timestamp).First();
            myFrame.Notes = notes;
            myFrame.Chords = chords;
            myFrame.KeySignature = key;
        }

        public void Dispose()
        {
            Frames.Clear();
        }
    }
}
