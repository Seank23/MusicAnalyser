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
        }

        private void SavePrefs()
        {
            string[] prefsToSave = new string[16];
            prefsToSave[0] = "UI_THEME=" + DictIndexOf(uiThemeDict, comboTheme.Text);
            prefsToSave[1] = "FOLLOW_SECS=" + numFollowSecs.Value;
            prefsToSave[2] = "UPDATE_MODE=" + DictIndexOf(updateModeDict, comboMode.Text);
            prefsToSave[3] = "MIN_UPDATE_TIME=" + numUpdateTime.Value;
            prefsToSave[7] = "SMOOTH_FACTOR=" + numSmooth.Value;
            prefsToSave[13] = "CHORD_DETECTION_INTERVAL=" + numChordInterval.Value;
            prefsToSave[14] = "CHORD_NOTE_OCCURENCE_OFFSET=" + numOccurThd.Value;
            prefsToSave[15] = "CAPTURE_DEVICE=" + comDevices.SelectedIndex; 
            FileHandler.WriteFile("prefs.ini", prefsToSave); 
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
    }
}
