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
        private Rectangle timeline;
        private Rectangle timeBar;
        private List<Rectangle> ticks;

        public SpectrogramOverlay(SpectrogramViewer p)
        {
            DoubleBuffered = true;
            parent = p;
            this.Parent = parent;
            ticks = new List<Rectangle>();
        }

        public void DrawTimestamps()
        {
            this.Controls.Clear();
            ticks.Clear();

            int waveformWidth = this.Width - SpectrogramViewer.PADDING;
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
                this.Controls.Add(timeStamp);
                timeStamp.TextAlign = ContentAlignment.MiddleCenter;
                timeStamp.Top = this.Height - 20;
                timeStamp.Left = SpectrogramViewer.PADDING + (int)(waveformWidth * timeStampPos) - timeStamp.Width / 2;
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
                if (timeStampSamples[0] - startMilliseconds >= (timeStampSamples[1] - timeStampSamples[0]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(ticks[0].X - (ticks[1].X - ticks[0].X), this.Height - 25, 1, 5);
                    ticks.Add(secondaryTick);
                }

                if (endMilliseconds - timeStampSamples[timeStampSamples.Count - 1] >= (timeStampSamples[timeStampSamples.Count - 1] - timeStampSamples[timeStampSamples.Count - 2]) / 2)
                {
                    Rectangle secondaryTick = new Rectangle(ticks[ticks.Count - 1].X + (ticks[1].X - ticks[0].X), this.Height - 25, 1, 5);
                    ticks.Add(secondaryTick);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            timeline = new Rectangle(0, Height - 25, Width, 1);
            g.FillRectangle(new SolidBrush(Color.Black), timeline);
            timeBar = new Rectangle(0, Height - 24, Width, 24);
            g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlLight)), timeBar);
            if (ticks != null)
            {
                foreach (Rectangle tick in ticks)
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

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (parent.OnKeyPress(keyData))
        //        return true;
        //    else
        //        return base.ProcessCmdKey(ref msg, keyData);
        //}
    }
}
