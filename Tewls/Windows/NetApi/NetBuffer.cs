using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.NetApi
{
    public class NetBuffer : BufferBase<NetBuffer.Allocator>
    {
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferAllocate", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferAllocate(uint ByteCount, ref IntPtr Buffer);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferReallocate", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferReallocate(IntPtr OldBuffer, uint NewByteCount, ref IntPtr NewBuffer);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferSize", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferSize(IntPtr Buffer, ref uint ByteCount);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferFree(IntPtr Buffer);

        public static IntPtr BufferAllocate(uint byteCount)
        {
            IntPtr buffer = IntPtr.Zero;
            var result = NetApiBufferAllocate(byteCount, ref buffer);
            if (result == Error.Success)
            {
                return buffer;
            }

            throw new Win32Exception((int)result);
        }

        public static IntPtr BufferReAllocate(IntPtr oldBuffer, uint byteCount)
        {
            IntPtr buffer = IntPtr.Zero;
            var result = NetApiBufferReallocate(oldBuffer, byteCount, ref buffer);
            if (result == Error.Success)
            {
                return buffer;
            }

            throw new Win32Exception((int)result);
        }

        public static uint BufferSize(IntPtr buffer)
        {
            uint size = 0;

            var result = NetApiBufferSize(buffer, ref size);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }

            return size;
        }

        public static void BufferFree(IntPtr buffer)
        {
            var result = NetApiBufferFree(buffer);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public NetBuffer()
        {
        }

        public NetBuffer(IntPtr size) : base(size)
        {
        }

        public NetBuffer(uint size) : base((IntPtr)size)
        {
        }

        public uint GetSize()
        {
            return BufferSize(Buffer);
        }

        public class Allocator : IAllocator
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
    }

    public class NetBuffer<TStruct> : BufferBase<NetBuffer.Allocator, TStruct>
            where TStruct : class
    {
        public NetBuffer()
        {
        }

        public NetBuffer(IntPtr size) : base(size)
        {
        }

        public NetBuffer(TStruct s) : base(s)
        {
        }
    }
}
