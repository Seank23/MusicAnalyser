using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser
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
                double fundLow, fundHigh;
                if (percentChange != 0)
                {
                    fundLow = fundFreq + (fundFreq / 100 * percentChange) - (fundFreq / 100 * Tolerance);
                    fundHigh = fundFreq + (fundFreq / 100 * percentChange) + (fundFreq / 100 * Tolerance);
                }
                else
                {
                    fundLow = fundFreq - (fundFreq / 100 * Tolerance);
                    fundHigh = fundFreq + (fundFreq / 100 * Tolerance);
                }
                if (freq >= fundLow && freq <= fundHigh)
                {
                    double errorCents = ((freq - fundFreq) / fundFreq * 100) / CentPercent;
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

        public static int[] FindScale(string[] notes)
        {
            int[] scaleProbs = new int[12];

            for(int i = 0; i < Scales.Length; i += 7)
            {
                string[] scale = new string[7];
                Array.Copy(Scales, i, scale, 0, scale.Length);

                for (int j = 0; j < notes.Length; j++)
                {
                    if(Array.Exists(scale, element => element == notes[j]))
                    {
                        scaleProbs[GetNoteIndex(scale[0] + "0")]++;
                    }
                }
            }
            return scaleProbs;
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

        public static string GetChordQuality(string interval)
        {
            int notesOmitted = 0;
            string star = "";
            while (notesOmitted <= 1)
            {
                switch (interval)
                {
                    case "47":
                        return "" + star;
                    case "37":
                        return "m" + star;
                    case "4710":
                        return "7" + star;
                    case "3710":
                        return "m7" + star;
                    case "4711":
                        return "maj7" + star;
                    case "3711":
                        return "mM7" + star;
                    case "4610":
                        return "7b5" + star;
                    case "4810":
                        return "7#5" + star;
                    case "3610":
                        return "7b5" + star;
                    case "14710":
                        return "7b9" + star;
                    case "46":
                        return "b5" + star;
                    case "7":
                        return "5" + star;
                    case "479":
                        return "6" + star;
                    case "379":
                        return "m6" + star;
                    case "2479":
                        return "6/9" + star;
                    case "24710":
                        return "9" + star;
                    case "24610":
                        return "9b5" + star;
                    case "24810":
                        return "9#5" + star;
                    case "23710":
                        return "m9" + star;
                    case "24711":
                        return "maj9" + star;
                    case "247":
                        return "add9" + star;
                    case "34710":
                        return "7#9" + star;
                    case "245710":
                        return "11" + star;
                    case "257910":
                        return "13" + star;
                    case "247911":
                        return "maj13" + star;
                    case "27":
                        return "sus2" + star;
                    case "57":
                        return "sus4" + star;
                    case "5710":
                        return "7sus4" + star;
                    case "25710":
                        return "9sus4" + star;
                    case "36":
                        return "dim" + star;
                    case "369":
                        return "dim7" + star;
                    case "48":
                        return "aug" + star;
                    default:
                        break;
                }
                if (!interval.Contains('7'))
                {
                    string temp = "";
                    for (int i = 0; i < interval.Length - 1; i++)
                    {
                        temp += interval[i];
                        if (Convert.ToInt32(interval[i].ToString()) < 7 && Convert.ToInt32(interval[i + 1].ToString()) > 7)
                            temp += 7;
                    }
                    if(interval.Length > 0)
                        temp += interval[interval.Length - 1];
                    
                    interval = temp;
                    notesOmitted++;
                    star = "*";
                }
                else
                    break;
            }
            return "N/A";
        }

        public void ResetNoteCount()
        {
            NoteOccurences = new int[12];
            NoteBuffer.Clear();
            NoteError.Clear();
        }

        public void GetPercentChange(int value)
        {
            percentChange = CentPercent * value;
        }

        public void DisposeMusic()
        {
            ResetNoteCount();
            GC.Collect();
        }
    }
}
