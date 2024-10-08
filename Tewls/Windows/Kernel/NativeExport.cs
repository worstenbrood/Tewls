using System;

namespace Tewls.Windows.Kernel
{
    public class NativeExport
    {
        public string Name { get; }
        public ushort Ordinal { get; }
        public IntPtr Address { get; }

        public NativeExport(string name, ushort ordinal, IntPtr address)
        {
            Name = name;
            Ordinal = ordinal;
            Address = address;
        }

        public override string ToString()
        {
            var name = string.IsNullOrEmpty(Name) ? Ordinal.ToString() : Name;
            return $"Name: {name} - Address: 0x{Address.ToInt64():X}";
        }
    }
}
