using System;
using System.Linq;
using Tewls.Windows.Kernel;
using Tewls.Windows.Kernel.Nt;
using Tewls.Windows.PE;

namespace Runner
{
    internal class Program
    {
        private delegate void Pointer(string[] argv);

        static void Main(string[] args)
        {
            /*ConvertTools.ConvertCHeaderToEnum("C:\\Users\\tim.horemans\\Downloads\\test.txt", "C:\\Users\\tim.horemans\\Downloads\\test.cs",
                "#define IMAGE_FILE_MACHINE_(?<name>(\\w+)).*(?<value>(0x[0-9a-fA-F]+))");*/

            var process = new NativeProcess("jusched", ProcessAccessRights.AllAccess);
            Console.WriteLine("Process ID: {0}", process.GetProcessId());

            var start = DateTime.Now;

            // 32/64
            foreach (var module in process.GetModules())
            {              
                foreach (var export in process.GetExports(module.Address))
                {
                    Console.WriteLine("{0}: {1} ({2}) => 0x{3}", module.Name, export.Name ?? export.Ordinal.ToString(), export.Ordinal, export.Address.ToString("X"));
                }
            }

            Console.ReadKey();

            if (IntPtr.Size == 8)
            {
                // 32 (Wow64)
                foreach (var module in process.GetModulesWow64())
                {
                    foreach (var export in process.GetExportsWow64((uint)module.Address))
                    {
                        Console.WriteLine("{0}: {1} ({2}) => 0x{3}", module.Name, export.Name ?? export.Ordinal.ToString(), export.Ordinal, export.Address.ToString("X"));
                    }
                }
            }

            Console.WriteLine("Time: {0}", (DateTime.Now - start).TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
