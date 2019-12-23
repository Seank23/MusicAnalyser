using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;


namespace MusicAnalyser
{
    public partial class Form1 : Form
    {
        private AppController app;
        private Music music;
        public DirectSoundOut output { get; set; }
        public int fftZoom = 1000;

        public Form1()
        {
            InitializeComponent();
            app = new AppController(this);
            music = new Music();
            SetupFFTPlot();
            barVolume.Value = 10;
            SetModeText("");
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (!app.Opened)
            {
                app.TriggerOpenFile();
                output = new DirectSoundOut();
                btnOpenClose.Text = "Close";
            }
            else
            {
                app.TriggerClose();
                btnOpenClose.Text = "Open";
            }
        }

        public void SetupPlaybackUI(WaveStream audioGraph)
        {
            cwvViewer.WaveStream = audioGraph;
            cwvViewer.FitToScreen();
            btnPlay.Enabled = true;
            btnStop.Enabled = true;
            playToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = false;
            barVolume.Enabled = true;
            barTempo.Enabled = true;
            barPitch.Enabled = true;
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

        public void ClearUI()
        {
            lstChords.Items.Clear();
            spFFT.plt.Clear();
            spFFT.Render();
            cwvViewer.WaveStream = null;
            cwvViewer.SelectSample = 0;
            cwvViewer.Overlay.Controls.Clear();
            cwvViewer.Overlay.ResetOverlay();
            txtPlayTime.Text = "";
            txtSelectTime.Text = "";
            SetErrorText("+ 0 Cents");
            UpdateFFTDrawsUI(0);
            SetExecTimeText(0);
            btnStop.Enabled = false;
            btnOpenClose.Enabled = true;
            btnPlay.Enabled = false;
            btnPlay.Text = "Play";
            music = new Music();
            barTempo.Enabled = false;
            barVolume.Enabled = false;
            barPitch.Enabled = false;
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

            while (app.Opened)
            {
                if (source.AudioStream.Position >= source.AudioStream.Length)
                {
                    InvokeUI(() => app.TriggerStop());
                    app.SetStarted(false);
                    break;
                }

                if (output.PlaybackState == PlaybackState.Playing)
                {
                    if(!app.IsStarted() && chbFollow.Checked)
                    {
                        cwvViewer.LeftSample = 0;
                        cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                        cwvViewer.Zoom();
                    }

                    app.SetStarted(true);
                    index++;
                    index = index % Prefs.UI_DELAY_FACTOR;

                    if (index == Prefs.UI_DELAY_FACTOR - 1)
                    {
                        InvokeUI(() => txtPlayTime.Text = source.AudioStream.CurrentTime.ToString(@"mm\:ss\:fff"));
                        currentSample = source.AudioStream.Position / 8;

                        if (currentSample > cwvViewer.LeftSample && currentSample < cwvViewer.RightSample)
                        {
                            currentPos = cwvViewer.GetWaveformPadding() + (int)(currentSample - cwvViewer.LeftSample) / cwvViewer.SamplesPerPixel;

                            if (currentPos != previousPos)
                            {
                                InvokeUI(() => cwvViewer.Overlay.MovePosIndicator(currentPos));
                                previousPos = currentPos;
                            }
                        }
                        else if(currentSample >= cwvViewer.RightSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = cwvViewer.RightSample;
                            cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
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
            switch(barZoom.Value)
            {
                case 0:
                    fftZoom = 500;
                    break;
                case 1:
                    fftZoom = 1000;
                    break;
                case 2:
                    fftZoom = 2000;
                    break;
                case 3:
                    fftZoom = 4000;
                    break;
            }
            spFFT.plt.Axis(0, fftZoom, avgGain - 5, maxGain + 10);  
        }

        public void UpdateNoteOccurencesUI(string noteName, int occurences, double percent, Color noteColor)
        {
            switch (noteName)
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
            app.VolumeChange(barVolume.Value);
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
            playToolStripMenuItem.Text = "Play";
            timerFFT.Enabled = false;
        }

        public void DrawPlayUI()
        {
            btnPlay.Text = "Pause";
            playToolStripMenuItem.Text = "Pause";
            timerFFT.Enabled = true;
        }

        public void EnableTimer(bool enable) { timerFFT.Enabled = enable; }
        public void UpdateFFTDrawsUI(int draws) { lblFFTDraws.Text = "FFT Updates: " + draws; }
        public void ClearNotesList() { lstChords.Items.Clear(); }
        public void PrintChord(string text) { lstChords.Items.Add(text); }
        public void PlotNote(string name, double freq, double gain, Color color, bool isBold) { spFFT.plt.PlotText(name, freq, gain, color, fontSize: 11, bold: isBold); }
        public void SetKeyText(string text) { lblKey.Text = text; }
        public void SetModeText(string text) { lblMode.Text = text; }
        public void SetErrorText(string text) { lblError.Text = text; }
        public void SetTimerInterval(int interval) { timerFFT.Interval = interval; }
        public void SetExecTimeText(int time) { lblExeTime.Text = "Execution Time: " + time + " ms"; }
        public void RenderSpectrum() { spFFT.Render(); }
        public void SetSelectTime(double seconds) { txtSelectTime.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:fff"); }
        public void SetPlayBtnText(string text) { btnPlay.Text = text; }

        public bool IsShowAllChordsChecked() { return chbAllChords.Checked; }
        public AppController GetApp() { return app; }

        private void perferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm prefs = new PreferencesForm(this);
            prefs.ShowDialog();
        }

        public void UpdateUI()
        {
            //if (Prefs.SPECTRUM_AA == 1)
            //    spFFT.plt.AntiAlias(true);
            //else if (Prefs.SPECTRUM_AA == 0)
            //    spFFT.plt.AntiAlias(false);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            app.TriggerStop();
        }
    }
}
