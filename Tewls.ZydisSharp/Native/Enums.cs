namespace Tewls.ZydisSharp
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
}
