using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MusicAnalyser
{
    public class OverlayPanel : Panel
    {
        private CustomWaveViewer parent;
        private Rectangle posIndicator;
        private Rectangle timeline;
        private Rectangle timeBar;

        public OverlayPanel(CustomWaveViewer p)
        {
            DoubleBuffered = true;
            parent = p;
            this.Parent = parent;
            posIndicator = new Rectangle(parent.GetWaveformPadding(), 0, 1, (parent.Height * 2) - 25); 
        }

        public void DrawTimestamps()
        {
            long startTime = parent.LeftSample;
            long timespan = parent.RightSample - parent.LeftSample;
            int numTimestamps = 11;
            this.Controls.Clear();

            for(int i = 0; i < numTimestamps; i++)
            {
                float timeStampPos = (float)i / (numTimestamps - 1);
                double seconds = (startTime + timespan * timeStampPos) / parent.GetSampleRate();
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                Label timeStamp = new Label();
                this.Controls.Add(timeStamp);
                timeStamp.TextAlign = ContentAlignment.MiddleCenter;
                timeStamp.Top = this.Height - 20;
                timeStamp.Left = parent.GetWaveformPadding() + (int)((this.Width - 2 * parent.GetWaveformPadding()) * timeStampPos) - timeStamp.Width / 2;
                timeStamp.Text = t.ToString(@"mm\:ss\:fff");
            }
        }

        public void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Black);
        }

        public void MovePosIndicator(int posX)
        {
            posIndicator.X = posX;
            Invalidate(false);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (parent.WaveStream != null)
            {
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(Color.Black), posIndicator);
                timeline = new Rectangle(0, Height - 25, Width, 1);
                g.FillRectangle(new SolidBrush(Color.Black), timeline);
                timeBar = new Rectangle(0, Height - 24, Width, 24);
                g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlLight)), timeBar);
            }
        }
    }
}
