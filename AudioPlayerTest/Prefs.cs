using System;
using System.Collections.Generic;

namespace MusicAnalyser
{
    public static class Prefs
    {
        public static Dictionary<string, string> DefaultsDict = new Dictionary<string, string>
        {
            { "UI_THEME", "0" }, { "FOLLOW_SECS", "10" }, { "UPDATE_MODE", "0" }, { "MIN_UPDATE_TIME", "10" }, { "NOTE_ALGORITHM", "0" }, { "MIN_FREQ", "30" },{ "MAX_FREQ", "2000" },
            { "SMOOTH_FACTOR", "4" }, { "SPECTRUM_AA", "1"}, { "PEAK_BUFFER", "90" }, { "MAX_GAIN_CHANGE", "8" }, { "MAX_FREQ_CHANGE", "2.8" }, { "SIMILAR_GAIN_THRESHOLD", "5" },
            { "CHORD_DETECTION_INTERVAL", "10"}, {"CHORD_NOTE_OCCURENCE_OFFSET", "8"}, {"CAPTURE_DEVICE", "0"}
        };

        public static int UI_DELAY_FACTOR = 1000000;
        public static int FOLLOW_SECS = 10;
        public static int BUFFERSIZE = (int)Math.Pow(2, 13);
        public static double PEAK_FFT_POWER = 20 * Math.Log10(12020);
        public static int SMOOTH_FACTOR = 3;
        public static int PEAK_BUFFER = 90;
        public static int MAX_FREQ = 2000;
        public static int MIN_FREQ = 30;
        public static float MAX_GAIN_CHANGE = 8.0f;
        public static float MAX_FREQ_CHANGE = 2.8f;
        public static float SIMILAR_GAIN_THRESHOLD = 5.0f;
        public static int NOTE_BUFFER_SIZE = 10000;
        public static int ERROR_DURATION = 5;
        public static int AVG_EXECUTIONS = 10;
        public static int MIN_UPDATE_TIME = 10;
        public static int UPDATE_MODE = 0;
        public static int NOTE_ALGORITHM = 0;
        public static int SPECTRUM_AA = 1;
        public static int UI_THEME = 0;
        public static float MODAL_ROOT_DIFF = 2.0f;
        public static int CHORD_DETECTION_INTERVAL = 10;
        public static int CHORD_NOTE_OCCURENCE_OFFSET = 8;
        public static int CAPTURE_DEVICE = 0;

        public static void LoadPrefs(Dictionary<string, string> prefsLoaded)
        {
            foreach(string key in prefsLoaded.Keys)
            {
                switch(key)
                {
                    case "UI_THEME":
                        UI_THEME = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "FOLLOW_SECS":
                        FOLLOW_SECS = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "UPDATE_MODE":
                        UPDATE_MODE = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "MIN_UPDATE_TIME":
                        MIN_UPDATE_TIME = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "NOTE_ALGORITHM":
                        NOTE_ALGORITHM = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "MIN_FREQ":
                        MIN_FREQ = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "MAX_FREQ":
                        MAX_FREQ = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SMOOTH_FACTOR":
                        SMOOTH_FACTOR = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SPECTRUM_AA":
                        SPECTRUM_AA = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "PEAK_BUFFER":
                        PEAK_BUFFER = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "MAX_GAIN_CHANGE":
                        MAX_GAIN_CHANGE = Convert.ToSingle(prefsLoaded[key]);
                        break;
                    case "MAX_FREQ_CHANGE":
                        MAX_FREQ_CHANGE = Convert.ToSingle(prefsLoaded[key]);
                        break;
                    case "SIMILAR_GAIN_THRESHOLD":
                        SIMILAR_GAIN_THRESHOLD = Convert.ToSingle(prefsLoaded[key]);
                        break;
                    case "CHORD_DETECTION_INTERVAL":
                        CHORD_DETECTION_INTERVAL = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "CHORD_NOTE_OCCURENCE_OFFSET":
                        CHORD_NOTE_OCCURENCE_OFFSET = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "CAPTURE_DEVICE":
                        CAPTURE_DEVICE = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    default:
                        Console.WriteLine("Error: Preference '" + key + "' could not be loaded");
                        break;
                }
            }
        }
    }
}
