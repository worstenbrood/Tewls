using System;

namespace Tewls.Windows.Utils
{
    public interface IMemory
    {
        IntPtr Alloc(IntPtr size);
        IntPtr ReAlloc(IntPtr buffer, IntPtr size);
        void Free(IntPtr buffer);
    }
}
