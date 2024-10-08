using System;

namespace Tewls.Windows.Utils
{
    public class NativePointer : IDisposable
    {
        public readonly IntPtr Pointer;
        protected bool Disposed = false;

        public static implicit operator IntPtr(NativePointer b) => b.Pointer;
        public static implicit operator NativePointer(IntPtr b) => new NativePointer(b);

        public NativePointer()
        {
        }

        public NativePointer(IntPtr pointer)
        {
            Pointer = pointer;
        }

        protected virtual void Dispose(bool disposing)
        {
            throw new NotImplementedException("Dispose"); 
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Dispose(true);
            Disposed = true;
        }

        public override int GetHashCode()
        {
            return Pointer.GetHashCode();
        }

        ~NativePointer()
        {
            if (Disposed)
            {
                return;
            }

            Dispose(false);
            Disposed = true;
        }
    }
}
