using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Utils
{
    public class HGlobalBuffer : BufferBase<HGlobalBuffer.Allocator>
    {
        public HGlobalBuffer(IntPtr size) : base(size)
        {
        }

        public class Allocator : IAllocator
        {
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
    }

    public class HGlobalBuffer<TStruct> : BufferBase<HGlobalBuffer.Allocator, TStruct>
            where TStruct : class
    {
        public HGlobalBuffer()
        {
        }

        public HGlobalBuffer(IntPtr size) : base(size)
        {
        }

        public HGlobalBuffer(TStruct s) : base(s)
        {
        }

        public HGlobalBuffer(uint size) : base((IntPtr)size)
        {
        }
    }
}
