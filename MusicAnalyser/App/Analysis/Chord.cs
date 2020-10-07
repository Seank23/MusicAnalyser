using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.Analysis
{
    class Chord
    {
        public string Name { get; set; }
        public string Root { get; set; }
        public string Quality { get; set; }
        public List<Note> Notes { get; set; }
        public int NumExtensions { get; set; }
        public double Probability { get; set; }
    }
}
