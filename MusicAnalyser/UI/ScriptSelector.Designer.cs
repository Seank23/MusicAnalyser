namespace MusicAnalyser.UI
{
    partial class ScriptSelector
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
            this.cbScript = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbScript
            // 
            this.cbScript.FormattingEnabled = true;
            this.cbScript.Location = new System.Drawing.Point(47, 2);
            this.cbScript.Margin = new System.Windows.Forms.Padding(2);
            this.cbScript.MaximumSize = new System.Drawing.Size(114, 0);
            this.cbScript.MinimumSize = new System.Drawing.Size(50, 0);
            this.cbScript.Name = "cbScript";
            this.cbScript.Size = new System.Drawing.Size(96, 21);
            this.cbScript.TabIndex = 0;
            this.cbScript.SelectedIndexChanged += new System.EventHandler(this.cbScript_SelectedIndexChanged);
            this.cbScript.Click += new System.EventHandler(this.cbScript_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Script 1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(147, 4);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(22, 19);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "X";
            this.btnRemove.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ScriptSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbScript);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ScriptSelector";
            this.Size = new System.Drawing.Size(172, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemove;
    }
}
