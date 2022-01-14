using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using MusicAnalyser.App;
using System.Collections.Generic;
using MusicAnalyser.UI;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Syncfusion.WinForms.Core.Utils;
using System.Drawing.Text;

namespace MusicAnalyser
{
    public partial class Form1 : Form
    {
        public static PrivateFontCollection fonts = new PrivateFontCollection();
        public DirectSoundOut Output { get; set; }
        public double spectrumStartX = 0;
        private AppController app;

        private int selectedScript;
        private Point filterDragPoint;
        private Point musicPanelLocation, controlPanelLocation, spectrumLocation;
        private BusyIndicator loadingIndicator = new BusyIndicator();

        public Form1()
        {
            LoadResources();
            InitializeComponent();
            app = new AppController(this);
            SetupFFTPlot();
            barVolume.Value = 10;
            SetModeText("");
            flpScripts.AutoScroll = false;
            flpScripts.HorizontalScroll.Enabled = false;
            flpScripts.HorizontalScroll.Visible = false;
            flpScripts.HorizontalScroll.Maximum = 0;
            flpScripts.AutoScroll = true;
            musicPanelLocation = pnlMusic.Location;
            controlPanelLocation = pnlSpectrumControls.Location;
            spectrumLocation = spFFT.Location;
            btnFilterDrag.Draggable(true);
            flpScripts.Controls.Add(new ScriptSelector(this) { Parent = flpScripts, Label = "Script " + flpScripts.Controls.Count });
            flpScripts.Controls.Add(new ScriptSelector(this) { Parent = flpScripts, Label = "Script " + flpScripts.Controls.Count });
            OnSelectorChange();
        }

        private void LoadResources()
        {
            loadingIndicator.Image = Image.FromFile("Resources\\loading.gif");
            fonts.AddFontFile("Resources\\OpenSans-Regular.ttf");
            fonts.AddFontFile("Resources\\OpenSans-SemiBold.ttf");
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(segMode.SelectedIndex == 1)
                app.Step(true);
            else if (!app.AudioOpened && !app.SpectrogramOpened)
                app.TriggerOpenFile();
            else
                app.TriggerClose();
        }

        public void SetupPlaybackUI(WaveStream audioGraph, bool wasRecording)
        {
            Output = new DirectSoundOut();
            cwvViewer.WaveStream = audioGraph;
            cwvViewer.FitToScreen();
            cwvViewer.BackColor = SystemColors.Control;
            cwvViewer.StartPosition = 0;
            segMode.SelectedIndex = 0;
            SetUIState();
            if (wasRecording)
            {
                saveRecordingToolStripMenuItem.Enabled = true;
                this.Text = "Music Analyser - Live Mode - Playback";
            }
            else
                this.Text = "Music Analyser - " + app.AudioSource.Filename;
        }

        public void SetUIState()
        {
            if(segMode.SelectedIndex == 0)
            {
                btnOpenClose.Text = "Close";
                btnPlay.Text = "Play";
                btnStop.Text = "Stop";
                btnOpenClose.Enabled = true;
                btnPlay.Visible = true;
                numStepVal.Visible = false;
                lblStep.Visible = false;
                closeToolStripMenuItem.Enabled = true;
                openToolStripMenuItem.Enabled = false;
                barVolume.Enabled = true;
                barTempo.Enabled = true;
                barPitch.Enabled = true;
                numPitch.Enabled = true;
                lblPlayTime.Text = "Playback Time:";
                lblSelectTime.Text = "Select Time:";
                lblLoopDuration.Visible = true;
                txtSelectTime.Visible = true;
                txtLoopTime.Visible = true;
                prbLevelMeter.Visible = false;
                SetSelectTime(0);
                SetLoopTime(0);
                chbFollow.Enabled = true;
                app.Mode = 0;
            }
            else if(segMode.SelectedIndex == 1)
            {
                btnOpenClose.Text = "Back";
                btnStop.Text = "Forward";
                btnPlay.Enabled = false;
                btnPlay.Visible = false;
                numStepVal.Visible = true;
                lblStep.Visible = true;
                barVolume.Enabled = false;
                barTempo.Enabled = false;
                numPitch.Enabled = false;
            }
            else if(segMode.SelectedIndex == 2)
            {
                this.Text = "Music Analyser - Record Mode";
                cwvViewer.BackColor = SystemColors.Control;
                btnOpenClose.Enabled = false;
                closeToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                btnStop.Enabled = false;
                barVolume.Enabled = false;
                barTempo.Enabled = false;
                numPitch.Enabled = false;
                chbFollow.Enabled = false;
                btnPlay.Enabled = true;
                btnPlay.Text = "Start Recording";
                lblPlayTime.Text = "Recording Time:";
                lblSelectTime.Text = "Recording Level:";
                lblLoopDuration.Visible = false;
                txtSelectTime.Visible = false;
                txtLoopTime.Visible = false;
                prbLevelMeter.Visible = true;
                btnOpenClose.Text = "Open";
                btnStop.Text = "Stop";
                btnPlay.Visible = true;
                numStepVal.Visible = false;
                lblStep.Visible = false;
            }
            CheckAppState();
        }

        public void CheckAppState()
        {
            if (segMode.SelectedIndex == 0)
            {
                if(app.AudioOpened)
                {
                    btnOpenClose.Text = "Close";
                    btnOpenClose.Enabled = true;
                    chbFilter.Enabled = true;
                    if (app.ScriptSelectionValid)
                    {
                        btnPlay.Enabled = true;
                        btnStop.Enabled = true;
                        stopToolStripMenuItem.Enabled = true;
                        playToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        btnPlay.Enabled = false;
                        btnStop.Enabled = false;
                        stopToolStripMenuItem.Enabled = false;
                        playToolStripMenuItem.Enabled = false;
                    }
                    if (Output != null)
                    {
                        if (Output.PlaybackState == PlaybackState.Playing)
                            segMode.Enabled = false;
                        else
                            segMode.Enabled = true;
                        if (Output.PlaybackState == PlaybackState.Paused || Output.PlaybackState == PlaybackState.Stopped && app.GetSpectrogramHandler().Spectrogram.Frames.Count > 0)
                        {
                            if(!app.SpectrogramOpened)
                                clearSpecToolStripMenuItem.Enabled = true;
                            btnViewSpec.Enabled = true;
                        }
                        else
                        {
                            clearSpecToolStripMenuItem.Enabled = false;
                            btnViewSpec.Enabled = false;
                        }
                    }
                }
                else
                {
                    cwvViewer.BackColor = SystemColors.ControlLight;
                    btnOpenClose.Text = "Open";
                    btnOpenClose.Enabled = true;
                    btnPlay.Enabled = false;
                    btnStop.Enabled = false;
                    stopToolStripMenuItem.Enabled = false;
                    playToolStripMenuItem.Enabled = false;
                    chbFilter.Enabled = false;
                }
                if (app.SpectrogramOpened)
                {
                    btnOpenClose.Text = "Close";
                    segMode.Enabled = false;
                    if (app.AudioOpened)
                    {
                        chbFilter.Enabled = true;
                        btnPlay.Enabled = true;
                        btnStop.Enabled = true;
                        stopToolStripMenuItem.Enabled = true;
                        playToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        chbFilter.Enabled = false;
                        btnPlay.Enabled = false;
                        btnStop.Enabled = false;
                        stopToolStripMenuItem.Enabled = false;
                        playToolStripMenuItem.Enabled = false;
                    }
                }
                if (!app.AudioOpened && !app.SpectrogramOpened)
                {
                    btnOpenClose.Text = "Open";
                    btnOpenClose.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = false;
                    openSpecToolStripMenuItem.Enabled = true;
                    segMode.Enabled = true;
                }
                else
                    openSpecToolStripMenuItem.Enabled = false;
            }
            else if(segMode.SelectedIndex == 1)
            {
                chbFilter.Enabled = false;
                if (app.AudioOpened && app.ScriptSelectionValid)
                {
                    btnOpenClose.Enabled = true;
                    btnStop.Enabled = true;
                }
                else
                {
                    btnOpenClose.Enabled = false;
                    btnStop.Enabled = false;
                }
            }
            else if(segMode.SelectedIndex == 2)
            {
                chbFilter.Enabled = false;
                if (app.IsRecording)
                    btnPlay.Text = "Stop Recording";
                else
                    btnPlay.Text = "Start Recording";
            }
            if (!Prefs.STORE_SPEC_DATA && !app.SpectrogramOpened)
            {
                btnViewSpec.Enabled = false;
                btnSpecEnlarge.Enabled = false;
            }
        }

        public bool SelectFile(out OpenFileDialog dialog)
        {
            dialog = new OpenFileDialog();
            dialog.Filter = "Audio/Spectrogram Files (*.wav, *.mp3, *.spec)|*.wav; *.mp3; *.spec;";

            if (dialog.ShowDialog() != DialogResult.OK)
                return false;
            else
                return true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (app.Mode == 2)
                app.TriggerLiveModeStartStop();
            else
                app.TriggerPlayPause();
        }

        public void ClearUI()
        {
            app.CloseSpectrogram();
            saveSpecToolStripMenuItem.Enabled = false;
            openSpecToolStripMenuItem.Enabled = true;
            saveSpecImageToolStripMenuItem.Enabled = false;
            clearSpecToolStripMenuItem.Enabled = false;
            btnViewSpec.Enabled = false;
            btnSpecEnlarge.Enabled = false;
            chbFilter.Checked = false;
            this.Text = "Music Analyser";
            lstChords.Items.Clear();
            spFFT.plt.Clear();
            spFFT.Render();
            cwvViewer.WaveStream = null;
            cwvViewer.SelectSample = 0;
            cwvViewer.LoopEndSample = 0;
            cwvViewer.Overlay.Controls.Clear();
            cwvViewer.Overlay.ResetOverlay();
            cwvViewer.BackColor = SystemColors.ControlLight;
            txtPlayTime.Text = "";
            txtSelectTime.Text = "";
            txtLoopTime.Text = "";
            SetErrorText("+ 0 Cents");
            SetExecTimeText(0);
            btnStop.Enabled = false;
            stopToolStripMenuItem.Enabled = false;
            btnOpenClose.Enabled = true;
            closeToolStripMenuItem.Enabled = false;
            btnPlay.Enabled = false;
            btnPlay.Text = "Play";
            playToolStripMenuItem.Enabled = false;
            playToolStripMenuItem.Text = "Play";
            openToolStripMenuItem.Enabled = true;
            btnOpenClose.Text = "Open";
            barTempo.Enabled = false;
            barVolume.Enabled = false;
            barPitch.Enabled = false;
            numPitch.Enabled = false;
            barVolume.Value = 10;
            barTempo.Value = 16;
            barPitch.Value = 50;
            numPitch.Value = 0;
            app.PitchSyncChange(barPitch.Value);
            saveRecordingToolStripMenuItem.Enabled = false;
            segMode.SelectedIndex = 0;
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
            // Variable references that do not change - improves performance
            AudioSource source = app.AudioSource;
            WaveChannel32 audioStream = source.AudioStream;
            WaveFormat waveFormat = audioStream.WaveFormat;
            int channels = cwvViewer.WaveStream.WaveFormat.Channels;
            int bytesPerSample = cwvViewer.BytesPerSample;

            while (app.AudioOpened)
            {
                if (audioStream.Position >= audioStream.Length)
                {
                    InvokeUI(() => app.TriggerStop());
                    app.SetStarted(false);
                    break;
                }

                if (Output.PlaybackState == PlaybackState.Playing && !app.SpectrogramOpened)
                {
                    if (!app.IsStarted() && chbFollow.Checked)
                    {
                        cwvViewer.LeftSample = 0;
                        cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * waveFormat.SampleRate);
                        cwvViewer.Zoom();
                    }

                    if (audioStream.Position >= cwvViewer.SelectSample * bytesPerSample * channels)
                    {
                        if (cwvViewer.GetIsLooping() && audioStream.Position >= cwvViewer.LoopEndSample * bytesPerSample * channels)
                        {
                            app.LoopPlayback();
                        }
                    }

                    app.SetStarted(true);
                    index++;
                    index %= Prefs.UI_DELAY_FACTOR;

                    if (index == Prefs.UI_DELAY_FACTOR - 1)
                    {
                        InvokeUI(() => SetTimeStamp(audioStream.CurrentTime));
                        currentSample = audioStream.Position / 8;

                        if (currentSample > cwvViewer.LeftSample && currentSample < cwvViewer.RightSample)
                        {
                            currentPos = cwvViewer.GetWaveformPadding() + (int)(currentSample - cwvViewer.LeftSample) / cwvViewer.SamplesPerPixel;

                            if (currentPos != previousPos)
                            {
                                InvokeUI(() => cwvViewer.Overlay.MovePosIndicator(currentPos));
                                previousPos = currentPos;
                            }
                        }
                        else if (currentSample >= cwvViewer.RightSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = cwvViewer.RightSample;
                            cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * waveFormat.SampleRate);
                            cwvViewer.Zoom();
                        }
                        else if(currentSample <= cwvViewer.LeftSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = Math.Max(currentSample - (Prefs.FOLLOW_SECS / 10) * waveFormat.SampleRate, 0);
                            cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * waveFormat.SampleRate);
                            cwvViewer.Zoom();
                        }
                    }
                }
                else
                    Thread.Sleep(100); // Prevents idle CPU usage
            }
        }

        public void DisplayFFT(double[] dataFft, double fftScale, double avgGain, double maxGain)
        {
            spFFT.plt.Clear();
            spFFT.plt.PlotSignal(dataFft, fftScale, markerSize: 0);
            spectrumStartX = (double)numZoomLow.Value;
            if(avgGain >= 0)
                spFFT.plt.Axis(spectrumStartX, (double)numZoomHigh.Value, 0, maxGain + Math.Abs(maxGain * 0.1));
            else
                spFFT.plt.Axis(spectrumStartX, (double)numZoomHigh.Value, avgGain - 5, maxGain + 10);
            spFFT.plt.Ticks(useMultiplierNotation: false, useExponentialNotation: false);
        }

        public void UpdateNoteOccurencesUI(string noteName, double percent, Color noteColor)
        {
            switch (noteName)
            {
                case "C":
                    lblC.Text = "C   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblC.ForeColor = noteColor;
                    return;
                case "Db":
                    lblDb.Text = "Db -  " + string.Format("{0:0.00}", percent) + "%";
                    lblDb.ForeColor = noteColor;
                    return;
                case "D":
                    lblD.Text = "D   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblD.ForeColor = noteColor;
                    return;
                case "Eb":
                    lblEb.Text = "Eb -  " + string.Format("{0:0.00}", percent) + "%";
                    lblEb.ForeColor = noteColor;
                    return;
                case "E":
                    lblE.Text = "E   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblE.ForeColor = noteColor;
                    return;
                case "F":
                    lblF.Text = "F   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblF.ForeColor = noteColor;
                    return;
                case "Gb":
                    lblGb.Text = "Gb -  " + string.Format("{0:0.00}", percent) + "%";
                    lblGb.ForeColor = noteColor;
                    return;
                case "G":
                    lblG.Text = "G   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblG.ForeColor = noteColor;
                    return;
                case "Ab":
                    lblAb.Text = "Ab -  " + string.Format("{0:0.00}", percent) + "%";
                    lblAb.ForeColor = noteColor;
                    return;
                case "A":
                    lblA.Text = "A   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblA.ForeColor = noteColor;
                    return;
                case "Bb":
                    lblBb.Text = "Bb -  " + string.Format("{0:0.00}", percent) + "%";
                    lblBb.ForeColor = noteColor;
                    return;
                case "B":
                    lblB.Text = "B   -  " + string.Format("{0:0.00}", percent) + "%";
                    lblB.ForeColor = noteColor;
                    return;
            }
        }

        private void timerFFT_Tick(object sender, EventArgs e)
        {
            if (Output == null)
                Output = new DirectSoundOut();
            app.RunAnalysis();
        }

        private void SetupFFTPlot()
        {
            spFFT.plt.Title("Frequency Spectrum", fontName: "Open Sans");
            spFFT.plt.YLabel("Magnitude", fontName: "Open Sans", fontSize: 12);
            spFFT.plt.XLabel("Frequency (Hz)", fontName: "Open Sans", fontSize: 12);
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

        private void numPitch_ValueChanged(object sender, EventArgs e)
        {
            app.PitchChange((float)numPitch.Value);
        }

        private void barPitch_Scroll(object sender, EventArgs e)
        {
            app.PitchSyncChange(barPitch.Value);
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
        public void ClearNotesList() { lstChords.Items.Clear(); }
        public void PrintChord(string text) { lstChords.Items.Add(text); }
        public void PlotNote(string name, double freq, double gain, Color color, bool isBold) { spFFT.plt.PlotText(name, freq, gain, color, fontName: "Open Sans", fontSize: 16, bold: isBold); }
        public void SetKeyText(string text) { lblKey.Text = text; }
        public void SetModeText(string text) { lblMode.Text = text; }
        public void SetErrorText(string text) { lblError.Text = text; }
        public void SetTimerInterval(int interval) { timerFFT.Interval = interval; }
        public void SetExecTimeText(int time) { lblExeTime.Text = "Execution Time: " + time + " ms"; }
        public void RenderSpectrum() { spFFT.Render(); }
        public void SetSelectTime(double seconds) { txtSelectTime.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:fff"); }
        public void SetLoopTime(double seconds) { txtLoopTime.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:fff"); }
        public void SetPlayBtnText(string text) { btnPlay.Text = text; }
        public bool IsShowAllChordsChecked() { return chbAllChords.Checked; }
        public void SetTimeStamp(TimeSpan time) { txtPlayTime.Text = time.ToString(@"mm\:ss\:fff"); }
        public AppController GetApp() { return app; }
        public float GetPitchValue() { return (float)numPitch.Value; }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm prefs = new PreferencesForm(this);
            prefs.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }
        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://musicanalyser-2893a.web.app/Index.html");
        }

        public void UpdateUI()
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (segMode.SelectedIndex == 1)
                app.Step(false);
            else
                app.TriggerStop();
        }

        private void segMode_IndexChanged(object sender, EventArgs e)
        {
            if (app.AudioOpened && segMode.SelectedIndex == 2)
                segMode.SelectedIndex = 0;
            else if (app.Mode != 2 && segMode.SelectedIndex == 2)
                app.EnableLiveMode();
            else if (app.Mode == 2 && segMode.SelectedIndex != 2)
                app.ExitLiveMode();
            else
                SetUIState();
        }

        public void OnRecordDataAvailable(byte[] data, float maxLevel)
        {
            this.Text = "Music Analyser - Record Mode - Recording";
            cwvViewer.WaveStream = new RawSourceWaveStream(new MemoryStream(data), new WaveFormat(48000, 2));
            cwvViewer.FitToScreen();
            txtPlayTime.Text = TimeSpan.FromSeconds((double)data.Length / (48000 * 2 * 2)).ToString(@"mm\:ss\:fff");
            prbLevelMeter.Value = (int)(maxLevel * 100);
        }

        private void saveRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "MP3 File (*.mp3)|*.mp3;";

            if (saveDialog.ShowDialog() == DialogResult.OK)
                app.SaveRecording(saveDialog.FileName);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.KeyCode)
            {
                case Keys.Space:
                    if (Output != null)
                        app.TriggerPlayPause();
                    break;
                case Keys.F2:
                    if (Output != null)
                        app.TriggerStop();
                    break;
                case Keys.Escape:
                    if (Output != null)
                        app.TriggerClose();
                    break;
                case Keys.F1:
                    app.TriggerOpenFile();
                    break;
                case Keys.ShiftKey:
                    if (Output != null)
                        cwvViewer.FitToScreen();
                    break;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app.TriggerClose();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            app.TriggerStop();
        }

        // Scripts interface

        private void btnApplyScripts_Click(object sender, EventArgs e)
        {
            if (app.CheckSelectionValidity(GetSelectionDict(), out string message))
            {
                app.ApplyScripts(GetSelectionDict());
                app.ApplyScriptSettings(selectedScript, GetSettingValues());
                ApplySuccessful();
            }
        }

        private void ApplySuccessful()
        {
            lblSelMessage.ForeColor = Color.LimeGreen;
            lblSelMessage.Text = "Applied!";
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                InvokeUI(() => {
                    lblSelMessage.Text = "";
                    lblSelMessage.ForeColor = Color.Crimson;
                });
            });
            btnApplyScripts.Enabled = false;
            CheckAppState();
        }

        public Dictionary<int, int> GetSelectionDict()
        {
            Dictionary<int, int> selectionDict = new Dictionary<int, int>();
            for (int i = 0; i < flpScripts.Controls.Count; i++)
            {
                ScriptSelector selector = (ScriptSelector)flpScripts.Controls[i];
                selectionDict.Add(i, selector.DropDown.SelectedIndex);
            }
            return selectionDict;
        }

        private void btnAddScript_Click(object sender, EventArgs e)
        {
            OnAddScript();
        }

        private void OnAddScript()
        {
            flpScripts.Controls.Add(new ScriptSelector(this) { Parent = flpScripts, Label = "Script " + flpScripts.Controls.Count});
            app.AddScript();
            OnSelectorChange();
        }

        public void OnSelectorChange()
        {
            btnSave.Enabled = false;
            if (app.CheckSelectionValidity(GetSelectionDict(), out string message))
            {
                btnApplyScripts.Enabled = true;
                lblSelMessage.Text = message;
            }
            else
            {
                btnApplyScripts.Enabled = false;
                lblSelMessage.Text = message;
            }
            CheckAppState();
        }

        public void SetScriptSelection(Dictionary<int, string> scripts, bool add)
        {
            if (add)
            {
                ScriptSelector selector = (ScriptSelector)flpScripts.Controls[flpScripts.Controls.Count - 1];
                AddScriptSelectionItems(selector, scripts);
                return;
            }
            foreach (ScriptSelector selector in flpScripts.Controls)
                AddScriptSelectionItems(selector, scripts);
        }

        public void SetAppliedScripts(Dictionary<int, int> selectionDict)
        {
            flpScripts.Controls.Clear();
            for(int i = 0; i < selectionDict.Count; i++)
            {
                OnAddScript();
                ScriptSelector selector = (ScriptSelector)flpScripts.Controls[i];
                selector.DropDown.SelectedIndex = selectionDict[i];
            }
            ApplySuccessful();
        }

        public void SetPresetSelection(string[] presetNames)
        {
            cbPresets.Items.Clear();
            cbPresets.Items.AddRange(presetNames);
        }

        public void AddScriptSelectionItems(ScriptSelector selector, Dictionary<int, string> scripts)
        {
            selector.DropDown.Items.Clear();
            for (int i = 0; i < scripts.Count; i++)
            {
                for (int j = 0; j < scripts.Count; j++)
                {
                    if (i == scripts.Keys.ElementAt(j))
                    {
                        selector.DropDown.Items.Add(scripts[i]);
                        break;
                    }
                }
            }
        }

        public void DisplayScriptSettings(int scriptIndex)
        {
            selectedScript = scriptIndex;
            tblSettings.Controls.Clear();
            tblSettings.RowCount = 0;
            tblSettings.HorizontalScroll.Maximum = 0;
            tblSettings.AutoScroll = false;
            tblSettings.VerticalScroll.Visible = false;
            tblSettings.AutoScroll = true;
            int scrollWidth = 10;

            Dictionary<string, string[]> settings = app.GetScriptSettings(scriptIndex);

            if (settings == null)
            {
                btnDefaults.Enabled = false;
                tblSettings.ColumnCount = 1;
                tblSettings.Controls.Add(new Label() { Text = "This script does not have settings.", AutoSize = true });
            }
            else
            {
                btnDefaults.Enabled = true;
                tblSettings.ColumnCount = 2;
                tblSettings.ColumnStyles[0] = new ColumnStyle(SizeType.AutoSize);
                foreach (string[] setting in settings.Values)
                {
                    tblSettings.RowCount++;
                    tblSettings.Controls.Add(new Label() { Text = setting[2], MinimumSize = new Size(tblSettings.Size.Width / 2 - scrollWidth, 0),
                        MaximumSize = new Size((int)(tblSettings.Size.Width / 1.75) - scrollWidth, 0), AutoSize = true }, 0, tblSettings.RowCount - 1);
                    if (setting[1] == "int")
                    {
                        var control = new NumericUpDown()
                        {
                            Minimum = int.Parse(setting[3]),
                            Maximum = int.Parse(setting[4]),
                            Value = int.Parse(setting[0]),
                            Size = new Size(95, 20),
                        };
                        control.ValueChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                    else if (setting[1] == "double")
                    {
                        var control = new NumericUpDown()
                        {
                            Minimum = (decimal)double.Parse(setting[3]),
                            Maximum = (decimal)double.Parse(setting[4]),
                            Value = (decimal)double.Parse(setting[0]),
                            DecimalPlaces = 2,
                            Size = new Size(95, 20)
                        };
                        control.ValueChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                    else if(setting[1] == "enum")
                    {
                        var control = new ComboBox();
                        control.Size = new Size(95, 20);
                        string[] options = setting[3].Split('|');
                        control.Items.AddRange(options);
                        control.SelectedItem = setting[0];
                        control.SelectedIndexChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                    else
                    {
                        var control = new TextBox() { Text = setting[0] };
                        control.Size = new Size(95, 20);
                        control.TextChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                }
                if (settings.Values.Count < 4)
                    tblSettings.RowCount++;
            }
        }

        private void SettingChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            if (app.ScriptSelectionValid)
                btnApplyScripts.Enabled = true;
        }

        public void ClearSettingsTable()
        {
            tblSettings.Controls.Clear();
            tblSettings.ColumnCount = 0;
            tblSettings.RowCount = 0;
        }

        public string[] GetSettingValues()
        {
            if (tblSettings.ColumnCount == 2)
            {
                List<string> settingsList = new List<string>();
                for (int i = 0; i < tblSettings.Controls.Count / Math.Max(1, tblSettings.ColumnCount); i++)
                    settingsList.Add(tblSettings.GetControlFromPosition(1, i).Text);

                return settingsList.ToArray();
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedScript != -1)
            {
                app.SaveScriptSettings(selectedScript);
                btnSave.Enabled = false;
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            if (selectedScript != -1)
            {
                app.SetDefaultSettingValues(selectedScript);
                btnSave.Enabled = true;
                if (app.ScriptSelectionValid)
                    btnApplyScripts.Enabled = true;
            }
        }

        private void btnSavePreset_Click(object sender, EventArgs e)
        {
            if (cbPresets.Text != "")
                app.SavePreset(cbPresets.Text);
        }

        private void cbPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            app.ApplyPreset(cbPresets.SelectedItem.ToString());
        }

        private void numStepVal_ValueChanged(object sender, EventArgs e)
        {
            app.StepMilliseconds = (int)numStepVal.Value;
        }

        private void btnFilterDrag_Move(object sender, EventArgs e)
        {
            if(btnFilterDrag.Location.X > spFFT.Location.X + 50 && btnFilterDrag.Location.X < spFFT.Location.X + spFFT.Width - 40
                && btnFilterDrag.Location.Y > spFFT.Location.Y + 20 && btnFilterDrag.Location.Y < spFFT.Location.Y + spFFT.Height - 60)
            {
                if (Output != null)
                {
                    lblFilterFreq.Location = new Point(btnFilterDrag.Location.X + 35, btnFilterDrag.Location.Y + 5);
                    double xCoord = spFFT.plt.CoordinateFromPixelX(btnFilterDrag.Location.X);
                    double yPercent = (double)Math.Abs(btnFilterDrag.Location.Y - (spFFT.Location.Y + spFFT.Height - 60)) / (spFFT.Height - 80);
                    app.GetFilterRange(xCoord, yPercent);
                }
            }
            else
            {
                btnFilterDrag.Location = filterDragPoint;
            }
            filterDragPoint = btnFilterDrag.Location;
        }

        private void chbFilter_CheckedChanged(object sender, EventArgs e)
        {
            if(chbFilter.Checked)
            {
                btnFilterDrag.Location = new Point(spFFT.Location.X + spFFT.Width / 2, spFFT.Location.Y + spFFT.Height / 2);
                btnFilterDrag.Enabled = true;
                btnFilterDrag.Visible = true;
                lblFilterFreq.Visible = true;
                btnFilterDrag_Move(null, null);
            }
            else
            {
                btnFilterDrag.Enabled = false;
                btnFilterDrag.Visible = false;
                lblFilterFreq.Visible = false;
                app.SetFilter(20000, 20, 10000, 1, 1);
            }
        }

        public void SetFilterText(string note, double freq) { lblFilterFreq.Text = note + "\n" + Math.Round(freq, 1) + " Hz"; }

        public void ShowLoadingIndicator(string message)
        {
            loadingIndicator.Show(cwvViewer);
            lblSpectrogram.Text = message;
            lblSpectrogram.Location = new Point(cwvViewer.Location.X + cwvViewer.Width / 2 - lblSpectrogram.Width / 2, cwvViewer.Location.Y + cwvViewer.Height / 2 + 50);
            lblSpectrogram.Visible = true;
            lblSpectrogram.BringToFront();
        }

        public void HideLoadingIndicator()
        {
            loadingIndicator.Hide();
            lblSpectrogram.Visible = false;
            lblSpectrogram.SendToBack();
        }

        public void ShowSpectrogramUI(bool fromFile)
        {
            HideLoadingIndicator();
            SetUIState();
            ResizeSpectrogramUI(true);
            segMode.SelectedIndex = 0;
            segMode.Enabled = false;
            saveSpecToolStripMenuItem.Enabled = true;
            openSpecToolStripMenuItem.Enabled = false;
            saveSpecImageToolStripMenuItem.Enabled = true;
            clearSpecToolStripMenuItem.Enabled = false;
            importSpecAudioToolStripMenuItem.Enabled = true;
            cwvViewer.Enabled = false;
            cwvViewer.Visible = false;
            specViewer.Enabled = true;
            specViewer.Visible = true;
            specViewer.BringToFront();
            btnViewSpec.Text = "Hide Spectrogram";
            btnSpecEnlarge.Enabled = true;
            chbNoteAnnotations.Enabled = true;
            chbChordKeyAnnotations.Enabled = true;
            app.Dsp.Analyser.DisposeAnalyser();
            app.TriggerStop();
            if (fromFile)
                btnViewSpec.Enabled = false;
        }

        public void HideSpectrogramUI()
        {
            specViewer.Visible = false;
            specViewer.Enabled = false;
            cwvViewer.Enabled = true;
            cwvViewer.Visible = true;
            specViewer.SendToBack();
            specViewer.Reset();
            ResizeSpectrogramUI(false);
            btnViewSpec.Text = "View Spectrogram";
            btnSpecEnlarge.Enabled = false;
            chbNoteAnnotations.Enabled = false;
            chbChordKeyAnnotations.Enabled = false;
            saveSpecToolStripMenuItem.Enabled = false;
            openSpecToolStripMenuItem.Enabled = false;
            saveSpecImageToolStripMenuItem.Enabled = false;
            clearSpecToolStripMenuItem.Enabled = true;
            importSpecAudioToolStripMenuItem.Enabled = false;
        }

        private void ResizeSpectrogramUI(bool show)
        {
            int scale = 1;
            if (!show)
                scale = 0;

            specViewer.Size = new Size(specViewer.Width, specViewer.Width / 3);
            pnlMusic.Location = new Point(pnlMusic.Location.X, musicPanelLocation.Y + (specViewer.Height - cwvViewer.Height) * scale);
            pnlSpectrumControls.Location = new Point(pnlSpectrumControls.Location.X, controlPanelLocation.Y + (specViewer.Height - cwvViewer.Height) * scale);
            spFFT.Location = new Point(spFFT.Location.X, spectrumLocation.Y + (specViewer.Height - cwvViewer.Height) * scale);
            
            int newHeight = spFFT.Location.Y + spFFT.Height + 60;
            if (this.WindowState != FormWindowState.Maximized)
                this.Height = newHeight;
        }

        public void ReassignSpectrogramViewer(SpectrogramViewer viewer)
        {
            specViewer = viewer;
            specViewer.SetNewParent(this);
            this.Controls.Add(specViewer);
            specViewer.Size = new Size((int)(specViewer.Width * 0.98), specViewer.Width / 3);
            btnSpecEnlarge.Enabled = true;
            ResizeSpectrogramUI(true);
        }

        private void btnSpecEnlarge_Click(object sender, EventArgs e)
        {
            if(specViewer.MySpectrogramHandler != null)
            {
                specViewer.Size = new Size((int)(specViewer.Width * 1.02), specViewer.Width / 3);
                SpectrogramWindow specWindow = new SpectrogramWindow(this, specViewer);
                specWindow.Show();
                btnSpecEnlarge.Enabled = false;
            }
        }

        private void btnViewSpec_Click(object sender, EventArgs e)
        {
            if (!specViewer.Enabled)
                app.LoadSpectrogram(false);
            else
                app.CloseSpectrogram();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (specViewer.Enabled)
                ResizeSpectrogramUI(true);
        }

        private void chbNoteAnnotations_CheckedChanged(object sender, EventArgs e)
        {
            specViewer.ShowNoteAnnotations = chbNoteAnnotations.Checked;
            specViewer.Refresh();
        }

        private void chbChordKeyAnnotations_CheckedChanged(object sender, EventArgs e)
        {
            specViewer.ShowChordKeyAnnotations = chbChordKeyAnnotations.Checked;
            specViewer.Refresh();
        }

        private void saveSpecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Spectrogram File (*.spec)|*.spec;";

            if (saveDialog.ShowDialog() == DialogResult.OK)
                app.SaveSpectrogram(saveDialog.FileName);
        }

        private void openSpecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Spectrogram File (*.spec)|*.spec;";

            if (openDialog.ShowDialog() == DialogResult.OK)
                app.OpenSpectrogram(openDialog.FileName);
        }

        private void saveSpecImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Bitmap File (*.bmp)|*.bmp;";

            if (saveDialog.ShowDialog() == DialogResult.OK)
                specViewer.SaveImage(saveDialog.FileName);
        }

        private void clearSpecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (app.Dsp.SpectrogramHandler != null && !app.SpectrogramOpened)
            {
                app.Dsp.ClearSpectrogramData();
                btnViewSpec.Enabled = false;
            }
        }

        private async void importSpecAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Audio Files (*.wav, *.mp3)|*.wav; *.mp3;";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                await Task.Run(() => app.LoadAudioSource(openDialog.FileName)); // Load audio async
                Output = new DirectSoundOut();
                app.AudioOpened = true;
                CheckAppState();
            }
        }
    }
}
