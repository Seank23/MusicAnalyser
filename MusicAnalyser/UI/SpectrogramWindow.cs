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
    public partial class SpectrogramWindow : Form
    {
        private Form1 myForm;
        private SpectrogramViewer myViewer;
        private DockStyle origDock;
        private AnchorStyles origAnchor;

        public SpectrogramWindow(Form1 frm, SpectrogramViewer viewer)
        {
            InitializeComponent();
            myForm = frm;
            myViewer = viewer;
        }

        private void SpectrogramWindow_Load(object sender, EventArgs e)
        {
            origDock = myViewer.Dock;
            origAnchor = myViewer.Anchor;
            myViewer.SetNewParent(this);
            myViewer.Dock = DockStyle.Fill;
            this.Controls.Add(myViewer);
        }

        private void SpectrogramWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            myViewer.Dock = origDock;
            myViewer.Anchor = origAnchor;
            myForm.ReassignSpectrogramViewer(myViewer);
        }
    }
}
