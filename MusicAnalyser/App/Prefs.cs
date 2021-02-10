using System;
using System.Collections.Generic;

namespace MusicAnalyser.App
{
    public static class Prefs
    {
        public static Dictionary<string, string> DefaultsDict = new Dictionary<string, string>
        {
            { "UI_THEME", "0" }, { "FOLLOW_SECS", "10" }, { "UPDATE_MODE", "0" }, { "MIN_UPDATE_TIME", "10" }, { "SMOOTH_FACTOR", "4" },
            { "CHORD_DETECTION_INTERVAL", "10" }, { "CHORD_NOTE_OCCURENCE_OFFSET", "8" }, { "CAPTURE_DEVICE", "0" }, { "STORE_SPEC_DATA", "1" }, { "SPEC_UPDATE_RATE", "30" }
        };

        public static int UI_DELAY_FACTOR = 1000000;
        public static int FOLLOW_SECS = 10;
        public static int BUFFERSIZE = (int)Math.Pow(2, 13);
        public static int SMOOTH_FACTOR = 3;
        public static int NOTE_BUFFER_SIZE = 10000;
        public static int ERROR_DURATION = 5;
        public static int AVG_EXECUTIONS = 10;
        public static int MIN_UPDATE_TIME = 10;
        public static int UPDATE_MODE = 0;
        public static int UI_THEME = 0;
        public static float MODAL_ROOT_DIFF = 4.0f;
        public static int CHORD_DETECTION_INTERVAL = 10;
        public static int CHORD_NOTE_OCCURENCE_OFFSET = 8;
        public static int CAPTURE_DEVICE = 0;
        public static bool STORE_SPEC_DATA = true;
        public static int SPEC_UPDATE_RATE = 30;

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
                    case "SMOOTH_FACTOR":
                        SMOOTH_FACTOR = Convert.ToInt32(prefsLoaded[key]);
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
                    case "STORE_SPEC_DATA":
                        STORE_SPEC_DATA = Convert.ToBoolean(prefsLoaded[key]);
                        break;
                    case "SPEC_UPDATE_RATE":
                        SPEC_UPDATE_RATE = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    default:
                        Console.WriteLine("Error: Preference '" + key + "' could not be loaded");
                        break;
                }
            }
        }
    }
}
