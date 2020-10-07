using MusicAnalyser.App;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicAnalyser
{
    public partial class PreferencesForm : Form
    {
        private Dictionary<int, string> updateModeDict = new Dictionary<int, string> { { 0, "Dynamic" }, { 1, "Manual" } };
        private Dictionary<int, string> noteAlgorithmDict = new Dictionary<int, string>() { { 0, "By Magnitude" }, { 1, "By Slope (Alpha)" } };
        private Dictionary<int, string> uiThemeDict = new Dictionary<int, string>() { { 0, "Light" }, { 1, "Dark" } };

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
            comboNoteAlgorithm.Text = noteAlgorithmDict[Prefs.NOTE_ALGORITHM];
            numMinFreq.Value = Prefs.MIN_FREQ;
            numMaxFreq.Value = Prefs.MAX_FREQ;
            numSmooth.Value = Prefs.SMOOTH_FACTOR;
            chbAntiAlias.Checked = Convert.ToBoolean(Prefs.SPECTRUM_AA);
            numPeakBuffer.Value = Prefs.PEAK_BUFFER;
            txtMaxGainChange.Text = Prefs.MAX_GAIN_CHANGE.ToString();
            txtMaxFreqChange.Text = Prefs.MAX_FREQ_CHANGE.ToString();
            txtSimilarGainThd.Text = Prefs.SIMILAR_GAIN_THRESHOLD.ToString();
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
            prefsToSave[4] = "NOTE_ALGORITHM=" + DictIndexOf(noteAlgorithmDict, comboNoteAlgorithm.Text);
            prefsToSave[5] = "MIN_FREQ=" + numMinFreq.Value;
            prefsToSave[6] = "MAX_FREQ=" + numMaxFreq.Value;
            prefsToSave[7] = "SMOOTH_FACTOR=" + numSmooth.Value;
            prefsToSave[8] = "SPECTRUM_AA=" + Convert.ToInt32(chbAntiAlias.Checked);
            prefsToSave[9] = "PEAK_BUFFER=" + numPeakBuffer.Value;
            prefsToSave[10] = "MAX_GAIN_CHANGE=" + txtMaxGainChange.Text;
            prefsToSave[11] = "MAX_FREQ_CHANGE=" + txtMaxFreqChange.Text;
            prefsToSave[12] = "SIMILAR_GAIN_THRESHOLD=" + txtSimilarGainThd.Text;
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
