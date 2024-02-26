using System;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class CredBuffer : BufferBase<CredBuffer.Allocator>
    {
        public class Allocator : IAllocator
        {
            public IntPtr Alloc(IntPtr size)
            {
                throw new NotImplementedException();
            }

            public void Free(IntPtr buffer)
            {
                Cred.CredFree(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                throw new NotImplementedException();
            }
        }
    }
}
