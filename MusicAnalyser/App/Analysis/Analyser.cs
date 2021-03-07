using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.Analysis
{
    public class Analyser
    {
        public List<Note> Notes { get; set; }
        public List<Chord> Chords { get; set; }
        public string CurrentKey { get; set; }
        public string CurrentMode { get; set; }

        private Music music;
        private static List<Note> aggregateNotes = new List<Note>();
        private static List<Note>[] chordNotes;
        private static double[] notePercent = new double[12];
        private List<int> avgError = new List<int>();
        private static readonly Color[] noteColors = new Color[12];
        private Chord prevChord;
        private string majorKeyRoot;

        public Analyser()
        {
            music = new Music();
            Chords = new List<Chord>();
        }

        public List<int> GetAvgError() { return avgError; }
        public Music GetMusic() { return music; }
        public List<Note>[] GetChordNotes() { return chordNotes; }
        public void ResetError() { avgError.Clear(); }
        public double[] GetNotePercents() { return notePercent; }
        public Color[] GetNoteColors() { return noteColors; }

        /*
         * Identifies valid notes from the peaks in the frequency spectrum and plots them
         */
        public void GetNotes(Dictionary<double, double> fftPeaks, double[] positions, int timeStamp)
        {
            music.NoteError = new List<int>();
            Notes = new List<Note>();

            if (fftPeaks == null)
                return;

            int i = 0;
            foreach (double freq in fftPeaks.Keys)
            {
                string noteName = music.GetNote(freq);

                if (noteName == "N/A") // Ignore peaks that are not valid notes
                    continue;

                Note myNote;
                if (positions == null || positions.Length == 0)
                    myNote = CreateNote(noteName, freq, fftPeaks[freq], freq, timeStamp);
                else
                    myNote = CreateNote(noteName, freq, fftPeaks[freq], positions[i], timeStamp);
                aggregateNotes.Add(myNote);
                Notes.Add(myNote);
                music.CountNote(noteName);
                BufferNote(myNote.NoteIndex);
                i++;
            }
            CalculateNotePercentages();

            if (music.NoteError.Count > 0)
                avgError.Add((int)music.NoteError.Average());
        }

        /*
       * Calculates a color dynamically based on the actualValue in relation to the specified range of values
       */
        public static Color GetNoteColor(int rangeStart, int rangeEnd, int actualValue)
        {
            if (rangeStart >= rangeEnd) return Color.Black;

            actualValue = Math.Min(actualValue, rangeEnd);
            int max = rangeEnd - rangeStart;
            int value = actualValue - rangeStart;

            int blue = 0;
            int green = Math.Min(255 * value / (max / 2), 255);
            int red = 0;
            if (value > max / 2)
            {
                blue = Math.Min(value - (max / 2), 255);
                green = 255 - blue;
                red = 0;
            }
            else
                red = 255 - green;

            return Color.FromArgb((byte)red, (byte)green, (byte)blue);
        }

        public void CalculateNotePercentages()
        {
            for(int i = 0; i < music.NoteOccurences.Length; i++)
            {
                int occurences = music.NoteOccurences[i];
                double percent = ((double)occurences / (double)music.NoteBuffer.Count) * 100;
                notePercent[i] = percent;
                Color noteColor = GetNoteColor(0, (int)(10000 / 7), (int)(percent * 100));
                noteColors[i] = noteColor; 
            }
        }

        private Note CreateNote(string name, double freq, double gain, double position, int timeStamp)
        {
            Note myNote = new Note();
            myNote.Name = name.Substring(0, name.Length - 1);
            myNote.Octave = Convert.ToInt32(name.Substring(name.Length - 1, 1));
            myNote.NoteIndex = Music.GetNoteIndex(name);
            myNote.Frequency = freq;
            myNote.Magnitude = gain;
            myNote.Position = position;
            myNote.TimeStamp = timeStamp;
            return myNote;
        }

        /*
         * Handles the note buffer
         */
        public void BufferNote(int noteIndex)
        {
            music.NoteBuffer.Add(noteIndex);
            if (music.NoteBuffer.Count > Prefs.NOTE_BUFFER_SIZE)
            {
                int noteToRemove = music.NoteBuffer[0];
                music.NoteOccurences[noteToRemove]--;
                music.NoteBuffer.RemoveAt(0);
            }
        }

        /*
         * Predicts the key signature of the audio based on the distrubution of notes identified
         */
        public void FindKey()
        {
            double[] percents = new double[notePercent.Length];
            Array.Copy(notePercent, percents, percents.Length);
            Dictionary<int, double> noteDict = new Dictionary<int, double>();

            for (int i = 0; i < percents.Length; i++)
                noteDict.Add(i, percents[i]);

            double[] keyPercents = Music.FindTotalScalePercentages(noteDict);
            var maxPercent = keyPercents.Select((n, i) => (Number: n, Index: i)).Max();

            string keyRoot = Music.GetNoteName(maxPercent.Index);
            if (music.IsMinor(keyRoot, out string minorRoot)) // Checks if key is most likely the relative minor of original prediction
                CurrentKey = minorRoot + " Minor";
            else
                CurrentKey = keyRoot + " Major";

            majorKeyRoot = keyRoot;
            CurrentMode = Music.GetMode(notePercent, keyRoot, minorRoot);
        }

        public bool FindChordsNotes()
        {
            if (aggregateNotes.Count == 0)
                return false;

            // UNUSED
            //List<Note>[] notesByName = new List<Note>[12];
            //for (int i = 0; i < 12; i++)
            //    notesByName[i] = new List<Note>();
            //Dictionary<string, double> noteDistributionDict = new Dictionary<string, double>();

            //for(int i = 0; i < aggregateNotes.Count; i++)
            //{
            //    string noteName = aggregateNotes[i].Name;
            //    double noteMag = aggregateNotes[i].Magnitude;
            //    int index = aggregateNotes[i].NoteIndex;
            //    notesByName[index].Add(aggregateNotes[i]);
            //    if (noteDistributionDict.ContainsKey(noteName))
            //        noteDistributionDict[noteName] += noteMag;
            //    else
            //        noteDistributionDict.Add(noteName, noteMag);
            //}
            //string[] keys = noteDistributionDict.Keys.ToArray();
            //foreach (string key in keys)
            //    noteDistributionDict[key] /= Prefs.CHORD_DETECTION_INTERVAL;

            //noteDistributionDict = noteDistributionDict.OrderByDescending(x => x.Value).Take(4).ToDictionary(x => x.Key, x => x.Value);

            //chordNotes = new List<Note>[noteDistributionDict.Count];
            //keys = noteDistributionDict.Keys.ToArray();
            //for (int i = 0; i < chordNotes.Length; i++)
            //{
            //    chordNotes[i] = new List<Note>();
            //    List<int> octaves = new List<int>();
            //    int noteIndex = Music.GetNoteIndex(keys[i] + "0");
            //    for (int j = 0; j < notesByName[noteIndex].Count; j++)
            //    {
            //        if (!octaves.Contains(notesByName[noteIndex][j].Octave)) // Stores each prominent note in collected notes only once
            //        {
            //            chordNotes[i].Add(notesByName[noteIndex][j]);
            //            octaves.Add(notesByName[noteIndex][j].Octave);
            //        }
            //    }
            //    chordNotes[i] = chordNotes[i].OrderBy(x => x.Frequency).ToList(); // Order: Frequency - low to high
            //}
            //aggregateNotes.Clear();
            //return true;

            int[,] tempNoteOccurences = new int[12, 2]; // Note index, timestamp
            List<Note>[] notesByName = new List<Note>[12];
            for (int i = 0; i < 12; i++)
                notesByName[i] = new List<Note>();

            int initialTimeStamp = aggregateNotes[0].TimeStamp;
            int timeStampOffset = 0;

            for (int i = 0; i < aggregateNotes.Count; i++)
            {
                int index = aggregateNotes[i].NoteIndex;
                notesByName[index].Add(aggregateNotes[i]);

                if (tempNoteOccurences[index, 1] != initialTimeStamp + timeStampOffset)
                {
                    tempNoteOccurences[index, 0]++;
                    tempNoteOccurences[index, 1] = aggregateNotes[i].TimeStamp;
                }
                timeStampOffset = aggregateNotes[i].TimeStamp - initialTimeStamp;
            }
            aggregateNotes.Clear();

            List<int> chordNoteIndexes = new List<int>();
            for (int i = 0; i < tempNoteOccurences.Length / 2; i++)
            {
                if (tempNoteOccurences[i, 0] >= Prefs.CHORD_NOTE_OCCURENCE_OFFSET) // Prunes spurious notes
                    chordNoteIndexes.Add(i);
            }

            chordNotes = new List<Note>[chordNoteIndexes.Count];
            for (int i = 0; i < chordNotes.Length; i++)
            {
                chordNotes[i] = new List<Note>();
                List<int> octaves = new List<int>();
                for (int j = 0; j < notesByName[chordNoteIndexes[i]].Count; j++)
                {
                    if (!octaves.Contains(notesByName[chordNoteIndexes[i]][j].Octave)) // Stores each prominent note in collected notes only once
                    {
                        chordNotes[i].Add(notesByName[chordNoteIndexes[i]][j]);
                        octaves.Add(notesByName[chordNoteIndexes[i]][j].Octave);
                    }
                }
                chordNotes[i] = chordNotes[i].OrderBy(x => x.Frequency).ToList(); // Order: Frequency - low to high
            }
            return true;
        }

        /*
         *  Finds all possible chords from sequence of chord notes
         */
        public void FindChords()
        {
            Chords.Clear();
            List<Note> myChordNotes = new List<Note>();
            for (int i = 0; i < chordNotes.Length; i++) // Creates iterable chord notes list
            {
                double avgMagnitude = 0;
                for(int j = 0; j < chordNotes[i].Count; j++) // Finds average magnitude of that note type and assigns it to magnitude of chord note
                    avgMagnitude += chordNotes[i][j].Magnitude;
                avgMagnitude /= chordNotes[i].Count;

                Note newChordNote = (Note)chordNotes[i][0].Clone();
                newChordNote.Magnitude = avgMagnitude;
                newChordNote.Octave = chordNotes[i].Count;
                myChordNotes.Add(newChordNote);
            }

            for (int i = 0; i < chordNotes.Length; i++)
            {
                List<int> intervals = new List<int>();
                for (int j = 1; j < chordNotes.Length; j++) // Finds interval between adjacent notes
                {
                    int noteDifference = myChordNotes[j].NoteIndex - myChordNotes[0].NoteIndex;
                    if (noteDifference < 0)
                        noteDifference = 12 + noteDifference;
                    intervals.Add(noteDifference);
                }
                string chordQuality = Music.GetChordQuality(intervals, out int fifthOmitted); // Determines chord quality from intervals
                if (chordQuality != "N/A")
                    Chords.Add(CreateChord(myChordNotes[0].Name, chordQuality, myChordNotes, fifthOmitted));

                myChordNotes = NextChord(myChordNotes); // Iterates chord root note
            }
            AdjustChordProbabilities();
            Chords = Chords.OrderByDescending(x => x.Probability).ToList();
            if(Chords.Count > 0)
                prevChord = Chords[0];
        }

        private Chord CreateChord(string root, string quality, List<Note> notes, int fifthOmitted)
        {
            Note[] tempNotes = new Note[notes.Count];
            Array.Copy(notes.ToArray(), tempNotes, notes.Count);
            int numExtensions = 0;
            if(quality.Contains("("))
            {
                string extensions = quality.Substring(quality.IndexOf("(") + 1);
                numExtensions = extensions.Count(f => f == ',') + 1;
            }
            Chord myChord = new Chord
            {
                Name = root + quality,
                Root = root,
                Quality = quality,
                Notes = tempNotes.ToList(),
                NumExtensions = numExtensions,
                FifthOmitted = fifthOmitted
            };
            return myChord;
        }

        private void AdjustChordProbabilities()
        {
            if (Chords.Count == 0)
                return;

            double[] rootMagnitudes = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                rootMagnitudes[i] = Chords[i].Notes[0].Magnitude;
            double avgMagnitude = rootMagnitudes.Average();
            for (int i = 0; i < rootMagnitudes.Length; i++)
                rootMagnitudes[i] -= avgMagnitude;
            rootMagnitudes = Normalise(rootMagnitudes);

            double[] rootOccurences = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                rootOccurences[i] = Chords[i].Notes[0].Octave;
            double avgOccurences = rootOccurences.Average();
            for (int i = 0; i < rootOccurences.Length; i++)
                rootOccurences[i] -= avgOccurences;
            rootOccurences = Normalise(rootOccurences);

            double[] rootFreq = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                rootFreq[i] = Chords[i].Notes[0].Frequency;
            double avgFreq = rootFreq.Average();
            for (int i = 0; i < rootFreq.Length; i++)
                rootFreq[i] = avgFreq - rootFreq[i];
            rootFreq = Normalise(rootFreq);

            double[] chordExtensions = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                chordExtensions[i] = Chords[i].NumExtensions;
            double avgExtensions = chordExtensions.Average();
            for (int i = 0; i < chordExtensions.Length; i++)
                chordExtensions[i] = avgExtensions - chordExtensions[i];
            chordExtensions = Normalise(chordExtensions);

            double[] notSuspended = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
            {
                if (Chords[i].Name.Contains("sus"))
                    notSuspended[i] = -1;
                else
                    notSuspended[i] = 1;
            }

            double[] fifthOmitted = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                fifthOmitted[i] = Chords[i].FifthOmitted;
            fifthOmitted = Normalise(fifthOmitted);

            double[] chordPredictedBefore = new double[Chords.Count];
            if (prevChord != null)
            {
                for (int i = 0; i < Chords.Count; i++)
                {
                    if (Chords[i].Root == prevChord.Root)
                        chordPredictedBefore[i] += 1;
                }
            }

            double[] overallProb = new double[Chords.Count];
            for (int i = 0; i < Chords.Count; i++)
                //overallProb[i] = 1.0 * rootMagnitudes[i] + 1.0 * rootOccurences[i] + 1.0 * chordExtensions[i] + 2 * rootFreq[i] + 1.0 * fifthOmitted[i] + 1.5 * chordPredictedBefore[i];
                overallProb[i] = 1.1*rootMagnitudes[i] + 1.0*rootOccurences[i] + 2.3*chordExtensions[i] + 1.8*rootFreq[i] + 2.5*notSuspended[i] + 1.0*fifthOmitted[i] + 0.7*chordPredictedBefore[i];
            overallProb = Normalise(overallProb);
            double probSum = overallProb[0] + 1;
            for (int i = 1; i < Chords.Count; i++)
                probSum += overallProb[i] + 1;
            for (int i = 0; i < Chords.Count; i++)
                Chords[i].Probability = (overallProb[i] + 1) / probSum * 100;
        }

        private double[] Normalise(double[] values)
        {
            double max = Math.Abs(values[0]);
            for(int i = 1; i < values.Length; i++)
            {
                if (Math.Abs(values[i]) > max)
                    max = Math.Abs(values[i]);
            }
            if (max > 0)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] /= max;
            }
            return values;
        }

        private List<Note> NextChord(List<Note> chord)
        {
            Note firstNote = chord[0];
            for(int i = 0; i < chord.Count - 1; i++)
                chord[i] = chord[i + 1];

            chord[chord.Count - 1] = firstNote;
            return chord;
        }

        public void DisposeAnalyser()
        {
            aggregateNotes.Clear();
            avgError.Clear();
            notePercent = new double[12];
            music.DisposeMusic();
            GC.Collect();
        }
    }
}
