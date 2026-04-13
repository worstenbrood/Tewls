
namespace TewlKit.Asm
{
    /// <summary>
    /// ASM opcodes used for generating stubs and hooks.
    /// </summary>
    public enum OpCodes : ushort
    {
        RelativeJump = 0xE9,
        IndirectJump = 0x25FF,
        MovRax = 0xB848,
        JmpRax = 0xE0FF,
    }
}
