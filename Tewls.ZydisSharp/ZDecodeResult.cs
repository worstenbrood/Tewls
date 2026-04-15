using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZDecodeResult(ZydisDecodedInstruction instruction, ZydisDecodedOperand[] operands)
    {
        public ZydisDecodedInstruction Instruction { get; } = instruction;
        public ZydisDecodedOperand[] Operands { get; } = operands ?? [];

        public bool HasRipRelativeMemory(ZydisRegister ripRegister)
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
    }
}