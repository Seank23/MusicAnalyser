using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.Analysis
{
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
