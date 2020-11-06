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
            this.lblFFTDraws = new System.Windows.Forms.Label();
            this.lstChords = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chbFollow = new System.Windows.Forms.CheckBox();
            this.txtPlayTime = new System.Windows.Forms.TextBox();
            this.lblPlayTime = new System.Windows.Forms.Label();
            this.lblC = new System.Windows.Forms.Label();
            this.lblDb = new System.Windows.Forms.Label();
            this.lblD = new System.Windows.Forms.Label();
            this.lblEb = new System.Windows.Forms.Label();
            this.lblE = new System.Windows.Forms.Label();
            this.lblF = new System.Windows.Forms.Label();
            this.lblGb = new System.Windows.Forms.Label();
            this.lblG = new System.Windows.Forms.Label();
            this.lblAb = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblBb = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblExeTime = new System.Windows.Forms.Label();
            this.barTempo = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.barPitch = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.barZoom = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMode = new System.Windows.Forms.Label();
            this.chbAllChords = new System.Windows.Forms.CheckBox();
            this.lblSelectTime = new System.Windows.Forms.Label();
            this.txtSelectTime = new System.Windows.Forms.TextBox();
            this.lblLoopDuration = new System.Windows.Forms.Label();
            this.txtLoopTime = new System.Windows.Forms.TextBox();
            this.btnLiveMode = new System.Windows.Forms.Button();
            this.prbLevelMeter = new System.Windows.Forms.ProgressBar();
            this.flpScripts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddScript = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSelMessage = new System.Windows.Forms.Label();
            this.btnApplyScripts = new System.Windows.Forms.Button();
            this.tblSettings = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.cwvViewer = new MusicAnalyser.CustomWaveViewer();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barZoom)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenClose
            // 
            this.btnOpenClose.Location = new System.Drawing.Point(13, 31);
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
            this.btnPlay.Location = new System.Drawing.Point(13, 64);
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
            this.btnStop.Location = new System.Drawing.Point(94, 31);
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
            this.spFFT.Location = new System.Drawing.Point(13, 533);
            this.spFFT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spFFT.Name = "spFFT";
            this.spFFT.Size = new System.Drawing.Size(1509, 369);
            this.spFFT.TabIndex = 3;
            // 
            // timerFFT
            // 
            this.timerFFT.Interval = 17;
            this.timerFFT.Tick += new System.EventHandler(this.timerFFT_Tick);
            // 
            // lblFFTDraws
            // 
            this.lblFFTDraws.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFFTDraws.AutoSize = true;
            this.lblFFTDraws.Location = new System.Drawing.Point(47, 473);
            this.lblFFTDraws.Name = "lblFFTDraws";
            this.lblFFTDraws.Size = new System.Drawing.Size(106, 17);
            this.lblFFTDraws.TabIndex = 4;
            this.lblFFTDraws.Text = "FFT Updates: 0";
            // 
            // lstChords
            // 
            this.lstChords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstChords.FormattingEnabled = true;
            this.lstChords.ItemHeight = 16;
            this.lstChords.Location = new System.Drawing.Point(1710, 626);
            this.lstChords.Name = "lstChords";
            this.lstChords.Size = new System.Drawing.Size(120, 100);
            this.lstChords.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1710, 596);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Chords Found:";
            // 
            // barVolume
            // 
            this.barVolume.Enabled = false;
            this.barVolume.Location = new System.Drawing.Point(431, 34);
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
            this.label2.Location = new System.Drawing.Point(439, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "0%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(516, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "50%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(596, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "100%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(301, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Playback Volume: ";
            // 
            // chbFollow
            // 
            this.chbFollow.AutoSize = true;
            this.chbFollow.Checked = true;
            this.chbFollow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbFollow.Location = new System.Drawing.Point(1604, 35);
            this.chbFollow.Name = "chbFollow";
            this.chbFollow.Size = new System.Drawing.Size(130, 21);
            this.chbFollow.TabIndex = 13;
            this.chbFollow.Text = "Follow Playback";
            this.chbFollow.UseVisualStyleBackColor = true;
            // 
            // txtPlayTime
            // 
            this.txtPlayTime.Location = new System.Drawing.Point(1095, 33);
            this.txtPlayTime.Name = "txtPlayTime";
            this.txtPlayTime.Size = new System.Drawing.Size(90, 22);
            this.txtPlayTime.TabIndex = 16;
            this.txtPlayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblPlayTime
            // 
            this.lblPlayTime.AutoSize = true;
            this.lblPlayTime.Location = new System.Drawing.Point(971, 36);
            this.lblPlayTime.Name = "lblPlayTime";
            this.lblPlayTime.Size = new System.Drawing.Size(104, 17);
            this.lblPlayTime.TabIndex = 17;
            this.lblPlayTime.Text = "Playback Time:";
            // 
            // lblC
            // 
            this.lblC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblC.AutoSize = true;
            this.lblC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblC.Location = new System.Drawing.Point(1541, 596);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(37, 17);
            this.lblC.TabIndex = 18;
            this.lblC.Text = "C: 0";
            // 
            // lblDb
            // 
            this.lblDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDb.AutoSize = true;
            this.lblDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDb.Location = new System.Drawing.Point(1541, 613);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(47, 17);
            this.lblDb.TabIndex = 19;
            this.lblDb.Text = "Db: 0";
            // 
            // lblD
            // 
            this.lblD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblD.AutoSize = true;
            this.lblD.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblD.Location = new System.Drawing.Point(1541, 630);
            this.lblD.Name = "lblD";
            this.lblD.Size = new System.Drawing.Size(38, 17);
            this.lblD.TabIndex = 20;
            this.lblD.Text = "D: 0";
            // 
            // lblEb
            // 
            this.lblEb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEb.AutoSize = true;
            this.lblEb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEb.Location = new System.Drawing.Point(1541, 647);
            this.lblEb.Name = "lblEb";
            this.lblEb.Size = new System.Drawing.Size(46, 17);
            this.lblEb.TabIndex = 20;
            this.lblEb.Text = "Eb: 0";
            // 
            // lblE
            // 
            this.lblE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblE.AutoSize = true;
            this.lblE.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblE.Location = new System.Drawing.Point(1541, 664);
            this.lblE.Name = "lblE";
            this.lblE.Size = new System.Drawing.Size(37, 17);
            this.lblE.TabIndex = 20;
            this.lblE.Text = "E: 0";
            // 
            // lblF
            // 
            this.lblF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblF.AutoSize = true;
            this.lblF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblF.Location = new System.Drawing.Point(1541, 681);
            this.lblF.Name = "lblF";
            this.lblF.Size = new System.Drawing.Size(36, 17);
            this.lblF.TabIndex = 20;
            this.lblF.Text = "F: 0";
            // 
            // lblGb
            // 
            this.lblGb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGb.AutoSize = true;
            this.lblGb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGb.Location = new System.Drawing.Point(1541, 698);
            this.lblGb.Name = "lblGb";
            this.lblGb.Size = new System.Drawing.Size(48, 17);
            this.lblGb.TabIndex = 20;
            this.lblGb.Text = "Gb: 0";
            // 
            // lblG
            // 
            this.lblG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblG.AutoSize = true;
            this.lblG.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblG.Location = new System.Drawing.Point(1541, 715);
            this.lblG.Name = "lblG";
            this.lblG.Size = new System.Drawing.Size(39, 17);
            this.lblG.TabIndex = 20;
            this.lblG.Text = "G: 0";
            // 
            // lblAb
            // 
            this.lblAb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAb.AutoSize = true;
            this.lblAb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAb.Location = new System.Drawing.Point(1541, 732);
            this.lblAb.Name = "lblAb";
            this.lblAb.Size = new System.Drawing.Size(46, 17);
            this.lblAb.TabIndex = 20;
            this.lblAb.Text = "Ab: 0";
            // 
            // lblA
            // 
            this.lblA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblA.Location = new System.Drawing.Point(1541, 749);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(37, 17);
            this.lblA.TabIndex = 20;
            this.lblA.Text = "A: 0";
            // 
            // lblBb
            // 
            this.lblBb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBb.AutoSize = true;
            this.lblBb.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBb.Location = new System.Drawing.Point(1541, 766);
            this.lblBb.Name = "lblBb";
            this.lblBb.Size = new System.Drawing.Size(46, 17);
            this.lblBb.TabIndex = 20;
            this.lblBb.Text = "Bb: 0";
            // 
            // lblB
            // 
            this.lblB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblB.AutoSize = true;
            this.lblB.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblB.Location = new System.Drawing.Point(1541, 783);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(37, 17);
            this.lblB.TabIndex = 20;
            this.lblB.Text = "B: 0";
            // 
            // lblKey
            // 
            this.lblKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.Location = new System.Drawing.Point(1544, 819);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(119, 17);
            this.lblKey.TabIndex = 21;
            this.lblKey.Text = "Predicted Key: ";
            // 
            // lblExeTime
            // 
            this.lblExeTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExeTime.AutoSize = true;
            this.lblExeTime.Location = new System.Drawing.Point(47, 501);
            this.lblExeTime.Name = "lblExeTime";
            this.lblExeTime.Size = new System.Drawing.Size(142, 17);
            this.lblExeTime.TabIndex = 4;
            this.lblExeTime.Text = "Execution Time: 0 ms";
            // 
            // barTempo
            // 
            this.barTempo.Enabled = false;
            this.barTempo.Location = new System.Drawing.Point(766, 34);
            this.barTempo.Maximum = 20;
            this.barTempo.Name = "barTempo";
            this.barTempo.Size = new System.Drawing.Size(199, 56);
            this.barTempo.TabIndex = 7;
            this.barTempo.Value = 10;
            this.barTempo.Scroll += new System.EventHandler(this.barTempo_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(636, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Playback Tempo: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(769, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 17);
            this.label8.TabIndex = 8;
            this.label8.Text = "50%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(846, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "100%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(931, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 17);
            this.label10.TabIndex = 10;
            this.label10.Text = "150%";
            // 
            // barPitch
            // 
            this.barPitch.Enabled = false;
            this.barPitch.Location = new System.Drawing.Point(493, 473);
            this.barPitch.Maximum = 100;
            this.barPitch.Name = "barPitch";
            this.barPitch.Size = new System.Drawing.Size(199, 56);
            this.barPitch.TabIndex = 7;
            this.barPitch.Value = 50;
            this.barPitch.Scroll += new System.EventHandler(this.barPitch_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(490, 508);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "-50";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(585, 508);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 17);
            this.label12.TabIndex = 9;
            this.label12.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(660, 508);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "+50";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(405, 473);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "Pitch Sync: ";
            // 
            // barZoom
            // 
            this.barZoom.Location = new System.Drawing.Point(274, 473);
            this.barZoom.Maximum = 3;
            this.barZoom.Name = "barZoom";
            this.barZoom.Size = new System.Drawing.Size(110, 56);
            this.barZoom.TabIndex = 7;
            this.barZoom.Value = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(359, 512);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 17);
            this.label15.TabIndex = 8;
            this.label15.Text = "4k";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(330, 512);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 17);
            this.label16.TabIndex = 8;
            this.label16.Text = "2k";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(271, 512);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(32, 17);
            this.label17.TabIndex = 8;
            this.label17.Text = "500";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(301, 512);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "1k";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(209, 473);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(48, 17);
            this.label19.TabIndex = 11;
            this.label19.Text = "Zoom:";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(748, 473);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(75, 17);
            this.lblError.TabIndex = 18;
            this.lblError.Text = "+ 0 cents";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(698, 473);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 17);
            this.label20.TabIndex = 6;
            this.label20.Text = "Error:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1908, 30);
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perferencesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 26);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // perferencesToolStripMenuItem
            // 
            this.perferencesToolStripMenuItem.Name = "perferencesToolStripMenuItem";
            this.perferencesToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.perferencesToolStripMenuItem.Text = "Perferences";
            this.perferencesToolStripMenuItem.Click += new System.EventHandler(this.perferencesToolStripMenuItem_Click);
            // 
            // lblMode
            // 
            this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(1648, 845);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(47, 17);
            this.lblMode.TabIndex = 21;
            this.lblMode.Text = "Mode";
            // 
            // chbAllChords
            // 
            this.chbAllChords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbAllChords.AutoSize = true;
            this.chbAllChords.Location = new System.Drawing.Point(1710, 745);
            this.chbAllChords.Name = "chbAllChords";
            this.chbAllChords.Size = new System.Drawing.Size(83, 21);
            this.chbAllChords.TabIndex = 13;
            this.chbAllChords.Text = "Show All";
            this.chbAllChords.UseVisualStyleBackColor = true;
            // 
            // lblSelectTime
            // 
            this.lblSelectTime.AutoSize = true;
            this.lblSelectTime.Location = new System.Drawing.Point(1191, 36);
            this.lblSelectTime.Name = "lblSelectTime";
            this.lblSelectTime.Size = new System.Drawing.Size(86, 17);
            this.lblSelectTime.TabIndex = 24;
            this.lblSelectTime.Text = "Select Time:";
            // 
            // txtSelectTime
            // 
            this.txtSelectTime.Location = new System.Drawing.Point(1283, 33);
            this.txtSelectTime.Name = "txtSelectTime";
            this.txtSelectTime.Size = new System.Drawing.Size(90, 22);
            this.txtSelectTime.TabIndex = 23;
            this.txtSelectTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLoopDuration
            // 
            this.lblLoopDuration.AutoSize = true;
            this.lblLoopDuration.Location = new System.Drawing.Point(1379, 36);
            this.lblLoopDuration.Name = "lblLoopDuration";
            this.lblLoopDuration.Size = new System.Drawing.Size(102, 17);
            this.lblLoopDuration.TabIndex = 26;
            this.lblLoopDuration.Text = "Loop Duration:";
            // 
            // txtLoopTime
            // 
            this.txtLoopTime.Location = new System.Drawing.Point(1487, 33);
            this.txtLoopTime.Name = "txtLoopTime";
            this.txtLoopTime.Size = new System.Drawing.Size(90, 22);
            this.txtLoopTime.TabIndex = 25;
            this.txtLoopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLiveMode
            // 
            this.btnLiveMode.Location = new System.Drawing.Point(176, 31);
            this.btnLiveMode.Name = "btnLiveMode";
            this.btnLiveMode.Size = new System.Drawing.Size(119, 60);
            this.btnLiveMode.TabIndex = 27;
            this.btnLiveMode.Text = "Live Mode";
            this.btnLiveMode.UseVisualStyleBackColor = true;
            this.btnLiveMode.Click += new System.EventHandler(this.btnLiveMode_Click);
            // 
            // prbLevelMeter
            // 
            this.prbLevelMeter.Location = new System.Drawing.Point(1309, 32);
            this.prbLevelMeter.Name = "prbLevelMeter";
            this.prbLevelMeter.Size = new System.Drawing.Size(268, 23);
            this.prbLevelMeter.TabIndex = 28;
            this.prbLevelMeter.Visible = false;
            // 
            // flpScripts
            // 
            this.flpScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpScripts.AutoScroll = true;
            this.flpScripts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpScripts.Location = new System.Drawing.Point(1528, 137);
            this.flpScripts.Name = "flpScripts";
            this.flpScripts.Size = new System.Drawing.Size(255, 152);
            this.flpScripts.TabIndex = 29;
            this.flpScripts.WrapContents = false;
            // 
            // btnAddScript
            // 
            this.btnAddScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddScript.Location = new System.Drawing.Point(1789, 137);
            this.btnAddScript.Name = "btnAddScript";
            this.btnAddScript.Size = new System.Drawing.Size(88, 30);
            this.btnAddScript.TabIndex = 30;
            this.btnAddScript.Text = "Add";
            this.btnAddScript.UseVisualStyleBackColor = true;
            this.btnAddScript.Click += new System.EventHandler(this.btnAddScript_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1636, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 20);
            this.label6.TabIndex = 31;
            this.label6.Text = "Processing Chain";
            // 
            // lblSelMessage
            // 
            this.lblSelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelMessage.AutoSize = true;
            this.lblSelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelMessage.ForeColor = System.Drawing.Color.Crimson;
            this.lblSelMessage.Location = new System.Drawing.Point(1529, 114);
            this.lblSelMessage.Name = "lblSelMessage";
            this.lblSelMessage.Size = new System.Drawing.Size(0, 17);
            this.lblSelMessage.TabIndex = 32;
            // 
            // btnApplyScripts
            // 
            this.btnApplyScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyScripts.Location = new System.Drawing.Point(1789, 173);
            this.btnApplyScripts.Name = "btnApplyScripts";
            this.btnApplyScripts.Size = new System.Drawing.Size(88, 30);
            this.btnApplyScripts.TabIndex = 33;
            this.btnApplyScripts.Text = "Apply";
            this.btnApplyScripts.UseVisualStyleBackColor = true;
            this.btnApplyScripts.Click += new System.EventHandler(this.btnApplyScripts_Click);
            // 
            // tblSettings
            // 
            this.tblSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tblSettings.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblSettings.ColumnCount = 1;
            this.tblSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSettings.Location = new System.Drawing.Point(1528, 296);
            this.tblSettings.Name = "tblSettings";
            this.tblSettings.RowCount = 1;
            this.tblSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSettings.Size = new System.Drawing.Size(349, 119);
            this.tblSettings.TabIndex = 34;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(1789, 258);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 30);
            this.btnSave.TabIndex = 36;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefaults.Enabled = false;
            this.btnDefaults.Location = new System.Drawing.Point(1789, 222);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(88, 30);
            this.btnDefaults.TabIndex = 37;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // cwvViewer
            // 
            this.cwvViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cwvViewer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cwvViewer.BytesPerSample = 0;
            this.cwvViewer.LeftSample = ((long)(0));
            this.cwvViewer.Location = new System.Drawing.Point(32, 114);
            this.cwvViewer.LoopEndSample = ((long)(0));
            this.cwvViewer.Name = "cwvViewer";
            this.cwvViewer.PenColor = System.Drawing.Color.DodgerBlue;
            this.cwvViewer.PenWidth = 1F;
            this.cwvViewer.RightSample = ((long)(0));
            this.cwvViewer.SamplesPerPixel = 128;
            this.cwvViewer.SelectSample = ((long)(0));
            this.cwvViewer.Size = new System.Drawing.Size(1490, 301);
            this.cwvViewer.StartPosition = ((long)(0));
            this.cwvViewer.TabIndex = 2;
            this.cwvViewer.WaveStream = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1908, 945);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tblSettings);
            this.Controls.Add(this.btnApplyScripts);
            this.Controls.Add(this.lblSelMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAddScript);
            this.Controls.Add(this.flpScripts);
            this.Controls.Add(this.prbLevelMeter);
            this.Controls.Add(this.btnLiveMode);
            this.Controls.Add(this.lblLoopDuration);
            this.Controls.Add(this.txtLoopTime);
            this.Controls.Add(this.lblSelectTime);
            this.Controls.Add(this.txtSelectTime);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.lblB);
            this.Controls.Add(this.lblBb);
            this.Controls.Add(this.lblA);
            this.Controls.Add(this.lblAb);
            this.Controls.Add(this.lblG);
            this.Controls.Add(this.lblGb);
            this.Controls.Add(this.lblF);
            this.Controls.Add(this.lblE);
            this.Controls.Add(this.lblEb);
            this.Controls.Add(this.lblD);
            this.Controls.Add(this.lblDb);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblC);
            this.Controls.Add(this.lblPlayTime);
            this.Controls.Add(this.txtPlayTime);
            this.Controls.Add(this.chbAllChords);
            this.Controls.Add(this.chbFollow);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.barPitch);
            this.Controls.Add(this.barZoom);
            this.Controls.Add(this.barTempo);
            this.Controls.Add(this.barVolume);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstChords);
            this.Controls.Add(this.lblExeTime);
            this.Controls.Add(this.lblFFTDraws);
            this.Controls.Add(this.spFFT);
            this.Controls.Add(this.cwvViewer);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnOpenClose);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Music Analyser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barZoom)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenClose;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private ScottPlot.FormsPlot spFFT;
        private System.Windows.Forms.Timer timerFFT;
        private System.Windows.Forms.Label lblFFTDraws;
        private System.Windows.Forms.ListBox lstChords;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar barVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbFollow;
        private System.Windows.Forms.TextBox txtPlayTime;
        private System.Windows.Forms.Label lblPlayTime;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Label lblD;
        private System.Windows.Forms.Label lblEb;
        private System.Windows.Forms.Label lblE;
        private System.Windows.Forms.Label lblF;
        private System.Windows.Forms.Label lblGb;
        private System.Windows.Forms.Label lblG;
        private System.Windows.Forms.Label lblAb;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblBb;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblExeTime;
        private System.Windows.Forms.TrackBar barTempo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar barPitch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar barZoom;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perferencesToolStripMenuItem;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.CheckBox chbAllChords;
        private System.Windows.Forms.Label lblSelectTime;
        private System.Windows.Forms.TextBox txtSelectTime;
        internal CustomWaveViewer cwvViewer;
        private System.Windows.Forms.Label lblLoopDuration;
        private System.Windows.Forms.TextBox txtLoopTime;
        private System.Windows.Forms.Button btnLiveMode;
        private System.Windows.Forms.ProgressBar prbLevelMeter;
        private System.Windows.Forms.ToolStripMenuItem saveRecordingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flpScripts;
        private System.Windows.Forms.Button btnAddScript;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSelMessage;
        private System.Windows.Forms.Button btnApplyScripts;
        private System.Windows.Forms.TableLayoutPanel tblSettings;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDefaults;
    }
}

