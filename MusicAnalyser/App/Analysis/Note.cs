using System;

namespace MusicAnalyser.App.Analysis
{
    [Serializable]
    public class Note : ICloneable
    {
        public string Name { get; set; }
        public int Octave { get; set; }
        public int NoteIndex { get; set; }
        public double Frequency { get; set; }
        public double Magnitude { get; set; }
        public double Position { get; set; }
        public int TimeStamp { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
