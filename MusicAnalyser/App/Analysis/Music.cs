using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.Analysis
{
    public class Music
    {
        private static readonly double CentPercent = 0.05778;
        private static readonly double FundThreshold = 31.78;
        private static readonly double Tolerance = 2;

        private static readonly Dictionary<double, string> NoteMap = new Dictionary<double, string>
        {
            { 32.70, "C" },
            { 34.65, "Db" },
            { 36.71, "D" },
            { 38.89, "Eb" },
            { 41.20, "E" },
            { 43.65, "F" },
            { 46.25, "Gb" },
            { 49.00, "G" },
            { 51.91, "Ab" },
            { 55.00, "A" },
            { 58.27, "Bb" },
            { 61.74, "B" }
        };

        public static readonly string[] Scales = new string[]
        {
            "C", "D", "E", "F", "G", "A", "B" ,
            "Db", "Eb", "F", "Gb", "Ab", "Bb", "C",
            "D", "E", "Gb", "G", "A", "B", "Db",
            "Eb", "F", "G", "Ab", "Bb", "C", "D",
            "E", "Gb", "Ab", "A", "B", "Db", "Eb",
            "F", "G", "A", "Bb", "C", "D", "E",
            "Gb", "Ab", "Bb", "B", "Db", "Eb", "F",
            "G", "A", "B", "C", "D", "E", "Gb",
            "Ab", "Bb", "C", "Db", "Eb", "F", "G",
            "A", "B", "Db", "D", "E", "Gb", "Ab",
            "Bb", "C", "D", "Eb", "F", "G", "A",
            "B", "Db", "Eb", "E", "Gb", "Ab", "Bb"
        };

        public int[] NoteOccurences { get; set; }
        public List<int> NoteBuffer { get; set; }
        public List<int> NoteError { get; set; }
        private double percentChange = 0;

        public Music()
        {
            NoteOccurences = new int[12];
            NoteBuffer = new List<int>();
            NoteError = new List<int>();
        }

        public string GetNote(double freq)
        {
            if (freq < FundThreshold)
                return "N/A";

            int octave = 1;
            if (freq > FundThreshold * 2)
            {
                while (freq > FundThreshold * 2)
                {
                    freq /= 2;
                    octave++;
                }
            }
            foreach(double fundFreq in NoteMap.Keys)
            {
                double fundChange, fundLow, fundHigh;

                fundChange = fundFreq + ((fundFreq / 100) * percentChange);
                fundLow = fundChange - (fundFreq / 100 * Tolerance);
                fundHigh = fundChange + (fundFreq / 100 * Tolerance);

                if (freq >= fundLow && freq <= fundHigh)
                {
                    double errorCents = ((freq - fundChange) / fundChange * 100) / CentPercent;
                    NoteError.Add((int)errorCents);
                    return NoteMap[fundFreq] + octave.ToString();
                }
            }
            return "N/A";
        }

        public static int GetNoteIndex(string noteName)
        {
            string note = noteName.Substring(0, noteName.Length - 1);
            switch (note)
            {
                case "C":
                    return 0;
                case "Db":
                    return 1;
                case "D":
                    return 2;
                case "Eb":
                    return 3;
                case "E":
                    return 4;
                case "F":
                    return 5;
                case "Gb":
                    return 6;
                case "G":
                    return 7;
                case "Ab":
                    return 8;
                case "A":
                    return 9;
                case "Bb":
                    return 10;
                case "B":
                    return 11;
            }
            return 0;
        }

        public void CountNote(string noteName)
        {
            string note = noteName.Substring(0, noteName.Length - 1);
            switch(note)
            {
                case "C":
                    NoteOccurences[0]++;
                    break;
                case "Db":
                    NoteOccurences[1]++;
                    break;
                case "D":
                    NoteOccurences[2]++;
                    break;
                case "Eb":
                    NoteOccurences[3]++;
                    break;
                case "E":
                    NoteOccurences[4]++;
                    break;
                case "F":
                    NoteOccurences[5]++;
                    break;
                case "Gb":
                    NoteOccurences[6]++;
                    break;
                case "G":
                    NoteOccurences[7]++;
                    break;
                case "Ab":
                    NoteOccurences[8]++;
                    break;
                case "A":
                    NoteOccurences[9]++;
                    break;
                case "Bb":
                    NoteOccurences[10]++;
                    break;
                case "B":
                    NoteOccurences[11]++;
                    break;
            }
        }

        public static string GetNoteName(int index)
        {
            return Scales[index * 7];
        }

        public static int[] FindScaleProbability(string[] notes)
        {
            int[] scaleProbs = new int[12];

            for(int i = 0; i < Scales.Length; i += 7)
            {
                string[] scale = new string[7];
                Array.Copy(Scales, i, scale, 0, scale.Length);
                string[] commonNotes = scale.Intersect(notes).ToArray();
                scaleProbs[i / 7] = commonNotes.Length;
            }
            return scaleProbs;
        }

        public static double[] FindTotalScalePercentages(Dictionary<int, double> notePercents)
        {
            double[] scalePercents = new double[12];
            for (int i = 0; i < Scales.Length; i += 7)
            {
                string[] scale = new string[7];
                Array.Copy(Scales, i, scale, 0, scale.Length);
                for (int j = 0; j < scale.Length; j++)
                {
                    int scaleIndex = GetNoteIndex(scale[j] + "0");
                    if (notePercents.ContainsKey(scaleIndex))
                        scalePercents[i / 7] += notePercents[scaleIndex];
                }
            }
            return scalePercents;
        }

        public bool IsMinor(string root, out string minorRoot)
        {
            minorRoot = root;
            string[] scale = new string[7];
            Array.Copy(Scales, GetNoteIndex(root + "0") * 7, scale, 0, scale.Length);

            if(NoteOccurences[GetNoteIndex(scale[5] + "0")] > NoteOccurences[GetNoteIndex(root + "0")])
            {
                minorRoot = scale[5];
                return true;
            }
            return false;
        }

        public static string GetMode(double[] percent, string majorRoot, string minorRoot)
        {
            int majorIndex = GetNoteIndex(majorRoot + "0");
            int minorIndex = GetNoteIndex(minorRoot + "0");
            percent[majorIndex] += Prefs.MODAL_ROOT_DIFF;
            percent[minorIndex] += Prefs.MODAL_ROOT_DIFF;
            double largestPercent = percent[0];
            int largestIndex = 0;
            
            for(int i = 1; i < percent.Length; i++)
            {
                if (percent[i] > largestPercent)
                {
                    largestPercent = percent[i];
                    largestIndex = i;
                }
            }

            string modalRoot = GetNoteName(largestIndex);
            string[] majorScale = new string[7];
            Array.Copy(Scales, majorIndex * 7, majorScale, 0, majorScale.Length);
            int modalInterval = 0;

            for(int i = 1; i < majorScale.Length; i++)
            {
                if (majorScale[i] == modalRoot)
                    modalInterval = i;
            }

            switch(modalInterval)
            {
                case 1:
                    return modalRoot + " Dorian";
                case 2:
                    return modalRoot + " Phrygian";
                case 3:
                    return modalRoot + " Lydian";
                case 4:
                    return modalRoot + " Mixolydian";
                case 6:
                    return modalRoot + " Locrian";
            }
            return "";
        }

        public static string GetChordQuality(List<int> intervals)
        {
            int fifthOmitted = 0;
            while (fifthOmitted <= 1)
            {
                if (intervals.Contains(4) && intervals.Contains(7)) // Major chords
                {
                    intervals.Remove(4);
                    intervals.Remove(7);

                    if (intervals.Contains(2)) // Contains 2/9
                    {
                        intervals.Remove(2);
                        if (intervals.Contains(9)) // Contains 6
                        {
                            intervals.Remove(9);
                            return "6/9" + AddRemainingNotes(intervals);
                        }
                        if (intervals.Contains(10)) // Contains m7
                        {
                            intervals.Remove(10);
                            if (intervals.Contains(5))
                            {
                                intervals.Remove(5);
                                return "11" + AddRemainingNotes(intervals);
                            }
                            return "9" + AddRemainingNotes(intervals);
                        }
                        if (intervals.Contains(11)) // Contains maj7
                        {
                            intervals.Remove(11);
                            if (intervals.Contains(9)) // Contains maj6
                            {
                                intervals.Remove(9);
                                return "maj13" + AddRemainingNotes(intervals);
                            }
                            return "maj9" + AddRemainingNotes(intervals);
                        }
                        return "add9" + AddRemainingNotes(intervals);
                    }

                    if (intervals.Contains(9)) // Contains maj6
                    {
                        intervals.Remove(9);
                        return "6" + AddRemainingNotes(intervals);
                    }

                    if (intervals.Contains(10)) // Contains m7
                    {
                        intervals.Remove(10);
                        if (intervals.Contains(1)) // Contains b2/9
                        {
                            intervals.Remove(1);
                            return "7b9" + AddRemainingNotes(intervals);
                        }

                        if (intervals.Contains(3)) // Contains #2/9
                        {
                            intervals.Remove(3);
                            return "7#9" + AddRemainingNotes(intervals);
                        }
                        return "7" + AddRemainingNotes(intervals);
                    }

                    if (intervals.Contains(11)) // Contains maj7
                    {
                        intervals.Remove(11);
                        return "maj7" + AddRemainingNotes(intervals);
                    }
                    return "" + AddRemainingNotes(intervals);
                }

                if (intervals.Contains(3) && intervals.Contains(7)) // Minor chords
                {
                    intervals.Remove(3);
                    intervals.Remove(7);

                    if (intervals.Contains(9)) // Contains 6
                    {
                        intervals.Remove(9);
                        return "m6" + AddRemainingNotes(intervals);
                    }

                    if (intervals.Contains(10)) // Contains m7
                    {
                        intervals.Remove(10);
                        if (intervals.Contains(2)) // Contains 2/9
                        {
                            intervals.Remove(2);
                            return "m9" + AddRemainingNotes(intervals);
                        }
                        return "m7" + AddRemainingNotes(intervals);
                    }

                    if (intervals.Contains(11)) // Contains maj7
                    {
                        intervals.Remove(11);
                        return "mM7" + AddRemainingNotes(intervals);
                    }
                    return "m" + AddRemainingNotes(intervals);
                }

                if (intervals.Contains(2) && intervals.Contains(7)) // Suspended 2 chords
                {
                    intervals.Remove(2);
                    intervals.Remove(7);

                    if (intervals.Contains(10)) // Contains m7
                    {
                        intervals.Remove(10);
                        if (intervals.Contains(2)) // Contains 2/9
                        {
                            intervals.Remove(2);
                            if (intervals.Contains(5))
                            {
                                intervals.Remove(5);
                                return "9sus2" + AddRemainingNotes(intervals);
                            }
                            return "7sus2" + AddRemainingNotes(intervals);
                        }
                    }
                    return "sus2" + AddRemainingNotes(intervals);
                }

                if (intervals.Contains(5) && intervals.Contains(7)) // Suspended 4 chords
                {
                    intervals.Remove(5);
                    intervals.Remove(7);

                    if (intervals.Contains(10)) // Contains m7
                    {
                        intervals.Remove(10);
                        if (intervals.Contains(2)) // Contains 2/9
                        {
                            intervals.Remove(2);
                            if (intervals.Contains(5))
                            {
                                intervals.Remove(5);
                                return "9sus4" + AddRemainingNotes(intervals);
                            }
                            return "7sus4" + AddRemainingNotes(intervals);
                        }
                    }
                    return "sus4" + AddRemainingNotes(intervals);
                }

                if (intervals.Contains(3) && intervals.Contains(6)) // Diminished chords
                {
                    intervals.Remove(3);
                    intervals.Remove(6);

                    if (intervals.Contains(9)) // Contains 6
                    {
                        intervals.Remove(9);
                        return "dim7" + AddRemainingNotes(intervals);
                    }
                    if (intervals.Contains(10)) // Contains m7
                    {
                        intervals.Remove(10);
                        return "m7b5" + AddRemainingNotes(intervals);
                    }
                    return "dim" + AddRemainingNotes(intervals);
                }

                if (intervals.Contains(4) && intervals.Contains(8)) // Augmented chords
                {
                    intervals.Remove(4);
                    intervals.Remove(8);
                    return "aug" + AddRemainingNotes(intervals);
                }

                if(intervals.Contains(7) && intervals.Count == 1 && fifthOmitted == 0) // 5 chord/power chord
                {
                    return "5";
                }

                intervals.Add(7);
                fifthOmitted += 1;
            }
            return "N/A";
        }

        private static string AddRemainingNotes(List<int> intervals)
        {
            if (intervals.Count == 0)
                return "";

            string remainder = " (";
            for(int i = 0; i < intervals.Count; i++)
            {
                switch(intervals[i])
                {
                    case 1:
                        remainder += "b9";
                        break;
                    case 2:
                        remainder += "9";
                        break;
                    case 3:
                        remainder += "#9";
                        break;
                    case 4:
                        remainder += "b11";
                        break;
                    case 5:
                        remainder += "11";
                        break;
                    case 6:
                        remainder += "#11";
                        break;
                    case 8:
                        remainder += "#5";
                        break;
                    case 9:
                        remainder += "6";
                        break;
                    case 10:
                        remainder += "m7";
                        break;
                    case 11:
                        remainder += "maj7";
                        break;
                }
                if(i < intervals.Count - 1)
                    remainder += ", ";
            }
            return remainder + ")";
        }

        public void ResetNoteCount()
        {
            NoteOccurences = new int[12];
            NoteBuffer.Clear();
            NoteError.Clear();
        }

        public void SetTuningPercent(int value)
        {
            percentChange = CentPercent * value;
        }

        public double GetTuningPercent() { return percentChange; }

        public void DisposeMusic()
        {
            ResetNoteCount();
            GC.Collect();
        }
    }
}
