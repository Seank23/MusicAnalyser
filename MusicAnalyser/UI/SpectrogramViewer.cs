﻿using MusicAnalyser.App.Analysis;
using MusicAnalyser.App.DSP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser.UI
{
    public partial class SpectrogramViewer : UserControl
    {
        public static readonly int PADDING_BOTTOM = 25;
        public static readonly int PADDING_LEFT = 50;
        public SpectrogramHandler MySpectrogram { get; set; }
        public SpectrogramOverlay Overlay { get; }
        public bool ShowAnnotations { get; set; }
        private Bitmap spectrogramImage;
        private float binsPerPixel;
        private float framesPerPixel;
        private RectangleF projectionRect;
        private RectangleF prevProjectionRect;
        private float projectionWidthRatio;
        private float projectionHeightRatio;
        private Point prevPanLocation;
        private bool moving;

        public SpectrogramViewer(Form frm)
        {
            InitializeComponent();
            SetNewParent(frm);
            this.DoubleBuffered = true;
            this.MouseWheel += SpectrogramViewer_MouseWheel;
            prevProjectionRect = new RectangleF();

            Overlay = new SpectrogramOverlay(this);
            Overlay.Dock = DockStyle.Fill;
            Overlay.BackColor = Color.Transparent;
            Overlay.BringToFront();
            ShowAnnotations = true;
        }

        public void SetNewParent(Form newParent)
        {
            if(Parent != null)
                Parent.KeyDown -= SpectrogramViewer_KeyDown;
            Parent = newParent;
            Parent.KeyDown += new KeyEventHandler(SpectrogramViewer_KeyDown);
        }

        public Form1 GetForm() 
        {
            if (Parent.GetType().Name == "Form1")
                return (Form1)Parent;
            else
                return (Form1)Parent.Parent;
        }

        public void CreateSpectrogram()
        {
            GenerateSpectrogramImage();
            MySpectrogram.GenerateAnnotations();
        }

        public RectangleF GetProjectionRect() { return projectionRect; }

        public double[] GetFrequencyRangeInView()
        {
            if (MySpectrogram == null)
                return null;

            double[] binEnds = GetFrequencyBinsInView();

            if (MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
            {
                Func<double, double> scale = (Func<double, double>)MySpectrogram.FrequencyScale;
                return new double[] { scale(binEnds[0]), scale(binEnds[1]) };
            }
            else
                return new double[] { (double)MySpectrogram.FrequencyScale * binEnds[0] * 0.95, (double)MySpectrogram.FrequencyScale * binEnds[1] * 0.95 };
        }

        public double[] GetTimeEndsInView()
        {
            if (MySpectrogram == null)
                return null;

            int[] frameEnds = GetFramesInView();

            return new double[] { MySpectrogram.Frames[frameEnds[0]].Timestamp, MySpectrogram.Frames[frameEnds[1]].Timestamp };
        }

        public double[] GetFrequencyBinsInView()
        {
            if (MySpectrogram == null)
                return null;

            int bins = MySpectrogram.FrequencyBins;
            int top = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);
            int bottom = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y + projectionRect.Height), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);

            return new double[] { top, bottom };
        }

        public int[] GetFramesInView()
        {
            if (MySpectrogram == null)
                return null;

            int startIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X), MySpectrogram.Frames.Count - 1), 0);
            int endIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X + projectionRect.Width), MySpectrogram.Frames.Count - 1), 0);

            return new int[] { startIndex, endIndex };
        }

        public double GetFrequencyPoint(float relativePos)
        {
            int bins = MySpectrogram.FrequencyBins;
            int top = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);
            int bottom = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y + projectionRect.Height), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);
            double binPos = bottom + (relativePos * (top - bottom));

            if (MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
            {
                Func<double, double> scale = (Func<double, double>)MySpectrogram.FrequencyScale;
                return scale(binPos);
            }
            else
                return (double)MySpectrogram.FrequencyScale * binPos * 0.95;
        }

        public double GetTimePointSeconds(float relativePos)
        {
            double[] ends = GetTimeEndsInView();
            return (ends[0] + relativePos * (ends[1] - ends[0])) / 1000;
        }

        private void GenerateSpectrogramImage()
        {
            if (MySpectrogram == null)
                return;

            spectrogramImage = new Bitmap(MySpectrogram.Frames.Count, MySpectrogram.FrequencyBins);
            projectionRect = new RectangleF(0, 0, spectrogramImage.Width, spectrogramImage.Height);
            binsPerPixel = (float)spectrogramImage.Height / this.Height;
            framesPerPixel = (float)spectrogramImage.Width / this.Width;
            byte pixelVal;

            for (int i = 0; i < MySpectrogram.Frames.Count; i++)
            {
                SpectrogramFrame frame = MySpectrogram.Frames[i];
                for (int j = 0; j < MySpectrogram.FrequencyBins; j++)
                {
                    pixelVal = frame.SpectrumData[j];
                    spectrogramImage.SetPixel(i, MySpectrogram.FrequencyBins - 1 - j, Color.FromArgb(pixelVal, pixelVal, pixelVal));
                }
            }
            spectrogramImage.Save("test.bmp");
        }

        private SpectrogramHandler.NoteAnnotation[] DrawNoteAnnotations()
        {
            List<SpectrogramHandler.NoteAnnotation> noteAnnotations = new List<SpectrogramHandler.NoteAnnotation>();
            double[] freqRange = GetFrequencyRangeInView();
            int startFreq = Math.Max(Music.GetNoteIndexFromFrequency(freqRange[1]), 0);
            int endFreq = Music.GetNoteIndexFromFrequency(freqRange[0]) + 1;
            int[] frameRange = GetFramesInView();

            for (int i = startFreq; i < endFreq; i++)
            {
                for (int j = frameRange[0]; j < frameRange[1]; j++)
                {
                    if (MySpectrogram.NoteAnnotationMatrix[i, j].freq != 0)
                    {
                        noteAnnotations.Add(MySpectrogram.NoteAnnotationMatrix[i, j]);
                        j += MySpectrogram.NoteAnnotationMatrix[i, j].length - 1;
                    }
                }
            }

            for (int i = 0; i < noteAnnotations.Count; i++)
            {
                SpectrogramHandler.NoteAnnotation myNote = noteAnnotations[i];
                int startY;
                if (MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
                    startY = (int)GetFrequencyCoordinate(myNote.position);
                else
                    startY = (int)GetFrequencyCoordinate(myNote.freq);
                int startX = (int)GetTimeCoordinate(myNote.startIndex);
                int width = (int)GetTimeCoordinate(myNote.startIndex + myNote.length) - startX;
                myNote.shape = new Rectangle(startX, startY, width, 10);
                noteAnnotations[i] = myNote;
            }
            return noteAnnotations.ToArray();
        }

        private SpectrogramHandler.ChordAnnotation[] DrawChordAnnotations()
        {
            List<SpectrogramHandler.ChordAnnotation> chordAnnotations = new List<SpectrogramHandler.ChordAnnotation>();
            int[] frameRange = GetFramesInView();

            for (int i = frameRange[0]; i < frameRange[1]; i++)
            {
                if (MySpectrogram.ChordAnnotationVect[i].name != null)
                {
                    chordAnnotations.Add(MySpectrogram.ChordAnnotationVect[i]);
                    i += MySpectrogram.ChordAnnotationVect[i].length - 1;
                }
            }

            for(int i = 0; i < chordAnnotations.Count; i++)
            {
                SpectrogramHandler.ChordAnnotation myChord = chordAnnotations[i];
                int startX = (int)GetTimeCoordinate(myChord.startIndex);
                int width = (int)GetTimeCoordinate(myChord.startIndex + myChord.length) - startX;
                myChord.shape = new Rectangle(startX, 20, width, 10);
                chordAnnotations[i] = myChord;
            }
            return chordAnnotations.ToArray();
        }

        private SpectrogramHandler.KeyAnnotation[] DrawKeyAnnotations()
        {
            List<SpectrogramHandler.KeyAnnotation> keyAnnotations = new List<SpectrogramHandler.KeyAnnotation>();
            int[] frameRange = GetFramesInView();

            for (int i = frameRange[0]; i < frameRange[1]; i++)
            {
                if (MySpectrogram.KeyAnnotationVect[i].name != null)
                {
                    keyAnnotations.Add(MySpectrogram.KeyAnnotationVect[i]);
                    i += MySpectrogram.KeyAnnotationVect[i].length - 1;
                }
            }

            for (int i = 0; i < keyAnnotations.Count; i++)
            {
                SpectrogramHandler.KeyAnnotation myKey = keyAnnotations[i];
                int startX = (int)GetTimeCoordinate(myKey.startIndex);
                int width = (int)GetTimeCoordinate(myKey.startIndex + myKey.length) - startX;
                myKey.shape = new Rectangle(startX, 5, width, 10);
                keyAnnotations[i] = myKey;
            }
            return keyAnnotations.ToArray();
        }

        private void Zoom(bool zoomOut)
        {
            RectangleF prevProjection = projectionRect;
            projectionRect.Width /= 1.1f;
            projectionRect.Height /= 1.1f;
            if (zoomOut)
            {
                projectionRect.Width /= 0.8f;
                projectionRect.Height /= 0.8f;
            }
            if (projectionRect.Width > spectrogramImage.Width)
            {
                projectionRect.Width = spectrogramImage.Width;
                projectionRect.Height = spectrogramImage.Height;
            }
            float cornerX = Math.Max(Math.Min(projectionRect.Location.X + (prevProjection.Width - projectionRect.Width) / 2, spectrogramImage.Width - projectionRect.Width), 0);
            float cornerY = Math.Max(Math.Min(projectionRect.Location.Y + (prevProjection.Height - projectionRect.Height) / 2, spectrogramImage.Height - projectionRect.Height), 0);
            PointF newCorner = new PointF(cornerX, cornerY);
            projectionRect.Location = newCorner;
            Refresh();
        }

        private void Pan(float deltaX, float deltaY)
        {
            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
                moving = true;
            else
                moving = false;

            float sensitivity = 0.3f;
            if (projectionRect.X - deltaX >= 0 && projectionRect.X - deltaX + projectionRect.Width <= spectrogramImage.Width)
                projectionRect.X -= deltaX * sensitivity;
            if (projectionRect.Y - deltaY >= 0 && projectionRect.Y - deltaY + projectionRect.Height <= spectrogramImage.Height)
                projectionRect.Y -= deltaY * sensitivity;
            Refresh();
        }

        private double GetFrequencyCoordinate(double freq)
        {
            double[] range;
            if (MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
                range = GetFrequencyBinsInView();
            else
                range = GetFrequencyRangeInView();
            double relPos = (freq - range[1]) / (range[0] - range[1]);
            int specHeight = this.Height - PADDING_BOTTOM;
            return specHeight - (relPos * specHeight);
        }

        private double GetTimeCoordinate(double framePos)
        {
            int[] frameEnds = GetFramesInView();
            double relPos = (framePos - frameEnds[0]) / (frameEnds[1] - frameEnds[0]);
            int specWidth = this.Width - PADDING_LEFT;
            return PADDING_LEFT + relPos * specWidth;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(moving)
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            else
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            if (spectrogramImage == null)
            {
                e.Graphics.Clear(Color.White);
                return;
            }

            projectionHeightRatio = spectrogramImage.Height / projectionRect.Height;
            projectionWidthRatio = spectrogramImage.Width / projectionRect.Width;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.DrawImage(spectrogramImage, new Rectangle(PADDING_LEFT, 0, this.Width - PADDING_LEFT, this.Height - PADDING_BOTTOM), projectionRect, GraphicsUnit.Pixel);
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            if (ShowAnnotations)
            {
                SpectrogramHandler.NoteAnnotation[] noteAnnotations = DrawNoteAnnotations();
                if (noteAnnotations != null)
                {
                    float penSize = Math.Min(Math.Max(2 * projectionHeightRatio, 1), 12);
                    using (Pen p = new Pen(Color.Green, penSize))
                    {
                        foreach (SpectrogramHandler.NoteAnnotation note in noteAnnotations)
                        {
                            int alpha = Math.Min(Math.Max((int)(note.magnitude / MySpectrogram.GetMaxNoteMagnitude() * 255), 127), 220);
                            p.Color = Color.FromArgb(alpha, 0, 128, 0);
                            e.Graphics.DrawLine(p, new Point(note.shape.X, (int)(note.shape.Y + penSize / 2)), new Point(note.shape.X + note.shape.Width, (int)(note.shape.Y + penSize / 2)));
                            if (penSize > 4)
                                e.Graphics.DrawString(note.name, new Font(Form1.fonts.Families[0], penSize), new SolidBrush(Color.White), new Point(note.shape.X + 5, (int)(note.shape.Y - penSize / 2)));
                        }
                    }
                }

                SpectrogramHandler.ChordAnnotation[] chordAnnotations = DrawChordAnnotations();
                if(chordAnnotations != null)
                {
                    foreach (SpectrogramHandler.ChordAnnotation chord in chordAnnotations)
                    {
                        Pen p = new Pen(Color.FromArgb(200, Analyser.GetNoteColor(30, 60, (int)chord.confidence)), 5);
                        e.Graphics.DrawLine(p, new Point(chord.shape.X + 2, chord.shape.Y), new Point(chord.shape.X + chord.shape.Width - 2, chord.shape.Y));
                        int textX = Math.Max(chord.shape.X, PADDING_LEFT) + Math.Min(chord.shape.Width, this.Width - PADDING_LEFT) / 2 - 4 * chord.name.Length;
                        e.Graphics.DrawString(chord.name, new Font(Form1.fonts.Families[0], 8), new SolidBrush(Color.White), new Point(textX, chord.shape.Y + 6));
                    }
                }

                SpectrogramHandler.KeyAnnotation[] keyAnnotations = DrawKeyAnnotations();
                if(keyAnnotations != null)
                {
                    using (Pen p = new Pen(Color.FromArgb(200, Color.Green), 10))
                    {
                        foreach (SpectrogramHandler.KeyAnnotation key in keyAnnotations)
                        {
                            e.Graphics.DrawLine(p, new Point(key.shape.X + 2, key.shape.Y), new Point(key.shape.X + key.shape.Width - 2, key.shape.Y));
                            int textX = Math.Max(key.shape.X, PADDING_LEFT) + Math.Min(key.shape.Width, this.Width - PADDING_LEFT) / 2 - 4 * key.name.Length;
                            e.Graphics.DrawString(key.name, new Font(Form1.fonts.Families[0], 8), new SolidBrush(Color.White), new Point(textX, key.shape.Y - 10));
                        }
                    }
                }
            }

            if (projectionRect != prevProjectionRect)
            {
                Overlay.DrawTimeAxis();
                Overlay.DrawFrequencyAxis();
                prevProjectionRect = projectionRect;
            }
            base.OnPaint(e);
        }

        public void InteractDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                prevPanLocation = e.Location;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                projectionRect = new RectangleF(0, 0, spectrogramImage.Width, spectrogramImage.Height);
                Refresh();
            }
        }

        public void InteractMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Pan((e.Location.X - prevPanLocation.X) * framesPerPixel, (e.Location.Y - prevPanLocation.Y) * binsPerPixel);
                prevPanLocation = e.Location;
            }
        }

        public void InteractUp(MouseEventArgs e)
        {

        }

        private void SpectrogramViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Zoom(false);
            else
                Zoom(true);
        }

        public void Reset()
        {
            MySpectrogram = null;
            spectrogramImage = null;
            Refresh();
        }

        private void SpectrogramViewer_Resize(object sender, EventArgs e)
        {
            if (spectrogramImage != null)
            {
                binsPerPixel = (float)spectrogramImage.Height / this.Height;
                framesPerPixel = (float)spectrogramImage.Width / this.Width;
            }
            if (Overlay != null)
            {
                Overlay.DrawTimeAxis();
                Overlay.DrawFrequencyAxis();
                Refresh();
            }
        }

        public void SpectrogramViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (MySpectrogram == null)
                return;

            if (e.KeyCode == Keys.Add)
                Zoom(false);
            else if (e.KeyCode == Keys.Subtract)
                Zoom(true);
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.NumPad4)
                Pan(30f, 0);
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.NumPad6)
                Pan(-30f, 0);
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.NumPad8)
                Pan(0, 30f);
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.NumPad2)
                Pan(0, -30f);
        }
    }
}