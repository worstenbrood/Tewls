using Tewls.CapstoneSharp.Native;

namespace Tewls.CapstoneSharp
{
    /// <summary>
    /// Capstone wrapper around cs_malloc and cs_free
    /// </summary>
    public class CapstoneBuffer : IDisposable
    {
        private nint _buffer;
        private uint _size;

        /// <summary>
        /// Constructor that accepts a buffer and its size. This is used when Capstone returns a buffer that 
        /// needs to be freed with cs_free.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        public CapstoneBuffer(nint buffer, uint size)
        {
            _buffer = buffer;
            _size = size;
        }

        /// <summary>
        /// Constructor that accepts a Capstone handle and allocates a buffer using <see cref="Capstone.cs_malloc"/>
        /// Allocate memory for 1 instruction to be used by <see cref="Capstone.cs_disasm_iter"/>
        /// </summary>
        /// <param name="handle"></param>
        public CapstoneBuffer(nint handle)
        {
            _buffer = Capstone.cs_malloc(handle);
            if (_buffer == 0)
                CapstoneException.ThrowLastError(handle);
            _size = 1;
        }

        /// <summary>
        /// Free the buffer
        /// </summary>
        protected void Free()
        {
            if (_buffer != 0)
            {
                Capstone.cs_free(_buffer, _size);
                _buffer = 0;
                _size = 0;
            }
        }

        /// <summary>
        /// Dispose pattern implementation to free the buffer allocated by Capstone.cs_malloc
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            Free();
        }

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
        ~CapstoneBuffer()
        {
            Dispose(false);
        }
    }
}
