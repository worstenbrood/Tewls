using System.Runtime.InteropServices;

namespace Tewls.Shared
{
    public class Pinned<T>(T value) : IDisposable
    {
        private GCHandle _handle = GCHandle.Alloc(value, GCHandleType.Pinned);

        public IntPtr Address => _handle.AddrOfPinnedObject();

        public void Dispose() => _handle.Free();
    }
}
