using IlDasm_CSharp;
using System;
using System.Linq;
using Tewls.Windows.Kernel;

namespace Runner
{
    internal class Program
    {
        private delegate void Pointer(string[] argv);

        static void Main(string[] args)
        {
            /*ConvertTools.ConvertCHeaderToEnum("C:\\Users\\tim.horemans\\Downloads\\test.txt", "C:\\Users\\tim.horemans\\Downloads\\test.cs",
                "#define IMAGE_SCN_(?<name>(\\w+)).*(?<value>(0x[0-9a-fA-F]+))");*/

            var process = new NativeProcess("jusched", ProcessAccessRights.AllAccess);
            Console.WriteLine("Process ID: {0}", process.GetProcessId());

            var start = DateTime.Now;

            // 32/64
            foreach (var module in process.GetModules().Skip(1))
            {
                foreach (var export in process.GetExports(module.Address).OrderBy(e => e.Ordinal))
                {
                    var b = process.ReadBytes(export.Address, 15);
                    var len = b.GetILDasmLength(Environment.Is64BitProcess, 9);

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
                    foreach (var export in process.GetExportsWow64((uint)module.Address).OrderBy(e => e.Ordinal))
                    {
                        var b = process.ReadBytes(export.Address, 15);
                        var len = b.GetILDasmLength(false, 5);

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
