using System;
using System.Collections.Generic;

namespace Tewls.Windows.Kernel
{
    public class NativeModule
    {
        public NativeProcess Process { get; }
        public string Name { get; }
        public IntPtr Address { get; }
        public uint Size { get; }
        public bool Is64Bit { get; }
        public bool IsWow64 { get; }

        public NativeModule(NativeProcess process, string name, IntPtr address, uint size, bool is64Bit, bool isWow64)
        {
            Process = process;
            Name = name;
            Address = address;
            Size = size;
            Is64Bit = is64Bit;
            IsWow64 = isWow64;
        }

        public IEnumerable<NativeExport> GetExports()
        {
            return IsWow64 ? Process.GetExportsWow64((uint)Address.ToInt32()) : Process.GetExports(Address);
        }

        public override string ToString()
        {
            return $"PID: {Process.GetProcessId()} - Name: {Name} - Address: 0x{Address.ToInt64():X}";
        }
    }
}
