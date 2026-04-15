namespace Tewls.Kit.Asm
{
    /// <summary>
    /// ASM opcodes used for generating stubs and hooks.
    /// </summary>
    public static class OpCodes 
    {
        public const byte RelativeJump = 0xE9;
        public const ushort IndirectJump = 0x25FF;
        public const ushort MovRax = 0xB848;
        public const byte MovEax = 0xB8;
        public const ushort JmpEaxRax = 0xE0FF;
    }
}
