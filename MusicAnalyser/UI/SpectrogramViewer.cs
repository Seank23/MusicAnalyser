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
        public List<SpectrogramFrame> MySpectrogramFrames { get; set; }
        private Bitmap spectrogramImage;
        private float binsPerPixel;
        private float framesPerPixel;
        private RectangleF projectionRect;
        private Point prevPanLocation;

        public SpectrogramViewer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseWheel += SpectrogramViewer_MouseWheel;
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

        private void Zoom(bool zoomOut)
        {
            RectangleF prevProjection = projectionRect;
            projectionRect.Width /= 1.1f;
            projectionRect.Height /= 1.1f;
            if (zoomOut)
            {
                projectionRect.Width /= 0.7f;
                projectionRect.Height /= 0.7f;
            }
            if (projectionRect.Width > spectrogramImage.Width)
            {
                projectionRect.Width = spectrogramImage.Width;
                projectionRect.Height = spectrogramImage.Height;
            }
            PointF newCorner = new PointF(projectionRect.Location.X + (prevProjection.Width - projectionRect.Width) / 2, projectionRect.Location.Y + (prevProjection.Height - projectionRect.Height) / 2);
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
            e.Graphics.DrawImage(spectrogramImage, new Rectangle(0, 0, this.Width, this.Height), projectionRect, GraphicsUnit.Pixel);
        }

        private void SpectrogramViewer_MouseDown(object sender, MouseEventArgs e)
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

        private void SpectrogramViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Pan((e.Location.X - prevPanLocation.X) * framesPerPixel, (e.Location.Y - prevPanLocation.Y) * binsPerPixel);
                prevPanLocation = e.Location;
            }
        }

        private void SpectrogramViewer_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void SpectrogramViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Zoom(false);
            else
                Zoom(true);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add || keyData == Keys.Subtract || keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
            {
                if (keyData == Keys.Add)
                    Zoom(false);
                else if (keyData == Keys.Subtract)
                    Zoom(true);
                else if (keyData == Keys.Left)
                    Pan(30f, 0);
                else if (keyData == Keys.Right)
                    Pan(-30f, 0);
                else if (keyData == Keys.Up)
                    Pan(0, 30f);
                else if (keyData == Keys.Down)
                    Pan(0, -30f);
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        public void Reset()
        {
            MySpectrogramFrames = null;
            spectrogramImage = null;
            Refresh();
        }
    }
}