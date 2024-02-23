using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tewls.Windows.NetApi
{
    public class NetBuffer : IDisposable
    {
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferAllocate", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferAllocate(uint ByteCount, ref IntPtr Buffer);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferReallocate", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferReallocate(IntPtr OldBuffer, uint NewByteCount, ref IntPtr NewBuffer);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferSize", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferSize(IntPtr Buffer, ref uint ByteCount);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error NetApiBufferFree(IntPtr Buffer);

        private bool _disposed = false;

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

        public IntPtr Buffer = IntPtr.Zero;

        public static implicit operator IntPtr(NetBuffer b) => b.Buffer;
        public static implicit operator uint(NetBuffer b) => b.GetSize();

        public NetBuffer()
        {
        }

        public NetBuffer(uint size)
        {
            Buffer = BufferAllocate(size);
        }

        public void ReAlloc(uint size)
        {
            if (Buffer == IntPtr.Zero)
            {
                Buffer = BufferAllocate(size);
            }
            else
            {
                Buffer = BufferReAllocate(Buffer, size);
            }
        }

        public uint GetSize()
        {
            return BufferSize(Buffer);
        }

        public IEnumerable<T> EnumStructure<T>(IntPtr buffer, uint entries, T structure = null)
            where T : class, new()
        {
            if (structure == null)
            {
                structure = new T();
            }

            IntPtr index = buffer;
            for (int i = 0; i < entries; i++)
            {
                Marshal.PtrToStructure(index, structure);
                yield return structure;
                index += Marshal.SizeOf(typeof(T));
            }
        }

        public void Free()
        {
            BufferFree(Buffer);
            Buffer = IntPtr.Zero;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (Buffer != IntPtr.Zero)
                {
                    Free();
                }
            }

            _disposed = true;
        }
              

        ~NetBuffer()
        {
            Dispose(false);
        }
    }
}
