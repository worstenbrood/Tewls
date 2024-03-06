using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Utils
{
    public class Pin : IDisposable
    {
        protected bool Disposed = false;
        public GCHandle Handle;

        public Pin(object o) 
        { 
            Handle = GCHandle.Alloc(o, GCHandleType.Pinned);
        }

        public IntPtr AddrOfPinnedObject() 
        { 
            return Handle.AddrOfPinnedObject(); 
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (Handle != null) 
                { 
                    Handle.Free();
                }

                Disposed = true;
            }
        }
    }
}
