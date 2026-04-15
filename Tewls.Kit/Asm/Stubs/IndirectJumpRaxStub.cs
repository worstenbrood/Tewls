namespace Tewls.Kit.Asm.Stubs
{
    /// <summary>
    /// Indirect jump stub for 64-bit architecture. 
    /// This stub uses the MOV RAX, imm64 and JMP RAX instructions to 
    /// perform an indirect jump to the specified address.
    /// </summary>
    public class IndirectJumpRaxStub : StubGeneratorBase<IndirectJumpRaxStub>
    {
        /// <inheritdoc />
        public override int Size => 12;

        /// <inheritdoc />
        public override byte[] GetBuffer(nint address)
        {
            using var stub = GetWriter();
            // Opcode for MOV RAX, imm64 is 0x48B8
            stub.Write(OpCodes.MovRax);
            // Write the 64-bit address to jump to
            stub.Write(address);
            // Opcode for JMP RAX is 0xFFE0
            stub.Write(OpCodes.JmpEaxRax);
            // Return the generated stub as a byte array
            return stub.GetBuffer();
        }
    }
}
