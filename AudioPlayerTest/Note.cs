using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser
{
    public class Note
    {
        public string Name { get; set; }
        public int Octave { get; set; }
        public int NoteIndex { get; set; }
        public double Frequency { get; set; }
        public double Magnitude { get; set; }
        public int TimeStamp { get; set; }
    }
}
