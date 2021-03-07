using MusicAnalyser.App.Analysis;
using System;

namespace MusicAnalyser.App.Spectrogram
{
    [Serializable]
    public class SpectrogramFrame
    {
        public double Timestamp { get; set; }
        public byte[] SpectrumData { get; set; }
        public Note[] Notes { get; set; }
        public Chord[] Chords { get; set; }
        public string KeySignature { get; set; }
        public double QuantisationScale { get; set; }

        public SpectrogramFrame(double timestamp, byte[] data, Note[] notes, Chord[] chords, string key, double quantScale = 1)
        {
            Timestamp = timestamp;
            SpectrumData = data;
            Notes = notes;
            Chords = chords;
            KeySignature = key;
            QuantisationScale = quantScale;
        }
    }
}
