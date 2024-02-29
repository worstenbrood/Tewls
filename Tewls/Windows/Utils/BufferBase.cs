using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Utils
{
    public abstract class BufferBase<TAlloc> : IDisposable
        where TAlloc : class, IAllocator, new()
    {
        private bool _disposed = false;
        protected static readonly TAlloc allocator = new TAlloc();

        public IntPtr Size = IntPtr.Zero;
        public IntPtr Buffer = IntPtr.Zero;
        
        public static implicit operator IntPtr(BufferBase<TAlloc> b) => b.Buffer;
        public static implicit operator uint(BufferBase<TAlloc> b) => (uint)b.Size;

        protected BufferBase()
        {
        }

        protected BufferBase(IntPtr size)
        {
            if (size != IntPtr.Zero) 
            {
                Buffer = Alloc(size);
            }
            
            Size = size;
        }

        public virtual IntPtr Alloc(IntPtr size)
        {
            return allocator.Alloc(size);
        }
              
        public virtual void ReAlloc(IntPtr size)
        {
            if (Buffer == null)
            {
                Buffer = allocator.Alloc(size);
            }
            else
            {
                Buffer = allocator.ReAlloc(Buffer, size);
            }
            Size = size;
        }

        public virtual void Free()
        {
            if (Buffer != IntPtr.Zero)
            {
                allocator.Free(Buffer);
                Buffer = IntPtr.Zero;
                Size = IntPtr.Zero;
            }
        }

        public static T PtrToStructure<T>(IntPtr buffer, T resource = null)
          where T : class, new()
        {
            if (resource == null)
            {
                resource = new T();
            }

            Marshal.PtrToStructure(buffer, resource);

            return resource;
        }

        public T PtrToStructure<T>(T resource = null)
          where T : class, new()
        {
            return PtrToStructure(Buffer, resource);
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

        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
               Free();
            }

            _disposed = true;
        }

        ~BufferBase()
        {
            Dispose(false);
        }
    }

    public abstract class BufferBase<TAlloc, TStruct> : BufferBase<TAlloc>
        where TAlloc : class, IAllocator, new()
        where TStruct : class
    {
        public override void Free()
        {
            if (typeof(TStruct).IsLayoutSequential)
            {
                Marshal.DestroyStructure(Buffer, typeof(TStruct));
            }
            base.Free();
        }

        public override void ReAlloc(IntPtr size)
        {
            if (typeof(TStruct).IsLayoutSequential)
            {
                Marshal.DestroyStructure(Buffer, typeof(TStruct));
            }
            base.ReAlloc(size);
        }

        protected BufferBase() 
        {
        }
                
        protected BufferBase(TStruct s) : base((IntPtr) Marshal.SizeOf(s))
        {
            Marshal.StructureToPtr(s, Buffer, false);
        }

        protected BufferBase(IntPtr size) : base(size)
        {
        }
    }
}
