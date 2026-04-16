namespace Tewls.ZydisSharp.Native
{
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
        ZYDIS_REGISTER_NONE = 0,

        // Alleen toevoegen als ge ze effectief nodig hebt in managed code:
        ZYDIS_REGISTER_IP = 297, // voorbeeld: niet hardcoden zonder uw eigen build te checken
        ZYDIS_REGISTER_EIP = 298,
        ZYDIS_REGISTER_RIP = 299,
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

    public enum ZydisFormatterStyle : uint
    {
        ZYDIS_FORMATTER_STYLE_INTEL = 0,
        ZYDIS_FORMATTER_STYLE_ATT = 1,
        ZYDIS_FORMATTER_STYLE_MASM = 2,
        ZYDIS_FORMATTER_STYLE_GAS = 3,
        ZYDIS_FORMATTER_STYLE_NASM = 4,
    }

    public enum ZydisFormatterProperty
    {
        /* ---------------------------------------------------------------------------------------- */
        /* General                                                                                  */
        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the printing of effective operand-size suffixes (`AT&T`) or operand-sizes
         * of memory operands (`INTEL`).
         *
         * Pass `ZYAN_TRUE` as value to force the formatter to always print the size, or `ZYAN_FALSE`
         * to only print it if needed.
         */
        ZYDIS_FORMATTER_PROP_FORCE_SIZE,
        /*
         * Controls the printing of segment prefixes.
         *
         * Pass `ZYAN_TRUE` as value to force the formatter to always print the segment register of
         * memory-operands or `ZYAN_FALSE` to omit implicit `DS`/`SS` segments.
         */
        ZYDIS_FORMATTER_PROP_FORCE_SEGMENT,
        /*
         * Controls the printing of the scale-factor component for memory operands.
         *
         * Pass `ZYAN_TRUE` as value to force the formatter to always print the scale-factor component
         * of memory operands or `ZYAN_FALSE` to omit the scale factor for values of `1`.
         */
        ZYDIS_FORMATTER_PROP_FORCE_SCALE_ONE,
        /*
         * Controls the printing of branch addresses.
         *
         * Pass `ZYAN_TRUE` as value to force the formatter to always print relative branch addresses
         * or `ZYAN_FALSE` to use absolute addresses, if a runtime-address different to
         * `ZYDIS_RUNTIME_ADDRESS_NONE` was passed.
         */
        ZYDIS_FORMATTER_PROP_FORCE_RELATIVE_BRANCHES,
        /*
         * Controls the printing of `EIP`/`RIP`-relative addresses.
         *
         * Pass `ZYAN_TRUE` as value to force the formatter to always print relative addresses for
         * `EIP`/`RIP`-relative operands or `ZYAN_FALSE` to use absolute addresses, if a runtime-
         * address different to `ZYDIS_RUNTIME_ADDRESS_NONE` was passed.
         */
        ZYDIS_FORMATTER_PROP_FORCE_RELATIVE_RIPREL,
        /*
         * Controls the printing of branch-instructions sizes.
         *
         * Pass `ZYAN_TRUE` as value to print the size (`short`, `near`) of branch
         * instructions or `ZYAN_FALSE` to hide it.
         *
         * Note that the `far`/`l` modifier is always printed.
         */
        ZYDIS_FORMATTER_PROP_PRINT_BRANCH_SIZE,

        /*
         * Controls the printing of instruction prefixes.
         *
         * Pass `ZYAN_TRUE` as value to print all instruction-prefixes (even ignored or duplicate
         * ones) or `ZYAN_FALSE` to only print prefixes that are effectively used by the instruction.
         */
        ZYDIS_FORMATTER_PROP_DETAILED_PREFIXES,

        /* ---------------------------------------------------------------------------------------- */
        /* Numeric values                                                                           */
        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the base of address values.
         */
        ZYDIS_FORMATTER_PROP_ADDR_BASE,
        /*
         * Controls the signedness of relative addresses. Absolute addresses are
         * always unsigned.
         */
        ZYDIS_FORMATTER_PROP_ADDR_SIGNEDNESS,
        /*
         * Controls the padding of absolute address values.
         *
         * Pass `ZYDIS_PADDING_DISABLED` to disable padding, `ZYDIS_PADDING_AUTO` to pad all
         * addresses to the current address width (hexadecimal only), or any other integer value for
         * custom padding.
         */
        ZYDIS_FORMATTER_PROP_ADDR_PADDING_ABSOLUTE,
        /*
         * Controls the padding of relative address values.
         *
         * Pass `ZYDIS_PADDING_DISABLED` to disable padding, `ZYDIS_PADDING_AUTO` to pad all
         * addresses to the current address width (hexadecimal only), or any other integer value for
         * custom padding.
         */
        ZYDIS_FORMATTER_PROP_ADDR_PADDING_RELATIVE,

        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the base of displacement values.
         */
        ZYDIS_FORMATTER_PROP_DISP_BASE,
        /*
         * Controls the signedness of displacement values.
         */
        ZYDIS_FORMATTER_PROP_DISP_SIGNEDNESS,
        /*
         * Controls the padding of displacement values.
         *
         * Pass `ZYDIS_PADDING_DISABLED` to disable padding, or any other integer value for custom
         * padding.
         */
        ZYDIS_FORMATTER_PROP_DISP_PADDING,

        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the base of immediate values.
         */
        ZYDIS_FORMATTER_PROP_IMM_BASE,
        /*
         * Controls the signedness of immediate values.
         *
         * Pass `ZYDIS_SIGNEDNESS_AUTO` to automatically choose the most suitable mode based on the
         * operands `ZydisDecodedOperand.imm.is_signed` attribute.
         */
        ZYDIS_FORMATTER_PROP_IMM_SIGNEDNESS,
        /*
         * Controls the padding of immediate values.
         *
         * Pass `ZYDIS_PADDING_DISABLED` to disable padding, `ZYDIS_PADDING_AUTO` to pad all
         * immediates to the operand-width (hexadecimal only), or any other integer value for custom
         * padding.
         */
        ZYDIS_FORMATTER_PROP_IMM_PADDING,

        /* ---------------------------------------------------------------------------------------- */
        /* Text formatting                                                                          */
        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the letter-case for prefixes.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase or `ZYAN_FALSE` to format in lowercase.
         */
        ZYDIS_FORMATTER_PROP_UPPERCASE_PREFIXES,
        /*
         * Controls the letter-case for the mnemonic.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase or `ZYAN_FALSE` to format in lowercase.
         */
        ZYDIS_FORMATTER_PROP_UPPERCASE_MNEMONIC,
        /*
         * Controls the letter-case for registers.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase or `ZYAN_FALSE` to format in lowercase.
         */
        ZYDIS_FORMATTER_PROP_UPPERCASE_REGISTERS,
        /*
         * Controls the letter-case for typecasts.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase or `ZYAN_FALSE` to format in lowercase.
         */
        ZYDIS_FORMATTER_PROP_UPPERCASE_TYPECASTS,
        /*
         * Controls the letter-case for decorators.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase or `ZYAN_FALSE` to format in lowercase.
         *
         * WARNING: this is currently not implemented (ignored).
         */
        ZYDIS_FORMATTER_PROP_UPPERCASE_DECORATORS,

        /* ---------------------------------------------------------------------------------------- */
        /* Number formatting                                                                        */
        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the prefix for decimal values.
         *
         * Pass a pointer to a null-terminated C-style string with a maximum length of 10 characters
         * to set a custom prefix, or `ZYAN_NULL` to disable it.
         *
         * The string is deep-copied into an internal buffer.
         */
        ZYDIS_FORMATTER_PROP_DEC_PREFIX,
        /*
         * Controls the suffix for decimal values.
         *
         * Pass a pointer to a null-terminated C-style string with a maximum length of 10 characters
         * to set a custom suffix, or `ZYAN_NULL` to disable it.
         *
         * The string is deep-copied into an internal buffer.
         */
        ZYDIS_FORMATTER_PROP_DEC_SUFFIX,

        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the letter-case of hexadecimal values.
         *
         * Pass `ZYAN_TRUE` as value to format in uppercase and `ZYAN_FALSE` to format in lowercase.
         *
         * The default value is `ZYAN_TRUE`.
         */
        ZYDIS_FORMATTER_PROP_HEX_UPPERCASE,
        /*
         * Controls whether to prepend hexadecimal values with a leading zero if the first character
         * is non-numeric.
         *
         * Pass `ZYAN_TRUE` to prepend a leading zero if the first character is non-numeric or
         * `ZYAN_FALSE` to disable this functionality.
         *
         * The default value is `ZYAN_FALSE`.
         */
        ZYDIS_FORMATTER_PROP_HEX_FORCE_LEADING_NUMBER,
        /*
         * Controls the prefix for hexadecimal values.
         *
         * Pass a pointer to a null-terminated C-style string with a maximum length of 10 characters
         * to set a custom prefix, or `ZYAN_NULL` to disable it.
         *
         * The string is deep-copied into an internal buffer.
         */
        ZYDIS_FORMATTER_PROP_HEX_PREFIX,
        /*
         * Controls the suffix for hexadecimal values.
         *
         * Pass a pointer to a null-terminated C-style string with a maximum length of 10 characters
         * to set a custom suffix, or `ZYAN_NULL` to disable it.
         *
         * The string is deep-copied into an internal buffer.
         */
        ZYDIS_FORMATTER_PROP_HEX_SUFFIX,

        /* ---------------------------------------------------------------------------------------- */
        /* Decorator formatting                                                                     */
        /* ---------------------------------------------------------------------------------------- */

        /*
         * Controls the printing of the APX `nf` decorator.
         *
         * Pass `ZYAN_TRUE` to append the `nf` decorator as a suffix to the instruction mnemonic
         * instead of prepending it as a pseudo prefix.
         *
         * The default value is implementation specific: `ZYAN_FALSE` for Intel and `ZYAN_TRUE` for ATT.
         *
         * WARNING: Suffix mode currently does not correctly follow the standard. The `nf` suffix should
         *          appear before any additional `zu` and/or `cc` suffix. This is not the case.
         *          The current implementation would e.g. emit `imulzunf` instead of `imulnfzu`.
         */
        ZYDIS_FORMATTER_PROP_DECO_APX_NF_USE_SUFFIX,

        /*
         * Controls the printing of the APX `dfv` decorator.
         *
         * Pass `ZYAN_TRUE` to use the immediate notation instead of the finite set notation.
         *
         * The default value is implementation specific: `ZYAN_FALSE` for Intel and `ZYAN_TRUE` for ATT.
         */
        ZYDIS_FORMATTER_PROP_DECO_APX_DFV_USE_IMMEDIATE,

        /* ---------------------------------------------------------------------------------------- */

        /*
         * Maximum value of this enum.
         */
        ZYDIS_FORMATTER_PROP_MAX_VALUE = ZYDIS_FORMATTER_PROP_DECO_APX_DFV_USE_IMMEDIATE,
        
        /*
         * The minimum number of bits required to represent all values of this enum.
         */
        //ZYDIS_FORMATTER_PROP_REQUIRED_BITS = ZYAN_BITS_TO_REPRESENT(ZYDIS_FORMATTER_PROP_MAX_VALUE)
    }

    /*
    * Enum of all decorator types.
    */
    public enum ZydisDecorator
    {
        ZYDIS_DECORATOR_INVALID,
        /**
         * The embedded-mask decorator.
         */
        ZYDIS_DECORATOR_MASK,
        /**
         * The broadcast decorator.
         */
        ZYDIS_DECORATOR_BC,
        /**
         * The rounding-control decorator.
         */
        ZYDIS_DECORATOR_RC,
        /**
         * The suppress-all-exceptions decorator.
         */
        ZYDIS_DECORATOR_SAE,
        /**
         * The register-swizzle decorator.
         */
        ZYDIS_DECORATOR_SWIZZLE,
        /**
         * The conversion decorator.
         */
        ZYDIS_DECORATOR_CONVERSION,
        /**
         * The eviction-hint decorator.
         */
        ZYDIS_DECORATOR_EH,
        /**
         * The APX no-flags decorator.
         */
        ZYDIS_DECORATOR_APX_NF,
        /**
         * The APX default flags value decorator.
         */
        ZYDIS_DECORATOR_APX_DFV,

        /**
         * Maximum value of this enum.
         */
        ZYDIS_DECORATOR_MAX_VALUE = ZYDIS_DECORATOR_APX_DFV,
        /**
         * The minimum number of bits required to represent all values of this enum.
         */
        //ZYDIS_DECORATOR_REQUIRED_BITS = ZYAN_BITS_TO_REPRESENT(ZYDIS_DECORATOR_MAX_VALUE)
    }
}
