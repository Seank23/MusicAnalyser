using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;


namespace MusicAnalyser
{
    public partial class Form1 : Form
    {
        private static int UI_DELAY_FACTOR = 100000;
        private static int FOLLOW_SECS = 5;

        private AppController app;
        private Music music;
        public DirectSoundOut output { get; set; }

        public Form1()
        {
            InitializeComponent();
            app = new AppController(this);
            music = new Music();
            SetupFFTPlot();
            barVolume.Value = 10;
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            app.TriggerOpenFile();
            output = new DirectSoundOut();
        }

        public void SetupPlaybackUI(WaveStream audioGraph)
        {
            cwvViewer.WaveStream = audioGraph;
            cwvViewer.FitToScreen();
            btnPlay.Enabled = true;
            btnOpen.Enabled = false;
        }

        public bool SelectFile(out OpenFileDialog dialog)
        {
            dialog = new OpenFileDialog();
            dialog.Filter = "Audio Files (*.wav; *.mp3)|*.wav; *.mp3;";

            if (dialog.ShowDialog() != DialogResult.OK)
                return false;
            else
                return true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            app.TriggerPlayPause();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            app.TriggerClose();
        }

        public void ClearUI()
        {
            lstNotes.Items.Clear();
            spFFT.plt.Clear();
            spFFT.Render();
            cwvViewer.WaveStream = null;
            btnClose.Enabled = false;
            btnOpen.Enabled = true;
            btnPlay.Enabled = false;
            btnPlay.Text = "Play";
            music = new Music();
            chbTempo.Enabled = true;
            barTempo.Enabled = true;
            barVolume.Value = 10;
            barTempo.Value = 10;
            barPitch.Value = 50;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.DisposeAudio();
        }

        public void UpdatePlayPosition()
        {
            int currentPos = 0;
            int previousPos = 0;
            long currentSample;
            int index = 0;
            AudioSource source = app.GetSource();

            while (output.PlaybackState != PlaybackState.Stopped)
            {
                if (output.PlaybackState == PlaybackState.Playing)
                {
                    if(!app.IsStarted() && chbFollow.Checked)
                    {
                        cwvViewer.LeftSample = 0;
                        cwvViewer.RightSample = cwvViewer.LeftSample + (FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                        cwvViewer.Zoom();
                    }

                    app.SetStarted(true);
                    index++;
                    index = index % UI_DELAY_FACTOR;

                    if (index == UI_DELAY_FACTOR - 1)
                    {
                        InvokeUI(() => txtTime.Text = source.AudioStream.CurrentTime.ToString());
                        currentSample = source.AudioStream.Position / 8;

                        if (currentSample > cwvViewer.LeftSample && currentSample < cwvViewer.RightSample)
                        {
                            currentPos = (int)(currentSample - cwvViewer.LeftSample) / cwvViewer.SamplesPerPixel;

                            if (currentPos != previousPos)
                            {
                                InvokeUI(() => cwvViewer.Overlay.MovePosIndicator(currentPos));
                                previousPos = currentPos;
                            }
                        }
                        else if(currentSample >= cwvViewer.RightSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = cwvViewer.RightSample;
                            cwvViewer.RightSample = cwvViewer.LeftSample + (FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                            cwvViewer.Zoom();
                        }
                    }
                }
            }
        }

        public void DisplayFFT(double[] dataFft, double fftScale, double avgGain, double maxGain)
        {
            spFFT.plt.Clear();
            spFFT.plt.PlotSignal(dataFft, fftScale, markerSize: 0);
            int zoom = 0;
            switch(barZoom.Value)
            {
                case 0:
                    zoom = 500;
                    break;
                case 1:
                    zoom = 1000;
                    break;
                case 2:
                    zoom = 2000;
                    break;
                case 3:
                    zoom = 4000;
                    break;
            }
            spFFT.plt.Axis(0, zoom, avgGain - 10, maxGain + 10);
        }

        public void UpdateNoteOccurencesUI(string noteName, int occurences, double percent, Color noteColor)
        {
            string note = noteName.Substring(0, noteName.Length - 1);
            switch (note)
            {
                case "C":
                    lblC.Text = "C: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblC.ForeColor = noteColor;
                    return;
                case "Db":
                    lblDb.Text = "Db: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblDb.ForeColor = noteColor;
                    return;
                case "D":
                    lblD.Text = "D: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblD.ForeColor = noteColor;
                    return;
                case "Eb":
                    lblEb.Text = "Eb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblEb.ForeColor = noteColor;
                    return;
                case "E":
                    lblE.Text = "E: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblE.ForeColor = noteColor;
                    return;
                case "F":
                    lblF.Text = "F: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblF.ForeColor = noteColor;
                    return;
                case "Gb":
                    lblGb.Text = "Gb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblGb.ForeColor = noteColor;
                    return;
                case "G":
                    lblG.Text = "G: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblG.ForeColor = noteColor;
                    return;
                case "Ab":
                    lblAb.Text = "Ab: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblAb.ForeColor = noteColor;
                    return;
                case "A":
                    lblA.Text = "A: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblA.ForeColor = noteColor;
                    return;
                case "Bb":
                    lblBb.Text = "Bb: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblBb.ForeColor = noteColor;
                    return;
                case "B":
                    lblB.Text = "B: " + occurences + " (" + string.Format("{0:0.00}", percent) + "%)";
                    lblB.ForeColor = noteColor;
                    return;
            }
        } 

        private void timerFFT_Tick(object sender, EventArgs e)
        {
            app.RunAnalysis();
        }

        private void SetupFFTPlot()
        {
            spFFT.plt.Title("Frequency Spectrum");
            spFFT.plt.YLabel("Gain (dB)", fontSize: 12);
            spFFT.plt.XLabel("Frequency (Hz)", fontSize: 12);
            spFFT.Render();
        }

        private void barVolume_Scroll(object sender, EventArgs e)
        {
            app.VolumeChange(barVolume.Value);;
        }

        private void barTempo_Scroll(object sender, EventArgs e)
        {
            app.TempoChange(barTempo.Value);
        }

        private void barPitch_Scroll(object sender, EventArgs e)
        {
            app.PitchChange(barPitch.Value);
        }

        public void DrawPauseUI()
        {
            btnPlay.Text = "Play";
            timerFFT.Enabled = false;
            btnClose.Enabled = true;
        }

        public void DrawPlayUI()
        {
            btnClose.Enabled = false;
            btnPlay.Text = "Pause";
            timerFFT.Enabled = true;
        }

        public void SetTempoState()
        {
            chbTempo.Enabled = false;

            if (!chbTempo.Checked)
                barTempo.Enabled = false;
        }

        public void EnableTimer(bool enable) { timerFFT.Enabled = enable; }
        public void UpdateFFTDrawsUI(int draws) { lblFFTDraws.Text = "FFT Updates: " + draws; }
        public void ClearNotesList() { lstNotes.Items.Clear(); }
        public void PrintNote(string note, double freq, double gain) { lstNotes.Items.Add(note + " (" + String.Format("{0:0.00}", freq) + " Hz) @ " + String.Format("{0:0.00}", gain) + " dB"); }
        public void PlotNote(string name, double freq, double gain, Color color) { spFFT.plt.PlotText(name, freq, gain, color, fontSize: 11); }
        public void SetKeyText(string text) { lblKey.Text = text; }
        public void SetErrorText(string text) { lblError.Text = text; }
        public void SetTimerInterval(int interval) { timerFFT.Interval = interval; }
        public void SetExecTimeText(string text) { lblExeTime.Text = text; }
        public void RenderSpectrum() { spFFT.Render(); }

        public bool IsTempoEnabled() { return chbTempo.Enabled; }
        public bool IsTempoChecked() { return chbTempo.Checked; }
        public bool IsOrderChecked() { return chbOrder.Checked; }
    }
}
