namespace Tewls.CapstoneSharp
{
    public enum cs_err
    {
        CS_ERR_OK = 0, ///< No error: everything was fine
        CS_ERR_MEM, ///< Out-Of-Memory error: cs_open(), cs_disasm(), cs_disasm_iter()
        CS_ERR_ARCH, ///< Unsupported architecture: cs_open()
        CS_ERR_HANDLE, ///< Invalid handle: cs_op_count(), cs_op_index()
        CS_ERR_CSH, ///< Invalid csh argument: cs_close(), cs_errno(), cs_option()
        CS_ERR_MODE, ///< Invalid/unsupported mode: cs_open()
        CS_ERR_OPTION, ///< Invalid/unsupported option: cs_option()
        CS_ERR_DETAIL, ///< Information is unavailable because detail option is OFF
        CS_ERR_MEMSETUP, ///< Dynamic memory management uninitialized (see CS_OPT_MEM)
        CS_ERR_VERSION, ///< Unsupported version (bindings)
        CS_ERR_DIET, ///< Access irrelevant data in "diet" engine
        CS_ERR_SKIPDATA, ///< Access irrelevant data for "data" instruction in SKIPDATA mode
        CS_ERR_X86_ATT, ///< X86 AT&T syntax is unsupported (opt-out at compile time)
        CS_ERR_X86_INTEL, ///< X86 Intel syntax is unsupported (opt-out at compile time)
        CS_ERR_X86_MASM, ///< X86 Masm syntax is unsupported (opt-out at compile time)
    }

    public enum cs_arch : ushort
    {
        CS_ARCH_ARM = 0, ///< ARM architecture (including Thumb, Thumb-2)
    	CS_ARCH_ARM64 = 1, ///< ARM64
    	CS_ARCH_AARCH64 = 1, ///< AArch64
    	CS_ARCH_SYSZ = 2, ///< SystemZ architecture
    	CS_ARCH_SYSTEMZ = 2, ///< SystemZ architecture
    	CS_ARCH_MIPS, ///< Mips architecture
        CS_ARCH_X86, ///< X86 architecture (including x86 & x86-64)
        CS_ARCH_PPC, ///< PowerPC architecture
        CS_ARCH_SPARC, ///< Sparc architecture
        CS_ARCH_XCORE, ///< XCore architecture
        CS_ARCH_M68K, ///< 68K architecture
        CS_ARCH_TMS320C64X, ///< TMS320C64x architecture
        CS_ARCH_M680X, ///< 680X architecture
        CS_ARCH_EVM, ///< Ethereum architecture
        CS_ARCH_MOS65XX, ///< MOS65XX architecture (including MOS6502)
        CS_ARCH_WASM, ///< WebAssembly architecture
        CS_ARCH_BPF, ///< Berkeley Packet Filter architecture (including eBPF)
        CS_ARCH_RISCV, ///< RISCV architecture
        CS_ARCH_SH, ///< SH architecture
        CS_ARCH_TRICORE, ///< TriCore architecture
        CS_ARCH_ALPHA, ///< Alpha architecture
        CS_ARCH_HPPA, ///< HPPA architecture
        CS_ARCH_LOONGARCH, ///< LoongArch architecture
        CS_ARCH_XTENSA, ///< Xtensa architecture
        CS_ARCH_ARC, ///< ARC architecture
        CS_ARCH_MAX,
        CS_ARCH_ALL = 0xFFFF, // All architectures - for cs_support()
    }

    public enum cs_mode : uint
    {
        CS_MODE_LITTLE_ENDIAN = 0, ///< little-endian mode (default mode)
        CS_MODE_ARM = 0, ///< 32-bit ARM
        CS_MODE_16 = 1 << 1, ///< 16-bit mode (X86)
        CS_MODE_32 = 1 << 2, ///< 32-bit mode (X86)
        CS_MODE_64 = 1 << 3, ///< 64-bit mode (X86, PPC)
        // ARM
        CS_MODE_THUMB = 1 << 4, ///< ARM's Thumb mode, including Thumb-2
        CS_MODE_MCLASS = 1 << 5, ///< ARM's Cortex-M series
        CS_MODE_V8 = 1 << 6, ///< ARMv8 A32 encodings for ARM
        // AArch64
        CS_MODE_APPLE_PROPRIETARY =
            1 << Constants.CS_MODE_VENDOR_AARCH64_BIT0, ///< Enable Apple proprietary AArch64 instructions like AMX, MUL53, and others.
        // SPARC
        CS_MODE_V9 = 1 << 4, ///< SparcV9 mode (Sparc)
        // PPC
        CS_MODE_QPX = 1 << 4, ///< Quad Processing eXtensions mode (PPC)
        CS_MODE_SPE = 1 << 5, ///< Signal Processing Engine mode (PPC)
        CS_MODE_BOOKE = 1 << 6, ///< Book-E mode (PPC)
        CS_MODE_PS = 1 << 7, ///< Paired-singles mode (PPC)
        CS_MODE_AIX_OS = 1 << 8, ///< PowerPC AIX-OS
        CS_MODE_PWR7 = 1 << 9, ///< Power 7
        CS_MODE_PWR8 = 1 << 10, ///< Power 8
        CS_MODE_PWR9 = 1 << 11, ///< Power 9
        CS_MODE_PWR10 = 1 << 12, ///< Power 10
        CS_MODE_PPC_ISA_FUTURE = 1 << 13, ///< Power ISA Future
        CS_MODE_MODERN_AIX_AS = 1
                    << 14, ///< PowerPC AIX-OS with modern assembly
        CS_MODE_MSYNC =
            1
            << 15, ///< PowerPC Has only the msync instruction instead of sync. Implies BOOKE
        CS_MODE_M68K_000 = 1 << 1, ///< M68K 68000 mode
        CS_MODE_M68K_010 = 1 << 2, ///< M68K 68010 mode
        CS_MODE_M68K_020 = 1 << 3, ///< M68K 68020 mode
        CS_MODE_M68K_030 = 1 << 4, ///< M68K 68030 mode
        CS_MODE_M68K_040 = 1 << 5, ///< M68K 68040 mode
        CS_MODE_M68K_060 = 1 << 6, ///< M68K 68060 mode
        CS_MODE_BIG_ENDIAN = 1U << 31, ///< big-endian mode
        CS_MODE_MIPS16 = CS_MODE_16, ///< Generic mips16
        CS_MODE_MIPS32 = CS_MODE_32, ///< Generic mips32
        CS_MODE_MIPS64 = CS_MODE_64, ///< Generic mips64
        CS_MODE_MICRO = 1 << 4, ///< microMips
        CS_MODE_MIPS1 = 1 << 5, ///< Mips I ISA Support
        CS_MODE_MIPS2 = 1 << 6, ///< Mips II ISA Support
        CS_MODE_MIPS32R2 = 1 << 7, ///< Mips32r2 ISA Support
        CS_MODE_MIPS32R3 = 1 << 8, ///< Mips32r3 ISA Support
        CS_MODE_MIPS32R5 = 1 << 9, ///< Mips32r5 ISA Support
        CS_MODE_MIPS32R6 = 1 << 10, ///< Mips32r6 ISA Support
        CS_MODE_MIPS3 = 1 << 11, ///< MIPS III ISA Support
        CS_MODE_MIPS4 = 1 << 12, ///< MIPS IV ISA Support
        CS_MODE_MIPS5 = 1 << 13, ///< MIPS V ISA Support
        CS_MODE_MIPS64R2 = 1 << 14, ///< Mips64r2 ISA Support
        CS_MODE_MIPS64R3 = 1 << 15, ///< Mips64r3 ISA Support
        CS_MODE_MIPS64R5 = 1 << 16, ///< Mips64r5 ISA Support
        CS_MODE_MIPS64R6 = 1 << 17, ///< Mips64r6 ISA Support
        CS_MODE_OCTEON = 1 << 18, ///< Octeon cnMIPS Support
        CS_MODE_OCTEONP = 1 << 19, ///< Octeon+ cnMIPS Support
        CS_MODE_NANOMIPS = 1 << 20, ///< Generic nanomips
        CS_MODE_NMS1 = ((1 << 21) | CS_MODE_NANOMIPS), ///< nanoMips NMS1
        CS_MODE_I7200 = ((1 << 22) | CS_MODE_NANOMIPS), ///< nanoMips I7200
        CS_MODE_MIPS_NOFLOAT = 1 << 23, ///< Disable floating points ops
        CS_MODE_MIPS_PTR64 = 1 << 24, ///< Mips pointers are 64-bit
        CS_MODE_MICRO32R3 =
            (CS_MODE_MICRO | CS_MODE_MIPS32R3), ///< microMips32r3
        CS_MODE_MICRO32R6 =
            (CS_MODE_MICRO | CS_MODE_MIPS32R6), ///< microMips32r6
        CS_MODE_M680X_6301 = 1 << 1, ///< M680X Hitachi 6301,6303 mode
        CS_MODE_M680X_6309 = 1 << 2, ///< M680X Hitachi 6309 mode
        CS_MODE_M680X_6800 = 1 << 3, ///< M680X Motorola 6800,6802 mode
        CS_MODE_M680X_6801 = 1 << 4, ///< M680X Motorola 6801,6803 mode
        CS_MODE_M680X_6805 = 1 << 5, ///< M680X Motorola/Freescale 6805 mode
        CS_MODE_M680X_6808 = 1
                     << 6, ///< M680X Motorola/Freescale/NXP 68HC08 mode
        CS_MODE_M680X_6809 = 1 << 7, ///< M680X Motorola 6809 mode
        CS_MODE_M680X_6811 = 1
                     << 8, ///< M680X Motorola/Freescale/NXP 68HC11 mode
        CS_MODE_M680X_CPU12 = 1 << 9, ///< M680X Motorola/Freescale/NXP CPU12
                                      ///< used on M68HC12/HCS12
        CS_MODE_M680X_HCS08 = 1 << 10, ///< M680X Freescale/NXP HCS08 mode
        CS_MODE_M680X_RS08 = 1 << 11, ///< M680X Freescale/NXP RS08 mode
        CS_MODE_BPF_CLASSIC = 0, ///< Classic BPF mode (default)
        CS_MODE_BPF_EXTENDED = 1 << 0, ///< Extended BPF mode
        CS_MODE_RISCV32 = 1 << 0, ///< RISCV RV32G
        CS_MODE_RISCV64 = 1 << 1, ///< RISCV RV64G
        CS_MODE_RISCV_C = 1 << 2, ///< RISCV compressed instructure mode
        CS_MODE_RISCV_FD = 1 << 3,
        CS_MODE_RISCV_V = 1 << 4,
        CS_MODE_RISCV_ZFINX = 1 << 5,
        CS_MODE_RISCV_ZCMP_ZCMT_ZCE = 1 << 6,
        CS_MODE_RISCV_ZICFISS = 1 << 7,
        CS_MODE_RISCV_E = 1 << 8,
        CS_MODE_RISCV_A = 1 << 9,
        CS_MODE_RISCV_COREV = 1 << 10,
        CS_MODE_RISCV_THEAD = 1 << 11,
        CS_MODE_RISCV_SIFIVE = 1 << 12,
        CS_MODE_RISCV_BITMANIP = 1 << 13,
        CS_MODE_RISCV_ZBA = 1 << 14,
        CS_MODE_RISCV_ZBB = 1 << 15,
        CS_MODE_RISCV_ZBC = 1 << 16,
        CS_MODE_RISCV_ZBKB = 1 << 17,
        CS_MODE_RISCV_ZBKC = 1 << 18,
        CS_MODE_RISCV_ZBKX = 1 << 19,
        CS_MODE_RISCV_ZBS = 1 << 20,
        CS_MODE_MOS65XX_6502 = 1 << 1, ///< MOS65XXX MOS 6502
        CS_MODE_MOS65XX_65C02 = 1 << 2, ///< MOS65XXX WDC 65c02
        CS_MODE_MOS65XX_W65C02 = 1 << 3, ///< MOS65XXX WDC W65c02
        CS_MODE_MOS65XX_65816 = 1 << 4, ///< MOS65XXX WDC 65816, 8-bit m/x
        CS_MODE_MOS65XX_65816_LONG_M =
            (1 << 5), ///< MOS65XXX WDC 65816, 16-bit m, 8-bit x
        CS_MODE_MOS65XX_65816_LONG_X =
            (1 << 6), ///< MOS65XXX WDC 65816, 8-bit m, 16-bit x
        CS_MODE_MOS65XX_65816_LONG_MX = CS_MODE_MOS65XX_65816_LONG_M |
                        CS_MODE_MOS65XX_65816_LONG_X,
        CS_MODE_SH2 = 1 << 1, ///< SH2
        CS_MODE_SH2A = 1 << 2, ///< SH2A
        CS_MODE_SH3 = 1 << 3, ///< SH3
        CS_MODE_SH4 = 1 << 4, ///< SH4
        CS_MODE_SH4A = 1 << 5, ///< SH4A
        CS_MODE_SHFPU = 1 << 6, ///< w/ FPU
        CS_MODE_SHDSP = 1 << 7, ///< w/ DSP
        CS_MODE_TRICORE_110 = 1 << 1, ///< Tricore 1.1
        CS_MODE_TRICORE_120 = 1 << 2, ///< Tricore 1.2
        CS_MODE_TRICORE_130 = 1 << 3, ///< Tricore 1.3
        CS_MODE_TRICORE_131 = 1 << 4, ///< Tricore 1.3.1
        CS_MODE_TRICORE_160 = 1 << 5, ///< Tricore 1.6
        CS_MODE_TRICORE_161 = 1 << 6, ///< Tricore 1.6.1
        CS_MODE_TRICORE_162 = 1 << 7, ///< Tricore 1.6.2
        CS_MODE_TRICORE_180 = 1 << 8, ///< Tricore 1.8.0
        CS_MODE_HPPA_11 = 1 << 1, ///< HPPA 1.1
        CS_MODE_HPPA_20 = 1 << 2, ///< HPPA 2.0
        CS_MODE_HPPA_20W = CS_MODE_HPPA_20 | (1 << 3), ///< HPPA 2.0 wide
        CS_MODE_LOONGARCH32 = 1 << 0, ///< LoongArch32
        CS_MODE_LOONGARCH64 = 1 << 1, ///< LoongArch64
        CS_MODE_SYSTEMZ_ARCH8 =
            1 << 1, ///< Enables features of the ARCH8 processor
        CS_MODE_SYSTEMZ_ARCH9 =
            1 << 2, ///< Enables features of the ARCH9 processor
        CS_MODE_SYSTEMZ_ARCH10 =
            1 << 3, ///< Enables features of the ARCH10 processor
        CS_MODE_SYSTEMZ_ARCH11 =
            1 << 4, ///< Enables features of the ARCH11 processor
        CS_MODE_SYSTEMZ_ARCH12 =
            1 << 5, ///< Enables features of the ARCH12 processor
        CS_MODE_SYSTEMZ_ARCH13 =
            1 << 6, ///< Enables features of the ARCH13 processor
        CS_MODE_SYSTEMZ_ARCH14 =
            1 << 7, ///< Enables features of the ARCH14 processor
        CS_MODE_SYSTEMZ_Z10 = 1 << 8, ///< Enables features of the Z10 processor
        CS_MODE_SYSTEMZ_Z196 = 1
                       << 9, ///< Enables features of the Z196 processor
        CS_MODE_SYSTEMZ_ZEC12 =
            1 << 10, ///< Enables features of the ZEC12 processor
        CS_MODE_SYSTEMZ_Z13 = 1
                      << 11, ///< Enables features of the Z13 processor
        CS_MODE_SYSTEMZ_Z14 = 1
                      << 12, ///< Enables features of the Z14 processor
        CS_MODE_SYSTEMZ_Z15 = 1
                      << 13, ///< Enables features of the Z15 processor
        CS_MODE_SYSTEMZ_Z16 = 1
                      << 14, ///< Enables features of the Z16 processor
        CS_MODE_SYSTEMZ_GENERIC =
            1 << 15, ///< Enables features of the generic processor
        CS_MODE_XTENSA_ESP32 = 1 << 1, ///< Xtensa ESP32
        CS_MODE_XTENSA_ESP32S2 = 1 << 2, ///< Xtensa ESP32S2
        CS_MODE_XTENSA_ESP8266 = 1 << 3, ///< Xtensa ESP328266
    }

    public enum cs_opt_type
    {
        CS_OPT_INVALID = 0, ///< No option specified
        CS_OPT_SYNTAX, ///< Assembly output syntax
        CS_OPT_DETAIL, ///< Break down instruction structure into details
        CS_OPT_MODE, ///< Change engine's mode at run-time
        CS_OPT_MEM, ///< User-defined dynamic memory related functions
        CS_OPT_SKIPDATA, ///< Skip data when disassembling. Then engine is in SKIPDATA mode.
        CS_OPT_SKIPDATA_SETUP, ///< Setup user-defined function for SKIPDATA option
        CS_OPT_MNEMONIC, ///< Customize instruction mnemonic
        CS_OPT_UNSIGNED, ///< print immediate operands in unsigned form
        CS_OPT_ONLY_OFFSET_BRANCH, ///< ARM, PPC, AArch64: Don't add the branch immediate value to the PC.
        CS_OPT_LITBASE, ///< Xtensa, set the LITBASE value. LITBASE is set to 0 by default.
    }

    public enum cs_opt_value
    {
        CS_OPT_OFF =
            0, ///< Turn OFF an option - default for CS_OPT_DETAIL, CS_OPT_SKIPDATA, CS_OPT_UNSIGNED.
        CS_OPT_ON =
            1 << 0, ///< Turn ON an option (CS_OPT_DETAIL, CS_OPT_SKIPDATA).
        CS_OPT_SYNTAX_DEFAULT = 1 << 1, ///< Default asm syntax (CS_OPT_SYNTAX).
        CS_OPT_SYNTAX_INTEL =
            1
            << 2, ///< X86 Intel asm syntax - default on X86 (CS_OPT_SYNTAX).
        CS_OPT_SYNTAX_ATT = 1 << 3, ///< X86 ATT asm syntax (CS_OPT_SYNTAX).
        CS_OPT_SYNTAX_NOREGNAME =
            1
            << 4, ///< Prints register name with only number (CS_OPT_SYNTAX)
        CS_OPT_SYNTAX_MASM = 1 << 5, ///< X86 Intel Masm syntax (CS_OPT_SYNTAX).
        CS_OPT_SYNTAX_MOTOROLA = 1 << 6, ///< MOS65XX use $ as hex prefix
        CS_OPT_SYNTAX_CS_REG_ALIAS =
            1
            << 7, ///< Prints common register alias which are not defined in LLVM (ARM: r9 = sb etc.)
        CS_OPT_SYNTAX_PERCENT =
            1 << 8, ///< Prints the % in front of PPC registers.
        CS_OPT_SYNTAX_NO_DOLLAR =
            1
            << 9, ///< Does not print the $ in front of Mips, LoongArch registers.
        CS_OPT_SYNTAX_NO_ALIAS_TEXT =
            1
            << 10, ///< Does not print an instruction's alias test if the instruction is an alias
        CS_OPT_SYNTAX_NO_ALIAS_TEXT_COMPRESSED =
            1
            << 11, ///< Does not print an instruction's alias test if the instruction is an alias
        CS_OPT_DETAIL_REAL =
            1
            << 1, ///< If enabled, always sets the real instruction detail. Even if the instruction is an alias.
    }
}
