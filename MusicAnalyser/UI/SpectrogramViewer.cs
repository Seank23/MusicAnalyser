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
        public static readonly int PADDING = 25; 
        public List<SpectrogramFrame> MySpectrogramFrames { get; set; }
        public SpectrogramOverlay Overlay { get; }

        private Bitmap spectrogramImage;
        private float binsPerPixel;
        private float framesPerPixel;
        private RectangleF projectionRect;
        private RectangleF prevProjectionRect;
        private Point prevPanLocation;

        public SpectrogramViewer(Form1 frm)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseWheel += SpectrogramViewer_MouseWheel;
            this.Parent = frm;
            this.Parent.KeyDown += new KeyEventHandler(SpectrogramViewer_KeyDown);
            prevProjectionRect = new RectangleF();

            Overlay = new SpectrogramOverlay(this);
            Overlay.Dock = DockStyle.Fill;
            Overlay.BackColor = Color.Transparent;
            Overlay.BringToFront();
        }

        public void GenerateSpectrogramImage()
        {
            if (MySpectrogramFrames == null)
                return;

            spectrogramImage = new Bitmap(MySpectrogramFrames.Count, MySpectrogramFrames[0].SpectrumData.Length);
            projectionRect = new RectangleF(0, 0, spectrogramImage.Width, spectrogramImage.Height);
            byte pixelVal;

            for (int i = 0; i < MySpectrogramFrames.Count; i++)
            {
                SpectrogramFrame frame = MySpectrogramFrames[i];
                for (int j = 0; j < frame.SpectrumData.Length; j++)
                {
                    pixelVal = frame.SpectrumData[j];
                    spectrogramImage.SetPixel(i, frame.SpectrumData.Length - 1 - j, Color.FromArgb(pixelVal, pixelVal, pixelVal));
                }
            }
        }

        public double[] GetTimeEndsInView()
        {
            if (MySpectrogramFrames == null)
                return null;

            int startIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X), MySpectrogramFrames.Count - 1), 0);
            int endIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X + projectionRect.Width), MySpectrogramFrames.Count - 1), 0);
            
            return new double[] { MySpectrogramFrames[startIndex].Timestamp, MySpectrogramFrames[endIndex].Timestamp };
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
            float sensitivity = 0.3f;
            if (projectionRect.X - deltaX >= 0 && projectionRect.X - deltaX + projectionRect.Width <= spectrogramImage.Width)
                projectionRect.X -= deltaX * sensitivity;
            if (projectionRect.Y - deltaY >= 0 && projectionRect.Y - deltaY + projectionRect.Height <= spectrogramImage.Height)
                projectionRect.Y -= deltaY * sensitivity;
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (spectrogramImage == null)
            {
                e.Graphics.Clear(Color.White);
                return;
            }

            binsPerPixel = (float)spectrogramImage.Height / this.Height;
            framesPerPixel = (float)spectrogramImage.Width / this.Width;
            e.Graphics.DrawImage(spectrogramImage, new Rectangle(PADDING, 0, this.Width, this.Height - PADDING), projectionRect, GraphicsUnit.Pixel);
            if (projectionRect != prevProjectionRect)
            {
                Overlay.DrawTimestamps();
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
            MySpectrogramFrames = null;
            spectrogramImage = null;
            Refresh();
        }

        private void SpectrogramViewer_Resize(object sender, EventArgs e)
        {
            Overlay.DrawTimestamps();
            Refresh();
        }

        private void SpectrogramViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (MySpectrogramFrames == null)
                return;

            if (e.KeyCode == Keys.Add)
                Zoom(false);
            else if (e.KeyCode == Keys.Subtract)
                Zoom(true);
            else if (e.KeyCode == Keys.Left)
                Pan(30f, 0);
            else if (e.KeyCode == Keys.Right)
                Pan(-30f, 0);
            else if (e.KeyCode == Keys.Up)
                Pan(0, 30f);
            else if (e.KeyCode == Keys.Down)
                Pan(0, -30f);
        }
    }
}