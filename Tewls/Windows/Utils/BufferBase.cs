using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Utils
{
    public class BufferBase<TAlloc> : IDisposable
        where TAlloc : class, IAllocator, new()
    {
        public bool _disposed = false;
        public IntPtr Size = IntPtr.Zero;
        public IntPtr Buffer = IntPtr.Zero;
        private readonly TAlloc allocator = new TAlloc();

        public static implicit operator IntPtr(BufferBase<TAlloc> b) => b.Buffer;
        public static implicit operator uint(BufferBase<TAlloc> b) => (uint)b.Size;

        public BufferBase()
        {
        }

        public BufferBase(IntPtr size)
        {
            Buffer = allocator.Alloc(size);
            Size = size;
        }

        public BufferBase(int size)
        {
            Buffer = allocator.Alloc((IntPtr) size);
            Size = (IntPtr)size;
        }

        public void ReAlloc(IntPtr size)
        {
            if (Buffer != null)
            {
                Buffer = allocator.Alloc(Buffer);
            }
            else
            {
                Buffer = allocator.ReAlloc(Buffer, size);
            }
            Size = size;
        }

        public IEnumerable<TStruct> EnumStructure<TStruct>(uint entries, TStruct structure = null)
            where TStruct : class, new()
        {
            if (structure == null)
            {
                structure = new TStruct();
            }

            for (int i = 0; i < entries; i++)
            {
                Marshal.PtrToStructure(Buffer + (Marshal.SizeOf(typeof(TStruct)) * i), structure);
                yield return structure;
            }
        }

        public void Free()
        {
            allocator.Free(Buffer);
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
                    allocator.Free(Buffer);
                }
            }

            _disposed = true;
        }

        ~BufferBase()
        {
            Dispose(false);
        }
    }
}
