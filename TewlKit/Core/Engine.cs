using System;
using System.Linq;
using IlDasm_CSharp;

namespace TewlKit.Core
{
    /// <summary>
    /// Core Engine
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Print stuff about all processes
        /// </summary>
        public static void Print()
        {
            Tools.Enum((p, n) =>
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
