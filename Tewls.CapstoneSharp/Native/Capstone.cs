using System.Runtime.InteropServices;
using Tewls.Shared;

namespace Tewls.CapstoneSharp.Native
{
    public class Capstone
    {
        public const string LibraryName = "Capstone";

        static Capstone()
        {
            NativeLoader.Load<Capstone>(LibraryName);
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int cs_version(ref int major, ref int minor);

        public static Version GetVersion()
        {
            int major = 0, minor = 0;
            int build = cs_version(ref major, ref minor);
            return new Version(major, minor, build);
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_arm();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_aarch64();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_x86();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_powerpc();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_sparc();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_systemz();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_xcore();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_m68k();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_tms320c64x();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_m680x();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_evm();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_mos65xx();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_wasm();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_bpf();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_riscv();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_sh();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_tricore();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_alpha();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_loongarch();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_arch_register_arc();

        /// <summary>
        /// This API can be used to either ask for archs supported by this library,
        /// or check to see if the library was compile with 'diet' option (or called
        /// in 'diet' mode).
        /// </summary>

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool cs_support(int query);

        /// <summary>
        /// Initialize CS handle: this must be done before any usage of CS.
        /// </summary>
        /// <param name="arch">architecture type(CS_ARCH_*)</param>
        /// <param name="mode">hardware mode.This is combined of CS_MODE_*</param>
        /// <param name="handle">pointer to handle, which will be updated at return time</param>
        /// <returns>CS_ERR_OK on success, or other value on failure (refer to cs_err enum
        ///for detailed error).</returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern cs_err cs_open(cs_arch arch, cs_mode mode, ref nint handle);

        /// <summary>
        ///  Close CS handle: MUST do to release the handle when it is not used anymore.
        ///  NOTE: this must be only called when there is no longer usage of Capstone,
        ///  not even access to cs_insn array.The reason is the this API releases some
        ///  cached memory, thus access to any Capstone API after cs_close() might crash
        ///  your application.
        ///  In fact, this API invalidate @handle by ZERO out its value (i.e* handle = 0).
        ///</summary>
        /// <param name="handle">pointer to a handle returned by cs_open()</param>
        /// <returns>CS_ERR_OK on success, or other value on failure(refer to cs_err enum</returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern cs_err cs_close(ref nint handle);

        /// <summary>
        /// Set option for disassembling engine at runtime
        /// NOTE: in the case of CS_OPT_MEM, handle's value can be anything,
        /// so that cs_option(handle, CS_OPT_MEM, value) can(i.e must) be called
        /// even before cs_open()
        /// </summary>
        /// <param name="handle">handle returned by cs_open()</param>
        /// <param name="type">type of option to be set</param>
        /// <param name="value">option value corresponding with @type</param>
        /// <returns>CS_ERR_OK on success, or other value on failure.
        /// Refer to cs_err enum for detailed error.</returns>
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern cs_err cs_option(nint handle, cs_opt_type type, IntPtr value);

        /// <summary>
        /// Report the last error number when some API function fail.
        /// Like glibc's errno, cs_errno might not retain its old value once accessed.
        /// </summary>
        /// <param name="handle">handle returned by cs_open()</param>
        /// <returns>error code of cs_err enum type (CS_ERR_*, see above)</returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern cs_err cs_errno(nint handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern nint cs_strerror(cs_err code);

        /// <summary>
        /// Disassemble binary code, given the code buffer, size, address and number
        /// of instructions to be decoded.
        /// This API dynamically allocate memory to contain disassembled instruction.
        /// Resulting instructions will be put into <paramref name="insn"/>
        /// NOTE 1: this API will automatically determine memory needed to contain
        /// output disassembled instructions in <paramref name="insn"/>.
        /// NOTE 2: caller must free the allocated memory itself to avoid memory leaking.
        /// NOTE 3: for system with scarce memory to be dynamically allocated such as
        /// OS kernel or firmware, the API cs_disasm_iter() might be a better choice than
        /// <see cref="cs_disasm"/>. The reason is that with <see cref="cs_disasm"/>, based on limited available
        /// memory, it complicates things. This is especially troublesome for the case @count=0,
        /// when <see cref="cs_disasm"/> runs uncontrollably (until either end of input buffer, or
        ///when it encounters an invalid instruction).
        /// </summary>
        /// <param name="handle">handle returned by <see cref="cs_open"/></param>
        /// <param name="code">buffer containing raw binary code to be disassembled.</param>
        /// <param name="code_size">size of the above code buffer.</param>
        /// <param name="address">address of the first instruction in given raw code buffer.</param>
        /// <param name="count">number of instructions to be disassembled, or 0 to get all of them</param>
        /// <param name="insn"></param>
        /// <returns>the number of successfully disassembled instructions,
        /// or 0 if this function failed to disassemble the given code</returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint cs_disasm(nint handle, nint code, uint code_size,
			      ulong address, uint count, ref nint insn);

        /// <summary>
        ///  Free memory allocated by cs_malloc() or <see cref="cs_disasm"/> (argument <paramref name="insn"/>)
        /// </summary>
        /// <param name="insn">pointer returned by @insn argument in <see cref="cs_disasm"/> or <see cref="cs_malloc"/></param>
        /// <param name="count">number of cs_insn structures returned by <see cref="cs_disasm"/>, or 1
        /// to free memory allocated by<see cref="cs_malloc"/>.</param>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cs_free(nint insn, uint count);

        /// <summary>
        /// Allocate memory for 1 instruction to be used by cs_disasm_iter().
        /// NOTE: when no longer in use, you can reclaim the memory allocated for
        /// this instruction with cs_free(insn, 1)
        /// </summary>
        /// <param name="handle">handle returned by cs_open()</param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern nint cs_malloc(nint handle);

        /// <summary>
        /// Fast API to disassemble binary code, given the code buffer, size, address
        /// and number of instructions to be decoded.
        /// This API puts the resulting instruction into a given cache in @insn.
        /// See tests/test_iter.c for sample code demonstrating this API.
        /// NOTE 1: this API will update @code, @size & @address to point to the next
        /// instruction in the input buffer.Therefore, it is convenient to use
        /// cs_disasm_iter() inside a loop to quickly iterate all the instructions.
        /// While decoding one instruction at a time can also be achieved with
        /// <see cref="cs_disasm"/>(count= 1), some benchmarks shown that cs_disasm_iter() can be 30%
        /// faster on random input.
        /// NOTE 2: the cache in @insn can be created with cs_malloc() API.
        /// NOTE 3: for system with scarce memory to be dynamically allocated such as
        /// OS kernel or firmware, this API is recommended over <see cref="cs_disasm"/>, which
        /// allocates memory based on the number of instructions to be disassembled.
        /// The reason is that with <see cref="cs_disasm"/>, based on limited available memory,
        /// we have to calculate in advance how many instructions to be disassembled,
        /// which complicates things.This is especially troublesome for the case
        /// @count= 0, when <see cref="cs_disasm"/> runs uncontrollably (until either end of input
        /// buffer, or when it encounters an invalid instruction).
        /// </summary>
        /// <param name="handle">handle returned by <see cref="cs_open"/></param>
        /// <param name="code">buffer containing raw binary code to be disassembled</param>
        /// <param name="size">size of above code</param>
        /// <param name="address">address of the first insn in given raw code buffer</param>
        /// <param name="insn">pointer to instruction to be filled in by this API.</param>
        /// <returns>true if this API successfully decode 1 instruction,
        /// or false otherwise. On failure, call <see cref="cs_errno"/> for error code.
        /// </returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool cs_disasm_iter(nint handle, nint code, ref uint size, ref nint address, ref cs_insn insn);

        /// <summary>
        /// Helper method to get error message string for a given error code. This is a wrapper around cs_strerror.
        /// </summary>
        /// <param name="code"><see cref="cs_err"/></param>
        /// <returns></returns>
        public static string GetErrorMessage(cs_err code) =>
            Marshal.PtrToStringAnsi(cs_strerror(code)) ?? $"Unknown error(0x{(int)code:X4})";
    }
}
