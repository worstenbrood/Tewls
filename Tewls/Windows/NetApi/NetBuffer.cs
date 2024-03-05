using System;
using System.ComponentModel;
using Tewls.Windows.Utils;

namespace Tewls.Windows.NetApi
{
    public class NetBuffer : BufferBase<NetBuffer.Allocator>
    {
        public class Allocator : IMemory
        {
            public IntPtr Alloc(IntPtr size)
            {
                return BufferAllocate((uint)size);
            }

            public void Free(IntPtr buffer)
            {
                BufferFree(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                return BufferReAllocate(buffer, (uint)size);
            }
        }       

        public static IntPtr BufferAllocate(uint byteCount)
        {
            IntPtr buffer = IntPtr.Zero;
            var result = Netapi32.NetApiBufferAllocate(byteCount, ref buffer);
            if (result == Error.Success)
            {
                return buffer;
            }

            throw new Win32Exception((int)result);
        }

        public static IntPtr BufferReAllocate(IntPtr oldBuffer, uint byteCount)
        {
            IntPtr buffer = IntPtr.Zero;
            var result = Netapi32.NetApiBufferReallocate(oldBuffer, byteCount, ref buffer);
            if (result == Error.Success)
            {
                return buffer;
            }

            throw new Win32Exception((int)result);
        }

        public static uint BufferSize(IntPtr buffer)
        {
            uint size = 0;

            var result = Netapi32.NetApiBufferSize(buffer, ref size);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }

            return size;
        }

        public static void BufferFree(IntPtr buffer)
        {
            var result = Netapi32.NetApiBufferFree(buffer);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public NetBuffer()
        {
        }
              
        public uint GetSize()
        {
            return BufferSize(Buffer);
        }
    }

    public class NetBuffer<TStruct> : BufferBase<NetBuffer.Allocator, TStruct>
            where TStruct : class
    {
        public NetBuffer(TStruct s) : base(s)
        {
        }
    }
}
