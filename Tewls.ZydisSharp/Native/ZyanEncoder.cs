namespace Tewls.ZydisSharp.Native
{
    public enum ZydisOpcodeMap
    {
        ZYDIS_OPCODE_MAP_DEFAULT,
        ZYDIS_OPCODE_MAP_0F,
        ZYDIS_OPCODE_MAP_0F38,
        ZYDIS_OPCODE_MAP_0F3A,
        ZYDIS_OPCODE_MAP_MAP4,
        ZYDIS_OPCODE_MAP_MAP5,
        ZYDIS_OPCODE_MAP_MAP6,
        ZYDIS_OPCODE_MAP_MAP7,
        ZYDIS_OPCODE_MAP_0F0F,
        ZYDIS_OPCODE_MAP_XOP8,
        ZYDIS_OPCODE_MAP_XOP9,
        ZYDIS_OPCODE_MAP_XOPA,
        ZYDIS_OPCODE_MAP_MAX_VALUE,
        ZYDIS_OPCODE_MAP_REQUIRED_BITS
    }

    public enum ZydisOperandAction
    {
        ZYDIS_OPERAND_ACTION_READ, //The operand is read by the instruction.
        ZYDIS_OPERAND_ACTION_WRITE, //The operand is written by the instruction (must write).
        ZYDIS_OPERAND_ACTION_CONDREAD, //The operand is conditionally read by the instruction.
        ZYDIS_OPERAND_ACTION_CONDWRITE, //The operand is conditionally written by the instruction (may write).
        ZYDIS_OPERAND_ACTION_READWRITE, //The operand is read (must read) and written by the instruction (must write).
        ZYDIS_OPERAND_ACTION_CONDREAD_CONDWRITE, //The operand is conditionally read (may read) and conditionally written by the instruction (may write).
        ZYDIS_OPERAND_ACTION_READ_CONDWRITE, //The operand is read (must read) and conditionally written by the instruction (may write).
        ZYDIS_OPERAND_ACTION_CONDREAD_WRITE, //The operand is conditionally read (may read) and written by the instruction (must write).
        ZYDIS_OPERAND_ACTION_WRITE_CONDREAD, //The operand is written (must write) and conditionally read by the instruction (may read).
        ZYDIS_OPERAND_ACTION_MASK_READ, //Mask combining all reading access flags.
        ZYDIS_OPERAND_ACTION_MASK_WRITE, //Mask combining all writing access flags.
        ZYDIS_OPERAND_ACTION_REQUIRED_BITS //The minimum number of bits required to represent all values of this bitset.
    }

    public enum ZydisOperandEncoding
    {
        ZYDIS_OPERAND_ENCODING_NONE,
        ZYDIS_OPERAND_ENCODING_MODRM_REG,
        ZYDIS_OPERAND_ENCODING_MODRM_RM,
        ZYDIS_OPERAND_ENCODING_OPCODE,
        ZYDIS_OPERAND_ENCODING_NDSNDD,
        ZYDIS_OPERAND_ENCODING_IS4,
        ZYDIS_OPERAND_ENCODING_MASK,
        ZYDIS_OPERAND_ENCODING_DISP8,
        ZYDIS_OPERAND_ENCODING_DISP16,
        ZYDIS_OPERAND_ENCODING_DISP32,
        ZYDIS_OPERAND_ENCODING_DISP64,
        ZYDIS_OPERAND_ENCODING_DISP16_32_64,
        ZYDIS_OPERAND_ENCODING_DISP32_32_64,
        ZYDIS_OPERAND_ENCODING_DISP16_32_32,
        ZYDIS_OPERAND_ENCODING_UIMM8,
        ZYDIS_OPERAND_ENCODING_UIMM16,
        ZYDIS_OPERAND_ENCODING_UIMM32,
        ZYDIS_OPERAND_ENCODING_UIMM64,
        ZYDIS_OPERAND_ENCODING_UIMM16_32_64,
        ZYDIS_OPERAND_ENCODING_UIMM32_32_64,
        ZYDIS_OPERAND_ENCODING_UIMM16_32_32,
        ZYDIS_OPERAND_ENCODING_SIMM8,
        ZYDIS_OPERAND_ENCODING_SIMM16,
        ZYDIS_OPERAND_ENCODING_SIMM32,
        ZYDIS_OPERAND_ENCODING_SIMM64,
        ZYDIS_OPERAND_ENCODING_SIMM16_32_64,
        ZYDIS_OPERAND_ENCODING_SIMM32_32_64,
        ZYDIS_OPERAND_ENCODING_SIMM16_32_32,
        ZYDIS_OPERAND_ENCODING_JIMM8,
        ZYDIS_OPERAND_ENCODING_JIMM16,
        ZYDIS_OPERAND_ENCODING_JIMM32,
        ZYDIS_OPERAND_ENCODING_JIMM64,
        ZYDIS_OPERAND_ENCODING_JIMM16_32_64,
        ZYDIS_OPERAND_ENCODING_JIMM32_32_64,
        ZYDIS_OPERAND_ENCODING_JIMM16_32_32,
        ZYDIS_OPERAND_ENCODING_MAX_VALUE,
        ZYDIS_OPERAND_ENCODING_REQUIRED_BITS
    }

    public enum ZydisOperandVisibility
    {
        ZYDIS_OPERAND_VISIBILITY_EXPLICIT,
        ZYDIS_OPERAND_VISIBILITY_IMPLICIT,
        ZYDIS_OPERAND_VISIBILITY_HIDDEN,
        ZYDIS_OPERAND_VISIBILITY_MAX_VALUE,
        ZYDIS_OPERAND_VISIBILITY_REQUIRED_BITS
    }
}

