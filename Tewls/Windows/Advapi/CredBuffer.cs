using System;
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
                Advapi32.CredFree(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                throw new NotImplementedException();
            }
        }
    }
}
