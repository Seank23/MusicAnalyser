using MusicAnalyser.App;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicAnalyser
{
    public partial class PreferencesForm : Form
    {
        private Dictionary<int, string> updateModeDict = new Dictionary<int, string> { { 0, "Dynamic" }, { 1, "Manual" } };
        private Dictionary<int, string> uiThemeDict = new Dictionary<int, string>() { { 0, "Light" } };

        private Form1 ui;

        public PreferencesForm(Form1 form)
        {
            InitializeComponent();
            ui = form;
            InitPrefs();
            SetupDeviceList();
        }

        private void InitPrefs()
        {
            comboTheme.Text = uiThemeDict[Prefs.UI_THEME];
            numFollowSecs.Value = Prefs.FOLLOW_SECS;
            comboMode.Text = updateModeDict[Prefs.UPDATE_MODE];
            numUpdateTime.Value = Prefs.MIN_UPDATE_TIME;
            numSmooth.Value = Prefs.SMOOTH_FACTOR;
            numChordInterval.Value = Prefs.CHORD_DETECTION_INTERVAL;
            numOccurThd.Value = Prefs.CHORD_NOTE_OCCURENCE_OFFSET;
            chbStoreSpecData.Checked = Prefs.STORE_SPEC_DATA;
            numSpecUpdates.Value = Prefs.SPEC_UPDATE_RATE;
            numSpecMaxFreq.Value = Prefs.SPEC_MAX_FREQ;
            numNoteDiff.Value = Prefs.SPEC_NOTE_DIFF;
            numChordBlock.Value = Prefs.SPEC_CHORD_BLOCK;
            numKeyBlock.Value = Prefs.SPEC_KEY_BLOCK;
        }

        private void SavePrefs()
        {
            List<string> prefsToSave = new List<string>();
            prefsToSave.Add("UI_THEME=" + DictIndexOf(uiThemeDict, comboTheme.Text));
            prefsToSave.Add("FOLLOW_SECS=" + numFollowSecs.Value);
            prefsToSave.Add("UPDATE_MODE=" + DictIndexOf(updateModeDict, comboMode.Text));
            prefsToSave.Add("MIN_UPDATE_TIME=" + numUpdateTime.Value);
            prefsToSave.Add("SMOOTH_FACTOR=" + numSmooth.Value);
            prefsToSave.Add("CHORD_DETECTION_INTERVAL=" + numChordInterval.Value);
            prefsToSave.Add("CHORD_NOTE_OCCURENCE_OFFSET=" + numOccurThd.Value);
            prefsToSave.Add("CAPTURE_DEVICE=" + comDevices.SelectedIndex);
            prefsToSave.Add("STORE_SPEC_DATA=" + chbStoreSpecData.Checked);
            prefsToSave.Add("SPEC_UPDATE_RATE=" + numSpecUpdates.Value);
            prefsToSave.Add("SPEC_MAX_FREQ=" + numSpecMaxFreq.Value);
            prefsToSave.Add("SPEC_NOTE_DIFF=" + numNoteDiff.Value);
            prefsToSave.Add("SPEC_CHORD_BLOCK=" + numChordBlock.Value);
            prefsToSave.Add("SPEC_KEY_BLOCK=" + numKeyBlock.Value);
            FileHandler.WriteFile("prefs.ini", prefsToSave.ToArray()); 
        }

        private void SetupDeviceList()
        {
            for (int n = 0; n < WaveIn.DeviceCount; n++)
            {
                var device = WaveIn.GetCapabilities(n);
                comDevices.Items.Add(device.ProductName);
            }
            comDevices.SelectedIndex = Prefs.CAPTURE_DEVICE;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePrefs();
            AppController.LoadPrefs();
            ui.UpdateUI();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            Prefs.LoadPrefs(Prefs.DefaultsDict);
            InitPrefs();
        }

        private int DictIndexOf(Dictionary<int, string> dict, string element)
        {
            int i = 0;
            foreach (var value in dict.Values)
            {
                if (value == element)
                    return i;
                i++;
            }
            return -1;
        }

        private void chbStoreSpecData_CheckedChanged(object sender, EventArgs e)
        {
            if (chbStoreSpecData.Checked)
                numSpecUpdates.Enabled = true;
            else
                numSpecUpdates.Enabled = false;
        }
    }
}
