using Tewls.CapstoneSharp.Native;
using Tewls.Shared;

namespace Tewls.CapstoneSharp
{
    /// <summary>
    /// Capstone wrapper around cs_malloc and cs_free
    /// </summary>
    /// <remarks>
    /// Constructor that accepts a buffer and its size. This is used when Capstone returns a buffer that 
    /// needs to be freed with cs_free.
    /// </remarks>
    /// <param name="buffer"></param>
    /// <param name="size"></param>
    public class CapstoneBuffer(nint buffer, uint size) : NativePointer(buffer)
    {
        private uint _size = size;

        /// <summary>
        /// Constructor that accepts a handle and allocates a buffer using <see cref="Capstone.cs_malloc"/>
        /// Allocate memory for 1 instruction to be used by <see cref="Capstone.cs_disasm_iter"/>
        /// </summary>
        /// <param name="handle"></param>
        public static CapstoneBuffer Create(nint handle)
        {
            var buffer = Capstone.cs_malloc(handle);
            if (buffer == 0)
                CapstoneException.ThrowLastError(handle);
            return new CapstoneBuffer(buffer, 1);
        }

        /// <summary>
        /// Free the buffer
        /// </summary>
        protected void Free()
        {
            if (Address != 0)
            {
                Capstone.cs_free(Address, _size);
                Address = 0;
                _size = 0;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            Free();
        }
    }
}
