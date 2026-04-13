using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TewlKit.Core.Functions;
using TewlKit.Hooks;
using Tewls.Windows.Kernel;

namespace TewlKit.Core
{
    /// <summary>
    /// Core Engine
    /// </summary>
    public class Engine
    {
        /*
        private static IHook[] _hookTable = new[]
        {
            new OpenProcessHook(),
        };

        private static Dictionary<string, NativeModule> GetModules(NativeProcess nativeProcess)
        {
            return nativeProcess.GetModules()
                // Skip main
                .Skip(1)
                .Join(_hookTable, l => l.Name, r => r.ModuleName, (l, r) => l, StringComparer.OrdinalIgnoreCase)
                // Order by base address
                .OrderBy(m => m.Address.ToInt64())
                .ToDictionary(m => m.Name);
        }

        private static Dictionary<string, NativeExport> GetFunctions(NativeModule nativeModule)
        {
            return nativeModule.GetExports()
                .Where(m => !string.IsNullOrEmpty(m.Name))
                .Join(_hookTable, l => l.Name, r => r.ProcName, (l, r) => l, StringComparer.OrdinalIgnoreCase)
                // Order by base address
                .OrderBy(m => m.Address.ToInt64())
                .ToDictionary(m => m.Name);
        }

        private static void Inject(NativeProcess nativeProcess)
        {
            Console.WriteLine("[Process] {0}", nativeProcess.ProcessId);
            foreach (var module in GetModules(nativeProcess))
            {
                Console.WriteLine("[Module] {0}: {1}", module.Key, module.Value.Address.ToString("X"));
                foreach(var function in GetFunctions(module.Value))
                {
                    Console.WriteLine("[Function] {0}: {1}", function.Key, function.Value.Address.ToString("X"));
                    var hook = new Procedure<OpenProcessHook.OpenProcess>(function.Value.Address);
                    
                    //var test = hook..Invoke(0, true, 0);
                }
            }
        }

        /// <summary>
        /// Start the engine
        /// </summary>
        public static void Start()
        {
            var currentProcess = Process.GetCurrentProcess();
            foreach(var process in Tools.Enum().Where(p => p.ProcessId != currentProcess.Id))
            {
                Inject(process);
            }
        }*/
    }
}
