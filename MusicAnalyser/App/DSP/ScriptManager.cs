﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MusicAnalyser.App.DSP
{
    public class ScriptManager
    {
        public Dictionary<int, ISignalProcessor> ProcessorScripts { get; set; }
        public Dictionary<int, ISignalDetector> DetectorScripts { get; set; }
        public Dictionary<int, string[]> SettingDefaults { get; set; }
        public Dictionary<string, Dictionary<string, string[]>> Presets { get; set; }

        public ScriptManager()
        {
            ProcessorScripts = new Dictionary<int, ISignalProcessor>();
            DetectorScripts = new Dictionary<int, ISignalDetector>();
            SettingDefaults = new Dictionary<int, string[]>();
            Presets = new Dictionary<string, Dictionary<string, string[]>>();
        }

        public Dictionary<int, string> GetAllScriptNames()
        {
            var scripts = GetProcessorNames();
            var detectors = GetDetectorNames();
            detectors.ToList().ForEach(x => scripts.Add(x.Key, x.Value));
            return scripts;
        }

        public Dictionary<int, string> GetProcessorNames()
        {
            Dictionary<int, string> names = new Dictionary<int, string>();
            for (int i = 0; i < ProcessorScripts.Keys.Count; i++)
                names[ProcessorScripts.Keys.ElementAt(i)] = ProcessorScripts[ProcessorScripts.Keys.ElementAt(i)].GetType().Name.Replace("Processor", "");
            return names;
        }

        public Dictionary<int, string> GetDetectorNames()
        {
            Dictionary<int, string> names = new Dictionary<int, string>();
            for (int i = 0; i < DetectorScripts.Keys.Count; i++)
                names[DetectorScripts.Keys.ElementAt(i)] = DetectorScripts[DetectorScripts.Keys.ElementAt(i)].GetType().Name.Replace("Detector", "");
            return names;
        }

        public int GetScriptCount() { return ProcessorScripts.Count + DetectorScripts.Count; }

        public string GetScriptName(int scriptId)
        {
            if (ProcessorScripts.ContainsKey(scriptId))
                return ProcessorScripts[scriptId].GetType().Name;
            else if (DetectorScripts.ContainsKey(scriptId))
                return DetectorScripts[scriptId].GetType().Name;
            else
                return null;
        }

        public Dictionary<string, string[]> GetScriptSettings(int scriptId)
        {
            if (ProcessorScripts.ContainsKey(scriptId))
                return ProcessorScripts[scriptId].Settings;
            else if (DetectorScripts.ContainsKey(scriptId))
                return DetectorScripts[scriptId].Settings;
            else
                return null;
        }

        public string[] GetPresetNames()
        {
            return Presets.Keys.ToArray();
        }

        public Dictionary<int, int> GetPresetSelectionDict(string presetName = null, Dictionary<string, string[]> preset = null)
        {
            if(presetName != null && preset == null)
                preset = Presets[presetName];

            Dictionary<int, int> selectionDict = new Dictionary<int, int>();
            for(int i = 0; i < preset.Count; i++)
            {
                int scriptVal = -1;
                foreach(ISignalProcessor processor in ProcessorScripts.Values)
                {
                    if (processor.GetType().Name == preset.Keys.ElementAt(i))
                        scriptVal = ProcessorScripts.FirstOrDefault(x => x.Value == processor).Key;
                }
                foreach(ISignalDetector detector in DetectorScripts.Values)
                {
                    if (detector.GetType().Name == preset.Keys.ElementAt(i))
                        scriptVal = DetectorScripts.FirstOrDefault(x => x.Value == detector).Key;
                }
                selectionDict.Add(i, scriptVal);
            }
            return selectionDict;
        }

        public void SetScriptSettings(int scriptId, string[] settings)
        {
            Dictionary<string, string[]> settingsDict = GetScriptSettings(scriptId);

            if (settingsDict == null || settings == null)
                return;

            for (int i = 0; i < settingsDict.Count; i++)
            {
                if (settings.Length == i)
                    break;
                string[] vals = settingsDict[settingsDict.ElementAt(i).Key];
                vals[0] = settings[i];
            }

            if (ProcessorScripts.ContainsKey(scriptId))
                ProcessorScripts[scriptId].OnSettingsChange();
            else if (DetectorScripts.ContainsKey(scriptId))
                DetectorScripts[scriptId].OnSettingsChange();
        }

        public void SaveScriptSettings(int scriptId)
        {
            string scriptName = GetScriptName(scriptId);
            Dictionary<string, string[]> settingsDict = GetScriptSettings(scriptId);

            if (settingsDict == null)
                return;

            List<string> saveSettings = new List<string>();

            for(int i = 0; i < settingsDict.Count; i++)
                saveSettings.Add(settingsDict.ElementAt(i).Key + "=" + settingsDict.ElementAt(i).Value[0]);

            FileHandler.WriteFile("Scripts\\" + scriptName + ".ini", saveSettings.ToArray());
        }

        public void LoadScriptSettings(int scriptId)
        {
            string scriptName = GetScriptName(scriptId);
            string[] loadedSettings = FileHandler.ReadFile("Scripts\\" + scriptName + ".ini");
            if(loadedSettings != null)
            {
                Dictionary<string, string[]> settingsDict = GetScriptSettings(scriptId);
                for(int i = 0; i < loadedSettings.Length; i++)
                {
                    string[] settingSplit = loadedSettings[i].Split('=');
                    if(settingsDict.ContainsKey(settingSplit[0]))
                    {
                        string[] vals = settingsDict[settingSplit[0]];
                        vals[0] = settingSplit[1];
                    }
                }
            }
        }

        public void SavePreset(string presetName, Dictionary<string, Dictionary<string, string[]>> presetData)
        {
            List<string> presetLines = new List<string>();
            presetLines.Add("{" + presetName + "}");
            for(int i = 0; i < presetData.Count; i++)
            {
                string scriptName = presetData.Keys.ToArray()[i];
                Dictionary<string, string[]> settings = presetData.Values.ToArray()[i];
                presetLines.Add("[" + scriptName + "]");

                for (int j = 0; j < settings.Count; j++)
                    presetLines.Add(settings.ElementAt(j).Key + "=" + settings.ElementAt(j).Value[0]);
            }
            FileHandler.AppendOrReplace("Scripts\\presets.ini", presetLines.ToArray(), presetName, "{");
        }

        public void LoadPresets()
        {
            Presets.Clear();
            string[] loadedFile = FileHandler.ReadFile("Scripts\\presets.ini");
            if (loadedFile == null)
                return;

            List<string> file = loadedFile.ToList();
            Dictionary<string, string[]> preset = null;
            List<string> script = null;
            string presetName = "";
            string scriptName = "";
            for (int i = 0; i < file.Count; i++)
            {
                if(file[i].Contains('{')) // Preset name
                {
                    if (scriptName != "")
                        preset.Add(scriptName, script.ToArray());
                    if (presetName != "")
                        Presets.Add(presetName, preset);

                    preset = new Dictionary<string, string[]>();
                    script = new List<String>();
                    presetName = file[i].Substring(1, file[i].Length - 2);
                    scriptName = "";
                }
                else if(file[i].Contains('[')) // Script name
                {
                    if (scriptName != "")
                        preset.Add(scriptName, script.ToArray());

                    script = new List<string>();
                    scriptName = file[i].Substring(1, file[i].Length - 2);
                }
                else if(file[i].Contains('=')) // Script setting
                {
                    string[] setting = file[i].Split('=');
                    script.Add(setting[1]);
                }
            }
            if (scriptName != "")
                preset.Add(scriptName, script.ToArray());
            if (presetName != "")
                Presets.Add(presetName, preset);
        }

        public void LoadScripts()
        {
            string[] files = Directory.GetFiles("Scripts");
            int scriptIndex = 0;
            for(int i = 0; i < files.Length; i++)
            {
                if (files[i].Substring(files[i].LastIndexOf('.') + 1) == "cs")
                {
                    CompileScript(files[i], scriptIndex);
                    scriptIndex++;
                }
            }
        }

        public void CompileScript(string filepath, int index)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters()
            {
                GenerateInMemory = true,
                ReferencedAssemblies = {
                    Assembly.GetEntryAssembly().Location,
                    "System.dll",
                    "System.Core.dll",
                    "NAudio.dll",
                    "NAudio.Core.dll",
                    "MathNet.Numerics.dll",
                    "System.Numerics.dll",
                    "netstandard.dll"
                }
            };

            CompilerResults results = provider.CompileAssemblyFromFile(parameters, filepath);
            if (results.Errors.HasErrors)
            {
                string errors = "";
                foreach (CompilerError error in results.Errors)
                {
                    errors += string.Format("Error #{0}: {1}\n", error.ErrorNumber, error.ErrorText);
                }
                Console.Write(errors);
            }
            else
            {
                Assembly assembly = results.CompiledAssembly;
                Object instance = assembly.CreateInstance(assembly.GetTypes()[0].FullName);

                if (instance.GetType().GetInterfaces().Contains(typeof(ISignalProcessor)))
                {
                    ProcessorScripts.Add(index, (ISignalProcessor)instance);
                    if(ProcessorScripts[index].Settings != null)
                        SettingDefaults[index] = ProcessorScripts[index].Settings.Values.ToList().Select(s => s.First()).ToArray();
                    Console.WriteLine("Loaded processor script successfully - " + instance.GetType().Name);
                }
                else if (instance.GetType().GetInterfaces().Contains(typeof(ISignalDetector)))
                {
                    DetectorScripts.Add(index, (ISignalDetector)instance);
                    if (DetectorScripts[index].Settings != null)
                        SettingDefaults[index] = DetectorScripts[index].Settings.Values.ToList().Select(s => s.First()).ToArray();
                    Console.WriteLine("Loaded detector script successfully - " + instance.GetType().Name);
                }
            }
        }
    }
}

