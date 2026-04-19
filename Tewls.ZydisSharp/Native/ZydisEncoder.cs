using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    using ZydisDefaultFlagsValue = Byte;
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
    public enum ZydisEncodableEncoding : uint
    {
        ZYDIS_ENCODABLE_ENCODING_DEFAULT = 0x00000000,
        ZYDIS_ENCODABLE_ENCODING_LEGACY = 0x00000001,
        ZYDIS_ENCODABLE_ENCODING_3DNOW = 0x00000002,
        ZYDIS_ENCODABLE_ENCODING_XOP = 0x00000004,
        ZYDIS_ENCODABLE_ENCODING_VEX = 0x00000008,
        ZYDIS_ENCODABLE_ENCODING_EVEX = 0x00000010,
        ZYDIS_ENCODABLE_ENCODING_MVEX = 0x00000020,
        ZYDIS_ENCODABLE_ENCODING_MAX_VALUE = (ZYDIS_ENCODABLE_ENCODING_MVEX | (ZYDIS_ENCODABLE_ENCODING_MVEX - 1)),
    }

    public enum ZydisBranchType
    {
        ZYDIS_BRANCH_TYPE_NONE,
        ZYDIS_BRANCH_TYPE_SHORT,
        ZYDIS_BRANCH_TYPE_NEAR,
        ZYDIS_BRANCH_TYPE_FAR,
        ZYDIS_BRANCH_TYPE_ABSOLUTE,
        ZYDIS_BRANCH_TYPE_MAX_VALUE = ZYDIS_BRANCH_TYPE_ABSOLUTE,
    }

    public enum ZydisBranchWidth
    {
        ZYDIS_BRANCH_WIDTH_NONE,
        ZYDIS_BRANCH_WIDTH_8,
        ZYDIS_BRANCH_WIDTH_16,
        ZYDIS_BRANCH_WIDTH_32,
        ZYDIS_BRANCH_WIDTH_64,
        ZYDIS_BRANCH_WIDTH_MAX_VALUE = ZYDIS_BRANCH_WIDTH_64,
    }

    public enum ZydisAddressSizeHint
    {
        ZYDIS_ADDRESS_SIZE_HINT_NONE,
        ZYDIS_ADDRESS_SIZE_HINT_16,
        ZYDIS_ADDRESS_SIZE_HINT_32,
        ZYDIS_ADDRESS_SIZE_HINT_64,
        ZYDIS_ADDRESS_SIZE_HINT_MAX_VALUE = ZYDIS_ADDRESS_SIZE_HINT_64,
    }
    public enum ZydisOperandSizeHint
    {
        ZYDIS_OPERAND_SIZE_HINT_NONE,
        ZYDIS_OPERAND_SIZE_HINT_8,
        ZYDIS_OPERAND_SIZE_HINT_16,
        ZYDIS_OPERAND_SIZE_HINT_32,
        ZYDIS_OPERAND_SIZE_HINT_64,
        ZYDIS_OPERAND_SIZE_HINT_MAX_VALUE = ZYDIS_OPERAND_SIZE_HINT_64,
    }

    public enum ZydisBroadcastMode
    {
        ZYDIS_BROADCAST_MODE_NONE,
        ZYDIS_BROADCAST_MODE_1_TO_2,
        ZYDIS_BROADCAST_MODE_1_TO_4,
        ZYDIS_BROADCAST_MODE_1_TO_8,
        ZYDIS_BROADCAST_MODE_1_TO_16,
        ZYDIS_BROADCAST_MODE_1_TO_32,
        ZYDIS_BROADCAST_MODE_1_TO_64,
        ZYDIS_BROADCAST_MODE_2_TO_4,
        ZYDIS_BROADCAST_MODE_2_TO_8,
        ZYDIS_BROADCAST_MODE_2_TO_16,
        ZYDIS_BROADCAST_MODE_4_TO_8,
        ZYDIS_BROADCAST_MODE_4_TO_16,
        ZYDIS_BROADCAST_MODE_8_TO_16,
        ZYDIS_BROADCAST_MODE_MAX_VALUE = ZYDIS_BROADCAST_MODE_8_TO_16,
    }

    public enum ZydisRoundingMode
    {
        ZYDIS_ROUNDING_MODE_NONE,
        ZYDIS_ROUNDING_MODE_RN,
        ZYDIS_ROUNDING_MODE_RD,
        ZYDIS_ROUNDING_MODE_RU,
        ZYDIS_ROUNDING_MODE_RZ,

        ZYDIS_ROUNDING_MODE_MAX_VALUE = ZYDIS_ROUNDING_MODE_RZ,
    }

    public enum ZydisConversionMode
    {
        ZYDIS_CONVERSION_MODE_NONE,
        ZYDIS_CONVERSION_MODE_FLOAT16,
        ZYDIS_CONVERSION_MODE_SINT8,
        ZYDIS_CONVERSION_MODE_UINT8,
        ZYDIS_CONVERSION_MODE_SINT16,
        ZYDIS_CONVERSION_MODE_UINT16,
        ZYDIS_CONVERSION_MODE_MAX_VALUE = ZYDIS_CONVERSION_MODE_UINT16,
    }

    public enum ZydisSwizzleMode
    {
        ZYDIS_SWIZZLE_MODE_NONE,
        ZYDIS_SWIZZLE_MODE_DCBA,
        ZYDIS_SWIZZLE_MODE_CDAB,
        ZYDIS_SWIZZLE_MODE_BADC,
        ZYDIS_SWIZZLE_MODE_DACB,
        ZYDIS_SWIZZLE_MODE_AAAA,
        ZYDIS_SWIZZLE_MODE_BBBB,
        ZYDIS_SWIZZLE_MODE_CCCC,
        ZYDIS_SWIZZLE_MODE_DDDD,
        ZYDIS_SWIZZLE_MODE_MAX_VALUE = ZYDIS_SWIZZLE_MODE_DDDD,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderOperandReg
    {
        public ZydisRegister Value;
        public byte Is4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderOperandMem
    {
        public ZydisRegister Base;
        public ZydisRegister Index;
        public byte Scale;
        public long Displacement;
        public ushort Size;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct ZydisEncoderOperandImm
    {
        [FieldOffset(0)]
        public ulong U;
        [FieldOffset(0)]
        public long S;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderOperandPtr
    {
        public ushort Segment;
        public uint Offset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderOperand
    {
        public ZydisOperandType Type;
        public ZydisEncoderOperandReg Reg;
        public ZydisEncoderOperandMem Mem;
        public ZydisEncoderOperandPtr Ptr;
        public ZydisEncoderOperandImm Imm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderRequestEvexFeatures
    {
        public ZydisBroadcastMode Broadcast;
        public ZydisRoundingMode Rounding;
        public byte Sae;
        public byte ZeroingMask;
        public byte NoFlags;
        public ZydisDefaultFlagsValue DefaultFlags;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderRequestMvexFeatures
    {
        public ZydisBroadcastMode Broadcast;
        public ZydisConversionMode Conversion;
        public ZydisRoundingMode Rounding;
        public ZydisSwizzleMode Swizzle;
        public byte Sae;
        public byte EvictionHint;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisEncoderRequest
    {
        public ZydisMachineMode MachineMode;
        public ZydisEncodableEncoding AllowedEncodings;
        public ZydisMnemonic Mnemonic;
        public ZydisInstructionAttributes Prefixes;
        public ZydisBranchType BranchType;
        public ZydisBranchWidth BranchWidth;
        public ZydisAddressSizeHint AddressSizeHint;
        public ZydisOperandSizeHint OperandSizeHint;
        public byte OperandCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Zydis.ZYDIS_ENCODER_MAX_OPERANDS)]
        public ZydisEncoderOperand[] Operands;
        public ZydisEncoderRequestEvexFeatures Evex;
        public ZydisEncoderRequestMvexFeatures Mvex;
    }
}

