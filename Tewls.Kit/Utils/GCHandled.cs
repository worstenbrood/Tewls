using System;
using System.Runtime.InteropServices;

namespace Tewls.Kit.Utils
{
    /// <summary>
    /// GCHandled is a class that wraps a GCHandle to ensure that the object is not collected by the garbage collector 
    /// while it is still in use. It implements IDisposable to allow for proper cleanup of the handle 
    /// when it is no longer needed.
    /// </summary>
    public class GCHandled : IDisposable
    {
        private GCHandle _handle;

        /// <summary>
        /// Constructs a new GCHandled object and allocates a GCHandle for the current instance.
        /// </summary>
        /// <param name="type"></param>
        public GCHandled(GCHandleType type)
        {
            // Alloc handle
            _handle = GCHandle.Alloc(this, type);
        }

        /// <summary>
        /// Constructs a new GCHandled object with a normal GCHandle type. 
        /// This is the default constructor that can be used when no specific handle type is required.
        /// </summary>
        public GCHandled(): this(GCHandleType.Normal)
        {
        }

        /// <summary>
        /// Address of the pinned object
        /// </summary>
        public nint AddressOfPinnedObject => _handle.AddrOfPinnedObject();

        /// <summary>
        /// Disposes of the GCHandle by freeing it, allowing the garbage collector to collect the 
        /// object if there are no other references to it.
        /// </summary>
        public void Dispose()
        {
            // Clean up resources if necessary
            _handle.Free();
        }
    }
}
