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

namespace MusicAnalyser
{
    public partial class Form1 : Form
    {
        public DirectSoundOut Output { get; set; }
        public int fftZoom = 1000;
        private AppController app;

        private int selectedScript;
        private Point filterDragPoint;

        public Form1()
        {
            InitializeComponent();
            app = new AppController(this);
            SetupFFTPlot();
            barVolume.Value = 10;
            SetModeText("");
            btnFilterDrag.Location = new Point(spFFT.Location.X + spFFT.Width / 2, spFFT.Location.Y + spFFT.Height / 2);
            btnFilterDrag.Draggable(true);
            flpScripts.Controls.Add(new ScriptSelector(this) { Parent = flpScripts, Label = "Script " + flpScripts.Controls.Count });
            flpScripts.Controls.Add(new ScriptSelector(this) { Parent = flpScripts, Label = "Script " + flpScripts.Controls.Count });
            OnSelectorChange();
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(segMode.SelectedIndex == 1)
                app.Step(true);
            else if (!app.Opened)
                app.TriggerOpenFile();
            else
                app.TriggerClose();
        }

        public void SetupPlaybackUI(WaveStream audioGraph, string filename, bool wasRecording)
        {
            Output = new DirectSoundOut();
            cwvViewer.WaveStream = audioGraph;
            cwvViewer.FitToScreen();
            cwvViewer.BackColor = SystemColors.Control;
            segMode.SelectedIndex = 0;
            SetUIState();
            if (wasRecording)
            {
                saveRecordingToolStripMenuItem.Enabled = true;
                this.Text = "Music Analyser - Live Mode - Playback";
            }
            else
                this.Text = "Music Analyser - " + filename;
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
            if(segMode.SelectedIndex == 0)
            {
                if(app.Opened)
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
                    if (Output.PlaybackState == PlaybackState.Playing)
                        segMode.Enabled = false;
                    else
                        segMode.Enabled = true;
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
            }
            else if(segMode.SelectedIndex == 1)
            {
                chbFilter.Enabled = false;
                if (app.Opened && app.ScriptSelectionValid)
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
            if (app.Mode == 2)
                app.TriggerLiveModeStartStop();
            else
                app.TriggerPlayPause();
        }

        public void ClearUI()
        {
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
            barVolume.Value = 10;
            barTempo.Value = 10;
            barPitch.Value = 50;
            app.PitchChange(barPitch.Value);
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
            AudioSource source = app.AudioSource;

            while (app.Opened)
            {
                if (source.AudioStream.Position >= source.AudioStream.Length)
                {
                    InvokeUI(() => app.TriggerStop());
                    app.SetStarted(false);
                    break;
                }

                if (Output.PlaybackState == PlaybackState.Playing)
                {
                    if (!app.IsStarted() && chbFollow.Checked)
                    {
                        cwvViewer.LeftSample = 0;
                        cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                        cwvViewer.Zoom();
                    }

                    if (source.AudioStream.Position >= cwvViewer.SelectSample * cwvViewer.BytesPerSample * cwvViewer.WaveStream.WaveFormat.Channels)
                    {
                        if (cwvViewer.GetIsLooping() && source.AudioStream.Position >= cwvViewer.LoopEndSample * cwvViewer.BytesPerSample * cwvViewer.WaveStream.WaveFormat.Channels)
                        {
                            app.LoopPlayback();
                        }
                    }

                    app.SetStarted(true);
                    index++;
                    index = index % Prefs.UI_DELAY_FACTOR;

                    if (index == Prefs.UI_DELAY_FACTOR - 1)
                    {
                        InvokeUI(() => SetTimeStamp(source.AudioStream.CurrentTime));
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
                        else if (currentSample >= cwvViewer.RightSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = cwvViewer.RightSample;
                            cwvViewer.RightSample = cwvViewer.LeftSample + (Prefs.FOLLOW_SECS * source.AudioStream.WaveFormat.SampleRate);
                            cwvViewer.Zoom();
                        }
                        else if(currentSample <= cwvViewer.LeftSample && chbFollow.Checked)
                        {
                            cwvViewer.LeftSample = Math.Max(currentSample - (Prefs.FOLLOW_SECS / 10) * source.AudioStream.WaveFormat.SampleRate, 0);
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
            switch (barZoom.Value)
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
            if (dataFft.Length < fftZoom)
                fftZoom = dataFft.Length;
            if(avgGain >= 0)
                spFFT.plt.Axis(0, fftZoom, 0, maxGain + Math.Abs(maxGain * 0.1));
            else
                spFFT.plt.Axis(0, fftZoom, avgGain - 5, maxGain + 10);
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

        private void perferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm prefs = new PreferencesForm(this);
            prefs.ShowDialog();
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
            if (app.Opened && segMode.SelectedIndex == 2)
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
                case Keys.Back:
                    if (Output != null)
                        app.TriggerStop();
                    break;
                case Keys.Escape:
                    if (Output != null)
                        app.TriggerClose();
                    break;
                case Keys.O:
                    app.TriggerOpenFile();
                    break;
                case Keys.Tab:
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
                    tblSettings.Controls.Add(new Label() { Text = setting[2], MinimumSize = new Size(tblSettings.Size.Width / 2, 0),
                        MaximumSize = new Size(tblSettings.Size.Width / 4 * 3, 0), AutoSize = true }, 0, tblSettings.RowCount - 1);
                    if (setting[1] == "int")
                    {
                        var control = new NumericUpDown()
                        {
                            Minimum = int.Parse(setting[3]),
                            Maximum = int.Parse(setting[4]),
                            Value = int.Parse(setting[0]),
                            Size = new Size(100, 20),
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
                            Size = new Size(100, 20)
                        };
                        control.ValueChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                    else if(setting[1] == "enum")
                    {
                        var control = new ComboBox();
                        string[] options = setting[3].Split('|');
                        control.Items.AddRange(options);
                        control.SelectedItem = setting[0];
                        control.SelectedIndexChanged += new EventHandler(SettingChanged);
                        tblSettings.Controls.Add(control, 1, tblSettings.RowCount - 1);
                    }
                    else
                    {
                        var control = new TextBox() { Text = setting[0] };
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
    }
}
