﻿
namespace MusicAnalyser.UI
{
    partial class SpectrogramWindow
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
            this.SuspendLayout();
            // 
            // SpectrogramWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1527, 571);
            this.KeyPreview = true;
            this.Name = "SpectrogramWindow";
            this.Text = "SpectrogramWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SpectrogramWindow_FormClosed);
            this.Load += new System.EventHandler(this.SpectrogramWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}