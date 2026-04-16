using System.Runtime.CompilerServices;

namespace Tewls.ZydisSharp.Native
{
    public class ZyanStatus(uint value)
    {
        public uint Status { get; private set; } = value;
        public bool Success => Status >= 0;
        public bool Failed => Status < 0;

        public void ThrowIfFailed([CallerMemberName] string apiName = "")
        {
            if (Failed)
            {
                throw new InvalidOperationException($"{apiName} failed: 0x{Status:X8}");
            }
        } 
    }
}
