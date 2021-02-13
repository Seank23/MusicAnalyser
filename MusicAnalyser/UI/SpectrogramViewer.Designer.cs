
namespace MusicAnalyser.UI
{
    partial class SpectrogramViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpectrogramViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SpectrogramViewer";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SpectrogramViewer_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SpectrogramViewer_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SpectrogramViewer_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
