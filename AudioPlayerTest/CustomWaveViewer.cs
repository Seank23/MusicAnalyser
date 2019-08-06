using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace MusicAnalyser
{
    /// <summary>
    /// Control for viewing waveforms
    /// </summary>
    public class CustomWaveViewer : UserControl
    {
        private Container components = null;
        private WaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;

        public Color PenColor { get; set; }
        public float PenWidth { get; set; }
        public long LeftSample { get; set; }
        public long RightSample { get; set; }
        public OverlayPanel Overlay { get; }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        public void FitToScreen()
        {
            if (waveStream == null)
                return;

            int samples = (int)(waveStream.Length / bytesPerSample);
            startPosition = 0;
            SamplesPerPixel = samples / this.Width;
            LeftSample = StartPosition;
            RightSample = waveStream.Length;
        }

        public void Zoom()
        {
            startPosition = LeftSample * bytesPerSample;
            SamplesPerPixel = (int)(RightSample - LeftSample) / this.Width;
            InvokeUI(() => Overlay.MovePosIndicator(0));
        }

        private Point mousePos, startPos;
        private bool mouseDrag = false;

        public void InteractDown(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                startPos = e.Location;
                mousePos = new Point(-1, -1);
                mouseDrag = true;
                Overlay.DrawVerticalLine(e.X);
            }
            base.OnMouseDown(e);
        }

        public void InteractMove(MouseEventArgs e)
        {
            if(mouseDrag)
            {
                Overlay.DrawVerticalLine(e.X);
                if (mousePos.X != -1)
                    Overlay.DrawVerticalLine(mousePos.X);
                mousePos = e.Location;
            }
            base.OnMouseMove(e);
        }

        public void InteractUp(MouseEventArgs e)
        {
            if(mouseDrag && e.Button == MouseButtons.Left)
            {
                mouseDrag = false;
                Overlay.DrawVerticalLine(startPos.X);

                if (mousePos.X != -1)
                    Overlay.DrawVerticalLine(mousePos.X);

                LeftSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Min(startPos.X, mousePos.X));
                RightSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Max(startPos.X, mousePos.X));
                Zoom();
            }
            else if(e.Button == MouseButtons.Middle)
            {
                FitToScreen();
            }
            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            FitToScreen();
            base.OnResize(e);
        }

        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;
            this.PenColor = Color.DodgerBlue;
            this.PenWidth = 1;

            Overlay = new OverlayPanel(this);
            Overlay.Dock = DockStyle.Fill;
            Overlay.BackColor = Color.Transparent;
            Overlay.BringToFront();
        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                this.Invalidate();
            }
        }

        public int BytesPerSample
        {
            get { return bytesPerSample; }
            set { bytesPerSample = value; }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                using (Pen linePen = new Pen(PenColor, PenWidth))
                {
                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;
                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                        if (bytesRead == 0)
                            break;
                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = BitConverter.ToInt16(waveData, n);
                            if (sample < low) low = sample;
                            if (sample > high) high = sample;
                        }
                        float lowPercent = ((((float)low) - short.MinValue) / ushort.MaxValue);
                        float highPercent = ((((float)high) - short.MinValue) / ushort.MaxValue);
                        e.Graphics.DrawLine(linePen, x, this.Height * lowPercent, x, this.Height * highPercent);
                    }
                }
            }

            base.OnPaint(e);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
