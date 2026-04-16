using System.Runtime.InteropServices;
using Tewls.Shared;
using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZFormatter : IDisposable
    {
        private readonly IntPtr _formatter;

        public ZFormatter(ZydisFormatterStyle style) 
        { 
            _formatter = Marshal.AllocHGlobal(512);
            var status = Zydis.ZydisFormatterInit(_formatter, style);
            if (status.Failed)
            {
                Marshal.FreeHGlobal(_formatter);
                status.ThrowIfFailed(nameof(Zydis.ZydisFormatterInit));
            }
        }

        public void SetProperty(ZydisFormatterProperty property, IntPtr value)
        {
            var status = Zydis.ZydisFormatterSetProperty(_formatter, property, value);
            status.ThrowIfFailed(nameof(Zydis.ZydisFormatterSetProperty));
        }

        public string FormatInstruction(ref ZydisDecodedInstruction instruction, ulong runtimeAddress = 0)
        {
            var buffer = new char[256];
            using var bufferPin = new PinnedArray<char>(buffer);
            
            var status = Zydis.ZydisFormatterFormatInstruction(_formatter, ref instruction, bufferPin.Address, (uint)buffer.Length, runtimeAddress);
            status.ThrowIfFailed(nameof(Zydis.ZydisFormatterFormatInstruction));
            return new string(buffer);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(_formatter);
        }
    }
}
