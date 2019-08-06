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

        public OverlayPanel(CustomWaveViewer p)
        {
            DoubleBuffered = true;
            parent = p;
            this.Parent = parent;
            posIndicator = new Rectangle(0, 0, 1, Height * 4); 
        }

        public void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Black);
        }

        public void MovePosIndicator(int posX)
        {
            posIndicator.X = posX;
            Refresh();
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
                timeline = new Rectangle(0, Height - 1, Width, 1);
                g.FillRectangle(new SolidBrush(Color.Black), timeline);
            }
        }
    }
}
