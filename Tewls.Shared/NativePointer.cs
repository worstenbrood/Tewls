namespace Tewls.Shared
{
    /// <summary>
    /// Handle native pointers
    /// </summary>
    /// <param name="address">Address of the pointer</param>
    public abstract class NativePointer(nint address) : IDisposable
    {
        protected nint Address = address;

        /// <summary>
        /// Dispose pattern implementation to free the buffer allocated by Capstone.cs_malloc
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~NativePointer()
        {
            Dispose(false);
        }
    }
}
