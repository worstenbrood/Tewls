using System.Runtime.InteropServices;

namespace Tewls.Shared
{
    /// <summary>
    /// Gives a disposable wrapper around a pinned object, allowing you to get the address of the 
    /// pinned object and ensuring that it is unpinned when disposed.
    /// </summary>
    /// <typeparam name="T">type of <paramref name="value"/></typeparam>
    /// <param name="value">value to pin</param>
    public class Pinned<T>(T value) : IDisposable
    {
        private GCHandle _gcHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
        public IntPtr Address => _gcHandle.AddrOfPinnedObject();
        public void Dispose() => _gcHandle.Free();
    }

    /// <summary>
    /// Same as <see cref="Pinned{T}"/> but with a more specific type of T[] to avoid having to specify 
    /// the type parameter when pinning arrays.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public class PinnedArray<T>(T[] value) : Pinned<T[]>(value)
    {
    }
}
