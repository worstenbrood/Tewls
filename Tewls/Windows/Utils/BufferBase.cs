using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Utils
{
    public abstract class BufferBase : object, IDisposable
    {
        protected bool Disposed = false;
       
        public virtual IMemory Memory { get { throw new NotImplementedException(); } }

        public IntPtr Size = IntPtr.Zero;
        public IntPtr Buffer = IntPtr.Zero;
        
        public static implicit operator IntPtr(BufferBase b) => b.Buffer;
        public static implicit operator uint(BufferBase b) => (uint)b.Size;

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
                
        public virtual IntPtr Alloc(IntPtr size)
        {
            return Memory.Alloc(size);
        }
              
        public virtual void ReAlloc(IntPtr size)
        {
            if (Buffer == IntPtr.Zero)
            {
                Buffer = Memory.Alloc(size);
            }
            else
            {
                Buffer = Memory.ReAlloc(Buffer, size);
            }
            Size = size;
        }

        public virtual void Free()
        {
            if (Buffer != IntPtr.Zero)
            {
                Memory.Free(Buffer);
                Buffer = IntPtr.Zero;
                Size = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
               Free();
            }

            Disposed = true;
        }

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

        public void Set(IntPtr buffer, IntPtr size)
        {
            Buffer = buffer;
            Size = size;
        }

        public override int GetHashCode()
        {
            return Buffer.GetHashCode();
        }

        ~BufferBase()
        {
            Dispose(false);
        }
    }

    public abstract class BufferBase<TMemory> : BufferBase
       where TMemory : class, IMemory, new()
    {
        private static readonly IMemory _memory = new TMemory();

        public override IMemory Memory { get { return _memory; } }

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

        public BufferBase()
        {
        }

        public BufferBase(IntPtr size) : base(size)
        {
        }
    }

    public abstract class BufferBase<TMemory, TStruct> : BufferBase<TMemory>
        where TMemory : class, IMemory, new()
        where TStruct : class
    {
        protected readonly static Type Type = typeof(TStruct);

        public override void Free()
        {
            if (Buffer != IntPtr.Zero && Type.IsLayoutSequential)
            {
                Marshal.DestroyStructure(Buffer, Type);
            }
            base.Free();
        }

        public override void ReAlloc(IntPtr size)
        {
            if (Buffer != IntPtr.Zero && Type.IsLayoutSequential)
            {
                Marshal.DestroyStructure(Buffer, Type);
            }
            base.ReAlloc(size);
        }

        protected BufferBase() 
        {
        }
                
        protected BufferBase(TStruct s, bool copy = true) : base((IntPtr) Marshal.SizeOf(s))
        {
            if (copy)
            {
                Marshal.StructureToPtr(s, Buffer, false);
            }
        }

        protected BufferBase(IntPtr size) : base(size)
        {
        }
    }
}
