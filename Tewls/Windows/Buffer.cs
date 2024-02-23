using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows
{
    public class HGlobalBuffer : IDisposable
    {
        public bool _disposed = false;
        public IntPtr Size = IntPtr.Zero;
        public IntPtr Buffer = IntPtr.Zero;

        public static implicit operator IntPtr(HGlobalBuffer b) => b.Buffer;
        public static implicit operator uint(HGlobalBuffer b) => (uint) b.Size;

        public HGlobalBuffer(IntPtr size)
        {
            Buffer = Marshal.AllocHGlobal(size);
            Size = size;
        }

        public HGlobalBuffer(int size)
        {
            Buffer = Marshal.AllocHGlobal(size);
            Size = (IntPtr) size;
        }

        public void ReAlloc(IntPtr size)
        {
            Buffer = Marshal.ReAllocHGlobal(Buffer, size);
            Size = size;
        }

        public void Free()
        {
            Marshal.FreeHGlobal(Buffer);
            Buffer = IntPtr.Zero;
            Size = IntPtr.Zero;
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

        ~HGlobalBuffer()
        {
            Dispose(false);
        }
    }
}
