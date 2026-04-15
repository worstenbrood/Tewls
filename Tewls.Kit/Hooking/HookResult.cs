using System.Runtime.InteropServices;

namespace Tewls.Kit.Hooking
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="originalAddress"></param>
    /// <param name="stubAddress"></param>
    /// <param name="stubSize"></param>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct HookResult(nint originalAddress, nint stubAddress, int stubSize)
    {
        /// <summary>
        /// Empty result with null addresses and zero size.
        /// </summary>
        public static readonly HookResult Empty = new (0, 0, 0);

        /// <summary>
        /// Original method address. This is the address of the original method that was hooked. 
        /// </summary>
        public nint OriginalAddress { get; } = originalAddress;

        /// <summary>
        /// Gets the memory address of the stub function.
        /// </summary>
        public nint StubAddress { get; } = stubAddress;

        /// <summary>
        /// Stub size
        /// </summary>
        public int StubSize { get; } = stubSize;
    }
}
