using System;
using System.Diagnostics;
using System.Linq;
using IlDasm_CSharp;
using Tewls.Windows.Kernel;

namespace TewlKit.Core
{
    public class Engine
    {
        public static void EnumProcesses(Action<Process, NativeProcess> action)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    var nativeProcess = new NativeProcess(process.Id, ProcessAccessRights.AllAccess);
                    action.Invoke(process, nativeProcess);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void Print()
        {
            EnumProcesses((p, n) =>
            {
                Console.WriteLine("Process ID: {0}", n.GetProcessId());

                // 32/64
                foreach (var module in n.GetModules().Skip(1))
                {
                    Console.WriteLine("{0} Base: 0x{1}", module.Name, module.Address.ToString("X"));
                    foreach (var export in n.GetExports(module.Address))
                    {
                        var b = n.ReadBytes(export.Address, 20);
                        var len = b.GetASMLength(0, 9, Environment.Is64BitProcess);

                        Console.WriteLine("{0}: ({1}) => 0x{2} ({3})", export.Name ?? export.Ordinal.ToString(), export.Ordinal, export.Address.ToString("X"),
                           len);
                    }
                }
            });
        }
    }
}
