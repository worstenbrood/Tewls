using System;

namespace TewlKit.Asm.Stubs
{
    /// <summary>
    /// Indirect jump stub for 32-bit architecture. 
    /// This stub uses the MOV EAX, imm32 and JMP EAX instructions to 
    /// perform an indirect jump to the specified address.
    /// </summary>
    public class IndirectJumpEaxStub : StubGeneratorBase<IndirectJumpEaxStub>
    {
        /// <inheritdoc />
        public override int Size => 6;

        /// <inheritdoc />
        public override byte[] GetBuffer(IntPtr address)
        {
            using var stub = GetWriter();
            // Opcode for MOV EAX, imm32 is 0xB8
            stub.Write(OpCodes.MovEax);
            // Write the 32-bit address to jump to
            stub.Write(address.ToInt32());
            // Opcode for JMP EAX is 0xFFE0
            stub.Write(OpCodes.JmpEaxRax);
            // Return the generated stub as a byte array
            return stub.GetBuffer();
        }
    }
}
