using System;
using System.ComponentModel;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class LocalBuffer : BufferBase<LocalBuffer.Allocator>
    {
        public class Allocator : IMemory
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

        public static IntPtr Alloc(IntPtr size, LocalMemFlags uFlags = LocalMemFlags.Fixed)
        {
            var result = Kernel32.LocalAlloc(uFlags, size);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public static IntPtr ReAlloc(IntPtr buffer, IntPtr size, LocalMemFlags flags = LocalMemFlags.Fixed)
        {
            var result = Kernel32.LocalReAlloc(buffer, size, flags);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public static void Free(IntPtr buffer)
        {
            var result = Kernel32.LocalFree(buffer);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }
    }
}
