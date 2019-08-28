using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAnalyser
{
    class Analyser
    {
        private Form1 ui;
        private Music music;
        private AppController app;

        private List<Note> notes;
        private List<Note>[] chordNotes;
        private List<string> chords = new List<string>();
        private double[] notePercent = new double[12];
        private List<int> avgError = new List<int>();

        public Analyser(Form1 form, AppController appControl)
        {
            ui = form;
            app = appControl;
            music = new Music();
            notes = new List<Note>();
        }

        public List<int> GetAvgError() { return avgError; }
        public Music GetMusic() { return music; }
        public List<Note>[] GetChordNotes() { return chordNotes; }
        public List<string> GetChords() { return chords; }
        public void ResetError() { avgError.Clear(); }

        /*
         * Identifies valid notes from the peaks in the frequency spectrum and plots them
         */
        public void GetNotes(Dictionary<double, double> fftPeaks, int timeStamp)
        {
            music.NoteError = new List<int>();

            foreach (double freq in fftPeaks.Keys)
            {
                string noteName = music.GetNote(freq);

                if (noteName == "N/A") // Ignore peaks that are not valid notes
                    continue;

                Note myNote = CreateNote(noteName, freq, fftPeaks[freq], timeStamp);
                notes.Add(myNote);
                //ui.PrintNote(noteName, freq, myNote.Magnitude);
                int occurences = music.NoteOccurences[myNote.NoteIndex];
                double percent = ((double)occurences / (double)music.NoteBuffer.Count) * 100;
                Color noteColor = app.GetNoteColor(0, (int)(10000 / 7), (int)(percent * 100));
                notePercent[myNote.NoteIndex] = percent;
                music.CountNote(noteName);
                BufferNote(myNote.NoteIndex);
                ui.UpdateNoteOccurencesUI(noteName, occurences, percent, noteColor);
                ui.PlotNote(noteName, freq, myNote.Magnitude, noteColor);
            } 

            if (music.NoteError.Count > 0)
                avgError.Add((int)music.NoteError.Average());
        }

        private Note CreateNote(string name, double freq, double gain, int timeStamp)
        {
            Note myNote = new Note();
            myNote.Name = name.Substring(0, name.Length - 1);
            myNote.Octave = Convert.ToInt32(name.Substring(name.Length - 1, 1));
            myNote.NoteIndex = Music.GetNoteIndex(name);
            myNote.Frequency = freq;
            myNote.Magnitude = gain;
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

            int[] keyProbability = Music.FindScale(dominantNotes);

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
                ui.InvokeUI(() => ui.SetKeyText("Predicted Key: N/A"));
                ui.InvokeUI(() => ui.SetModeText(""));
                //for (int i = 0; i < keyProbability.Length; i++) // DEBUG
                //{
                //    Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
                //}
                //Console.WriteLine("");
                return;
            }
            string keyRoot = Music.GetNoteName(largestIndex);
            if (music.IsMinor(keyRoot, out string minorRoot)) // Checks if key is most likely the relative minor of original prediction
                ui.InvokeUI(() => ui.SetKeyText("Predicted Key: " + minorRoot + " Minor"));
            else
                ui.InvokeUI(() => ui.SetKeyText("Predicted Key: " + keyRoot + " Major"));

            string mode = Music.GetMode(notePercent, keyRoot, minorRoot);
            if(mode != "")
                ui.InvokeUI(() => ui.SetModeText("(" + mode + ")"));
            else
                ui.InvokeUI(() => ui.SetModeText(""));

            //for (int i = 0; i < keyProbability.Length; i++) // DEBUG
            //{
            //    Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
            //}
            //Console.WriteLine("");
        }

        public void FindChordsNotes()
        {
            ui.InvokeUI(() => ui.ClearNotesList());

            int[] tempNoteOccurences = new int[12];
            List<Note>[] notesByName = new List<Note>[12];
            for (int i = 0; i < 12; i++)
                notesByName[i] = new List<Note>();

            for(int i = 0; i < notes.Count; i++)
            {
                int index = notes[i].NoteIndex;
                tempNoteOccurences[index]++;
                notesByName[index].Add(notes[i]);
            }
            notes.Clear();

            int numChordNotes = 0;
            List<int> chordNoteIndexes = new List<int>();
            for(int i = 0; i < tempNoteOccurences.Length; i++)
            {
                if(tempNoteOccurences[i] >= Prefs.CHORD_DETECTION_INTERVAL + Prefs.CHORD_NOTE_OCCURENCE_OFFSET)
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
        }

        public void FindChords()
        {
            chords.Clear();
            List<Note> chord = new List<Note>();
            for (int i = 0; i < chordNotes.Length; i++)
                chord.Add(chordNotes[i][0]);

            for (int i = 0; i < chordNotes.Length; i++)
            {
                string interval = "";
                for (int j = 1; j < chordNotes.Length; j++)
                {
                    int noteDifference = chord[j].NoteIndex - chord[0].NoteIndex;
                    if (noteDifference < 0)
                        noteDifference = 12 + noteDifference;
                    interval += noteDifference;
                }
                string chordQuality = Music.GetChordQuality(interval);
                if(chordQuality != "N/A")
                    chords.Add(chord[0].Name + chordQuality);
                else
                    chords.Add(chordQuality);
                chord = NextChord(chord);
            }
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
            notes.Clear();
            avgError.Clear();
            notePercent = new double[12];
            music.DisposeMusic();
            GC.Collect();
        }
    }
}
