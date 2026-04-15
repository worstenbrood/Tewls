using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    public class ZyanStatus(int value)
    {
        public int Status { get; private set; } = value;
        public bool Success => Status >= 0;
        public bool Failed => Status < 0;

        public void ThrowIfFailed(string apiName)
        {
            if (Failed)
            {
                throw new InvalidOperationException($"{apiName} failed: 0x{Status:X8}");
            }
        } 
    }
}
