using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Utils
{
    public class HGlobalBuffer : BufferBase<HGlobalBuffer.Allocator>
    {
        public class Allocator : IMemory
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

        public HGlobalBuffer()
        {
        }

        public HGlobalBuffer(IntPtr size) : base(size)
        {
        }

        public HGlobalBuffer(int size) : base((IntPtr) size)
        {
        }

        public HGlobalBuffer(uint size) : base((IntPtr)size)
        {
        }

        public HGlobalBuffer(byte[] bytes) : base((IntPtr) bytes.Length)
        {
            Marshal.Copy(bytes, 0, Buffer, bytes.Length);
        }

        public HGlobalBuffer(string s) : base((IntPtr)((s.Length + 1) * sizeof(char)))
        {
            NativeBuffer.lstrcpyn(Buffer, s);
        }
    }

    public class HGlobalBuffer<TStruct> : BufferBase<HGlobalBuffer.Allocator, TStruct>
            where TStruct : class
    {
        public HGlobalBuffer() : base((IntPtr) Marshal.SizeOf(typeof(TStruct)))
        {
        }

        public HGlobalBuffer(TStruct s, bool copy = true) : base(s, copy)
        {
        }
    }
}
