using System;

namespace TewlKit.Asm
{
    /// <summary>
    /// Interface for generating stubs for assembly code injection.
    /// </summary>
    public interface IStubGenerator
    {   
        /// <summary>
        /// Gets the size of the stub in bytes.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Get the stub bytes.
        /// </summary>
        /// <returns></returns>
        byte[] GetBuffer(IntPtr address);
    }
}
