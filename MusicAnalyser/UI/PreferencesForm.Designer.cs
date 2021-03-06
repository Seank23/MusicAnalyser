﻿namespace MusicAnalyser
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
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.comDevices = new System.Windows.Forms.ComboBox();
            this.numFollowSecs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTheme = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAnalysis = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numOccurThd = new System.Windows.Forms.NumericUpDown();
            this.numChordInterval = new System.Windows.Forms.NumericUpDown();
            this.numSmooth = new System.Windows.Forms.NumericUpDown();
            this.numUpdateTime = new System.Windows.Forms.NumericUpDown();
            this.comboMode = new System.Windows.Forms.ComboBox();
            this.tabSpectrogram = new System.Windows.Forms.TabPage();
            this.chbStoreSpecData = new System.Windows.Forms.CheckBox();
            this.numSpecMaxFreq = new System.Windows.Forms.NumericUpDown();
            this.numKeyBlock = new System.Windows.Forms.NumericUpDown();
            this.numChordBlock = new System.Windows.Forms.NumericUpDown();
            this.numNoteDiff = new System.Windows.Forms.NumericUpDown();
            this.numSpecUpdates = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.numMinNote = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.tabControlPrefs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFollowSecs)).BeginInit();
            this.tabAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOccurThd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmooth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateTime)).BeginInit();
            this.tabSpectrogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecMaxFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinNote)).BeginInit();
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
            this.tabControlPrefs.Controls.Add(this.tabGeneral);
            this.tabControlPrefs.Controls.Add(this.tabAnalysis);
            this.tabControlPrefs.Controls.Add(this.tabSpectrogram);
            this.tabControlPrefs.Location = new System.Drawing.Point(12, 12);
            this.tabControlPrefs.Name = "tabControlPrefs";
            this.tabControlPrefs.SelectedIndex = 0;
            this.tabControlPrefs.Size = new System.Drawing.Size(776, 381);
            this.tabControlPrefs.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.label24);
            this.tabGeneral.Controls.Add(this.comDevices);
            this.tabGeneral.Controls.Add(this.numFollowSecs);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.comboTheme);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Location = new System.Drawing.Point(4, 25);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(768, 352);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(26, 52);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(109, 17);
            this.label24.TabIndex = 7;
            this.label24.Text = "Capture Device:";
            // 
            // comDevices
            // 
            this.comDevices.FormattingEnabled = true;
            this.comDevices.Location = new System.Drawing.Point(209, 49);
            this.comDevices.Name = "comDevices";
            this.comDevices.Size = new System.Drawing.Size(273, 24);
            this.comDevices.TabIndex = 6;
            // 
            // numFollowSecs
            // 
            this.numFollowSecs.Location = new System.Drawing.Point(209, 81);
            this.numFollowSecs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFollowSecs.Name = "numFollowSecs";
            this.numFollowSecs.Size = new System.Drawing.Size(61, 22);
            this.numFollowSecs.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "seconds";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Following Interval:";
            // 
            // comboTheme
            // 
            this.comboTheme.FormattingEnabled = true;
            this.comboTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.comboTheme.Location = new System.Drawing.Point(209, 19);
            this.comboTheme.Name = "comboTheme";
            this.comboTheme.Size = new System.Drawing.Size(92, 24);
            this.comboTheme.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Theme:";
            // 
            // tabAnalysis
            // 
            this.tabAnalysis.Controls.Add(this.label10);
            this.tabAnalysis.Controls.Add(this.label9);
            this.tabAnalysis.Controls.Add(this.label8);
            this.tabAnalysis.Controls.Add(this.label7);
            this.tabAnalysis.Controls.Add(this.label6);
            this.tabAnalysis.Controls.Add(this.label5);
            this.tabAnalysis.Controls.Add(this.label4);
            this.tabAnalysis.Controls.Add(this.numOccurThd);
            this.tabAnalysis.Controls.Add(this.numChordInterval);
            this.tabAnalysis.Controls.Add(this.numSmooth);
            this.tabAnalysis.Controls.Add(this.numUpdateTime);
            this.tabAnalysis.Controls.Add(this.comboMode);
            this.tabAnalysis.Location = new System.Drawing.Point(4, 25);
            this.tabAnalysis.Name = "tabAnalysis";
            this.tabAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnalysis.Size = new System.Drawing.Size(768, 352);
            this.tabAnalysis.TabIndex = 1;
            this.tabAnalysis.Text = "Analysis";
            this.tabAnalysis.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(182, 17);
            this.label10.TabIndex = 17;
            this.label10.Text = "Chord Detection Threshold:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(164, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "Chord Detection Interval:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "draws";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Spectrum Smoothing:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(281, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "milliseconds";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Update Time:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Update Mode:";
            // 
            // numOccurThd
            // 
            this.numOccurThd.Location = new System.Drawing.Point(214, 139);
            this.numOccurThd.Name = "numOccurThd";
            this.numOccurThd.Size = new System.Drawing.Size(61, 22);
            this.numOccurThd.TabIndex = 9;
            // 
            // numChordInterval
            // 
            this.numChordInterval.Location = new System.Drawing.Point(214, 109);
            this.numChordInterval.Name = "numChordInterval";
            this.numChordInterval.Size = new System.Drawing.Size(61, 22);
            this.numChordInterval.TabIndex = 10;
            // 
            // numSmooth
            // 
            this.numSmooth.Location = new System.Drawing.Point(214, 80);
            this.numSmooth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSmooth.Name = "numSmooth";
            this.numSmooth.Size = new System.Drawing.Size(61, 22);
            this.numSmooth.TabIndex = 8;
            this.numSmooth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numUpdateTime
            // 
            this.numUpdateTime.Location = new System.Drawing.Point(214, 51);
            this.numUpdateTime.Name = "numUpdateTime";
            this.numUpdateTime.Size = new System.Drawing.Size(61, 22);
            this.numUpdateTime.TabIndex = 7;
            // 
            // comboMode
            // 
            this.comboMode.FormattingEnabled = true;
            this.comboMode.Items.AddRange(new object[] {
            "Dynamic",
            "Manual"});
            this.comboMode.Location = new System.Drawing.Point(214, 20);
            this.comboMode.Name = "comboMode";
            this.comboMode.Size = new System.Drawing.Size(92, 24);
            this.comboMode.TabIndex = 6;
            // 
            // tabSpectrogram
            // 
            this.tabSpectrogram.Controls.Add(this.chbStoreSpecData);
            this.tabSpectrogram.Controls.Add(this.numSpecMaxFreq);
            this.tabSpectrogram.Controls.Add(this.numKeyBlock);
            this.tabSpectrogram.Controls.Add(this.numChordBlock);
            this.tabSpectrogram.Controls.Add(this.numMinNote);
            this.tabSpectrogram.Controls.Add(this.numNoteDiff);
            this.tabSpectrogram.Controls.Add(this.numSpecUpdates);
            this.tabSpectrogram.Controls.Add(this.label23);
            this.tabSpectrogram.Controls.Add(this.label22);
            this.tabSpectrogram.Controls.Add(this.label21);
            this.tabSpectrogram.Controls.Add(this.label20);
            this.tabSpectrogram.Controls.Add(this.label15);
            this.tabSpectrogram.Controls.Add(this.label13);
            this.tabSpectrogram.Controls.Add(this.label14);
            this.tabSpectrogram.Controls.Add(this.label18);
            this.tabSpectrogram.Controls.Add(this.label17);
            this.tabSpectrogram.Controls.Add(this.label19);
            this.tabSpectrogram.Controls.Add(this.label16);
            this.tabSpectrogram.Controls.Add(this.label12);
            this.tabSpectrogram.Controls.Add(this.label11);
            this.tabSpectrogram.Location = new System.Drawing.Point(4, 25);
            this.tabSpectrogram.Name = "tabSpectrogram";
            this.tabSpectrogram.Size = new System.Drawing.Size(768, 352);
            this.tabSpectrogram.TabIndex = 2;
            this.tabSpectrogram.Text = "Spectrogram";
            this.tabSpectrogram.UseVisualStyleBackColor = true;
            // 
            // chbStoreSpecData
            // 
            this.chbStoreSpecData.AutoSize = true;
            this.chbStoreSpecData.Location = new System.Drawing.Point(256, 21);
            this.chbStoreSpecData.Name = "chbStoreSpecData";
            this.chbStoreSpecData.Size = new System.Drawing.Size(18, 17);
            this.chbStoreSpecData.TabIndex = 16;
            this.chbStoreSpecData.UseVisualStyleBackColor = true;
            // 
            // numSpecMaxFreq
            // 
            this.numSpecMaxFreq.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numSpecMaxFreq.Location = new System.Drawing.Point(256, 77);
            this.numSpecMaxFreq.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numSpecMaxFreq.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numSpecMaxFreq.Name = "numSpecMaxFreq";
            this.numSpecMaxFreq.Size = new System.Drawing.Size(61, 22);
            this.numSpecMaxFreq.TabIndex = 14;
            this.numSpecMaxFreq.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // numKeyBlock
            // 
            this.numKeyBlock.Location = new System.Drawing.Point(256, 189);
            this.numKeyBlock.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numKeyBlock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numKeyBlock.Name = "numKeyBlock";
            this.numKeyBlock.Size = new System.Drawing.Size(61, 22);
            this.numKeyBlock.TabIndex = 15;
            this.numKeyBlock.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numChordBlock
            // 
            this.numChordBlock.Location = new System.Drawing.Point(256, 161);
            this.numChordBlock.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numChordBlock.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numChordBlock.Name = "numChordBlock";
            this.numChordBlock.Size = new System.Drawing.Size(61, 22);
            this.numChordBlock.TabIndex = 15;
            this.numChordBlock.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numNoteDiff
            // 
            this.numNoteDiff.Location = new System.Drawing.Point(256, 105);
            this.numNoteDiff.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numNoteDiff.Name = "numNoteDiff";
            this.numNoteDiff.Size = new System.Drawing.Size(61, 22);
            this.numNoteDiff.TabIndex = 15;
            this.numNoteDiff.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSpecUpdates
            // 
            this.numSpecUpdates.Location = new System.Drawing.Point(256, 48);
            this.numSpecUpdates.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSpecUpdates.Name = "numSpecUpdates";
            this.numSpecUpdates.Size = new System.Drawing.Size(61, 22);
            this.numSpecUpdates.TabIndex = 15;
            this.numSpecUpdates.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(323, 79);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 17);
            this.label15.TabIndex = 9;
            this.label15.Text = "Hz";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(323, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "FPS";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(26, 79);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(228, 25);
            this.label14.TabIndex = 11;
            this.label14.Text = "Spectrogram Maximum Frequency:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(26, 191);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(105, 17);
            this.label18.TabIndex = 12;
            this.label18.Text = "Key Block Size:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(26, 163);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(119, 17);
            this.label17.TabIndex = 12;
            this.label17.Text = "Chord Block Size:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(26, 107);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(179, 17);
            this.label16.TabIndex = 12;
            this.label16.Text = "Note Difference Threshold:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(171, 17);
            this.label12.TabIndex = 12;
            this.label12.Text = "Spectrogram Frame Rate:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 17);
            this.label11.TabIndex = 13;
            this.label11.Text = "Store Spectrogram Data:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(26, 134);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(149, 17);
            this.label19.TabIndex = 12;
            this.label19.Text = "Minimum Note Length:";
            // 
            // numMinNote
            // 
            this.numMinNote.Location = new System.Drawing.Point(256, 132);
            this.numMinNote.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numMinNote.Name = "numMinNote";
            this.numMinNote.Size = new System.Drawing.Size(61, 22);
            this.numMinNote.TabIndex = 15;
            this.numMinNote.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(323, 107);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 17);
            this.label20.TabIndex = 9;
            this.label20.Text = "Frames";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(323, 134);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(55, 17);
            this.label21.TabIndex = 9;
            this.label21.Text = "Frames";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(323, 163);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(55, 17);
            this.label22.TabIndex = 9;
            this.label22.Text = "Frames";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(323, 191);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(55, 17);
            this.label23.TabIndex = 9;
            this.label23.Text = "Frames";
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
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFollowSecs)).EndInit();
            this.tabAnalysis.ResumeLayout(false);
            this.tabAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOccurThd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmooth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateTime)).EndInit();
            this.tabSpectrogram.ResumeLayout(false);
            this.tabSpectrogram.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecMaxFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeyBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChordBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNoteDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSpecUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinNote)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.TabControl tabControlPrefs;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabAnalysis;
        private System.Windows.Forms.ComboBox comboTheme;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comDevices;
        private System.Windows.Forms.NumericUpDown numFollowSecs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numOccurThd;
        private System.Windows.Forms.NumericUpDown numChordInterval;
        private System.Windows.Forms.NumericUpDown numSmooth;
        private System.Windows.Forms.NumericUpDown numUpdateTime;
        private System.Windows.Forms.ComboBox comboMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabSpectrogram;
        private System.Windows.Forms.CheckBox chbStoreSpecData;
        private System.Windows.Forms.NumericUpDown numSpecMaxFreq;
        private System.Windows.Forms.NumericUpDown numNoteDiff;
        private System.Windows.Forms.NumericUpDown numSpecUpdates;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numKeyBlock;
        private System.Windows.Forms.NumericUpDown numChordBlock;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numMinNote;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
    }
}