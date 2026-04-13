using System;

namespace TewlKit.Asm.Stubs
{
    /// <summary>
    /// Relative jump stub for 32-bit architecture.
    /// </summary>
    public class RelativeJumpStub32 : IStubGenerator
    {
        /// <inheritdoc />
        public int Size => 5;

        /// <inheritdoc />
        public byte[] GetStub(IntPtr address)
        {
            using var stub = new Stub(Size);
            // Opcode for JMP rel32 is 0xE9
            stub.Write((char)OpCodes.RelativeJump);
            // Set the jump distance
            stub.Write(address.ToInt32());
            // Return the generated stub as a byte array
            return stub.GetBuffer();
        }
    }
}
