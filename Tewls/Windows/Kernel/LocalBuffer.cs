using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class LocalBuffer : BufferBase<LocalBuffer.Allocator>
    {
        public class Allocator : IAllocator
        {
            public IntPtr Alloc(IntPtr size)
            {
                return LocalBuffer.Alloc(size);
            }

            public void Free(IntPtr buffer)
            {
                LocalBuffer.Free(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                return LocalBuffer.ReAlloc(buffer, size);
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LocalAlloc(LocalMemFlags uFlags, IntPtr uBytes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LocalReAlloc(IntPtr hMem, IntPtr uBytes, LocalMemFlags uFlags);

        public static IntPtr Alloc(IntPtr size, LocalMemFlags uFlags = LocalMemFlags.Fixed)
        {
            var result = LocalAlloc(uFlags, size);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public static IntPtr ReAlloc(IntPtr buffer, IntPtr size, LocalMemFlags flags = LocalMemFlags.Fixed)
        {
            var result = LocalReAlloc(buffer, size, flags);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public static void Free(IntPtr buffer)
        {
            var result = LocalFree(buffer);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }
    }
}
