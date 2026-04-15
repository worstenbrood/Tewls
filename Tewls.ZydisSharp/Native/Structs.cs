using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    using ZydisOperandVisibility = UInt32;
    using ZydisOperandActions = UInt32;
    using ZydisOperandEncoding = UInt32;
    using ZydisElementType = UInt32;
    using ZydisElementSize = UInt32;

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
