﻿using System;
using System.Collections.Generic;

namespace MusicAnalyser.App
{
    public static class Prefs
    {
        public static Dictionary<string, string> DefaultsDict = new Dictionary<string, string>
        {
            { "UI_THEME", "0" }, { "FOLLOW_SECS", "10" }, { "UPDATE_MODE", "0" }, { "MIN_UPDATE_TIME", "10" }, { "RESAMP_CHANNELS", "2" }, { "SMOOTH_FACTOR", "3" },
            { "CHORD_DETECTION_INTERVAL", "10" }, { "CHORD_NOTE_OCCURENCE_OFFSET", "8" }, { "CAPTURE_DEVICE", "0" }, { "STORE_SPEC_DATA", "1" }, 
            { "SPEC_UPDATE_RATE", "30" }, { "SPEC_MAX_FREQ", "2000" }, { "SPEC_NOTE_DIFF", "10" }, { "SPEC_MIN_NOTE", "10" }, { "SPEC_CHORD_BLOCK", "50" }, { "SPEC_KEY_BLOCK", "200" }
        };

        public static int UI_DELAY_FACTOR = 1000000;
        public static int FOLLOW_SECS = 10;
        public static int BUFFERSIZE = (int)Math.Pow(2, 12); // 4096
        public static int RESAMP_CHANNELS = 2;
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
        public static int SPEC_MAX_FREQ = 2000;
        public static int SPEC_NOTE_DIFF = 10;
        public static int SPEC_MIN_NOTE = 10;
        public static int SPEC_CHORD_BLOCK = 50;
        public static int SPEC_KEY_BLOCK = 200;

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
                    case "RESAMP_CHANNELS":
                        RESAMP_CHANNELS = Convert.ToInt32(prefsLoaded[key]);
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
                    case "SPEC_MAX_FREQ":
                        SPEC_MAX_FREQ = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SPEC_NOTE_DIFF":
                        SPEC_NOTE_DIFF = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SPEC_MIN_NOTE":
                        SPEC_MIN_NOTE = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SPEC_CHORD_BLOCK":
                        SPEC_CHORD_BLOCK = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    case "SPEC_KEY_BLOCK":
                        SPEC_KEY_BLOCK = Convert.ToInt32(prefsLoaded[key]);
                        break;
                    default:
                        Console.WriteLine("Error: Preference '" + key + "' could not be loaded");
                        break;
                }
            }
        }
    }
}
