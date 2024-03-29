﻿using MusicAnalyser.App.Analysis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MusicAnalyser.App.Spectrogram
{
    public class SpectrogramHandler
    {
        public struct NoteAnnotation
        {
            public string name;
            public double startIndex;
            public int length;
            public double freq;
            public double position;
            public double magnitude;
            public Rectangle shape;
        }

        public struct ChordAnnotation
        {
            public string name;
            public double startIndex;
            public int length;
            public double confidence;
            public Rectangle shape;
        }

        public struct KeyAnnotation
        {
            public string name;
            public double startIndex;
            public int length;
            public Rectangle shape;
        }

        public SpectrogramData Spectrogram;
        public NoteAnnotation[,] NoteAnnotationMatrix { get; set; }
        public ChordAnnotation[] ChordAnnotationVect { get; set; }
        public KeyAnnotation[] KeyAnnotationVect { get; set; }

        private double maxNoteMag;

        public SpectrogramHandler()
        {
            Spectrogram = new SpectrogramData();
        }

        public void CreateFrame(double timestamp, byte[] data, Note[] notes, Chord[] chords, string key, double quantScale)
        {
            SpectrogramFrame newFrame = new SpectrogramFrame(timestamp, data, notes, chords, key, quantScale);
            Spectrogram.Frames.Add(newFrame);
            if (Spectrogram.FrequencyBins == 0)
                Spectrogram.FrequencyBins = data.Length;
        }

        public void AddAnalysis(double timestamp, Note[] notes, Chord[] chords, string key)
        {
            SpectrogramFrame myFrame = Spectrogram.Frames.Where(frame => frame.Timestamp == timestamp).First();
            myFrame.Notes = notes;
            myFrame.Chords = chords;
            myFrame.KeySignature = key;
        }

        public void GenerateAnnotations()
        {
            GenerateNoteAnnotations();
            GenerateChordAnnotations();
            GenerateKeyAnnotations();
        }

        public void Clear()
        {
            Spectrogram.Frames.Clear();
            Spectrogram.FrequencyBins = 0;
            Spectrogram.FrequencyScale = null;
            Spectrogram.AudioFilename = null;
            Spectrogram.ScriptProperties = null;
            NoteAnnotationMatrix = null;
            ChordAnnotationVect = null;
            KeyAnnotationVect = null;
        }

        public double GetMaxNoteMagnitude() { return maxNoteMag; }

        private void GenerateNoteAnnotations()
        {
            List<NoteAnnotation> annotations = new List<NoteAnnotation>();

            for (int i = 0; i < Spectrogram.Frames.Count; i++)
            {
                Note[] frameNotes = Spectrogram.Frames[i].Notes;
                if (frameNotes == null)
                {
                    for (int j = 0; j < annotations.Count; j++)
                    {
                        NoteAnnotation myNote = annotations[j];
                        if (myNote.startIndex == i - myNote.length)
                        {
                            myNote.length++;
                            annotations[j] = myNote;
                        }
                    }
                    continue;
                }
                for (int j = 0; j < frameNotes.Length; j++)
                {
                    string noteName = frameNotes[j].Name + frameNotes[j].Octave;
                    frameNotes[j].Magnitude /= Spectrogram.Frames[i].QuantisationScale; // Scales note correctly for spectrum display
                    if (i > 0)
                    {
                        // Checks if current note is sustained from previous frame
                        bool notePresent = false;
                        for (int k = 0; k < annotations.Count; k++)
                        {
                            NoteAnnotation myNote = annotations[k];
                            if (myNote.name == noteName && myNote.startIndex == i - myNote.length)
                            {
                                myNote.length++;
                                myNote.magnitude += frameNotes[j].Magnitude;
                                notePresent = true;
                                annotations[k] = myNote;
                                break;
                            }
                        }
                        if (notePresent)
                            continue;
                    }
                    NoteAnnotation newNote = new NoteAnnotation();
                    newNote.name = noteName;
                    newNote.startIndex = i;
                    newNote.freq = frameNotes[j].Frequency;
                    newNote.position = frameNotes[j].Position;
                    newNote.length++;
                    newNote.magnitude = frameNotes[j].Magnitude;
                    annotations.Add(newNote);
                }
                Spectrogram.Frames[i].QuantisationScale = 1;
            }

            annotations = annotations.OrderBy(x => x.name).ToList();
            List<int> indexToRemove = new List<int>();
            int skip = 0;

            // Join notes that are close together into single longer note
            for (int i = 0; i < annotations.Count - 1; i++)
            {
                if (i + skip + 1 >= annotations.Count)
                    break;
                NoteAnnotation myNote = annotations[i];
                int nextIndex = i + skip + 1;
                double timeDifference = annotations[nextIndex].startIndex - (myNote.startIndex + myNote.length);
                if (timeDifference < Prefs.SPEC_NOTE_DIFF && annotations[nextIndex].name == myNote.name)
                {
                    myNote.length += (int)timeDifference + annotations[nextIndex].length;
                    myNote.magnitude += annotations[nextIndex].magnitude;
                    annotations[i] = myNote;
                    indexToRemove.Add(nextIndex);
                    skip++;
                    i--;
                }
                else
                {
                    i += skip;
                    skip = 0;
                }
            }

            for (int i = 0; i < indexToRemove.Count; i++)
                annotations.RemoveAt(indexToRemove[i] - i);

            // Remove short notes
            int index = 0;
            while (index < annotations.Count)
            {
                if (annotations[index].length < Prefs.SPEC_MIN_NOTE)
                    annotations.RemoveAt(index);
                else
                    index++;
            }

            // Create noteAnnotationMatrix
            double maxFreq;
            if (Spectrogram.FrequencyScale.GetType().Name == "Func`2")
            {
                Func<double, double> scale = (Func<double, double>)Spectrogram.FrequencyScale;
                maxFreq = scale(Spectrogram.Frames[0].SpectrumData.Length - 1);
            }
            else
                maxFreq = Spectrogram.Frames[0].SpectrumData.Length * (double)Spectrogram.FrequencyScale;

            int noteCount = Music.GetNoteIndexFromFrequency(maxFreq) + 1;
            NoteAnnotationMatrix = new NoteAnnotation[noteCount, Spectrogram.Frames.Count];
            maxNoteMag = 0;

            // Populate matrix
            for (int i = 0; i < annotations.Count; i++)
            {
                NoteAnnotation a = annotations[i];
                a.magnitude /= a.length;
                if (a.magnitude > maxNoteMag)
                    maxNoteMag = a.magnitude;

                int noteIndex = Music.GetNoteIndexFromFrequency(a.freq);
                int fullLength = a.length;
                double initialIndex = a.startIndex;
                for (int j = 0; j < fullLength; j++)
                {
                    a.length = fullLength - j;
                    a.startIndex = initialIndex + j;
                    NoteAnnotationMatrix[noteIndex, (int)a.startIndex] = a;
                }
            }
        }

        private void GenerateChordAnnotations()
        {
            List<ChordAnnotation> annotations = new List<ChordAnnotation>();
            int blockSize = Prefs.SPEC_CHORD_BLOCK;

            // Gets the most common chord over a given block size and adds it to annotations
            for (int i = 0; i < Spectrogram.Frames.Count; i += blockSize)
            {
                Dictionary<string, int> chordCountDict = new Dictionary<string, int>();
                Dictionary<string, int> chordRootDict = new Dictionary<string, int>();
                Dictionary<string, double> chordConfidenceDict = new Dictionary<string, double>();
                for (int j = 0; j < blockSize; j++)
                {
                    if (i + j >= Spectrogram.Frames.Count)
                        break;
                    if (Spectrogram.Frames[i + j].Chords == null)
                        continue;

                    if (Spectrogram.Frames[i + j].Chords.Length > 0)
                    {
                        string chord = Spectrogram.Frames[i + j].Chords[0].Name;
                        string root = Spectrogram.Frames[i + j].Chords[0].Root;
                        if (chordRootDict.ContainsKey(root))
                            chordRootDict[root]++;
                        else
                            chordRootDict.Add(root, 1);
                        if (chordCountDict.ContainsKey(chord))
                        {
                            chordCountDict[chord]++;
                            chordConfidenceDict[chord] += Spectrogram.Frames[i + j].Chords[0].Probability;
                        }
                        else
                        {
                            chordCountDict.Add(chord, 1);
                            chordConfidenceDict.Add(chord, Spectrogram.Frames[i + j].Chords[0].Probability);
                        }
                    }
                }
                if (chordCountDict.Values.Sum() > blockSize / 5)
                {
                    string bestRoot = chordRootDict.OrderByDescending(x => x.Value).FirstOrDefault().Key;
                    string bestChordGuess = chordCountDict.Where(x => x.Key.Contains(bestRoot)).OrderByDescending(x => x.Value).FirstOrDefault().Key;
                    ChordAnnotation myChord = new ChordAnnotation();
                    myChord.name = bestChordGuess;
                    myChord.startIndex = i;
                    myChord.length = Math.Min(blockSize, Spectrogram.Frames.Count - i);
                    myChord.confidence = chordConfidenceDict[bestChordGuess] / chordCountDict[bestChordGuess];
                    annotations.Add(myChord);
                }
            }

            annotations = annotations.OrderBy(x => x.name).ToList();
            List<int> indexToRemove = new List<int>();
            int skip = 0;

            // Joins adjacent chords if they are the same
            for (int i = 0; i < annotations.Count; i++)
            {
                if (i + skip + 1 >= annotations.Count)
                    break;
                ChordAnnotation myChord = annotations[i];
                int nextIndex = i + skip + 1;
                double timeDifference = annotations[nextIndex].startIndex - (myChord.startIndex + myChord.length);
                if (timeDifference < 16 && annotations[nextIndex].name == myChord.name)
                {
                    myChord.length += (int)timeDifference + annotations[nextIndex].length;
                    myChord.confidence += annotations[nextIndex].confidence;
                    annotations[i] = myChord;
                    indexToRemove.Add(nextIndex);
                    skip++;
                    i--;
                }
                else
                {
                    i += skip;
                    skip = 0;
                }
            }

            for (int i = 0; i < indexToRemove.Count; i++)
                annotations.RemoveAt(indexToRemove[i] - i);

            ChordAnnotationVect = new ChordAnnotation[Spectrogram.Frames.Count];

            // Populates chord annotations vector
            for (int i = 0; i < annotations.Count; i++)
            {
                ChordAnnotation a = annotations[i];
                a.confidence /= a.length / blockSize;
                int fullLength = a.length;
                double initialIndex = a.startIndex;
                for (int j = 0; j < fullLength; j++)
                {
                    a.length = fullLength - j;
                    a.startIndex = initialIndex + j;
                    ChordAnnotationVect[(int)a.startIndex] = a;
                }
            }
        }

        private void GenerateKeyAnnotations()
        {
            List<KeyAnnotation> annotations = new List<KeyAnnotation>();
            int blockSize = Prefs.SPEC_KEY_BLOCK;

            for (int i = 0; i < Spectrogram.Frames.Count; i += blockSize)
            {
                Dictionary<string, int> keyCountDict = new Dictionary<string, int>();

                for (int j = 0; j < blockSize; j++)
                {
                    if (i + j >= Spectrogram.Frames.Count)
                        break;
                    string key = Spectrogram.Frames[i + j].KeySignature;
                    if (key != null)
                    {
                        if (keyCountDict.ContainsKey(key))
                            keyCountDict[key]++;
                        else
                            keyCountDict.Add(key, 1);
                    }
                }
                if (keyCountDict.Values.Sum() > blockSize / 5)
                {
                    KeyAnnotation myKey = new KeyAnnotation();
                    myKey.name = keyCountDict.OrderByDescending(x => x.Value).FirstOrDefault().Key;
                    myKey.startIndex = i;
                    myKey.length = Math.Min(blockSize, Spectrogram.Frames.Count - i);
                    annotations.Add(myKey);
                }
            }

            annotations = annotations.OrderBy(x => x.name).ToList();
            List<int> indexToRemove = new List<int>();
            int skip = 0;

            // Joins adjacent keys if they are the same
            for (int i = 0; i < annotations.Count; i++)
            {
                if (i + skip + 1 >= annotations.Count)
                    break;
                KeyAnnotation myKey = annotations[i];
                int nextIndex = i + skip + 1;
                double timeDifference = annotations[nextIndex].startIndex - (myKey.startIndex + myKey.length);
                if (timeDifference < 16 && annotations[nextIndex].name == myKey.name)
                {
                    myKey.length += (int)timeDifference + annotations[nextIndex].length;
                    annotations[i] = myKey;
                    indexToRemove.Add(nextIndex);
                    skip++;
                    i--;
                }
                else
                {
                    i += skip;
                    skip = 0;
                }
            }

            for (int i = 0; i < indexToRemove.Count; i++)
                annotations.RemoveAt(indexToRemove[i] - i);

            KeyAnnotationVect = new KeyAnnotation[Spectrogram.Frames.Count];

            // Populates key annotations vector
            for (int i = 0; i < annotations.Count; i++)
            {
                KeyAnnotation a = annotations[i];
                int fullLength = a.length;
                double initialIndex = a.startIndex;
                for (int j = 0; j < fullLength; j++)
                {
                    a.length = fullLength - j;
                    a.startIndex = initialIndex + j;
                    KeyAnnotationVect[(int)a.startIndex] = a;
                }
            }
        }
    }
}
