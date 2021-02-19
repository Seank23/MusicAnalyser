using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser.UI
{
    public class SpectrogramOverlay : UserControl
    {
        private SpectrogramViewer parent;
        private List<Rectangle> timeTicks;
        private List<Rectangle> freqTicks;
        private List<Label> timeLabels;
        private List<Label> frequencyLabels;

        public SpectrogramOverlay(SpectrogramViewer p)
        {
            DoubleBuffered = true;
            parent = p;
            this.Parent = parent;
            timeTicks = new List<Rectangle>();
            freqTicks = new List<Rectangle>();
            timeLabels = new List<Label>();
            frequencyLabels = new List<Label>();
        }

        public void DrawTimeAxis()
        {
            this.Controls.Clear();
            timeTicks.Clear();
            timeLabels.Clear();

            float projectionWidth = this.Width - SpectrogramViewer.PADDING_LEFT;
            double[] timeEnds = parent.GetTimeEndsInView();
            if (timeEnds == null) return;
            long startMilliseconds = (long)timeEnds[0];
            long endMilliseconds = (long)timeEnds[1];
            if (startMilliseconds == endMilliseconds) return;
            long timespan = endMilliseconds - startMilliseconds;
            List<long> timeStampSamples = new List<long>();
            int timeStampSize;

            if (timespan <= 100)
                timeStampSize = 10;
            else if (timespan > 100 && timespan <= 500)
                timeStampSize = 50;
            else if (timespan > 500 && timespan <= 1000)
                timeStampSize = 100;
            else if (timespan > 1000 && timespan <= 2000)
                timeStampSize = 250;
            else if (timespan > 2000 && timespan <= 5000)
                timeStampSize = 500;
            else if (timespan > 5000 && timespan <= 10000)
                timeStampSize = 1000;
            else if (timespan > 10000 && timespan <= 30000)
                timeStampSize = 2000;
            else if (timespan > 30000 && timespan <= 80000)
                timeStampSize = 5000;
            else if (timespan > 80000 && timespan <= 180000)
                timeStampSize = 15000;
            else if (timespan > 180000 && timespan <= 360000)
                timeStampSize = 30000;
            else
                timeStampSize = 60000;

            int numTimeStamps = (int)(timespan / timeStampSize);
            long endRemainder = endMilliseconds % timeStampSize;
            long currentMilliseconds = endMilliseconds - endRemainder;
            timeStampSamples.Add(currentMilliseconds);

            for (int i = 0; i < numTimeStamps; i++)
            {
                currentMilliseconds -= timeStampSize;
                timeStampSamples.Add(currentMilliseconds);
            }

            for (int i = 0; i < timeStampSamples.Count; i++)
            {
                double timeStampPos = (double)(timeStampSamples[i] - startMilliseconds) / timespan;
                double seconds = (double)timeStampSamples[i] / 1000;
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                Label timeStamp = new Label();
                timeStamp.TextAlign = ContentAlignment.MiddleCenter;
                timeStamp.Top = this.Height - 20;
                timeStamp.Left = SpectrogramViewer.PADDING_LEFT + (int)(projectionWidth * timeStampPos) - timeStamp.Width / 2;
                timeStamp.Text = t.ToString(@"m\:ss\:fff");
                this.Controls.Add(timeStamp);
                timeLabels.Add(timeStamp);

                Rectangle primaryTick = new Rectangle(timeStamp.Left + timeStamp.Width / 2, this.Height - 25, 1, 10);

                if (timeTicks.Count > 0)
                {
                    Rectangle secondaryTick = new Rectangle((timeTicks[i * 2 - 2].X + primaryTick.X) / 2, this.Height - 25, 1, 5);
                    timeTicks.Add(secondaryTick);
                }
                timeTicks.Add(primaryTick);
            }

            if (timeStampSamples.Count > 1)
            {
                if (timeStampSamples[0] - startMilliseconds >= (timeStampSamples[1] - timeStampSamples[0]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(timeTicks[0].X - (timeTicks[1].X - timeTicks[0].X), this.Height - 25, 1, 5);
                    timeTicks.Add(secondaryTick);
                }

                if (endMilliseconds - timeStampSamples[timeStampSamples.Count - 1] >= (timeStampSamples[timeStampSamples.Count - 1] - timeStampSamples[timeStampSamples.Count - 2]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(timeTicks[timeTicks.Count - 1].X + (timeTicks[1].X - timeTicks[0].X), this.Height - 25, 1, 5);
                    timeTicks.Add(secondaryTick);
                }
            }
        }

        public void DrawFrequencyAxis()
        {
            freqTicks.Clear();
            frequencyLabels.Clear();

            float projectionHeight = this.Height - SpectrogramViewer.PADDING_BOTTOM;
            double[] freqRange = parent.GetFrequencyRangeInView();
            if (freqRange == null) return;
            double lowFreq = freqRange[1];
            double highFreq = freqRange[0];
            if (lowFreq == highFreq) return;
            double range = highFreq - lowFreq;
            List<double> freqSamples = new List<double>();
            double bandSize;

            if (range < 10)
                bandSize = 1;
            else if (range < 25)
                bandSize = 2;
            else if (range < 80)
                bandSize = 5;
            else if (range < 160)
                bandSize = 10;
            else if (range < 325)
                bandSize = 25;
            else if (range < 750)
                bandSize = 50;
            else if (range < 1500)
                bandSize = 100;
            else if (range < 4000)
                bandSize = 250;
            else
                bandSize = 500;

            int numBands = (int)(range / bandSize);
            double endRemainder = highFreq % bandSize;
            double currentFreq = highFreq - endRemainder;
            freqSamples.Add(currentFreq);

            for (int i = 0; i < numBands; i++)
            {
                currentFreq -= bandSize;
                freqSamples.Add(currentFreq);
            }

            for (int i = 0; i < freqSamples.Count; i++)
            {
                double freqPos = 1 - (double)(freqSamples[i] - lowFreq) / range;
                Label freqLabel = new Label();
                freqLabel.TextAlign = ContentAlignment.MiddleCenter;
                freqLabel.Top = (int)(projectionHeight * freqPos) - freqLabel.Height / 2;
                freqLabel.Left = -freqLabel.Width / 4;
                freqLabel.Text = freqSamples[i].ToString("0.##") + " Hz";
                this.Controls.Add(freqLabel);
                frequencyLabels.Add(freqLabel);

                Rectangle primaryTick = new Rectangle(SpectrogramViewer.PADDING_LEFT - 2, freqLabel.Top + freqLabel.Height / 2, 2, 1);

                if (freqTicks.Count > 0)
                {
                    Rectangle secondaryTick = new Rectangle(SpectrogramViewer.PADDING_LEFT - 5, (freqTicks[i * 2 - 2].Y + primaryTick.Y) / 2, 5, 1);
                    freqTicks.Add(secondaryTick);
                }
                freqTicks.Add(primaryTick);
            }

            if (freqSamples.Count > 1)
            {
                if (freqSamples[0] - lowFreq >= (freqSamples[1] - freqSamples[0]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(SpectrogramViewer.PADDING_LEFT - 5, freqTicks[0].Y - (freqTicks[1].Y - freqTicks[0].Y), 5, 1);
                    freqTicks.Add(secondaryTick);
                }

                if (highFreq - freqSamples[freqSamples.Count - 1] >= (freqSamples[freqSamples.Count - 1] - freqSamples[freqSamples.Count - 2]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(SpectrogramViewer.PADDING_LEFT - 5, freqTicks[freqTicks.Count - 1].Y + (freqTicks[1].Y - freqTicks[0].Y), 5, 1);
                    freqTicks.Add(secondaryTick);
                }
            }

            if(parent.MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
            {
                foreach(Label freqLabel in frequencyLabels)
                {
                    float labelPos = (projectionHeight - freqLabel.Top + freqLabel.Height / 2) / projectionHeight;
                    double newFreq = parent.GetNonLinearFrequencyPoint(labelPos);
                    freqLabel.Text = newFreq.ToString("0") + " Hz";
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Time axis background
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, Height - SpectrogramViewer.PADDING_BOTTOM, Width, 1));
            g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlLight)), new Rectangle(0, Height - SpectrogramViewer.PADDING_BOTTOM + 1, Width, SpectrogramViewer.PADDING_BOTTOM - 1));
            // Frequency axis background
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(SpectrogramViewer.PADDING_LEFT, 0, 1, Height - SpectrogramViewer.PADDING_BOTTOM));
            g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlLight)), new Rectangle(0, 0, SpectrogramViewer.PADDING_LEFT - 1, Height));
            if (timeTicks != null)
            {
                foreach (Rectangle tick in timeTicks)
                    g.FillRectangle(new SolidBrush(Color.Black), tick);
            }
            if (freqTicks != null)
            {
                foreach (Rectangle tick in freqTicks)
                    g.FillRectangle(new SolidBrush(Color.Black), tick);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            parent.InteractDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            parent.InteractMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            parent.InteractUp(e);
        }
    }
}
