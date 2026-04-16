using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZDecodeResult(ZydisDecodedInstruction instruction, ZydisDecodedOperand[] operands)
    {
        public ZydisDecodedInstruction Instruction = instruction;
        public ZydisDecodedOperand[] Operands = operands ?? [];

        public string FormatInstruction(ZydisFormatterStyle style = ZydisFormatterStyle.ZYDIS_FORMATTER_STYLE_MASM, ulong runtimeAddress = 0)
        {
            using var formatter = new ZFormatter(style);
            return formatter.FormatInstruction(this, runtimeAddress);
        }

        public bool HasRipRelativeMemory(ZydisRegister ripRegister = ZydisRegister.ZYDIS_REGISTER_RIP)
        {
            int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

            for (int i = 0; i < count; i++)
            {
                var op = Operands[i];
                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    op.Value.Mem.Base == ripRegister)
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
                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_IMMEDIATE &&
                    op.Value.Imm.IsRelative != 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ripRegister"></param>
        /// <returns></returns>
        public bool NeedsRelocation(ZydisRegister ripRegister)
        {
            int count = Math.Min((int)Instruction.OperandCount, Operands.Length);

            for (int i = 0; i < count; i++)
            {
                var op = Operands[i];
                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    op.Value.Mem.Base == ripRegister)
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

        public bool TryGetAbsoluteTarget(ulong instructionAddress, out ulong target)
        {
            target = 0;

            for (int i = 0; i < Instruction.OperandCount; i++)
            {
                ref var op = ref Operands[i];

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    op.Value.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP)
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
    }
}