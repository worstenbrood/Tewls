using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
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

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct ZydisFormatterContext
    {
        /**
         * A pointer to the `ZydisDecodedInstruction` struct.
         */
        public IntPtr Instruction;
        /**
         * A pointer to the first `ZydisDecodedOperand` struct of the instruction.
         */
        public IntPtr Operands;
        /**
         * A pointer to the `ZydisDecodedOperand` struct.
         */
        public IntPtr Operand;
        /**
         * The runtime address of the instruction.
         */
        public ulong RuntimeAddress;
        /**
         * A pointer to user-defined data.
         *
         * This is the value that was previously passed as the `user_data` argument to 
         * @ref ZydisFormatterFormatInstruction or @ref ZydisFormatterTokenizeOperand.
         */
        public IntPtr UserData;
    }
}
