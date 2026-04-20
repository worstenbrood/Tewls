using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    using ZydisElementSize = UInt16;
    using ZydisElementType = UInt32;
    using ZydisOperandActions = Byte;
    using ZydisOperandAttributes = Byte;

    public enum ZydisMachineMode : uint
    {
        ZYDIS_MACHINE_MODE_LONG_64 = 0,
        ZYDIS_MACHINE_MODE_LONG_COMPAT_32 = 1,
        ZYDIS_MACHINE_MODE_LONG_COMPAT_16 = 2,
        ZYDIS_MACHINE_MODE_LEGACY_32 = 3,
        ZYDIS_MACHINE_MODE_LEGACY_16 = 4,
        ZYDIS_MACHINE_MODE_REAL_16 = 5,
    }

    public enum ZydisStackWidth : uint
    {
        ZYDIS_STACK_WIDTH_16 = 0,
        ZYDIS_STACK_WIDTH_32 = 1,
        ZYDIS_STACK_WIDTH_64 = 2,
    }

    public enum ZydisDecoderMode : uint
    {
        ZYDIS_DECODER_MODE_MINIMAL = 0,
        ZYDIS_DECODER_MODE_AMD_BRANCHES = 1,
        ZYDIS_DECODER_MODE_KNC = 2,
        ZYDIS_DECODER_MODE_MPX = 3,
        ZYDIS_DECODER_MODE_CET = 4,
        ZYDIS_DECODER_MODE_LZCNT = 5,
        ZYDIS_DECODER_MODE_TZCNT = 6,
        ZYDIS_DECODER_MODE_WBNOINVD = 7,
        ZYDIS_DECODER_MODE_CLDEMOTE = 8,
        ZYDIS_DECODER_MODE_IPREFETCH = 9,
        ZYDIS_DECODER_MODE_UD0_COMPAT = 10,
        ZYDIS_DECODER_MODE_APX = 11,
    }

    public enum ZydisOperandType : uint
    {
        ZYDIS_OPERAND_TYPE_UNUSED = 0,
        ZYDIS_OPERAND_TYPE_REGISTER = 1,
        ZYDIS_OPERAND_TYPE_MEMORY = 2,
        ZYDIS_OPERAND_TYPE_POINTER = 3,
        ZYDIS_OPERAND_TYPE_IMMEDIATE = 4,
    }

    public enum ZydisMemoryOperandType : uint
    {
        ZYDIS_MEMOP_TYPE_INVALID = 0,
        ZYDIS_MEMOP_TYPE_MEM = 1,
        ZYDIS_MEMOP_TYPE_AGEN = 2,
        ZYDIS_MEMOP_TYPE_MIB = 3,
        ZYDIS_MEMOP_TYPE_VSIB = 4,
    }

    [Flags]
    public enum ZydisInstructionAttributes : ulong
    {
        ZYDIS_ATTRIB_HAS_MODRM = 1UL << 0,
        ZYDIS_ATTRIB_HAS_SIB = 1UL << 1,
        ZYDIS_ATTRIB_HAS_REX = 1UL << 2,
        ZYDIS_ATTRIB_HAS_XOP = 1UL << 3,
        ZYDIS_ATTRIB_HAS_VEX = 1UL << 4,
        ZYDIS_ATTRIB_HAS_EVEX = 1UL << 5,
        ZYDIS_ATTRIB_HAS_MVEX = 1UL << 6,
        ZYDIS_ATTRIB_IS_RELATIVE = 1UL << 7,
        ZYDIS_ATTRIB_IS_PRIVILEGED = 1UL << 8,
        ZYDIS_ATTRIB_CPUFLAG_ACCESS = 1UL << 9,
        ZYDIS_ATTRIB_CPU_STATE_CR = 1UL << 10,
        ZYDIS_ATTRIB_CPU_STATE_CW = 1UL << 11,
        ZYDIS_ATTRIB_FPU_STATE_CR = 1UL << 12,
        ZYDIS_ATTRIB_FPU_STATE_CW = 1UL << 13,
        ZYDIS_ATTRIB_XMM_STATE_CR = 1UL << 14,
        ZYDIS_ATTRIB_XMM_STATE_CW = 1UL << 15,
        ZYDIS_ATTRIB_ACCEPTS_LOCK = 1UL << 16,
        ZYDIS_ATTRIB_ACCEPTS_REP = 1UL << 17,
        ZYDIS_ATTRIB_ACCEPTS_REPE = 1UL << 18,
        ZYDIS_ATTRIB_ACCEPTS_REPZ = ZYDIS_ATTRIB_ACCEPTS_REP,
        ZYDIS_ATTRIB_ACCEPTS_REPNE = 1UL << 19,
        ZYDIS_ATTRIB_ACCEPTS_REPNZ = ZYDIS_ATTRIB_ACCEPTS_REPNE,
        ZYDIS_ATTRIB_ACCEPTS_BND = 1UL << 20,
        ZYDIS_ATTRIB_ACCEPTS_XACQUIRE = 1UL << 21,
        ZYDIS_ATTRIB_ACCEPTS_XRELEASE = 1UL << 22,
        ZYDIS_ATTRIB_ACCEPTS_HLE_WITHOUT_LOCK = 1UL << 23,
        ZYDIS_ATTRIB_ACCEPTS_BRANCH_HINTS = 1UL << 24,
        ZYDIS_ATTRIB_ACCEPTS_NOTRACK = 1UL << 25,
        ZYDIS_ATTRIB_ACCEPTS_SEGMENT = 1UL << 26,
        ZYDIS_ATTRIB_HAS_LOCK = 1UL << 27,
        ZYDIS_ATTRIB_HAS_REP = 1UL << 28,
        ZYDIS_ATTRIB_HAS_REPE = 1UL << 29,
        ZYDIS_ATTRIB_HAS_REPZ = ZYDIS_ATTRIB_HAS_REPE,
        ZYDIS_ATTRIB_HAS_REPNE = 1UL << 30,
        ZYDIS_ATTRIB_HAS_REPNZ = ZYDIS_ATTRIB_HAS_REPNE,
        ZYDIS_ATTRIB_HAS_BND = 1UL << 31,
        ZYDIS_ATTRIB_HAS_XACQUIRE = 1UL << 32,
        ZYDIS_ATTRIB_HAS_XRELEASE = 1UL << 33,
        ZYDIS_ATTRIB_HAS_BRANCH_NOT_TAKEN = 1UL << 34,
        ZYDIS_ATTRIB_HAS_BRANCH_TAKEN = 1UL << 35,
        ZYDIS_ATTRIB_HAS_NOTRACK = 1UL << 36,
        ZYDIS_ATTRIB_HAS_SEGMENT_CS = 1UL << 37,
        ZYDIS_ATTRIB_HAS_SEGMENT_SS = 1UL << 38,
        ZYDIS_ATTRIB_HAS_SEGMENT_DS = 1UL << 39,
        ZYDIS_ATTRIB_HAS_SEGMENT_ES = 1UL << 40,
        ZYDIS_ATTRIB_HAS_SEGMENT_FS = 1UL << 41,
        ZYDIS_ATTRIB_HAS_SEGMENT_GS = 1UL << 42,
        ZYDIS_ATTRIB_HAS_OPERANDSIZE = 1UL << 43,
        ZYDIS_ATTRIB_HAS_ADDRESSSIZE = 1UL << 44,
        ZYDIS_ATTRIB_HAS_EVEX_B = 1UL << 45,
        ZYDIS_ATTRIB_HAS_REX2 = 1UL << 46,
        ZYDIS_ATTRIB_HAS_EEVEX = 1UL << 47,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecoder
    {
        public ZydisMachineMode MachineMode;
        public ZydisStackWidth StackWidth;
        public ZydisDecoderMode DecoderMode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandMemDisp
    {
        public long Value;
        public byte Offset;
        public byte Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandMem
    {
        public ZydisMemoryOperandType Type;
        public ZydisRegister Segment;
        public ZydisRegister Base;
        public ZydisRegister Index;
        public byte Scale;

        // padding verwacht voor alignment naar de nested disp struct      
        public ZydisDecodedOperandMemDisp Disp;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct ZydisDecodedOperandImmValue
    {
        [FieldOffset(0)] public long U;
        [FieldOffset(0)] public long S;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandImm
    {
        public byte IsSigned;
        public byte IsAddress;
        public byte IsRelative;

        public ZydisDecodedOperandImmValue Value;

        public ushort Offset;
        public ushort Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandReg
    {
        public ZydisRegister Value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandPtr
    {
        public ushort Segment; // ZyanU16
        public uint Offset;    // ZyanU32
    }

    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct ZydisDecodedOperandValue
    {
        [FieldOffset(0)] public ZydisDecodedOperandReg Reg;
        [FieldOffset(0)] public ZydisDecodedOperandMem Mem;
        [FieldOffset(0)] public ZydisDecodedOperandPtr Ptr;
        [FieldOffset(0)] public ZydisDecodedOperandImm Imm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperand
    {
        public byte Id;
        public ZydisOperandVisibility Visibility;
        public ZydisOperandActions Actions;
        public ZydisOperandEncoding Encoding;
        public ushort Size;
        public ZydisElementType ElementType;
        public ZydisElementSize ElementSize;
        public ushort ElementCount;
        public ZydisOperandAttributes Attributes;
        public ZydisOperandType Type;
        public ZydisDecodedOperandValue Value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 1024)]
    public struct ZydisDecodedInstruction
    {
        public ZydisMachineMode MachineMode;
        public uint Mnemonic;
        public byte Length;
        public uint Encoding;
        public uint OpcodeMap;
        public byte Opcode;
        public byte StackWidth;
        public byte OperandWidth;
        public byte AddressWidth;
        public byte OperandCount;
        public byte OperandCountVisible;
        public ZydisInstructionAttributes Attributes;
        public IntPtr CpuFlags;
        public IntPtr FpuFlags;
    }
}
