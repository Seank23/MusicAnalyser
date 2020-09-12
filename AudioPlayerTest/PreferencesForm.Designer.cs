namespace MusicAnalyser
{
    partial class PreferencesForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.tabControlPrefs = new System.Windows.Forms.TabControl();
            this.tabUI = new System.Windows.Forms.TabPage();
            this.chbAntiAlias = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.comboTheme = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPlayback = new System.Windows.Forms.TabPage();
            this.numFollowSecs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.txtSimilarGainThd = new System.Windows.Forms.TextBox();
            this.txtMaxFreqChange = new System.Windows.Forms.TextBox();
            this.txtMaxGainChange = new System.Windows.Forms.TextBox();
            this.numMaxFreq = new System.Windows.Forms.NumericUpDown();
            this.numSmooth = new System.Windows.Forms.NumericUpDown();
            this.numMinFreq = new System.Windows.Forms.NumericUpDown();
            this.numPeakBuffer = new System.Windows.Forms.NumericUpDown();
            this.numUpdateTime = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.comboNoteAlgorithm = new System.Windows.Forms.ComboBox();
            this.comboMode = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabKey = new System.Windows.Forms.TabPage();
            this.tabChords = new System.Windows.Forms.TabPage();
            this.numOccurThd = new System.Windows.Forms.NumericUpDown();
            this.numChordInterval = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabLive = new System.Windows.Forms.TabPage();
            this.comDevices = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tabControlPrefs.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.tabPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFollowSecs)).BeginInit();
            this.tabNotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmooth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeakBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateTime)).BeginInit();
            this.tabChords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOccurThd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordInterval)).BeginInit();
            this.tabLive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(564, 399);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 27);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(660, 399);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 27);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Location = new System.Drawing.Point(24, 399);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(93, 27);
            this.btnDefaults.TabIndex = 0;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // tabControlPrefs
            // 
            this.tabControlPrefs.Controls.Add(this.tabUI);
            this.tabControlPrefs.Controls.Add(this.tabPlayback);
            this.tabControlPrefs.Controls.Add(this.tabLive);
            this.tabControlPrefs.Controls.Add(this.tabNotes);
            this.tabControlPrefs.Controls.Add(this.tabKey);
            this.tabControlPrefs.Controls.Add(this.tabChords);
            this.tabControlPrefs.Location = new System.Drawing.Point(12, 12);
            this.tabControlPrefs.Name = "tabControlPrefs";
            this.tabControlPrefs.SelectedIndex = 0;
            this.tabControlPrefs.Size = new System.Drawing.Size(776, 381);
            this.tabControlPrefs.TabIndex = 1;
            // 
            // tabUI
            // 
            this.tabUI.Controls.Add(this.chbAntiAlias);
            this.tabUI.Controls.Add(this.label21);
            this.tabUI.Controls.Add(this.comboTheme);
            this.tabUI.Controls.Add(this.label3);
            this.tabUI.Location = new System.Drawing.Point(4, 25);
            this.tabUI.Name = "tabUI";
            this.tabUI.Padding = new System.Windows.Forms.Padding(3);
            this.tabUI.Size = new System.Drawing.Size(768, 352);
            this.tabUI.TabIndex = 0;
            this.tabUI.Text = "User Interface";
            this.tabUI.UseVisualStyleBackColor = true;
            // 
            // chbAntiAlias
            // 
            this.chbAntiAlias.AutoSize = true;
            this.chbAntiAlias.Location = new System.Drawing.Point(174, 65);
            this.chbAntiAlias.Name = "chbAntiAlias";
            this.chbAntiAlias.Size = new System.Drawing.Size(18, 17);
            this.chbAntiAlias.TabIndex = 9;
            this.chbAntiAlias.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(20, 64);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(148, 17);
            this.label21.TabIndex = 8;
            this.label21.Text = "Spectrum Antialiasing:";
            // 
            // comboTheme
            // 
            this.comboTheme.FormattingEnabled = true;
            this.comboTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.comboTheme.Location = new System.Drawing.Point(92, 23);
            this.comboTheme.Name = "comboTheme";
            this.comboTheme.Size = new System.Drawing.Size(92, 24);
            this.comboTheme.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Theme:";
            // 
            // tabPlayback
            // 
            this.tabPlayback.Controls.Add(this.numFollowSecs);
            this.tabPlayback.Controls.Add(this.label2);
            this.tabPlayback.Controls.Add(this.label1);
            this.tabPlayback.Location = new System.Drawing.Point(4, 25);
            this.tabPlayback.Name = "tabPlayback";
            this.tabPlayback.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlayback.Size = new System.Drawing.Size(768, 352);
            this.tabPlayback.TabIndex = 1;
            this.tabPlayback.Text = "Playback";
            this.tabPlayback.UseVisualStyleBackColor = true;
            // 
            // numFollowSecs
            // 
            this.numFollowSecs.Location = new System.Drawing.Point(146, 23);
            this.numFollowSecs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFollowSecs.Name = "numFollowSecs";
            this.numFollowSecs.Size = new System.Drawing.Size(61, 22);
            this.numFollowSecs.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(213, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "seconds";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Following Interval:";
            // 
            // tabNotes
            // 
            this.tabNotes.Controls.Add(this.txtSimilarGainThd);
            this.tabNotes.Controls.Add(this.txtMaxFreqChange);
            this.tabNotes.Controls.Add(this.txtMaxGainChange);
            this.tabNotes.Controls.Add(this.numMaxFreq);
            this.tabNotes.Controls.Add(this.numSmooth);
            this.tabNotes.Controls.Add(this.numMinFreq);
            this.tabNotes.Controls.Add(this.numPeakBuffer);
            this.tabNotes.Controls.Add(this.numUpdateTime);
            this.tabNotes.Controls.Add(this.label6);
            this.tabNotes.Controls.Add(this.comboNoteAlgorithm);
            this.tabNotes.Controls.Add(this.comboMode);
            this.tabNotes.Controls.Add(this.label9);
            this.tabNotes.Controls.Add(this.label19);
            this.tabNotes.Controls.Add(this.label18);
            this.tabNotes.Controls.Add(this.label17);
            this.tabNotes.Controls.Add(this.label11);
            this.tabNotes.Controls.Add(this.label8);
            this.tabNotes.Controls.Add(this.label10);
            this.tabNotes.Controls.Add(this.label7);
            this.tabNotes.Controls.Add(this.label16);
            this.tabNotes.Controls.Add(this.label15);
            this.tabNotes.Controls.Add(this.label14);
            this.tabNotes.Controls.Add(this.label13);
            this.tabNotes.Controls.Add(this.label5);
            this.tabNotes.Controls.Add(this.label20);
            this.tabNotes.Controls.Add(this.label12);
            this.tabNotes.Controls.Add(this.label4);
            this.tabNotes.Location = new System.Drawing.Point(4, 25);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Size = new System.Drawing.Size(768, 352);
            this.tabNotes.TabIndex = 2;
            this.tabNotes.Text = "Note Detection";
            this.tabNotes.UseVisualStyleBackColor = true;
            // 
            // txtSimilarGainThd
            // 
            this.txtSimilarGainThd.Location = new System.Drawing.Point(175, 297);
            this.txtSimilarGainThd.Name = "txtSimilarGainThd";
            this.txtSimilarGainThd.Size = new System.Drawing.Size(68, 22);
            this.txtSimilarGainThd.TabIndex = 6;
            // 
            // txtMaxFreqChange
            // 
            this.txtMaxFreqChange.Location = new System.Drawing.Point(252, 269);
            this.txtMaxFreqChange.Name = "txtMaxFreqChange";
            this.txtMaxFreqChange.Size = new System.Drawing.Size(68, 22);
            this.txtMaxFreqChange.TabIndex = 6;
            // 
            // txtMaxGainChange
            // 
            this.txtMaxGainChange.Location = new System.Drawing.Point(215, 241);
            this.txtMaxGainChange.Name = "txtMaxGainChange";
            this.txtMaxGainChange.Size = new System.Drawing.Size(67, 22);
            this.txtMaxGainChange.TabIndex = 6;
            // 
            // numMaxFreq
            // 
            this.numMaxFreq.Location = new System.Drawing.Point(265, 109);
            this.numMaxFreq.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numMaxFreq.Name = "numMaxFreq";
            this.numMaxFreq.Size = new System.Drawing.Size(61, 22);
            this.numMaxFreq.TabIndex = 5;
            // 
            // numSmooth
            // 
            this.numSmooth.Location = new System.Drawing.Point(169, 141);
            this.numSmooth.Name = "numSmooth";
            this.numSmooth.Size = new System.Drawing.Size(61, 22);
            this.numSmooth.TabIndex = 5;
            // 
            // numMinFreq
            // 
            this.numMinFreq.Location = new System.Drawing.Point(151, 109);
            this.numMinFreq.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numMinFreq.Name = "numMinFreq";
            this.numMinFreq.Size = new System.Drawing.Size(61, 22);
            this.numMinFreq.TabIndex = 5;
            // 
            // numPeakBuffer
            // 
            this.numPeakBuffer.Location = new System.Drawing.Point(143, 213);
            this.numPeakBuffer.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numPeakBuffer.Name = "numPeakBuffer";
            this.numPeakBuffer.Size = new System.Drawing.Size(61, 22);
            this.numPeakBuffer.TabIndex = 5;
            // 
            // numUpdateTime
            // 
            this.numUpdateTime.Location = new System.Drawing.Point(123, 53);
            this.numUpdateTime.Name = "numUpdateTime";
            this.numUpdateTime.Size = new System.Drawing.Size(61, 22);
            this.numUpdateTime.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "milliseconds";
            // 
            // comboNoteAlgorithm
            // 
            this.comboNoteAlgorithm.FormattingEnabled = true;
            this.comboNoteAlgorithm.Items.AddRange(new object[] {
            "By Magnitude",
            "By Slope (Alpha)"});
            this.comboNoteAlgorithm.Location = new System.Drawing.Point(565, 22);
            this.comboNoteAlgorithm.Name = "comboNoteAlgorithm";
            this.comboNoteAlgorithm.Size = new System.Drawing.Size(114, 24);
            this.comboNoteAlgorithm.TabIndex = 3;
            // 
            // comboMode
            // 
            this.comboMode.FormattingEnabled = true;
            this.comboMode.Items.AddRange(new object[] {
            "Dynamic",
            "Manual"});
            this.comboMode.Location = new System.Drawing.Point(123, 22);
            this.comboMode.Name = "comboMode";
            this.comboMode.Size = new System.Drawing.Size(92, 24);
            this.comboMode.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(332, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 17);
            this.label9.TabIndex = 1;
            this.label9.Text = "Hz";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(326, 272);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(25, 17);
            this.label19.TabIndex = 1;
            this.label19.Text = "Hz";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(249, 300);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 17);
            this.label18.TabIndex = 1;
            this.label18.Text = "dB";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(287, 244);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(25, 17);
            this.label17.TabIndex = 1;
            this.label17.Text = "dB";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(236, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 17);
            this.label11.TabIndex = 1;
            this.label11.Text = "draws";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "Hz to";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 17);
            this.label10.TabIndex = 1;
            this.label10.Text = "Spectrum Smoothing:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Frequency Range:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 300);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(156, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "Similar Gain Threshold:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 272);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(233, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "Max Detectable Frequency Change:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 244);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(196, 17);
            this.label14.TabIndex = 1;
            this.label14.Text = "Max Detectable Gain Change:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 215);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "Peak Buffer Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Update Time:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(390, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(169, 17);
            this.label20.TabIndex = 1;
            this.label20.Text = "Note Detection Algorithm:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(25, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(234, 17);
            this.label12.TabIndex = 1;
            this.label12.Text = "Warning: Advanced Users Only";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Update Mode:";
            // 
            // tabKey
            // 
            this.tabKey.Location = new System.Drawing.Point(4, 25);
            this.tabKey.Name = "tabKey";
            this.tabKey.Size = new System.Drawing.Size(768, 352);
            this.tabKey.TabIndex = 3;
            this.tabKey.Text = "Key Detection";
            this.tabKey.UseVisualStyleBackColor = true;
            // 
            // tabChords
            // 
            this.tabChords.Controls.Add(this.numOccurThd);
            this.tabChords.Controls.Add(this.numChordInterval);
            this.tabChords.Controls.Add(this.label23);
            this.tabChords.Controls.Add(this.label22);
            this.tabChords.Location = new System.Drawing.Point(4, 25);
            this.tabChords.Name = "tabChords";
            this.tabChords.Size = new System.Drawing.Size(768, 352);
            this.tabChords.TabIndex = 4;
            this.tabChords.Text = "Chord Detection";
            this.tabChords.UseVisualStyleBackColor = true;
            // 
            // numOccurThd
            // 
            this.numOccurThd.Location = new System.Drawing.Point(208, 59);
            this.numOccurThd.Name = "numOccurThd";
            this.numOccurThd.Size = new System.Drawing.Size(61, 22);
            this.numOccurThd.TabIndex = 7;
            // 
            // numChordInterval
            // 
            this.numChordInterval.Location = new System.Drawing.Point(147, 23);
            this.numChordInterval.Name = "numChordInterval";
            this.numChordInterval.Size = new System.Drawing.Size(61, 22);
            this.numChordInterval.TabIndex = 7;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(19, 59);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(183, 17);
            this.label23.TabIndex = 6;
            this.label23.Text = "Note Occurence Threshold:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(19, 25);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(122, 17);
            this.label22.TabIndex = 6;
            this.label22.Text = "Analysis Duration:";
            // 
            // tabLive
            // 
            this.tabLive.Controls.Add(this.label24);
            this.tabLive.Controls.Add(this.comDevices);
            this.tabLive.Location = new System.Drawing.Point(4, 25);
            this.tabLive.Name = "tabLive";
            this.tabLive.Size = new System.Drawing.Size(768, 352);
            this.tabLive.TabIndex = 5;
            this.tabLive.Text = "Live Mode";
            this.tabLive.UseVisualStyleBackColor = true;
            // 
            // comDevices
            // 
            this.comDevices.FormattingEnabled = true;
            this.comDevices.Location = new System.Drawing.Point(144, 20);
            this.comDevices.Name = "comDevices";
            this.comDevices.Size = new System.Drawing.Size(394, 24);
            this.comDevices.TabIndex = 0;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(22, 23);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(116, 17);
            this.label24.TabIndex = 1;
            this.label24.Text = "Capture Devices:";
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlPrefs);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "PreferencesForm";
            this.Text = "Preferences";
            this.tabControlPrefs.ResumeLayout(false);
            this.tabUI.ResumeLayout(false);
            this.tabUI.PerformLayout();
            this.tabPlayback.ResumeLayout(false);
            this.tabPlayback.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFollowSecs)).EndInit();
            this.tabNotes.ResumeLayout(false);
            this.tabNotes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmooth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeakBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateTime)).EndInit();
            this.tabChords.ResumeLayout(false);
            this.tabChords.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOccurThd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordInterval)).EndInit();
            this.tabLive.ResumeLayout(false);
            this.tabLive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.TabControl tabControlPrefs;
        private System.Windows.Forms.TabPage tabUI;
        private System.Windows.Forms.TabPage tabPlayback;
        private System.Windows.Forms.TabPage tabNotes;
        private System.Windows.Forms.NumericUpDown numFollowSecs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTheme;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numUpdateTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numMaxFreq;
        private System.Windows.Forms.NumericUpDown numMinFreq;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numSmooth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numPeakBuffer;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSimilarGainThd;
        private System.Windows.Forms.TextBox txtMaxFreqChange;
        private System.Windows.Forms.TextBox txtMaxGainChange;
        private System.Windows.Forms.ComboBox comboNoteAlgorithm;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox chbAntiAlias;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TabPage tabKey;
        private System.Windows.Forms.TabPage tabChords;
        private System.Windows.Forms.NumericUpDown numChordInterval;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown numOccurThd;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TabPage tabLive;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comDevices;
    }
}