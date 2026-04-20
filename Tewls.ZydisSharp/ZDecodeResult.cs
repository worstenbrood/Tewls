using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZDecodeResult(ref ZydisDecodedInstruction instruction, ZydisDecodedOperand[] operands)
    {
        public ZydisDecodedInstruction Instruction = instruction;
        public ZydisDecodedOperand[] Operands = operands ?? [];

        public bool HasRipRelativeMemory()
        {
            int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

            for (int i = 0; i < count; i++)
            {
                var op = Operands[i];

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    (op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP ||
                    op.Value.Mem.Index == ZydisRegister.ZYDIS_REGISTER_EIP ||
                    op.Value.Mem.Index == ZydisRegister.ZYDIS_REGISTER_IP))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasRelativeImmediate()
        {
            int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

            for (int i = 0; i < count; i++)
            {
                var op = Operands[i];

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    (op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP ||
                    op.Value.Mem.Index == ZydisRegister.ZYDIS_REGISTER_EIP ||
                    op.Value.Mem.Index == ZydisRegister.ZYDIS_REGISTER_IP))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NeedsRelocation
        {
            get
            {
                int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

                for (int i = 0; i < count; i++)
                {
                    var op = Operands[i];

                    if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                        (op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP ||
                        op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_EIP ||
                        op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_IP))
                    {
                        return true;
                    }

                    if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_IMMEDIATE &&
                        op.Value.Imm.IsRelative != 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool TryGetAbsoluteTarget(ulong instructionAddress, out ulong target)
        {
            target = 0;
            int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

            for (int i = 0; i < count; i++)
            {
                ref var op = ref Operands[i];

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    (op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP ||
                    op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_EIP ||
                    op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_IP))
                {
                    target = Zydis.CalcAbsoluteAddress(ref Instruction, ref op, instructionAddress);
                    return true;
                }

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_IMMEDIATE &&
                    op.Value.Imm.IsRelative != 0)
                {
                    target = Zydis.CalcAbsoluteAddress(ref Instruction, ref op, instructionAddress);
                    return true;
                }
            }
            return false;
        }

        public byte[] Encode() => ZEncoder.Create(this).Encode();
        public byte[] EncodeAbsolute(ulong address) => ZEncoder.Create(this).EncodeAbsolute(address);
    }
}