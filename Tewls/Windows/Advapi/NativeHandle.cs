using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Advapi
{
    public class NativeHandle : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static implicit operator IntPtr(NativeHandle b) => b.Handle;

        public static void Close(IntPtr handle)
        {
            var result = CloseHandle(handle);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        private bool _disposed = false;
        public IntPtr Handle;

        public NativeHandle(IntPtr handle, bool dispose = true)
        {
            Handle = handle;
            _disposed = !dispose;
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
            if (!_disposed)
            {
                Close();
            }

            _disposed = true;
        }

        ~NativeHandle()
        {
            Dispose(false);
        }
    }
}
