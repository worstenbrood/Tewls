using System;

namespace Tewls.Windows.Utils
{
    public interface IAllocator
    {
        IntPtr Buffer { get; set; }
        IntPtr Size { get; set; }

        IntPtr Alloc(IntPtr size);
        IntPtr ReAlloc(IntPtr buffer, IntPtr size);
        void Free(IntPtr buffer);
    }
}
