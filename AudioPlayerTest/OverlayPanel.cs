using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser
{
    public class OverlayPanel : Panel
    {
        private CustomWaveViewer parent;
        private Rectangle posIndicator;
        private Rectangle selectMarker;
        private Rectangle loopEndMarker;
        private Rectangle loopSection;
        private Rectangle timeline;
        private Rectangle timeBar;
        private List<Rectangle> ticks;

        public OverlayPanel(CustomWaveViewer p)
        {
            DoubleBuffered = true;
            parent = p;
            this.Parent = parent;
            ticks = new List<Rectangle>();
            posIndicator = new Rectangle(parent.GetWaveformPadding(), 0, 1, (parent.Height * 2) - 25);
            selectMarker = new Rectangle(parent.GetWaveformPadding(), 0, 2, (parent.Height * 2) - 25);
            loopEndMarker = new Rectangle(-100, 0, 2, (parent.Height * 2) - 25);
            loopSection = new Rectangle(-100, 0, 0, (parent.Height * 2) - 25);
        }

        public void DrawTimestamps()
        {
            this.Controls.Clear();
            ticks.Clear();

            int waveformWidth = this.Width - 2 * parent.GetWaveformPadding();
            int sampleRate = parent.GetSampleRate();
            long startSample = parent.LeftSample;
            long endSample = parent.RightSample;
            long timespan = endSample - startSample;
            double duration = (double)timespan / sampleRate;
            List<long> timeStampSamples = new List<long>();
            float timeStampSize;

            if (duration <= 0.1)
                timeStampSize = 0.01f;
            else if (duration > 0.1 && duration <= 0.5)
                timeStampSize = 0.05f;
            else if (duration > 0.5 && duration <= 1)
                timeStampSize = 0.1f;
            else if (duration > 1 && duration <= 2)
                timeStampSize = 0.25f;
            else if (duration > 2 && duration <= 5)
                timeStampSize = 0.5f;
            else if (duration > 5 && duration <= 10)
                timeStampSize = 1;
            else if (duration > 10 && duration <= 30)
                timeStampSize = 2;
            else if (duration > 30 && duration <= 80)
                timeStampSize = 5;
            else if (duration > 80 && duration <= 180)
                timeStampSize = 15;
            else if (duration > 180 && duration <= 360)
                timeStampSize = 30;
            else
                timeStampSize = 60;

            int numTimeStamps = (int)Math.Floor(timespan / (sampleRate * timeStampSize));
            long endRemainder = endSample % (int)(sampleRate * timeStampSize);
            long currentSample = endSample - endRemainder;
            timeStampSamples.Add(currentSample);

            for (int i = 0; i < numTimeStamps; i++)
            {
                currentSample -= (int)(sampleRate * timeStampSize);
                timeStampSamples.Add(currentSample);
            }

            for (int i = 0; i < timeStampSamples.Count; i++)
            {
                double timeStampPos = ((double)timeStampSamples[i] - startSample) / timespan;
                double seconds = (double)timeStampSamples[i] / sampleRate;
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                Label timeStamp = new Label();
                this.Controls.Add(timeStamp);
                timeStamp.TextAlign = ContentAlignment.MiddleCenter;
                timeStamp.Top = this.Height - 20;
                timeStamp.Left = parent.GetWaveformPadding() + (int)(waveformWidth * timeStampPos) - timeStamp.Width / 2;
                timeStamp.Text = t.ToString(@"m\:ss\:fff");

                Rectangle primaryTick = new Rectangle(timeStamp.Left + timeStamp.Width / 2, this.Height - 25, 1, 10);
                    
                if (ticks.Count > 0)
                {
                    Rectangle secondaryTick = new Rectangle((ticks[i * 2 - 2].X + primaryTick.X) / 2, this.Height - 25, 1, 5);
                    ticks.Add(secondaryTick);
                }
                ticks.Add(primaryTick);
            }

            if (timeStampSamples.Count > 1)
            {
                if (timeStampSamples[0] - startSample >= (timeStampSamples[1] - timeStampSamples[0]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(ticks[0].X - (ticks[1].X - ticks[0].X), this.Height - 25, 1, 5);
                    ticks.Add(secondaryTick);
                }

                if (endSample - timeStampSamples[timeStampSamples.Count - 1] >= (timeStampSamples[timeStampSamples.Count - 1] - timeStampSamples[timeStampSamples.Count - 2]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(ticks[ticks.Count - 1].X + (ticks[1].X - ticks[0].X), this.Height - 25, 1, 5);
                    ticks.Add(secondaryTick);
                }
            }
        }

        public void DrawVerticalLine(int x, Color color)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), color);
        }

        public void MovePosIndicator(int posX)
        {
            posIndicator.X = posX;
            Invalidate(false);
        }

        public void MoveSelectMarker(int posX)
        {
            selectMarker.X = posX;
            Invalidate(false);
        }

        public void MoveLoopEndMarker(int posX)
        {
            loopEndMarker.X = posX;
            Invalidate(false);
        }

        public void DisplayLoopSection(int startPos, int width)
        {
            loopSection.X = startPos;
            loopSection.Width = width;
            Invalidate(false);
        }

        public void ResetOverlay()
        {
            posIndicator.X = parent.GetWaveformPadding();
            selectMarker.X = parent.GetWaveformPadding();
            loopEndMarker.X = -100;
            loopSection.X = -100;
            loopSection.Width = 0;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if(!parent.GetIsRecording())
                parent.InteractDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!parent.GetIsRecording())
                parent.InteractMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!parent.GetIsRecording())
                parent.InteractUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(parent.GetIsRecording())
            {
                g.DrawString("Recording...", new Font(this.Font.FontFamily, 16), new SolidBrush(Color.Red), new Point(this.Width / 2 - (this.Width / 14), this.Height / 2 - (this.Height / 10)));
            }
            if (parent.WaveStream != null)
            {
                g.FillRectangle(new SolidBrush(Color.Black), posIndicator);
                g.FillRectangle(new SolidBrush(Color.DarkBlue), selectMarker);
                g.FillRectangle(new SolidBrush(Color.Blue), loopEndMarker);
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, 220, 220, 220)), loopSection);
                timeline = new Rectangle(0, Height - 25, Width, 1);
                g.FillRectangle(new SolidBrush(Color.Black), timeline);
                timeBar = new Rectangle(0, Height - 24, Width, 24);
                g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlLight)), timeBar);

                foreach(Rectangle tick in ticks)
                {
                    g.FillRectangle(new SolidBrush(Color.Black), tick);
                }
            }
            else
            {
                g.DrawString("No file selected", new Font(this.Font.FontFamily, 12), new SolidBrush(Color.Gray), new Point(this.Width / 2 - (this.Width / 14), this.Height / 2));
            }
        }
    }
}
