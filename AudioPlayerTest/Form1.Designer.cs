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
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.spFFT = new ScottPlot.ScottPlotUC();
            this.timerFFT = new System.Windows.Forms.Timer(this.components);
            this.lblFFTDraws = new System.Windows.Forms.Label();
            this.lstNotes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chbOrder = new System.Windows.Forms.CheckBox();
            this.chbFollow = new System.Windows.Forms.CheckBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
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
            this.chbTempo = new System.Windows.Forms.CheckBox();
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
            this.cwvViewer = new MusicAnalyser.CustomWaveViewer();
            this.lblError = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 27);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(12, 45);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(156, 27);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnClose
            // 
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(93, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // spFFT
            // 
            this.spFFT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spFFT.Location = new System.Drawing.Point(12, 476);
            this.spFFT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spFFT.Name = "spFFT";
            this.spFFT.Size = new System.Drawing.Size(1313, 369);
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
            this.lblFFTDraws.Location = new System.Drawing.Point(62, 424);
            this.lblFFTDraws.Name = "lblFFTDraws";
            this.lblFFTDraws.Size = new System.Drawing.Size(106, 17);
            this.lblFFTDraws.TabIndex = 4;
            this.lblFFTDraws.Text = "FFT Updates: 0";
            // 
            // lstNotes
            // 
            this.lstNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstNotes.FormattingEnabled = true;
            this.lstNotes.ItemHeight = 16;
            this.lstNotes.Location = new System.Drawing.Point(1378, 56);
            this.lstNotes.Name = "lstNotes";
            this.lstNotes.Size = new System.Drawing.Size(206, 340);
            this.lstNotes.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1378, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Notes Detected:";
            // 
            // barVolume
            // 
            this.barVolume.Location = new System.Drawing.Point(317, 24);
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
            this.label2.Location = new System.Drawing.Point(325, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "0%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "50%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(482, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "100%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Playback Volume: ";
            // 
            // chbOrder
            // 
            this.chbOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbOrder.AutoSize = true;
            this.chbOrder.Location = new System.Drawing.Point(1494, 26);
            this.chbOrder.Name = "chbOrder";
            this.chbOrder.Size = new System.Drawing.Size(124, 21);
            this.chbOrder.TabIndex = 12;
            this.chbOrder.Text = "Order By Freq.";
            this.chbOrder.UseVisualStyleBackColor = true;
            // 
            // chbFollow
            // 
            this.chbFollow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbFollow.AutoSize = true;
            this.chbFollow.Checked = true;
            this.chbFollow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbFollow.Location = new System.Drawing.Point(1228, 25);
            this.chbFollow.Name = "chbFollow";
            this.chbFollow.Size = new System.Drawing.Size(130, 21);
            this.chbFollow.TabIndex = 13;
            this.chbFollow.Text = "Follow Playback";
            this.chbFollow.UseVisualStyleBackColor = true;
            // 
            // txtTime
            // 
            this.txtTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTime.Location = new System.Drawing.Point(1108, 23);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(90, 22);
            this.txtTime.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(998, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Playback Time:";
            // 
            // lblC
            // 
            this.lblC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblC.AutoSize = true;
            this.lblC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblC.Location = new System.Drawing.Point(1344, 547);
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
            this.lblDb.Location = new System.Drawing.Point(1344, 564);
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
            this.lblD.Location = new System.Drawing.Point(1344, 581);
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
            this.lblEb.Location = new System.Drawing.Point(1344, 598);
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
            this.lblE.Location = new System.Drawing.Point(1344, 615);
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
            this.lblF.Location = new System.Drawing.Point(1344, 632);
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
            this.lblGb.Location = new System.Drawing.Point(1344, 649);
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
            this.lblG.Location = new System.Drawing.Point(1344, 666);
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
            this.lblAb.Location = new System.Drawing.Point(1344, 683);
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
            this.lblA.Location = new System.Drawing.Point(1344, 700);
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
            this.lblBb.Location = new System.Drawing.Point(1344, 717);
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
            this.lblB.Location = new System.Drawing.Point(1344, 734);
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
            this.lblKey.Location = new System.Drawing.Point(1347, 770);
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
            this.lblExeTime.Location = new System.Drawing.Point(46, 452);
            this.lblExeTime.Name = "lblExeTime";
            this.lblExeTime.Size = new System.Drawing.Size(142, 17);
            this.lblExeTime.TabIndex = 4;
            this.lblExeTime.Text = "Execution Time: 0 ms";
            // 
            // barTempo
            // 
            this.barTempo.Location = new System.Drawing.Point(652, 24);
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
            this.label7.Location = new System.Drawing.Point(522, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Playback Tempo: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(655, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 17);
            this.label8.TabIndex = 8;
            this.label8.Text = "50%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(732, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "100%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(817, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 17);
            this.label10.TabIndex = 10;
            this.label10.Text = "150%";
            // 
            // chbTempo
            // 
            this.chbTempo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbTempo.AutoSize = true;
            this.chbTempo.Checked = true;
            this.chbTempo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbTempo.Location = new System.Drawing.Point(870, 26);
            this.chbTempo.Name = "chbTempo";
            this.chbTempo.Size = new System.Drawing.Size(122, 21);
            this.chbTempo.TabIndex = 13;
            this.chbTempo.Text = "Enable Tempo";
            this.chbTempo.UseVisualStyleBackColor = true;
            // 
            // barPitch
            // 
            this.barPitch.Location = new System.Drawing.Point(492, 424);
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
            this.label11.Location = new System.Drawing.Point(489, 459);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "-50";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(584, 459);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 17);
            this.label12.TabIndex = 9;
            this.label12.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(659, 459);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "+50";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(404, 424);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "Pitch Sync: ";
            // 
            // barZoom
            // 
            this.barZoom.Location = new System.Drawing.Point(273, 424);
            this.barZoom.Maximum = 3;
            this.barZoom.Name = "barZoom";
            this.barZoom.Size = new System.Drawing.Size(110, 56);
            this.barZoom.TabIndex = 7;
            this.barZoom.Value = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(358, 463);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 17);
            this.label15.TabIndex = 8;
            this.label15.Text = "4k";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(329, 463);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 17);
            this.label16.TabIndex = 8;
            this.label16.Text = "2k";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(270, 463);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(32, 17);
            this.label17.TabIndex = 8;
            this.label17.Text = "500";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(300, 463);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "1k";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(208, 424);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(48, 17);
            this.label19.TabIndex = 11;
            this.label19.Text = "Zoom:";
            // 
            // cwvViewer
            // 
            this.cwvViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cwvViewer.BytesPerSample = 0;
            this.cwvViewer.LeftSample = ((long)(0));
            this.cwvViewer.Location = new System.Drawing.Point(66, 95);
            this.cwvViewer.Name = "cwvViewer";
            this.cwvViewer.PenColor = System.Drawing.Color.DodgerBlue;
            this.cwvViewer.PenWidth = 1F;
            this.cwvViewer.RightSample = ((long)(0));
            this.cwvViewer.SamplesPerPixel = 128;
            this.cwvViewer.Size = new System.Drawing.Size(1259, 301);
            this.cwvViewer.StartPosition = ((long)(0));
            this.cwvViewer.TabIndex = 2;
            this.cwvViewer.WaveStream = null;
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(747, 424);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(75, 17);
            this.lblError.TabIndex = 18;
            this.lblError.Text = "+ 0 cents";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(697, 424);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 17);
            this.label20.TabIndex = 6;
            this.label20.Text = "Error:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1712, 875);
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
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.chbTempo);
            this.Controls.Add(this.chbFollow);
            this.Controls.Add(this.chbOrder);
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
            this.Controls.Add(this.lstNotes);
            this.Controls.Add(this.lblExeTime);
            this.Controls.Add(this.lblFFTDraws);
            this.Controls.Add(this.spFFT);
            this.Controls.Add(this.cwvViewer);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOpen);
            this.Name = "Form1";
            this.Text = "Music Analyser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnClose;
        private CustomWaveViewer cwvViewer;
        private ScottPlot.ScottPlotUC spFFT;
        private System.Windows.Forms.Timer timerFFT;
        private System.Windows.Forms.Label lblFFTDraws;
        private System.Windows.Forms.ListBox lstNotes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar barVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbOrder;
        private System.Windows.Forms.CheckBox chbFollow;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label6;
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
        private System.Windows.Forms.CheckBox chbTempo;
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
    }
}

