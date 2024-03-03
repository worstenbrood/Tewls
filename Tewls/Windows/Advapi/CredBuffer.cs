using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class CredBuffer : BufferBase<CredBuffer.Allocator>
    {
        public class Allocator : IMemory
        {
            public IntPtr Alloc(IntPtr size)
            {
                throw new NotImplementedException();
            }

            public void Free(IntPtr buffer)
            {
                CredFree(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                throw new NotImplementedException();
            }
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void CredFree(IntPtr Buffer);
    }
}
