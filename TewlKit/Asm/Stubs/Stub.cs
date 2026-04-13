using System.IO;

namespace TewlKit.Asm.Stubs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    public class Stub(int size) : StreamWriter(new MemoryStream(size))
    {
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
