using System;
using System.Collections.Generic;

namespace MusicAnalyser.App.Analysis
{
    [Serializable]
    public class Chord
    {
        public string Name { get; set; }
        public string Root { get; set; }
        public string Quality { get; set; }
        public List<Note> Notes { get; set; }
        public int NumExtensions { get; set; }
        public int FifthOmitted { get; set; }
        public double Probability { get; set; }
    }
}
