using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicAnalyser.App.DSP
{
    class SpectrogramFrame
    {
        public double Timestamp { get; set; }
        public byte[] SpectrumData { get; set; }
        public Note[] Notes { get; set; }
        public Chord[] Chords { get; set; }
        public string KeySignature { get; set; }

        public SpectrogramFrame(double timestamp, byte[] data)
        {
            Timestamp = timestamp;
            SpectrumData = data;
        }
    }
}
