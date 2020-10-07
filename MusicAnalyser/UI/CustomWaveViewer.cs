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
        private int prevSPP = 0;
        private long prevLeftSample = 0;
        private long startPosition;
        private int bytesPerSample;
        private int sampleRate;
        private int displaySpacing = 2;
        private int waveformPadding = 35;
        private float[] waveformLow;
        private float[] waveformHigh;
        private bool isLooping = false;

        public Color PenColor { get; set; }
        public float PenWidth { get; set; }
        public long LeftSample { get; set; }
        public long RightSample { get; set; }
        public long SelectSample { get; set; }
        public long LoopEndSample { get; set; }
        public OverlayPanel Overlay { get; }

        public int GetBytesPerSample() { return bytesPerSample; }
        public int GetSampleRate() { return sampleRate; }
        public int GetWaveformPadding() { return waveformPadding; }
        public bool GetIsLooping() { return isLooping; }

        public bool GetIsRecording() 
        {
            Form1 frm = (Form1)ParentForm;
            return frm.GetApp().IsRecording;
        }

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
            SamplesPerPixel = samples / (this.Width - 2 * waveformPadding);
            LeftSample = StartPosition;
            RightSample = waveStream.Length / bytesPerSample;
            ResetMarkers(); 
        }

        public void Zoom()
        {
            startPosition = LeftSample * bytesPerSample;
            SamplesPerPixel = (int)(RightSample - LeftSample) / (this.Width - 2 * waveformPadding);
            InvokeUI(() => Overlay.MovePosIndicator(waveformPadding));
            ResetMarkers();
        }

        private void ResetMarkers()
        {
            bool startInView = false;
            bool endInView = false;
            int selectPos = 0;
            int loopEndPos = 0;

            if (SelectSample >= LeftSample && SelectSample <= RightSample)
            {
                startInView = true;
                selectPos = (int)((SelectSample - LeftSample) / SamplesPerPixel) + waveformPadding;
                Overlay.MoveSelectMarker(selectPos);
            }
            else
                Overlay.MoveSelectMarker(-100);

            if (LoopEndSample >= LeftSample && LoopEndSample <= RightSample && LoopEndSample != 0)
            {
                endInView = true;
                loopEndPos = (int)((LoopEndSample - LeftSample) / SamplesPerPixel) + waveformPadding;
                Overlay.MoveLoopEndMarker(loopEndPos);
            }
            else
                Overlay.MoveLoopEndMarker(-100);

            if (startInView && endInView)
                Overlay.DisplayLoopSection(selectPos, loopEndPos - selectPos);
            else if(startInView && !endInView && LoopEndSample > SelectSample)
                Overlay.DisplayLoopSection(selectPos, Width - waveformPadding - selectPos);
            else if(!startInView && endInView)
                Overlay.DisplayLoopSection(waveformPadding, loopEndPos - waveformPadding);
            else if(LeftSample > SelectSample && RightSample < LoopEndSample)
                Overlay.DisplayLoopSection(waveformPadding, Width - waveformPadding);
            else
                Overlay.DisplayLoopSection(-100, 0);
        }

        private Point mousePos, startPos;
        private bool mouseDrag = false;

        public void InteractDown(MouseEventArgs e)
        {
            if (waveStream == null)
                return;

            if (e.Button == MouseButtons.Right) // Start zoom
            {
                if (e.X >= waveformPadding && e.X <= Width - waveformPadding)
                {
                    startPos = new Point(e.Location.X - waveformPadding, e.Location.Y);
                    mousePos = new Point(-1, -1);
                    mouseDrag = true;
                    Overlay.DrawVerticalLine(e.X, Color.Black);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (e.X >= waveformPadding && e.X <= Width - waveformPadding)
                {
                    isLooping = false;
                    Overlay.DisplayLoopSection(-100, 0);
                    Overlay.MoveLoopEndMarker(-100);
                    UI.SetLoopTime(0);
                    LoopEndSample = 0;

                    Overlay.MoveSelectMarker(e.X);
                    SelectSample = (e.X - waveformPadding) * SamplesPerPixel + LeftSample;
                    InvokeUI(() => UI.SetSelectTime((double)SelectSample / sampleRate));

                    startPos = new Point(e.Location.X - waveformPadding, e.Location.Y);
                    mousePos = new Point(-1, -1);
                    mouseDrag = true;
                }
            }
            base.OnMouseDown(e);
        }

        public void InteractMove(MouseEventArgs e)
        {
            if (waveStream == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (mouseDrag && e.X >= waveformPadding && e.X <= Width - waveformPadding)
                {
                    Overlay.DrawVerticalLine(e.X, Color.Black);
                    if (mousePos.X != -1)
                        Overlay.DrawVerticalLine(mousePos.X, Color.Black);
                    mousePos = e.Location;
                }
            }
            else if(e.Button == MouseButtons.Left)
            {
                if (mouseDrag && e.X >= startPos.X + waveformPadding && e.X <= Width - waveformPadding && e.X - waveformPadding - startPos.X >= 10)
                {
                    isLooping = true;
                    Overlay.MoveLoopEndMarker(e.X);
                    Overlay.DisplayLoopSection(startPos.X + waveformPadding, e.X - startPos.X - waveformPadding);
                    mousePos = e.Location;
                    LoopEndSample = (e.X - waveformPadding) * SamplesPerPixel + LeftSample;
                    UI.SetLoopTime((double)(LoopEndSample - SelectSample) / sampleRate);
                }
            }
            base.OnMouseMove(e);
        }

        public void InteractUp(MouseEventArgs e)
        {
            if (waveStream == null)
                return;

            if (mouseDrag && e.Button == MouseButtons.Right) // End zoom
            {
                mouseDrag = false;
                Overlay.DrawVerticalLine(e.X, Color.Black);

                if (mousePos.X != -1)
                {
                    LeftSample = (int)(StartPosition / bytesPerSample + SamplesPerPixel * Math.Min(startPos.X, mousePos.X - waveformPadding));
                    RightSample = (int)(StartPosition / bytesPerSample + SamplesPerPixel * Math.Max(startPos.X, mousePos.X - waveformPadding));
                    Zoom();
                }   
            }
            else if(e.Button == MouseButtons.Left)
            {
                if (e.X >= waveformPadding && e.X <= Width - waveformPadding) // Select play position
                {
                    if (UI.output.PlaybackState == PlaybackState.Stopped)
                        UI.GetApp().TriggerStop();
                }
                else if (e.X < waveformPadding) // Pan left
                {
                    long newLeftSample = Math.Max(LeftSample - (RightSample - LeftSample), 0);
                    if (newLeftSample != LeftSample)
                        RightSample = LeftSample;

                    LeftSample = newLeftSample;
                    Zoom();
                }
                else if (e.X > Width - waveformPadding) // Pan right
                {
                    long newRightSample = Math.Min(RightSample + (RightSample - LeftSample), WaveStream.Length / BytesPerSample);
                    if (newRightSample != RightSample)
                        LeftSample = RightSample;

                    RightSample = newRightSample;
                    Zoom();
                }
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

        public Form1 UI
        {
            get
            {
                return (Form1)Parent;
            }
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
                    sampleRate = WaveStream.WaveFormat.SampleRate;
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
            if (waveStream == null)
                return;

            if (SamplesPerPixel != prevSPP || LeftSample != prevLeftSample)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[SamplesPerPixel * bytesPerSample * displaySpacing];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * SamplesPerPixel);
                waveformLow = new float[(e.ClipRectangle.Right - e.ClipRectangle.X) / displaySpacing];
                waveformHigh = new float[(e.ClipRectangle.Right - e.ClipRectangle.X) / displaySpacing];

                for (float x = e.ClipRectangle.X + waveformPadding; x < e.ClipRectangle.Right - waveformPadding; x += displaySpacing)
                {
                    short low = 0;
                    short high = 0;
                    bytesRead = waveStream.Read(waveData, 0, SamplesPerPixel * bytesPerSample * displaySpacing);
                    if (bytesRead == 0)
                        break;
                    for (int n = 0; n < bytesRead; n += 2)
                    {
                        short sample = BitConverter.ToInt16(waveData, n);
                        if (sample < low) low = sample;
                        if (sample > high) high = sample;
                    }
                    float lowPercent = (((float)low) - short.MinValue) / ushort.MaxValue;
                    float highPercent = (((float)high) - short.MinValue) / ushort.MaxValue;
                    waveformLow[(int)(x / displaySpacing)] = lowPercent;
                    waveformHigh[(int)(x / displaySpacing)] = highPercent;
                }
                prevSPP = SamplesPerPixel;
                prevLeftSample = LeftSample;

                Overlay.DrawTimestamps();
            }

            if (waveformHigh != null && waveformLow != null)
            {
                using (Pen outerPen = new Pen(PenColor, PenWidth))
                {
                    using (Pen innerPen = new Pen(Color.LightBlue, PenWidth))
                    {
                        for (float x = e.ClipRectangle.X + waveformPadding; x < e.ClipRectangle.Right - waveformPadding; x += displaySpacing)
                        {
                            e.Graphics.DrawLine(outerPen, x, this.Height * waveformLow[(int)(x / displaySpacing)] * 0.9f, x, this.Height * waveformHigh[(int)(x / displaySpacing)] * 0.9f);
                            //e.Graphics.DrawLine(innerPen, x, this.Height * waveformLow[(int)(x / displaySpacing)] * 0.9f + Height / 4, x, this.Height * waveformHigh[(int)(x / displaySpacing)] * 0.9f - Height / 4);
                        }
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
