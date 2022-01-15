namespace MusicAnalyser
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOpenClose = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.spFFT = new ScottPlot.FormsPlot();
            this.timerFFT = new System.Windows.Forms.Timer(this.components);
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chbFollow = new System.Windows.Forms.CheckBox();
            this.txtPlayTime = new System.Windows.Forms.TextBox();
            this.lblPlayTime = new System.Windows.Forms.Label();
            this.barTempo = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spectrogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpecImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSpecAudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.docsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSelectTime = new System.Windows.Forms.Label();
            this.txtSelectTime = new System.Windows.Forms.TextBox();
            this.lblLoopDuration = new System.Windows.Forms.Label();
            this.txtLoopTime = new System.Windows.Forms.TextBox();
            this.prbLevelMeter = new System.Windows.Forms.ProgressBar();
            this.segMode = new XanderUI.XUISegment();
            this.numStepVal = new System.Windows.Forms.NumericUpDown();
            this.lblStep = new System.Windows.Forms.Label();
            this.lblFilterFreq = new System.Windows.Forms.Label();
            this.btnFilterDrag = new MusicAnalyser.UI.RoundButton();
            this.cwvViewer = new MusicAnalyser.CustomWaveViewer();
            this.specViewer = new MusicAnalyser.UI.SpectrogramViewer(this);
            this.pnlMusic = new System.Windows.Forms.Panel();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblBb = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblAb = new System.Windows.Forms.Label();
            this.lblG = new System.Windows.Forms.Label();
            this.lblGb = new System.Windows.Forms.Label();
            this.lblF = new System.Windows.Forms.Label();
            this.lblE = new System.Windows.Forms.Label();
            this.lblEb = new System.Windows.Forms.Label();
            this.lblD = new System.Windows.Forms.Label();
            this.lblDb = new System.Windows.Forms.Label();
            this.lblC = new System.Windows.Forms.Label();
            this.chbAllChords = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstChords = new System.Windows.Forms.ListBox();
            this.pnlSpectrumControls = new System.Windows.Forms.Panel();
            this.btnSpecEnlarge = new System.Windows.Forms.Button();
            this.btnViewSpec = new System.Windows.Forms.Button();
            this.chbNoteAnnotations = new System.Windows.Forms.CheckBox();
            this.chbChordKeyAnnotations = new System.Windows.Forms.CheckBox();
            this.chbFilter = new System.Windows.Forms.CheckBox();
            this.lblError = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.barPitch = new System.Windows.Forms.TrackBar();
            this.numZoomLow = new System.Windows.Forms.NumericUpDown();
            this.numZoomHigh = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.lblExeTime = new System.Windows.Forms.Label();
            this.pnlScripts = new System.Windows.Forms.Panel();
            this.flpScripts = new System.Windows.Forms.TableLayoutPanel();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.cbPresets = new System.Windows.Forms.ComboBox();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tblSettings = new System.Windows.Forms.TableLayoutPanel();
            this.btnApplyScripts = new System.Windows.Forms.Button();
            this.lblSelMessage = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAddScript = new System.Windows.Forms.Button();
            this.lblSpectrogram = new System.Windows.Forms.Label();
            this.numPitch = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepVal)).BeginInit();
            this.pnlMusic.SuspendLayout();
            this.pnlSpectrumControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).BeginInit();
            this.pnlScripts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenClose.Location = new System.Drawing.Point(13, 31);
            this.btnOpenClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenClose.Name = "btnOpenClose";
            this.btnOpenClose.Size = new System.Drawing.Size(75, 27);
            this.btnOpenClose.TabIndex = 0;
            this.btnOpenClose.Text = "Open";
            this.btnOpenClose.UseVisualStyleBackColor = true;
            this.btnOpenClose.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(13, 64);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(156, 27);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(93, 31);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 27);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // spFFT
            // 
            this.spFFT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spFFT.BackColor = System.Drawing.SystemColors.Control;
            this.spFFT.Location = new System.Drawing.Point(12, 510);
            this.spFFT.Margin = new System.Windows.Forms.Padding(5);
            this.spFFT.Name = "spFFT";
            this.spFFT.Size = new System.Drawing.Size(1433, 369);
            this.spFFT.TabIndex = 3;
            // 
            // timerFFT
            // 
            this.timerFFT.Interval = 17;
            this.timerFFT.Tick += new System.EventHandler(this.timerFFT_Tick);
            // 
            // barVolume
            // 
            this.barVolume.Enabled = false;
            this.barVolume.Location = new System.Drawing.Point(440, 34);
            this.barVolume.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barVolume.Maximum = 20;
            this.barVolume.Name = "barVolume";
            this.barVolume.Size = new System.Drawing.Size(199, 56);
            this.barVolume.TabIndex = 7;
            this.barVolume.Value = 10;
            this.barVolume.Scroll += new System.EventHandler(this.barVolume_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(448, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "0%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(525, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "50%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(605, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 19);
            this.label4.TabIndex = 10;
            this.label4.Text = "100%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(371, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 19);
            this.label5.TabIndex = 11;
            this.label5.Text = "Volume: ";
            // 
            // chbFollow
            // 
            this.chbFollow.AutoSize = true;
            this.chbFollow.Checked = true;
            this.chbFollow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbFollow.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbFollow.Location = new System.Drawing.Point(1604, 37);
            this.chbFollow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbFollow.Name = "chbFollow";
            this.chbFollow.Size = new System.Drawing.Size(135, 23);
            this.chbFollow.TabIndex = 13;
            this.chbFollow.Text = "Follow Playback";
            this.chbFollow.UseVisualStyleBackColor = true;
            // 
            // txtPlayTime
            // 
            this.txtPlayTime.Location = new System.Drawing.Point(1263, 37);
            this.txtPlayTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPlayTime.Name = "txtPlayTime";
            this.txtPlayTime.Size = new System.Drawing.Size(89, 22);
            this.txtPlayTime.TabIndex = 16;
            this.txtPlayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPlayTime
            // 
            this.lblPlayTime.AutoSize = true;
            this.lblPlayTime.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayTime.Location = new System.Drawing.Point(1150, 38);
            this.lblPlayTime.Name = "lblPlayTime";
            this.lblPlayTime.Size = new System.Drawing.Size(108, 19);
            this.lblPlayTime.TabIndex = 17;
            this.lblPlayTime.Text = "Playback Time:";
            // 
            // barTempo
            // 
            this.barTempo.Enabled = false;
            this.barTempo.Location = new System.Drawing.Point(728, 31);
            this.barTempo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barTempo.Maximum = 16;
            this.barTempo.Name = "barTempo";
            this.barTempo.Size = new System.Drawing.Size(199, 56);
            this.barTempo.TabIndex = 7;
            this.barTempo.Value = 16;
            this.barTempo.Scroll += new System.EventHandler(this.barTempo_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(660, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 19);
            this.label7.TabIndex = 11;
            this.label7.Text = "Tempo: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(731, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 19);
            this.label8.TabIndex = 8;
            this.label8.Text = "20%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(815, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 19);
            this.label9.TabIndex = 9;
            this.label9.Text = "60%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(893, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 19);
            this.label10.TabIndex = 10;
            this.label10.Text = "100%";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.spectrogramToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1832, 30);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveRecordingToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 26);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveRecordingToolStripMenuItem
            // 
            this.saveRecordingToolStripMenuItem.Enabled = false;
            this.saveRecordingToolStripMenuItem.Name = "saveRecordingToolStripMenuItem";
            this.saveRecordingToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.saveRecordingToolStripMenuItem.Text = "Save Recording...";
            this.saveRecordingToolStripMenuItem.Click += new System.EventHandler(this.saveRecordingToolStripMenuItem_Click);
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(81, 26);
            this.playbackToolStripMenuItem.Text = "Playback";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Enabled = false;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(123, 26);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(123, 26);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // spectrogramToolStripMenuItem
            // 
            this.spectrogramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSpecToolStripMenuItem,
            this.openSpecToolStripMenuItem,
            this.saveSpecImageToolStripMenuItem,
            this.importSpecAudioToolStripMenuItem,
            this.clearSpecToolStripMenuItem});
            this.spectrogramToolStripMenuItem.Name = "spectrogramToolStripMenuItem";
            this.spectrogramToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.spectrogramToolStripMenuItem.Text = "Spectrogram";
            // 
            // saveSpecToolStripMenuItem
            // 
            this.saveSpecToolStripMenuItem.Enabled = false;
            this.saveSpecToolStripMenuItem.Name = "saveSpecToolStripMenuItem";
            this.saveSpecToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.saveSpecToolStripMenuItem.Text = "Save Spectrogram...";
            this.saveSpecToolStripMenuItem.Click += new System.EventHandler(this.saveSpecToolStripMenuItem_Click);
            // 
            // openSpecToolStripMenuItem
            // 
            this.openSpecToolStripMenuItem.Name = "saveSpecToolStripMenuItem";
            this.openSpecToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.openSpecToolStripMenuItem.Text = "Open Spectrogram...";
            this.openSpecToolStripMenuItem.Click += new System.EventHandler(this.openSpecToolStripMenuItem_Click);
            // 
            // saveSpecImageToolStripMenuItem
            // 
            this.saveSpecImageToolStripMenuItem.Enabled = false;
            this.saveSpecImageToolStripMenuItem.Name = "saveSpecImageToolStripMenuItem";
            this.saveSpecImageToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.saveSpecImageToolStripMenuItem.Text = "Save Spectrogram Image...";
            this.saveSpecImageToolStripMenuItem.Click += new System.EventHandler(this.saveSpecImageToolStripMenuItem_Click);
            // 
            // importSpecAudioToolStripMenuItem
            // 
            this.importSpecAudioToolStripMenuItem.Enabled = false;
            this.importSpecAudioToolStripMenuItem.Name = "importSpecAudioToolStripMenuItem";
            this.importSpecAudioToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.importSpecAudioToolStripMenuItem.Text = "Import Spectrogram Audio...";
            this.importSpecAudioToolStripMenuItem.Click += new System.EventHandler(this.importSpecAudioToolStripMenuItem_Click);
            // 
            // clearSpecToolStripMenuItem
            // 
            this.clearSpecToolStripMenuItem.Enabled = false;
            this.clearSpecToolStripMenuItem.Name = "clearSpecToolStripMenuItem";
            this.clearSpecToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.clearSpecToolStripMenuItem.Text = "Clear Spectrogram Data";
            this.clearSpecToolStripMenuItem.Click += new System.EventHandler(this.clearSpecToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perferencesToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.docsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 26);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // perferencesToolStripMenuItem
            // 
            this.perferencesToolStripMenuItem.Name = "perferencesToolStripMenuItem";
            this.perferencesToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.perferencesToolStripMenuItem.Text = "Perferences...";
            this.perferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // docsToolStripMenuItem
            // 
            this.docsToolStripMenuItem.Name = "docsToolStripMenuItem";
            this.docsToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.docsToolStripMenuItem.Text = "Open Documentation...";
            this.docsToolStripMenuItem.Click += new System.EventHandler(this.docsToolStripMenuItem_Click);
            // 
            // lblSelectTime
            // 
            this.lblSelectTime.AutoSize = true;
            this.lblSelectTime.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectTime.Location = new System.Drawing.Point(1150, 66);
            this.lblSelectTime.Name = "lblSelectTime";
            this.lblSelectTime.Size = new System.Drawing.Size(90, 19);
            this.lblSelectTime.TabIndex = 24;
            this.lblSelectTime.Text = "Select Time:";
            // 
            // txtSelectTime
            // 
            this.txtSelectTime.Location = new System.Drawing.Point(1263, 65);
            this.txtSelectTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSelectTime.Name = "txtSelectTime";
            this.txtSelectTime.Size = new System.Drawing.Size(89, 22);
            this.txtSelectTime.TabIndex = 23;
            this.txtSelectTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLoopDuration
            // 
            this.lblLoopDuration.AutoSize = true;
            this.lblLoopDuration.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoopDuration.Location = new System.Drawing.Point(1379, 38);
            this.lblLoopDuration.Name = "lblLoopDuration";
            this.lblLoopDuration.Size = new System.Drawing.Size(108, 19);
            this.lblLoopDuration.TabIndex = 26;
            this.lblLoopDuration.Text = "Loop Duration:";
            // 
            // txtLoopTime
            // 
            this.txtLoopTime.Location = new System.Drawing.Point(1493, 37);
            this.txtLoopTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLoopTime.Name = "txtLoopTime";
            this.txtLoopTime.Size = new System.Drawing.Size(89, 22);
            this.txtLoopTime.TabIndex = 25;
            this.txtLoopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // prbLevelMeter
            // 
            this.prbLevelMeter.Location = new System.Drawing.Point(1263, 64);
            this.prbLevelMeter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.prbLevelMeter.Name = "prbLevelMeter";
            this.prbLevelMeter.Size = new System.Drawing.Size(268, 23);
            this.prbLevelMeter.TabIndex = 28;
            this.prbLevelMeter.Visible = false;
            // 
            // segMode
            // 
            this.segMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.segMode.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.segMode.Items = "Play Mode, Step Mode, Record Mode";
            this.segMode.Location = new System.Drawing.Point(175, 33);
            this.segMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.segMode.Name = "segMode";
            this.segMode.SegmentActiveTextColor = System.Drawing.Color.White;
            this.segMode.SegmentBackColor = System.Drawing.SystemColors.Control;
            this.segMode.SegmentColor = System.Drawing.Color.White;
            this.segMode.SegmentInactiveTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.segMode.SegmentStyle = XanderUI.XUISegment.Style.Android;
            this.segMode.SelectedIndex = 0;
            this.segMode.Size = new System.Drawing.Size(181, 58);
            this.segMode.TabIndex = 41;
            this.segMode.IndexChanged += new System.EventHandler(this.segMode_IndexChanged);
            // 
            // numStepVal
            // 
            this.numStepVal.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numStepVal.Location = new System.Drawing.Point(92, 66);
            this.numStepVal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numStepVal.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numStepVal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStepVal.Name = "numStepVal";
            this.numStepVal.Size = new System.Drawing.Size(75, 25);
            this.numStepVal.TabIndex = 42;
            this.numStepVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numStepVal.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numStepVal.Visible = false;
            this.numStepVal.ValueChanged += new System.EventHandler(this.numStepVal_ValueChanged);
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStep.Location = new System.Drawing.Point(13, 68);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(75, 19);
            this.lblStep.TabIndex = 43;
            this.lblStep.Text = "Step (ms):";
            this.lblStep.Visible = false;
            // 
            // lblFilterFreq
            // 
            this.lblFilterFreq.AutoSize = true;
            this.lblFilterFreq.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterFreq.ForeColor = System.Drawing.Color.Gray;
            this.lblFilterFreq.Location = new System.Drawing.Point(764, 654);
            this.lblFilterFreq.Name = "lblFilterFreq";
            this.lblFilterFreq.Size = new System.Drawing.Size(54, 19);
            this.lblFilterFreq.TabIndex = 46;
            this.lblFilterFreq.Text = "440 Hz";
            this.lblFilterFreq.Visible = false;
            // 
            // btnFilterDrag
            // 
            this.btnFilterDrag.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnFilterDrag.Enabled = false;
            this.btnFilterDrag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterDrag.Location = new System.Drawing.Point(728, 647);
            this.btnFilterDrag.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFilterDrag.Name = "btnFilterDrag";
            this.btnFilterDrag.Size = new System.Drawing.Size(29, 30);
            this.btnFilterDrag.TabIndex = 44;
            this.btnFilterDrag.TabStop = false;
            this.btnFilterDrag.UseVisualStyleBackColor = false;
            this.btnFilterDrag.Visible = false;
            this.btnFilterDrag.Move += new System.EventHandler(this.btnFilterDrag_Move);
            // 
            // cwvViewer
            // 
            this.cwvViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cwvViewer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cwvViewer.BytesPerSample = 0;
            this.cwvViewer.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cwvViewer.LeftSample = ((long)(0));
            this.cwvViewer.Location = new System.Drawing.Point(32, 114);
            this.cwvViewer.LoopEndSample = ((long)(0));
            this.cwvViewer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cwvViewer.Name = "cwvViewer";
            this.cwvViewer.PenColor = System.Drawing.Color.DodgerBlue;
            this.cwvViewer.PenWidth = 1F;
            this.cwvViewer.RightSample = ((long)(0));
            this.cwvViewer.SamplesPerPixel = 128;
            this.cwvViewer.SelectSample = ((long)(0));
            this.cwvViewer.Size = new System.Drawing.Size(1414, 302);
            this.cwvViewer.StartPosition = ((long)(0));
            this.cwvViewer.TabIndex = 2;
            this.cwvViewer.WaveStream = null;
            // 
            // specViewer
            // 
            this.specViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specViewer.Enabled = false;
            this.specViewer.Location = new System.Drawing.Point(32, 114);
            this.specViewer.MinimumSize = new System.Drawing.Size(600, 200);
            this.specViewer.MySpectrogramHandler = null;
            this.specViewer.Name = "specViewer";
            this.specViewer.Size = new System.Drawing.Size(1413, 750);
            this.specViewer.TabIndex = 48;
            this.specViewer.Visible = false;
            // 
            // pnlMusic
            // 
            this.pnlMusic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMusic.Controls.Add(this.lblMode);
            this.pnlMusic.Controls.Add(this.lblKey);
            this.pnlMusic.Controls.Add(this.lblB);
            this.pnlMusic.Controls.Add(this.lblBb);
            this.pnlMusic.Controls.Add(this.lblA);
            this.pnlMusic.Controls.Add(this.lblAb);
            this.pnlMusic.Controls.Add(this.lblG);
            this.pnlMusic.Controls.Add(this.lblGb);
            this.pnlMusic.Controls.Add(this.lblF);
            this.pnlMusic.Controls.Add(this.lblE);
            this.pnlMusic.Controls.Add(this.lblEb);
            this.pnlMusic.Controls.Add(this.lblD);
            this.pnlMusic.Controls.Add(this.lblDb);
            this.pnlMusic.Controls.Add(this.lblC);
            this.pnlMusic.Controls.Add(this.chbAllChords);
            this.pnlMusic.Controls.Add(this.label1);
            this.pnlMusic.Controls.Add(this.lstChords);
            this.pnlMusic.Location = new System.Drawing.Point(1469, 569);
            this.pnlMusic.Name = "pnlMusic";
            this.pnlMusic.Size = new System.Drawing.Size(322, 279);
            this.pnlMusic.TabIndex = 50;
            // 
            // lblMode
            // 
            this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(115, 253);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(48, 19);
            this.lblMode.TabIndex = 38;
            this.lblMode.Text = "Mode";
            // 
            // lblKey
            // 
            this.lblKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.Location = new System.Drawing.Point(23, 224);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(112, 19);
            this.lblKey.TabIndex = 37;
            this.lblKey.Text = "Predicted Key: ";
            // 
            // lblB
            // 
            this.lblB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblB.AutoSize = true;
            this.lblB.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblB.Location = new System.Drawing.Point(20, 189);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(34, 19);
            this.lblB.TabIndex = 35;
            this.lblB.Text = "B: 0";
            // 
            // lblBb
            // 
            this.lblBb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBb.AutoSize = true;
            this.lblBb.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBb.Location = new System.Drawing.Point(20, 172);
            this.lblBb.Name = "lblBb";
            this.lblBb.Size = new System.Drawing.Size(43, 19);
            this.lblBb.TabIndex = 34;
            this.lblBb.Text = "Bb: 0";
            // 
            // lblA
            // 
            this.lblA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblA.Location = new System.Drawing.Point(20, 156);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(34, 19);
            this.lblA.TabIndex = 33;
            this.lblA.Text = "A: 0";
            // 
            // lblAb
            // 
            this.lblAb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAb.AutoSize = true;
            this.lblAb.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAb.Location = new System.Drawing.Point(20, 138);
            this.lblAb.Name = "lblAb";
            this.lblAb.Size = new System.Drawing.Size(43, 19);
            this.lblAb.TabIndex = 32;
            this.lblAb.Text = "Ab: 0";
            // 
            // lblG
            // 
            this.lblG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblG.AutoSize = true;
            this.lblG.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblG.Location = new System.Drawing.Point(20, 121);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(35, 19);
            this.lblG.TabIndex = 31;
            this.lblG.Text = "G: 0";
            // 
            // lblGb
            // 
            this.lblGb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGb.AutoSize = true;
            this.lblGb.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGb.Location = new System.Drawing.Point(20, 104);
            this.lblGb.Name = "lblGb";
            this.lblGb.Size = new System.Drawing.Size(44, 19);
            this.lblGb.TabIndex = 36;
            this.lblGb.Text = "Gb: 0";
            // 
            // lblF
            // 
            this.lblF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblF.AutoSize = true;
            this.lblF.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblF.Location = new System.Drawing.Point(20, 87);
            this.lblF.Name = "lblF";
            this.lblF.Size = new System.Drawing.Size(32, 19);
            this.lblF.TabIndex = 30;
            this.lblF.Text = "F: 0";
            // 
            // lblE
            // 
            this.lblE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblE.AutoSize = true;
            this.lblE.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE.Location = new System.Drawing.Point(20, 71);
            this.lblE.Name = "lblE";
            this.lblE.Size = new System.Drawing.Size(33, 19);
            this.lblE.TabIndex = 29;
            this.lblE.Text = "E: 0";
            // 
            // lblEb
            // 
            this.lblEb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEb.AutoSize = true;
            this.lblEb.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEb.Location = new System.Drawing.Point(20, 53);
            this.lblEb.Name = "lblEb";
            this.lblEb.Size = new System.Drawing.Size(42, 19);
            this.lblEb.TabIndex = 28;
            this.lblEb.Text = "Eb: 0";
            // 
            // lblD
            // 
            this.lblD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblD.AutoSize = true;
            this.lblD.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblD.Location = new System.Drawing.Point(20, 36);
            this.lblD.Name = "lblD";
            this.lblD.Size = new System.Drawing.Size(35, 19);
            this.lblD.TabIndex = 27;
            this.lblD.Text = "D: 0";
            // 
            // lblDb
            // 
            this.lblDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDb.AutoSize = true;
            this.lblDb.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDb.Location = new System.Drawing.Point(20, 19);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(44, 19);
            this.lblDb.TabIndex = 26;
            this.lblDb.Text = "Db: 0";
            // 
            // lblC
            // 
            this.lblC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblC.AutoSize = true;
            this.lblC.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblC.Location = new System.Drawing.Point(20, 2);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(34, 19);
            this.lblC.TabIndex = 25;
            this.lblC.Text = "C: 0";
            // 
            // chbAllChords
            // 
            this.chbAllChords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbAllChords.AutoSize = true;
            this.chbAllChords.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbAllChords.Location = new System.Drawing.Point(174, 152);
            this.chbAllChords.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbAllChords.Name = "chbAllChords";
            this.chbAllChords.Size = new System.Drawing.Size(88, 23);
            this.chbAllChords.TabIndex = 24;
            this.chbAllChords.Text = "Show All";
            this.chbAllChords.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(169, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 19);
            this.label1.TabIndex = 23;
            this.label1.Text = "Chords Found:";
            // 
            // lstChords
            // 
            this.lstChords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstChords.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstChords.FormattingEnabled = true;
            this.lstChords.ItemHeight = 18;
            this.lstChords.Location = new System.Drawing.Point(173, 29);
            this.lstChords.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstChords.Name = "lstChords";
            this.lstChords.Size = new System.Drawing.Size(120, 94);
            this.lstChords.TabIndex = 22;
            // 
            // pnlSpectrumControls
            // 
            this.pnlSpectrumControls.Controls.Add(this.btnSpecEnlarge);
            this.pnlSpectrumControls.Controls.Add(this.btnViewSpec);
            this.pnlSpectrumControls.Controls.Add(this.chbNoteAnnotations);
            this.pnlSpectrumControls.Controls.Add(this.chbChordKeyAnnotations);
            this.pnlSpectrumControls.Controls.Add(this.chbFilter);
            this.pnlSpectrumControls.Controls.Add(this.lblError);
            this.pnlSpectrumControls.Controls.Add(this.label19);
            this.pnlSpectrumControls.Controls.Add(this.label14);
            this.pnlSpectrumControls.Controls.Add(this.label13);
            this.pnlSpectrumControls.Controls.Add(this.label12);
            this.pnlSpectrumControls.Controls.Add(this.label18);
            this.pnlSpectrumControls.Controls.Add(this.label16);
            this.pnlSpectrumControls.Controls.Add(this.label11);
            this.pnlSpectrumControls.Controls.Add(this.barPitch);
            this.pnlSpectrumControls.Controls.Add(this.numZoomLow);
            this.pnlSpectrumControls.Controls.Add(this.numZoomHigh);
            this.pnlSpectrumControls.Controls.Add(this.label20);
            this.pnlSpectrumControls.Controls.Add(this.lblExeTime);
            this.pnlSpectrumControls.Location = new System.Drawing.Point(32, 439);
            this.pnlSpectrumControls.Name = "pnlSpectrumControls";
            this.pnlSpectrumControls.Size = new System.Drawing.Size(1500, 77);
            this.pnlSpectrumControls.TabIndex = 51;
            // 
            // btnSpecEnlarge
            // 
            this.btnSpecEnlarge.Enabled = false;
            this.btnSpecEnlarge.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSpecEnlarge.Location = new System.Drawing.Point(1061, 42);
            this.btnSpecEnlarge.Name = "btnSpecEnlarge";
            this.btnSpecEnlarge.Size = new System.Drawing.Size(148, 29);
            this.btnSpecEnlarge.TabIndex = 65;
            this.btnSpecEnlarge.Text = "Undock";
            this.btnSpecEnlarge.UseVisualStyleBackColor = true;
            this.btnSpecEnlarge.Click += new System.EventHandler(this.btnSpecEnlarge_Click);
            // 
            // btnViewSpec
            // 
            this.btnViewSpec.Enabled = false;
            this.btnViewSpec.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewSpec.Location = new System.Drawing.Point(1061, 8);
            this.btnViewSpec.Name = "btnViewSpec";
            this.btnViewSpec.Size = new System.Drawing.Size(148, 29);
            this.btnViewSpec.TabIndex = 64;
            this.btnViewSpec.Text = "View Spectrogram";
            this.btnViewSpec.UseVisualStyleBackColor = true;
            this.btnViewSpec.Click += new System.EventHandler(this.btnViewSpec_Click);
            // 
            // chbAnnotations
            // 
            this.chbNoteAnnotations.Enabled = false;
            this.chbNoteAnnotations.Checked = true;
            this.chbNoteAnnotations.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbNoteAnnotations.Location = new System.Drawing.Point(1220, 15);
            this.chbNoteAnnotations.Name = "chbAnnotations";
            this.chbNoteAnnotations.Size = new System.Drawing.Size(180, 23);
            this.chbNoteAnnotations.TabIndex = 64;
            this.chbNoteAnnotations.Text = "Show Note Annotations";
            this.chbNoteAnnotations.UseVisualStyleBackColor = true;
            this.chbNoteAnnotations.CheckedChanged += new System.EventHandler(this.chbNoteAnnotations_CheckedChanged);
            // 
            // chbAnnotations
            // 
            this.chbChordKeyAnnotations.Enabled = false;
            this.chbChordKeyAnnotations.Checked = true;
            this.chbChordKeyAnnotations.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbChordKeyAnnotations.Location = new System.Drawing.Point(1220, 43);
            this.chbChordKeyAnnotations.Name = "chbAnnotations";
            this.chbChordKeyAnnotations.Size = new System.Drawing.Size(250, 23);
            this.chbChordKeyAnnotations.TabIndex = 64;
            this.chbChordKeyAnnotations.Text = "Show Chord/Key Annotations";
            this.chbChordKeyAnnotations.UseVisualStyleBackColor = true;
            this.chbChordKeyAnnotations.CheckedChanged += new System.EventHandler(this.chbChordKeyAnnotations_CheckedChanged);
            // 
            // chbFilter
            // 
            this.chbFilter.AutoSize = true;
            this.chbFilter.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbFilter.Location = new System.Drawing.Point(867, 12);
            this.chbFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chbFilter.Name = "chbFilter";
            this.chbFilter.Size = new System.Drawing.Size(166, 23);
            this.chbFilter.TabIndex = 63;
            this.chbFilter.Text = "Note Highlight Filter";
            this.chbFilter.UseVisualStyleBackColor = true;
            this.chbFilter.CheckedChanged += new System.EventHandler(this.chbFilter_CheckedChanged);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font(Form1.fonts.Families[1], 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(754, 12);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(70, 19);
            this.lblError.TabIndex = 62;
            this.lblError.Text = "+ 0 cents";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(176, 12);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 19);
            this.label19.TabIndex = 60;
            this.label19.Text = "Show:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(425, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 19);
            this.label14.TabIndex = 61;
            this.label14.Text = "Pitch Sync:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(666, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 19);
            this.label13.TabIndex = 59;
            this.label13.Text = "+50";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(591, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 19);
            this.label12.TabIndex = 58;
            this.label12.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(296, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 19);
            this.label18.TabIndex = 55;
            this.label18.Text = "to";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(388, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(24, 19);
            this.label16.TabIndex = 56;
            this.label16.Text = "Hz";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(497, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 19);
            this.label11.TabIndex = 57;
            this.label11.Text = "-50";
            // 
            // barPitch
            // 
            this.barPitch.Enabled = false;
            this.barPitch.Location = new System.Drawing.Point(499, 12);
            this.barPitch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.barPitch.Maximum = 100;
            this.barPitch.Name = "barPitch";
            this.barPitch.Size = new System.Drawing.Size(199, 56);
            this.barPitch.TabIndex = 52;
            this.barPitch.Value = 50;
            this.barPitch.Scroll += new System.EventHandler(this.barPitch_Scroll);
            // 
            // numZoomLow
            // 
            this.numZoomLow.Location = new System.Drawing.Point(232, 11);
            this.numZoomLow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numZoomLow.Name = "numZoomLow";
            this.numZoomLow.Size = new System.Drawing.Size(60, 22);
            this.numZoomLow.TabIndex = 53;
            this.numZoomLow.Minimum = 0;
            this.numZoomLow.Maximum = 4000;
            this.numZoomLow.Value = 0;
            // 
            // numZoomHigh
            // 
            this.numZoomHigh.Location = new System.Drawing.Point(324, 11);
            this.numZoomHigh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numZoomHigh.Name = "numZoomHigh";
            this.numZoomHigh.Size = new System.Drawing.Size(60, 22);
            this.numZoomHigh.TabIndex = 53;
            this.numZoomHigh.Minimum = 0;
            this.numZoomHigh.Maximum = 4000;
            this.numZoomHigh.Value = 1000;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(705, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(47, 19);
            this.label20.TabIndex = 51;
            this.label20.Text = "Error:";
            // 
            // lblExeTime
            // 
            this.lblExeTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExeTime.AutoSize = true;
            this.lblExeTime.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExeTime.Location = new System.Drawing.Point(13, 12);
            this.lblExeTime.Name = "lblExeTime";
            this.lblExeTime.Size = new System.Drawing.Size(151, 19);
            this.lblExeTime.TabIndex = 50;
            this.lblExeTime.Text = "Execution Time: 0 ms";
            // 
            // pnlScripts
            // 
            this.pnlScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlScripts.Controls.Add(this.flpScripts);
            this.pnlScripts.Controls.Add(this.btnSavePreset);
            this.pnlScripts.Controls.Add(this.label21);
            this.pnlScripts.Controls.Add(this.cbPresets);
            this.pnlScripts.Controls.Add(this.btnDefaults);
            this.pnlScripts.Controls.Add(this.btnSave);
            this.pnlScripts.Controls.Add(this.tblSettings);
            this.pnlScripts.Controls.Add(this.btnApplyScripts);
            this.pnlScripts.Controls.Add(this.lblSelMessage);
            this.pnlScripts.Controls.Add(this.label6);
            this.pnlScripts.Controls.Add(this.btnAddScript);
            this.pnlScripts.Location = new System.Drawing.Point(1451, 84);
            this.pnlScripts.Name = "pnlScripts";
            this.pnlScripts.Size = new System.Drawing.Size(369, 365);
            this.pnlScripts.TabIndex = 52;
            // 
            // flpScripts
            // 
            this.flpScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpScripts.AutoScroll = true;
            this.flpScripts.AutoScrollMinSize = new System.Drawing.Size(0, 5);
            this.flpScripts.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.flpScripts.ColumnCount = 1;
            this.flpScripts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.flpScripts.Location = new System.Drawing.Point(10, 86);
            this.flpScripts.Margin = new System.Windows.Forms.Padding(4);
            this.flpScripts.Name = "flpScripts";
            this.flpScripts.RowCount = 1;
            this.flpScripts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flpScripts.Size = new System.Drawing.Size(255, 118);
            this.flpScripts.TabIndex = 58;
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavePreset.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePreset.Location = new System.Drawing.Point(271, 51);
            this.btnSavePreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(88, 30);
            this.btnSavePreset.TabIndex = 57;
            this.btnSavePreset.Text = "Save";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            this.btnSavePreset.Click += new System.EventHandler(this.btnSavePreset_Click);
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(23, 58);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(51, 19);
            this.label21.TabIndex = 56;
            this.label21.Text = "Preset";
            // 
            // cbPresets
            // 
            this.cbPresets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPresets.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPresets.FormattingEnabled = true;
            this.cbPresets.Location = new System.Drawing.Point(78, 56);
            this.cbPresets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbPresets.Name = "cbPresets";
            this.cbPresets.Size = new System.Drawing.Size(187, 26);
            this.cbPresets.TabIndex = 55;
            this.cbPresets.SelectedIndexChanged += new System.EventHandler(this.cbPresets_SelectedIndexChanged);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefaults.Enabled = false;
            this.btnDefaults.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefaults.Location = new System.Drawing.Point(176, 335);
            this.btnDefaults.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(88, 30);
            this.btnDefaults.TabIndex = 54;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(271, 335);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 30);
            this.btnSave.TabIndex = 53;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tblSettings
            // 
            this.tblSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tblSettings.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblSettings.ColumnCount = 1;
            this.tblSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSettings.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblSettings.Location = new System.Drawing.Point(10, 209);
            this.tblSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tblSettings.Name = "tblSettings";
            this.tblSettings.RowCount = 1;
            this.tblSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSettings.Size = new System.Drawing.Size(349, 119);
            this.tblSettings.TabIndex = 52;
            // 
            // btnApplyScripts
            // 
            this.btnApplyScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyScripts.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyScripts.Location = new System.Drawing.Point(271, 172);
            this.btnApplyScripts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApplyScripts.Name = "btnApplyScripts";
            this.btnApplyScripts.Size = new System.Drawing.Size(88, 30);
            this.btnApplyScripts.TabIndex = 51;
            this.btnApplyScripts.Text = "Apply";
            this.btnApplyScripts.UseVisualStyleBackColor = true;
            this.btnApplyScripts.Click += new System.EventHandler(this.btnApplyScripts_Click);
            // 
            // lblSelMessage
            // 
            this.lblSelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelMessage.AutoSize = true;
            this.lblSelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelMessage.ForeColor = System.Drawing.Color.Crimson;
            this.lblSelMessage.Location = new System.Drawing.Point(11, 28);
            this.lblSelMessage.Name = "lblSelMessage";
            this.lblSelMessage.Size = new System.Drawing.Size(0, 17);
            this.lblSelMessage.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font(Form1.fonts.Families[1], 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(118, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 23);
            this.label6.TabIndex = 49;
            this.label6.Text = "Processing Chain";
            // 
            // lblSpectrogram
            // 
            this.lblSpectrogram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpectrogram.AutoSize = true;
            this.lblSpectrogram.Visible = false;
            this.lblSpectrogram.Font = new System.Drawing.Font(Form1.fonts.Families[0], 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpectrogram.Location = new System.Drawing.Point(118, 0);
            this.lblSpectrogram.Name = "lblSpectrogram";
            this.lblSpectrogram.Size = new System.Drawing.Size(148, 23);
            this.lblSpectrogram.TabIndex = 49;
            this.lblSpectrogram.Text = "";
            // 
            // btnAddScript
            // 
            this.btnAddScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddScript.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddScript.Location = new System.Drawing.Point(271, 137);
            this.btnAddScript.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddScript.Name = "btnAddScript";
            this.btnAddScript.Size = new System.Drawing.Size(88, 30);
            this.btnAddScript.TabIndex = 48;
            this.btnAddScript.Text = "Add";
            this.btnAddScript.UseVisualStyleBackColor = true;
            this.btnAddScript.Click += new System.EventHandler(this.btnAddScript_Click);
            // 
            // numPitch
            // 
            this.numPitch.DecimalPlaces = 2;
            this.numPitch.Enabled = false;
            this.numPitch.Location = new System.Drawing.Point(1009, 37);
            this.numPitch.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numPitch.Minimum = new decimal(new int[] { 12, 0, 0, -2147483648 });
            this.numPitch.Name = "numPitch2";
            this.numPitch.Size = new System.Drawing.Size(61, 22);
            this.numPitch.TabIndex = 53;
            this.numPitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPitch.Increment = 0.05M;
            this.numPitch.ValueChanged += new System.EventHandler(this.numPitch_ValueChanged);
            this.numPitch.Click += new System.EventHandler(this.numPitch_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(956, 38);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 16);
            this.label17.TabIndex = 55;
            this.label17.Text = "Pitch: ";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font(Form1.fonts.Families[0], 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1076, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 16);
            this.label15.TabIndex = 56;
            this.label15.Text = "cents";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1832, 903);
            this.Controls.Add(this.pnlSpectrumControls);
            this.Controls.Add(this.pnlMusic);
            this.Controls.Add(this.lblFilterFreq);
            this.Controls.Add(this.btnFilterDrag);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.numStepVal);
            this.Controls.Add(this.segMode);
            this.Controls.Add(this.prbLevelMeter);
            this.Controls.Add(this.lblLoopDuration);
            this.Controls.Add(this.txtLoopTime);
            this.Controls.Add(this.lblSelectTime);
            this.Controls.Add(this.txtSelectTime);
            this.Controls.Add(this.lblPlayTime);
            this.Controls.Add(this.txtPlayTime);
            this.Controls.Add(this.chbFollow);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.barTempo);
            this.Controls.Add(this.barVolume);
            this.Controls.Add(this.spFFT);
            this.Controls.Add(this.cwvViewer);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnOpenClose);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.specViewer);
            this.Controls.Add(this.pnlScripts);
            this.Controls.Add(this.lblSpectrogram);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.numPitch);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Music Analyser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStepVal)).EndInit();
            this.pnlMusic.ResumeLayout(false);
            this.pnlMusic.PerformLayout();
            this.pnlSpectrumControls.ResumeLayout(false);
            this.pnlSpectrumControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).EndInit();
            this.pnlScripts.ResumeLayout(false);
            this.pnlScripts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenClose;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private ScottPlot.FormsPlot spFFT;
        private System.Windows.Forms.Timer timerFFT;
        private System.Windows.Forms.TrackBar barVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbFollow;
        private System.Windows.Forms.TextBox txtPlayTime;
        private System.Windows.Forms.Label lblPlayTime;
        private System.Windows.Forms.TrackBar barTempo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spectrogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSpecImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSpecAudioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem docsToolStripMenuItem;
        private System.Windows.Forms.Label lblSelectTime;
        private System.Windows.Forms.TextBox txtSelectTime;
        internal CustomWaveViewer cwvViewer;
        private System.Windows.Forms.Label lblLoopDuration;
        private System.Windows.Forms.TextBox txtLoopTime;
        private System.Windows.Forms.ProgressBar prbLevelMeter;
        private System.Windows.Forms.ToolStripMenuItem saveRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private XanderUI.XUISegment segMode;
        private System.Windows.Forms.NumericUpDown numStepVal;
        private System.Windows.Forms.Label lblStep;
        private MusicAnalyser.UI.RoundButton btnFilterDrag;
        private System.Windows.Forms.Label lblFilterFreq;
        internal UI.SpectrogramViewer specViewer;
        private System.Windows.Forms.Panel pnlMusic;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblBb;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblAb;
        private System.Windows.Forms.Label lblG;
        private System.Windows.Forms.Label lblGb;
        private System.Windows.Forms.Label lblF;
        private System.Windows.Forms.Label lblE;
        private System.Windows.Forms.Label lblEb;
        private System.Windows.Forms.Label lblD;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.CheckBox chbAllChords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstChords;
        private System.Windows.Forms.Panel pnlSpectrumControls;
        private System.Windows.Forms.Button btnViewSpec;
        private System.Windows.Forms.CheckBox chbNoteAnnotations;
        private System.Windows.Forms.CheckBox chbChordKeyAnnotations;
        private System.Windows.Forms.CheckBox chbFilter;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar barPitch;
        private System.Windows.Forms.NumericUpDown numZoomLow;
        private System.Windows.Forms.NumericUpDown numZoomHigh;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblExeTime;
        private System.Windows.Forms.Panel pnlScripts;
        private System.Windows.Forms.TableLayoutPanel flpScripts;
        private System.Windows.Forms.Button btnSavePreset;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox cbPresets;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tblSettings;
        private System.Windows.Forms.Button btnApplyScripts;
        private System.Windows.Forms.Label lblSelMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSpectrogram;
        private System.Windows.Forms.Button btnAddScript;
        private System.Windows.Forms.Button btnSpecEnlarge;
        private System.Windows.Forms.NumericUpDown numPitch;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
    }
}

