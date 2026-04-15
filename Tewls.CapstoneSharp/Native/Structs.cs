using System.Runtime.InteropServices;
using Tewls.CapstoneSharp.Native.Arch;

namespace Tewls.CapstoneSharp.Native
{
    

[StructLayout(LayoutKind.Explicit)]
    public struct cs_detail
    {
        /// <summary>
        /// list of implicit registers read by this insn
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public ushort[] RegsRead;

        /// <summary>
        /// number of implicit registers read by this insn
        /// </summary>
        [FieldOffset(32)]
        public byte RegsReadCount;

        /// <summary>
        /// list of implicit registers modified by this insn
        /// </summary>
        [FieldOffset(33)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 47)]
        public ushort[] RegsWrite;

        /// <summary>
        /// number of implicit registers modified by this insn
        /// </summary>
        [FieldOffset(49)]
        public byte RegsWriteCount;

        /// <summary>
        /// list of group this instruction belong to
        /// </summary>
        [FieldOffset(50)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Groups;

        /// <summary>
        /// number of groups this insn belongs to
        /// </summary>
        [FieldOffset(66)]
        public byte GroupsCount;

        /// <summary>
        /// Instruction has writeback operands.
        /// </summary>
        [FieldOffset(67)]
        public bool WriteBack;

        /// <summary>
        /// X86 architecture, including 16-bit, 32-bit & 64-bit mode
        /// </summary>
        [FieldOffset(68)]
		public cs_x86 x86;
	}

    [StructLayout(LayoutKind.Explicit)]
    public struct cs_insn
    {
        /// <summary>
        /// Instruction ID (basically a numeric ID for the instruction mnemonic)
        /// Find the instruction id in the '[ARCH]_insn' enum in the header file
        /// of corresponding architecture, such as 'arm_insn' in arm.h for ARM,
        /// 'x86_insn' in x86.h for X86, etc...
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// NOTE: in Skipdata mode, "data" instruction has 0 for this id field.
        /// </summary>
        [FieldOffset(0)]
        public uint Id;

        /// <summary>
        /// If this instruction is an alias instruction, this member is set with
        /// the alias ID.
        /// Otherwise to <ARCH>_INS_INVALID.
        /// -- Only supported by auto-sync archs --
        /// </summary>
        [FieldOffset(4)]
        public ulong AliasId;

        /// <summary>
        /// Address (EIP) of this instruction
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// </summary>
        [FieldOffset(12)]
        public ulong Address;

        /// <summary>
        /// Size of this instruction
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// </summary>
        [FieldOffset(20)]
        public ushort Size;

        /// <summary>
        /// Machine bytes of this instruction, with number of bytes indicated by @size above
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// </summary>
        [FieldOffset(22)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] Bytes;

        /// <summary>
        /// Ascii text of instruction mnemonic
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// </summary>
        [FieldOffset(46)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.CS_MNEMONIC_SIZE)]
        public char[] Mnemonic;

        /// <summary>
        /// Ascii text of instruction operands
        /// This information is available even when CS_OPT_DETAIL = CS_OPT_OFF
        /// </summary>
        [FieldOffset(78)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)]
        public char[] OpStr;

        /// <summary>
        /// True: This instruction is an alias.
        /// False: Otherwise.
        /// -- Only supported by auto-sync archs --
        /// </summary>
        [FieldOffset(238)]
        public bool IsAlias;

        /// <summary>
        /// True: The operands are the ones of the alias instructions.
        /// False: The detail operands are from the real instruction.
        /// </summary>
        [FieldOffset(239)]
        public bool UsesAliasDetails;

        /// <summary>
        /// True: The bytes disassemble to a valid instruction, but it is illegal by ISA definitions.
        /// For example the instruction uses a register which is not allowed or it appears in
        /// an invalid context.
        ///
        /// False: The instruction decoded correctly and is valid.
        /// </summary>
        [FieldOffset(240)]
        public bool Illegal;

        /// <summary>
        /// Pointer to cs_detail.
        /// NOTE: detail pointer is only valid when both requirements below are met:
        /// (1) CS_OP_DETAIL = CS_OPT_ON
        /// (2) Engine is not in Skipdata mode (CS_OP_SKIPDATA option set to CS_OPT_ON)
        ///
        /// NOTE 2: when in Skipdata mode, or when detail mode is OFF, even if this pointer
        ///     is not NULL, its content is still irrelevant.
        /// </summary>
        [FieldOffset(225)]
        public nint Detail;
    }
}
