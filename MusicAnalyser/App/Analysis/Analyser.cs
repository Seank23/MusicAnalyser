using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser.App.Analysis
{
    class Analyser
    {
        private Music music;

        private static List<Note> aggregateNotes = new List<Note>();
        private static List<Note> notes;
        private static List<Note>[] chordNotes;
        private static List<Chord> chords = new List<Chord>();
        private static double[] notePercent = new double[12];
        private List<int> avgError = new List<int>();
        private static readonly Color[] noteColors = new Color[12];
        private string currentKey;
        private string currentMode;
        private Chord prevChord;
        private string majorKeyRoot;

        public Analyser()
        {
            music = new Music(); 
        }

        public List<int> GetAvgError() { return avgError; }
        public Music GetMusic() { return music; }
        public List<Note> GetNotes() { return notes; }
        public List<Note>[] GetChordNotes() { return chordNotes; }
        public void ResetError() { avgError.Clear(); }
        public double[] GetNotePercents() { return notePercent; }
        public Color[] GetNoteColors() { return noteColors; }
        public string GetCurrentKey() { return currentKey; }
        public string GetCurrentMode() { return currentMode; }

        public void GetChords(out List<Chord> chordsList)
        {
            Chord[] chordsArray = new Chord[chords.Count];
            Array.Copy(chords.ToArray(), chordsArray, chords.Count);
            chordsList = chordsArray.ToList();
        }

        /*
         * Identifies valid notes from the peaks in the frequency spectrum and plots them
         */
        public void GetNotes(Dictionary<double, double> fftPeaks, double[] positions, int timeStamp)
        {
            music.NoteError = new List<int>();
            notes = new List<Note>();

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
                notes.Add(myNote);
                music.CountNote(noteName);
                BufferNote(myNote.NoteIndex);
                i++;
            }
            GetNotePercentages();

            if (music.NoteError.Count > 0)
                avgError.Add((int)music.NoteError.Average());
        }

        /*
       * Calculates a color dynamically based on the actualValue in relation to the specified range of values
       */
        public Color GetNoteColor(int rangeStart, int rangeEnd, int actualValue)
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

        private void GetNotePercentages()
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
        private void BufferNote(int noteIndex)
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
            string[] dominantNotes = new string[7];
            double largestPercent;
            int largestIndex;

            // Finds the 7 most common notes based on the percentage of the total notes identified that note occupies
            for (int i = 0; i < dominantNotes.Length; i++)
            {
                largestPercent = percents[0];
                largestIndex = 0;
                for (int j = 1; j < percents.Length; j++)
                {
                    if (percents[j] > largestPercent)
                    {
                        largestPercent = percents[j];
                        largestIndex = j;
                    }
                }
                dominantNotes[i] = Music.GetNoteName(largestIndex);
                percents[largestIndex] = 0;
            }

            int[] keyProbability = Music.FindScaleProbability(dominantNotes);

            largestIndex = 0;
            bool confident = false;
            List<int> possibleKeys = new List<int>();

            // Based on the key probablities, decides the most likely key signature at current time
            for (int i = 0; i < keyProbability.Length; i++)
            {
                if (keyProbability[i] == 7) // If the 7 most common notes are present in a single key signature, then that is the most likely key signature
                {
                    largestIndex = i;
                    possibleKeys.Clear();
                    confident = true;
                    break;
                }
                else if (keyProbability[i] == 6) // If 6 of the most common notes are present in a key signature, then that is a possible key
                {
                    possibleKeys.Add(i);
                }
            }
            if (possibleKeys.Count > 0)
            {
                List<double> lowNotes = new List<double>();
                // Finds most likely of possible keys by checking the missing note in each, if it is more common than the missing notes in other possible keys then that is the most likely key
                foreach (int keyIndex in possibleKeys)
                {
                    string[] scale = new string[7];
                    Array.Copy(Music.Scales, keyIndex * 7, scale, 0, scale.Length);
                    double lowest = notePercent[Music.GetNoteIndex(scale[0] + "0")];
                    for (int i = 1; i < scale.Length; i++)
                    {
                        double percentage = notePercent[Music.GetNoteIndex(scale[i] + "0")];
                        if (percentage < lowest)
                            lowest = percentage;
                    }
                    lowNotes.Add(lowest);
                }
                int highOfLowIndex = lowNotes.IndexOf(lowNotes.Max());
                largestIndex = possibleKeys[highOfLowIndex];
            }
            else if (!confident) // There is no discernable key
            {
                currentKey = "N/A";
                majorKeyRoot = "N/A";
                return;
            }
            string keyRoot = Music.GetNoteName(largestIndex);
            if (music.IsMinor(keyRoot, out string minorRoot)) // Checks if key is most likely the relative minor of original prediction
                currentKey = minorRoot + " Minor";
            else
                currentKey = keyRoot + " Major";

            majorKeyRoot = keyRoot;
            currentMode = Music.GetMode(notePercent, keyRoot, minorRoot);
        }

        public bool FindChordsNotes()
        {
            if (aggregateNotes.Count == 0)
                return false;

            int[,] tempNoteOccurences = new int[12, 2];
            List<Note>[] notesByName = new List<Note>[12];
            for (int i = 0; i < 12; i++)
                notesByName[i] = new List<Note>();

            int initialTimeStamp = aggregateNotes[0].TimeStamp;
            int timeStampOffset = 0;

            for(int i = 0; i < aggregateNotes.Count; i++)
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

            int numChordNotes = 0;
            List<int> chordNoteIndexes = new List<int>();
            for(int i = 0; i < tempNoteOccurences.Length / 2; i++)
            {
                if(tempNoteOccurences[i, 0] >= Prefs.CHORD_NOTE_OCCURENCE_OFFSET)
                {
                    numChordNotes++;
                    chordNoteIndexes.Add(i);
                }
            }

            chordNotes = new List<Note>[numChordNotes];
            for(int i = 0; i < numChordNotes; i++)
            {
                chordNotes[i] = new List<Note>();
                List<int> octaves = new List<int>();
                for(int j = 0; j < notesByName[chordNoteIndexes[i]].Count; j++)
                {
                    if (!octaves.Contains(notesByName[chordNoteIndexes[i]][j].Octave))
                    {
                        chordNotes[i].Add(notesByName[chordNoteIndexes[i]][j]);
                        octaves.Add(notesByName[chordNoteIndexes[i]][j].Octave);
                    }
                }
                chordNotes[i] = chordNotes[i].OrderBy(x => x.Frequency).ToList(); // Order: Frequency - low to high
            }

            int removed = 0;
            for(int i = 0; i < chordNotes.Length; i++)
            {
                if(chordNotes[i].Count == 1 && chordNotes[i][0].Octave > 4)
                {
                    chordNotes[i].Clear();
                    removed++;
                    continue;
                }

                int lowestOctave = chordNotes[i][0].Octave;
                for(int j = 1; j < chordNotes[i].Count; j++)
                {
                    if (chordNotes[i][j].Octave < lowestOctave)
                        lowestOctave = chordNotes[i][j].Octave;
                }

                if (lowestOctave > 4)
                {
                    chordNotes[i].Clear();
                    removed++;
                }
            }

            List<Note>[] chordTemp = new List<Note>[chordNotes.Length - removed];
            int tempIndex = 0;
            for(int i = 0; i < chordNotes.Length; i++)
            {
                if (chordNotes[i].Count != 0)
                {
                    chordTemp[tempIndex] = chordNotes[i];
                    tempIndex++;
                }
            }
            chordNotes = chordTemp;
            return true;
        }

        /*
         *  Finds all possible chords from sequence of chord notes
         */
        public void FindChords()
        {
            chords.Clear();
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
                string chordQuality = Music.GetChordQuality(intervals); // Determines chord quality from intervals
                if(chordQuality != "N/A")
                    chords.Add(CreateChord(myChordNotes[0].Name, chordQuality, myChordNotes));

                myChordNotes = NextChord(myChordNotes); // Iterates chord root note
            }
            AdjustChordProbabilities();
            chords = chords.OrderByDescending(x => x.Probability).ToList();
            if(chords.Count > 0)
                prevChord = chords[0];
        }

        private Chord CreateChord(string root, string quality, List<Note> notes)
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
                NumExtensions = numExtensions
            };
            return myChord;
        }

        private void AdjustChordProbabilities()
        {
            if (chords.Count == 0)
                return;

            double[] rootMagnitudes = new double[chords.Count];
            for (int i = 0; i < chords.Count; i++)
                rootMagnitudes[i] = chords[i].Notes[0].Magnitude;
            double avgMagnitude = rootMagnitudes.Average();
            for (int i = 0; i < rootMagnitudes.Length; i++)
                rootMagnitudes[i] -= avgMagnitude;
            rootMagnitudes = Normalise(rootMagnitudes);

            double[] rootOccurences = new double[chords.Count];
            for (int i = 0; i < chords.Count; i++)
                rootOccurences[i] = chords[i].Notes[0].Octave;
            double avgOccurences = rootOccurences.Average();
            for (int i = 0; i < rootOccurences.Length; i++)
                rootOccurences[i] -= avgOccurences;
            rootOccurences = Normalise(rootOccurences);

            double[] rootFreq = new double[chords.Count];
            for (int i = 0; i < chords.Count; i++)
                rootFreq[i] = chords[i].Notes[0].Frequency;
            double avgFreq = rootFreq.Average();
            for (int i = 0; i < rootFreq.Length; i++)
                rootFreq[i] -= avgFreq;
            rootFreq = Normalise(rootFreq);

            double[] chordExtensions = new double[chords.Count];
            for (int i = 0; i < chords.Count; i++)
                chordExtensions[i] = chords[i].NumExtensions;
            double avgExtensions = chordExtensions.Average();
            for (int i = 0; i < chordExtensions.Length; i++)
                chordExtensions[i] -= avgExtensions;
            chordExtensions = Normalise(chordExtensions);

            double[] chordPredictedBefore = new double[chords.Count];
            if (prevChord != null)
            { 
                for(int i = 0; i < chords.Count; i++)
                {
                    if (chords[i].Root == prevChord.Root)
                        chordPredictedBefore[i] += 1;
                }
            }

            double[] rootInKey = new double[chords.Count];
            if (majorKeyRoot != "N/A")
            {
                string[] scale = new string[7];
                Array.Copy(Music.Scales, Music.GetNoteIndex(majorKeyRoot + "0") * 7, scale, 0, scale.Length);
                for (int i = 0; i < chords.Count; i++)
                {
                    if (scale.Contains(chords[i].Root))
                        rootInKey[i] += 1;
                    else
                        rootInKey[i] -= 1;
                }
            }

            double[] overallProb = new double[chords.Count];
            for(int i = 0; i < chords.Count; i++)
                overallProb[i] = rootMagnitudes[i] + rootOccurences[i] - 2 * rootFreq[i] - chordExtensions[i] + 1.5 * chordPredictedBefore[i] + rootInKey[i];
            overallProb = Normalise(overallProb);
            double probSum = overallProb[0] + 1;
            for (int i = 1; i < chords.Count; i++)
                probSum += overallProb[i] + 1;
            for (int i = 0; i < chords.Count; i++)
                chords[i].Probability = (overallProb[i] + 1) / probSum * 100;
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
