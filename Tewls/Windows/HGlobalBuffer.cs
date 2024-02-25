using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows
{
    public class HGlobalAllocator : IAllocator
    {
        public IntPtr Buffer { get; set ; }
        public IntPtr Size { get ; set; }

        public IntPtr Alloc(IntPtr size)
        {
            return Marshal.AllocHGlobal(size);
        }

        public void Free(IntPtr buffer)
        {
            Marshal.FreeHGlobal(buffer);
        }

        public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
        {
            return Marshal.ReAllocHGlobal(buffer, size);
        }
    }

    public class HGlobalBuffer : BufferBase<HGlobalAllocator>, IDisposable
    {
        public HGlobalBuffer(IntPtr size): base(size) 
        { 
        }
        
        public HGlobalBuffer(int size) : base(size)
        {
        }
    }
}
