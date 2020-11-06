using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MusicAnalyser.UI
{
    public partial class ScriptSelector : UserControl
    {
        public string Label { set { label1.Text = value; } }
        public ComboBox DropDown { get { return cbScript; } }

        private Form1 frm;
        public ScriptSelector(Form1 form)
        {
            InitializeComponent();
            frm = form;
        }

        private void cbScript_SelectedIndexChanged(object sender, EventArgs e)
        {
            frm.OnSelectorChange();
            if (cbScript.SelectedIndex != -1)
                frm.DisplayScriptSettings(cbScript.SelectedIndex);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frm.OnSelectorChange();
            frm.ClearSettingsTable();
        }

        private void cbScript_Click(object sender, EventArgs e)
        {
            if(cbScript.SelectedIndex != -1)
                frm.DisplayScriptSettings(cbScript.SelectedIndex);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (cbScript.SelectedIndex != -1)
                frm.DisplayScriptSettings(cbScript.SelectedIndex);
        }
    }
}
