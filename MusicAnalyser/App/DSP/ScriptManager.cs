using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MusicAnalyser.App.DSP
{
    class ScriptManager
    {
        public Dictionary<int, ISignalProcessor> ProcessorScripts { get; set; }
        public Dictionary<int, ISignalDetector> DetectorScripts { get; set; }

        public ScriptManager()
        {
            ProcessorScripts = new Dictionary<int, ISignalProcessor>();
            DetectorScripts = new Dictionary<int, ISignalDetector>();
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

        public Dictionary<string, string[]> GetScriptSettings(int scriptId)
        {
            if (ProcessorScripts.ContainsKey(scriptId))
                return ProcessorScripts[scriptId].Settings;
            else if (DetectorScripts.ContainsKey(scriptId))
                return DetectorScripts[scriptId].Settings;
            else
                return null;
        }

        public void LoadScripts()
        {
            string[] files = Directory.GetFiles("Scripts");
            for(int i = 0; i < files.Length; i++)
            {
                if (files[i].Substring(files[i].LastIndexOf('.') + 1) == "cs")
                {
                    CompileScript(files[i], i);
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
                    Console.WriteLine("Loaded processor script successfully - " + instance.GetType().Name);
                }
                else if (instance.GetType().GetInterfaces().Contains(typeof(ISignalDetector)))
                {
                    DetectorScripts.Add(index, (ISignalDetector)instance);
                    Console.WriteLine("Loaded detector script successfully - " + instance.GetType().Name);
                }
            }
        }
    }
}
