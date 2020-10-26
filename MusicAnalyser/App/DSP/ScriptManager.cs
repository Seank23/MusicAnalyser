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
        public List<ISignalProcessor> ProcessorScripts { get; set; }
        public List<ISignalDetector> DetectorScripts { get; set; }

        public ScriptManager()
        {
            ProcessorScripts = new List<ISignalProcessor>();
            DetectorScripts = new List<ISignalDetector>();
        }

        public string[] GetProcessorNames()
        {
            string[] names = new string[ProcessorScripts.Count];
            for (int i = 0; i < names.Length; i++)
                names[i] = ProcessorScripts[i].GetType().Name.Replace("Processor", "");
            return names;
        }

        public string[] GetDetectorNames()
        {
            string[] names = new string[DetectorScripts.Count];
            for (int i = 0; i < names.Length; i++)
                names[i] = DetectorScripts[i].GetType().Name.Replace("Detector", "");
            return names;
        }

        public void LoadScripts()
        {
            string[] files = Directory.GetFiles("Scripts");
            foreach(string filepath in files)
            {
                if (filepath.Substring(filepath.LastIndexOf('.') + 1) == "cs")
                {
                    CompileScript(filepath);
                }
            }
        }

        public void CompileScript(string filepath)
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
                    ProcessorScripts.Add((ISignalProcessor)instance);
                    Console.WriteLine("Loaded processor script successfully - " + instance.GetType().Name);
                }
                else if (instance.GetType().GetInterfaces().Contains(typeof(ISignalDetector)))
                {
                    DetectorScripts.Add((ISignalDetector)instance);
                    Console.WriteLine("Loaded detector script successfully - " + instance.GetType().Name);
                }
            }
        }
    }
}
