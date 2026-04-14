using System;
using System.Runtime.InteropServices;

namespace TewlKit.Hooking
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="originalAddress"></param>
    /// <param name="stubAddress"></param>
    /// <param name="stubSize"></param>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct HookResult(IntPtr originalAddress, IntPtr stubAddress, int stubSize)
    {
        /// <summary>
        /// Empty result with null addresses and zero size.
        /// </summary>
        public static readonly HookResult Empty = new (IntPtr.Zero, IntPtr.Zero, 0);

        /// <summary>
        /// Original method address. This is the address of the original method that was hooked. 
        /// </summary>
        public IntPtr OriginalAddress { get; } = originalAddress;

        /// <summary>
        /// Gets the memory address of the stub function.
        /// </summary>
        public IntPtr StubAddress { get; } = stubAddress;

        /// <summary>
        /// Stub size
        /// </summary>
        public int StubSize { get; } = stubSize;
    }
}
