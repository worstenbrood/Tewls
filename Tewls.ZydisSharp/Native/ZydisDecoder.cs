using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    using ZydisElementSize = UInt32;
    using ZydisElementType = UInt32;
    using ZydisOperandActions = UInt32;

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

    public enum ZydisRegister : uint
    {
        ZYDIS_REGISTER_NONE,
        ZYDIS_REGISTER_AL,
        ZYDIS_REGISTER_CL,
        ZYDIS_REGISTER_DL,
        ZYDIS_REGISTER_BL,
        ZYDIS_REGISTER_AH,
        ZYDIS_REGISTER_CH,
        ZYDIS_REGISTER_DH,
        ZYDIS_REGISTER_BH,
        ZYDIS_REGISTER_SPL,
        ZYDIS_REGISTER_BPL,
        ZYDIS_REGISTER_SIL,
        ZYDIS_REGISTER_DIL,
        ZYDIS_REGISTER_R8B,
        ZYDIS_REGISTER_R9B,
        ZYDIS_REGISTER_R10B,
        ZYDIS_REGISTER_R11B,
        ZYDIS_REGISTER_R12B,
        ZYDIS_REGISTER_R13B,
        ZYDIS_REGISTER_R14B,
        ZYDIS_REGISTER_R15B,
        ZYDIS_REGISTER_AX,
        ZYDIS_REGISTER_CX,
        ZYDIS_REGISTER_DX,
        ZYDIS_REGISTER_BX,
        ZYDIS_REGISTER_SP,
        ZYDIS_REGISTER_BP,
        ZYDIS_REGISTER_SI,
        ZYDIS_REGISTER_DI,
        ZYDIS_REGISTER_R8W,
        ZYDIS_REGISTER_R9W,
        ZYDIS_REGISTER_R10W,
        ZYDIS_REGISTER_R11W,
        ZYDIS_REGISTER_R12W,
        ZYDIS_REGISTER_R13W,
        ZYDIS_REGISTER_R14W,
        ZYDIS_REGISTER_R15W,
        ZYDIS_REGISTER_EAX,
        ZYDIS_REGISTER_ECX,
        ZYDIS_REGISTER_EDX,
        ZYDIS_REGISTER_EBX,
        ZYDIS_REGISTER_ESP,
        ZYDIS_REGISTER_EBP,
        ZYDIS_REGISTER_ESI,
        ZYDIS_REGISTER_EDI,
        ZYDIS_REGISTER_R8D,
        ZYDIS_REGISTER_R9D,
        ZYDIS_REGISTER_R10D,
        ZYDIS_REGISTER_R11D,
        ZYDIS_REGISTER_R12D,
        ZYDIS_REGISTER_R13D,
        ZYDIS_REGISTER_R14D,
        ZYDIS_REGISTER_R15D,
        ZYDIS_REGISTER_RAX,
        ZYDIS_REGISTER_RCX,
        ZYDIS_REGISTER_RDX,
        ZYDIS_REGISTER_RBX,
        ZYDIS_REGISTER_RSP,
        ZYDIS_REGISTER_RBP,
        ZYDIS_REGISTER_RSI,
        ZYDIS_REGISTER_RDI,
        ZYDIS_REGISTER_R8,
        ZYDIS_REGISTER_R9,
        ZYDIS_REGISTER_R10,
        ZYDIS_REGISTER_R11,
        ZYDIS_REGISTER_R12,
        ZYDIS_REGISTER_R13,
        ZYDIS_REGISTER_R14,
        ZYDIS_REGISTER_R15,
        ZYDIS_REGISTER_ST0,
        ZYDIS_REGISTER_ST1,
        ZYDIS_REGISTER_ST2,
        ZYDIS_REGISTER_ST3,
        ZYDIS_REGISTER_ST4,
        ZYDIS_REGISTER_ST5,
        ZYDIS_REGISTER_ST6,
        ZYDIS_REGISTER_ST7,
        ZYDIS_REGISTER_X87CONTROL,
        ZYDIS_REGISTER_X87STATUS,
        ZYDIS_REGISTER_X87TAG,
        ZYDIS_REGISTER_MM0,
        ZYDIS_REGISTER_MM1,
        ZYDIS_REGISTER_MM2,
        ZYDIS_REGISTER_MM3,
        ZYDIS_REGISTER_MM4,
        ZYDIS_REGISTER_MM5,
        ZYDIS_REGISTER_MM6,
        ZYDIS_REGISTER_MM7,
        ZYDIS_REGISTER_XMM0,
        ZYDIS_REGISTER_XMM1,
        ZYDIS_REGISTER_XMM2,
        ZYDIS_REGISTER_XMM3,
        ZYDIS_REGISTER_XMM4,
        ZYDIS_REGISTER_XMM5,
        ZYDIS_REGISTER_XMM6,
        ZYDIS_REGISTER_XMM7,
        ZYDIS_REGISTER_XMM8,
        ZYDIS_REGISTER_XMM9,
        ZYDIS_REGISTER_XMM10,
        ZYDIS_REGISTER_XMM11,
        ZYDIS_REGISTER_XMM12,
        ZYDIS_REGISTER_XMM13,
        ZYDIS_REGISTER_XMM14,
        ZYDIS_REGISTER_XMM15,
        ZYDIS_REGISTER_XMM16,
        ZYDIS_REGISTER_XMM17,
        ZYDIS_REGISTER_XMM18,
        ZYDIS_REGISTER_XMM19,
        ZYDIS_REGISTER_XMM20,
        ZYDIS_REGISTER_XMM21,
        ZYDIS_REGISTER_XMM22,
        ZYDIS_REGISTER_XMM23,
        ZYDIS_REGISTER_XMM24,
        ZYDIS_REGISTER_XMM25,
        ZYDIS_REGISTER_XMM26,
        ZYDIS_REGISTER_XMM27,
        ZYDIS_REGISTER_XMM28,
        ZYDIS_REGISTER_XMM29,
        ZYDIS_REGISTER_XMM30,
        ZYDIS_REGISTER_XMM31,
        ZYDIS_REGISTER_YMM0,
        ZYDIS_REGISTER_YMM1,
        ZYDIS_REGISTER_YMM2,
        ZYDIS_REGISTER_YMM3,
        ZYDIS_REGISTER_YMM4,
        ZYDIS_REGISTER_YMM5,
        ZYDIS_REGISTER_YMM6,
        ZYDIS_REGISTER_YMM7,
        ZYDIS_REGISTER_YMM8,
        ZYDIS_REGISTER_YMM9,
        ZYDIS_REGISTER_YMM10,
        ZYDIS_REGISTER_YMM11,
        ZYDIS_REGISTER_YMM12,
        ZYDIS_REGISTER_YMM13,
        ZYDIS_REGISTER_YMM14,
        ZYDIS_REGISTER_YMM15,
        ZYDIS_REGISTER_YMM16,
        ZYDIS_REGISTER_YMM17,
        ZYDIS_REGISTER_YMM18,
        ZYDIS_REGISTER_YMM19,
        ZYDIS_REGISTER_YMM20,
        ZYDIS_REGISTER_YMM21,
        ZYDIS_REGISTER_YMM22,
        ZYDIS_REGISTER_YMM23,
        ZYDIS_REGISTER_YMM24,
        ZYDIS_REGISTER_YMM25,
        ZYDIS_REGISTER_YMM26,
        ZYDIS_REGISTER_YMM27,
        ZYDIS_REGISTER_YMM28,
        ZYDIS_REGISTER_YMM29,
        ZYDIS_REGISTER_YMM30,
        ZYDIS_REGISTER_YMM31,
        ZYDIS_REGISTER_ZMM0,
        ZYDIS_REGISTER_ZMM1,
        ZYDIS_REGISTER_ZMM2,
        ZYDIS_REGISTER_ZMM3,
        ZYDIS_REGISTER_ZMM4,
        ZYDIS_REGISTER_ZMM5,
        ZYDIS_REGISTER_ZMM6,
        ZYDIS_REGISTER_ZMM7,
        ZYDIS_REGISTER_ZMM8,
        ZYDIS_REGISTER_ZMM9,
        ZYDIS_REGISTER_ZMM10,
        ZYDIS_REGISTER_ZMM11,
        ZYDIS_REGISTER_ZMM12,
        ZYDIS_REGISTER_ZMM13,
        ZYDIS_REGISTER_ZMM14,
        ZYDIS_REGISTER_ZMM15,
        ZYDIS_REGISTER_ZMM16,
        ZYDIS_REGISTER_ZMM17,
        ZYDIS_REGISTER_ZMM18,
        ZYDIS_REGISTER_ZMM19,
        ZYDIS_REGISTER_ZMM20,
        ZYDIS_REGISTER_ZMM21,
        ZYDIS_REGISTER_ZMM22,
        ZYDIS_REGISTER_ZMM23,
        ZYDIS_REGISTER_ZMM24,
        ZYDIS_REGISTER_ZMM25,
        ZYDIS_REGISTER_ZMM26,
        ZYDIS_REGISTER_ZMM27,
        ZYDIS_REGISTER_ZMM28,
        ZYDIS_REGISTER_ZMM29,
        ZYDIS_REGISTER_ZMM30,
        ZYDIS_REGISTER_ZMM31,
        ZYDIS_REGISTER_TMM0,
        ZYDIS_REGISTER_TMM1,
        ZYDIS_REGISTER_TMM2,
        ZYDIS_REGISTER_TMM3,
        ZYDIS_REGISTER_TMM4,
        ZYDIS_REGISTER_TMM5,
        ZYDIS_REGISTER_TMM6,
        ZYDIS_REGISTER_TMM7,
        ZYDIS_REGISTER_FLAGS,
        ZYDIS_REGISTER_EFLAGS,
        ZYDIS_REGISTER_RFLAGS,
        ZYDIS_REGISTER_IP,
        ZYDIS_REGISTER_EIP,
        ZYDIS_REGISTER_RIP,
        ZYDIS_REGISTER_ES,
        ZYDIS_REGISTER_CS,
        ZYDIS_REGISTER_SS,
        ZYDIS_REGISTER_DS,
        ZYDIS_REGISTER_FS,
        ZYDIS_REGISTER_GS,
        ZYDIS_REGISTER_GDTR,
        ZYDIS_REGISTER_LDTR,
        ZYDIS_REGISTER_IDTR,
        ZYDIS_REGISTER_TR,
        ZYDIS_REGISTER_TR0,
        ZYDIS_REGISTER_TR1,
        ZYDIS_REGISTER_TR2,
        ZYDIS_REGISTER_TR3,
        ZYDIS_REGISTER_TR4,
        ZYDIS_REGISTER_TR5,
        ZYDIS_REGISTER_TR6,
        ZYDIS_REGISTER_TR7,
        ZYDIS_REGISTER_CR0,
        ZYDIS_REGISTER_CR1,
        ZYDIS_REGISTER_CR2,
        ZYDIS_REGISTER_CR3,
        ZYDIS_REGISTER_CR4,
        ZYDIS_REGISTER_CR5,
        ZYDIS_REGISTER_CR6,
        ZYDIS_REGISTER_CR7,
        ZYDIS_REGISTER_CR8,
        ZYDIS_REGISTER_CR9,
        ZYDIS_REGISTER_CR10,
        ZYDIS_REGISTER_CR11,
        ZYDIS_REGISTER_CR12,
        ZYDIS_REGISTER_CR13,
        ZYDIS_REGISTER_CR14,
        ZYDIS_REGISTER_CR15,
        ZYDIS_REGISTER_DR0,
        ZYDIS_REGISTER_DR1,
        ZYDIS_REGISTER_DR2,
        ZYDIS_REGISTER_DR3,
        ZYDIS_REGISTER_DR4,
        ZYDIS_REGISTER_DR5,
        ZYDIS_REGISTER_DR6,
        ZYDIS_REGISTER_DR7,
        ZYDIS_REGISTER_DR8,
        ZYDIS_REGISTER_DR9,
        ZYDIS_REGISTER_DR10,
        ZYDIS_REGISTER_DR11,
        ZYDIS_REGISTER_DR12,
        ZYDIS_REGISTER_DR13,
        ZYDIS_REGISTER_DR14,
        ZYDIS_REGISTER_DR15,
        ZYDIS_REGISTER_K0,
        ZYDIS_REGISTER_K1,
        ZYDIS_REGISTER_K2,
        ZYDIS_REGISTER_K3,
        ZYDIS_REGISTER_K4,
        ZYDIS_REGISTER_K5,
        ZYDIS_REGISTER_K6,
        ZYDIS_REGISTER_K7,
        ZYDIS_REGISTER_BND0,
        ZYDIS_REGISTER_BND1,
        ZYDIS_REGISTER_BND2,
        ZYDIS_REGISTER_BND3,
        ZYDIS_REGISTER_BNDCFG,
        ZYDIS_REGISTER_BNDSTATUS,
        ZYDIS_REGISTER_MXCSR,
        ZYDIS_REGISTER_PKRU,
        ZYDIS_REGISTER_XCR0,
        ZYDIS_REGISTER_UIF,
        ZYDIS_REGISTER_MAX_VALUE
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

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandImmValue
    {
        public ulong U;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisDecodedOperandImm
    {
        public byte IsSigned;
        public byte IsAddress;
        public byte IsRelative;

        private byte _pad0;
        private byte _pad1;
        private byte _pad2;
        private byte _pad3;
        private byte _pad4;

        public ZydisDecodedOperandImmValue Value;
        public byte Offset;
        public byte Size;

        private byte _pad5;
        private byte _pad6;
        private byte _pad7;
        private byte _pad8;
        private byte _pad9;
        private byte _pad10;
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

    [StructLayout(LayoutKind.Explicit)]
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
        public byte Attributes;
        public ZydisOperandType Type;
        public ZydisDecodedOperandValue Value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 512)]
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
