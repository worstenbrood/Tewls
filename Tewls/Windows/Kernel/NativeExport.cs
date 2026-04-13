using System;

namespace Tewls.Windows.Kernel
{
    public class NativeExport(string name, ushort ordinal, IntPtr address)
    {
        public string Name { get; } = name;
        public ushort Ordinal { get; } = ordinal;
        public IntPtr Address { get; } = address;

        public override string ToString()
        {
            var name = string.IsNullOrEmpty(Name) ? Ordinal.ToString() : Name;
            return $"Name: {name} - Address: 0x{Address.ToInt64():X}";
        }
    }
}
