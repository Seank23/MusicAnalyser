using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkGithub.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/Seank23/MusicAnalyser");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
