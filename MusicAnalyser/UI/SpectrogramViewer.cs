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

        public Form1 GetForm() { return (Form1)Parent; }

        public void GenerateSpectrogramImage()
        {
            if (MySpectrogram == null)
                return;

            spectrogramImage = new Bitmap(MySpectrogram.Frames.Count, MySpectrogram.FrequencyBins);
            projectionRect = new RectangleF(0, 0, spectrogramImage.Width, spectrogramImage.Height);
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

        public RectangleF GetProjectionRect() { return projectionRect; }

        public double[] GetTimeEndsInView()
        {
            if (MySpectrogram == null)
                return null;

            int startIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X), MySpectrogram.Frames.Count - 1), 0);
            int endIndex = (int)Math.Max(Math.Min(Math.Floor(projectionRect.X + projectionRect.Width), MySpectrogram.Frames.Count - 1), 0);
            
            return new double[] { MySpectrogram.Frames[startIndex].Timestamp, MySpectrogram.Frames[endIndex].Timestamp };
        }

        public double[] GetFrequencyRangeInView()
        {
            if (MySpectrogram == null)
                return null;

            int bins = MySpectrogram.FrequencyBins;
            int top = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);
            int bottom = bins - (int)Math.Max(Math.Min(Math.Floor(projectionRect.Y + projectionRect.Height), MySpectrogram.Frames[0].SpectrumData.Length - 1), 0);

            if (MySpectrogram.FrequencyScale.GetType().Name == "Func`2")
            {
                Func<double, double> scale = (Func<double, double>)MySpectrogram.FrequencyScale;
                return new double[] { scale(top), scale(bottom) };
            }
            else
                return new double[] { (double)MySpectrogram.FrequencyScale * top * 0.95, (double)MySpectrogram.FrequencyScale * bottom * 0.95 };
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
            e.Graphics.DrawImage(spectrogramImage, new Rectangle(PADDING_LEFT, 0, this.Width - PADDING_LEFT, this.Height - PADDING_BOTTOM), projectionRect, GraphicsUnit.Pixel);
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
            Overlay.DrawTimeAxis();
            Refresh();
        }

        private void SpectrogramViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (MySpectrogram == null)
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