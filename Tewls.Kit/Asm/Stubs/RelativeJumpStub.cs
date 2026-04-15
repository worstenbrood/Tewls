using System;
using Tewls.Kit.Asm;

namespace Tewls.Kit.Asm.Stubs
{
    /// <summary>
    /// Relative jump stub for 32/64-bit architecture.
    /// </summary>
    public class RelativeJumpStub : StubGeneratorBase<RelativeJumpStub>
    {
        /// <inheritdoc />
        public override int Size => 5;

        /// <inheritdoc />
        public override byte[] GetBuffer(nint address)
        {
            using var stub = GetWriter();
            // Opcode for JMP rel32 is 0xE9
            stub.Write(OpCodes.RelativeJump);
            // Set the jump distance
            stub.Write((int)address);
            // Return the generated stub as a byte array
            return stub.GetBuffer();
        }
    }
}
