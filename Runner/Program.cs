using IlDasm_CSharp;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Tewls.Windows;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel;
using Tewls.Windows.NetApi;
using Tewls.Windows.NetApi.Structures;
using Tewls.Windows.Utils;

namespace Runner
{
    internal class Program
    {
        private delegate void Pointer(string[] argv);

        static void Main(string[] args)
        {
            foreach (var info in NetGroup.Enum<GroupInfo0>())
            {
                Console.WriteLine($"{info.Name}");
            }

            foreach (var cred in Cred.Enumerate())
            {
                Console.WriteLine($"{cred.UserName} : {cred.TargetName} : {cred.GetPassword()}");
            }

            var process = new NativeProcess("jusched", ProcessAccessRights.AllAccess);
            Console.WriteLine("Process ID: {0}", process.GetProcessId());

            var start = DateTime.Now;

            // 32/64
            foreach (var module in process.GetModules().Skip(1))
            {
                foreach (var export in process.GetExports(module.Address))
                {
                    var b = process.ReadBytes(export.Address, 20);
                    var len = b.GetASMLength(0, 9, Environment.Is64BitProcess);

                    Console.WriteLine("{0}: {1} ({2}) => 0x{3} ({4})", module.Name, export.Name ?? export.Ordinal.ToString(), export.Ordinal, export.Address.ToString("X"),
                        len);
                }
            }           

            //Console.ReadKey();

            if (Environment.Is64BitProcess)
            {
                // 32 (Wow64). Only need to do this if we are 64bit and the process is 32bit.
                foreach (var module in process.GetModulesWow64().Skip(1))
                {
                    foreach (var export in process.GetExportsWow64((uint)module.Address))
                    {
                        var b = process.ReadBytes(export.Address, 15);
                        var len = b.GetASMLength(0, 5);

                        Console.WriteLine("{0}: {1} ({2}) => 0x{3} ({4})", module.Name, export.Name ?? export.Ordinal.ToString(), export.Ordinal, export.Address.ToString("X"),
                            len);
                    }
                }
            }

            Console.WriteLine("Time: {0}", (DateTime.Now - start).TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
