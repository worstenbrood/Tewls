﻿using System;
using System.ComponentModel;

namespace Tewls.Windows.Kernel
{
    public class NativeHandle : IDisposable
    {
        public static implicit operator IntPtr(NativeHandle b) => b.Handle;
        public static implicit operator NativeHandle(IntPtr b) => new NativeHandle(b);

        public static void Close(IntPtr handle)
        {
            var result = Kernel32.CloseHandle(handle);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        protected bool Disposed = false;
        public IntPtr Handle;

        protected NativeHandle() 
        { 
        }

        public NativeHandle(IntPtr handle, bool dispose = true)
        {
            Handle = handle;
            Disposed = !dispose;
        }

        public void Close()
        {
            Close(Handle);
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
                Close();
            }

            Disposed = true;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        ~NativeHandle()
        {
            Dispose(false);
        }
    }
}
