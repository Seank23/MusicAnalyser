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

        private Dictionary<double, double> notes;
        private double[] notePercent = new double[12];
        private List<int> avgError = new List<int>();

        public Analyser(Form1 form, AppController appControl)
        {
            ui = form;
            app = appControl;
            music = new Music();
        }

        public List<int> GetAvgError() { return avgError; }
        public Music GetMusic() { return music; }
        public void ResetError() { avgError.Clear(); }

        /*
         * Identifies valid notes from the peaks in the frequency spectrum and plots them
         */
        public void GetNotes(Dictionary<double, double> fftPeaks)
        {
            notes = new Dictionary<double, double>();
            ui.ClearNotesList();
            music.NoteError = new List<int>();

            foreach (double freq in fftPeaks.Keys)
            {
                string noteName = music.GetNote(freq);

                if (noteName == "N/A") // Ignore peaks that are not valid notes
                    continue;

                double gain = fftPeaks[freq];
                notes.Add(freq, gain);
                ui.PrintNote(noteName, freq, gain);
                int noteIndex = Music.GetNoteIndex(noteName); 
                int occurences = music.NoteOccurences[noteIndex];
                double percent = ((double)occurences / (double)music.NoteBuffer.Count) * 100;
                Color noteColor = app.GetNoteColor(0, (int)(10000 / 7), (int)(percent * 100));
                notePercent[noteIndex] = percent;
                music.CountNote(noteName);
                BufferNote(noteIndex);
                ui.UpdateNoteOccurencesUI(noteName, occurences, percent, noteColor);
                ui.PlotNote(noteName, freq, gain, noteColor);
            } 

            if (music.NoteError.Count > 0)
                avgError.Add((int)music.NoteError.Average());
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
                for (int i = 0; i < keyProbability.Length; i++) // DEBUG
                {
                    Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
                }
                Console.WriteLine("");
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

            for (int i = 0; i < keyProbability.Length; i++) // DEBUG
            {
                Console.WriteLine(Music.GetNoteName(i) + ": " + keyProbability[i]);
            }
            Console.WriteLine("");
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
