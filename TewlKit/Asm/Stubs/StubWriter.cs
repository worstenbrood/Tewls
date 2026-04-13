using System.IO;

namespace TewlKit.Asm.Stubs
{
    /// <summary>
    /// Class for creating stub buffers. This class inherits from BinaryWriter and 
    /// uses a MemoryStream as the underlying stream to write bytes to.
    /// </summary>
    public class StubWriter : BinaryWriter
    {
        /// <summary>
        /// Unlimited size stub that uses a MemoryStream as the underlying stream.
        /// </summary>
        public StubWriter() : base(new MemoryStream())
        {
        }

        /// <summary>
        /// Fixed size stub that uses a MemoryStream with a specified capacity as the underlying stream.
        /// </summary>
        /// <param name="size"></param>
        public StubWriter(int size) : base(new MemoryStream(size))
        {
        }

        /// <summary>
        /// Returns the underlying byte array of the stub.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffer() => ((MemoryStream)BaseStream).ToArray();

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Dispose the underlying stream as well
            BaseStream?.Dispose();
        }
    }
}
