using System.Runtime.InteropServices;
using Tewls.CapstoneSharp.Native;
using Tewls.Shared;

namespace Tewls.CapstoneSharp
{
    public class CDecoder : NativePointer
    {
        /// <summary>
        /// Creates a new CDecoder instance with the specified architecture and mode. 
        /// This method will throw a CapstoneException if the decoder cannot be created successfully.
        /// </summary>
        /// <param name="arch">architecture</param>
        /// <param name="mode">mode</param>
        /// <returns></returns>
        /// <exception cref="CapstoneException"></exception>
        public static CDecoder Create(cs_arch arch, cs_mode mode)
        {
            nint handle = new ();
            var result = Capstone.cs_open(arch, mode, ref handle);
            if (result != cs_err.CS_ERR_OK)
            {
                throw new CapstoneException(result);
            }
            return new CDecoder(handle);
        }

        public static CDecoder Create64() => Create(cs_arch.CS_ARCH_X86, cs_mode.CS_MODE_64);
        public static CDecoder Create32() => Create(cs_arch.CS_ARCH_X86, cs_mode.CS_MODE_32);
        public static CDecoder CreateArm() => Create(cs_arch.CS_ARCH_ARM, cs_mode.CS_MODE_ARM);
        public static CDecoder CreateArm64() => Create(cs_arch.CS_ARCH_ARM64, cs_mode.CS_MODE_ARM);

        protected override void Dispose(bool disposing)
        {
            if (Address != 0)
            {
                // cs_close will set the handle to 0 if it succeeds, so we don't need to set it to 0 ourselves.
                Capstone.cs_close(ref Address);
            }
        }

        /// <summary>
        /// Constructor for the CDecoder class. This constructor is internal and should only be called by the Create method.
        /// </summary>
        /// <param name="handle"></param>
        internal CDecoder(nint handle) : base(handle)
        {
        }

        public cs_insn Disassemble(byte[] code)
        {
            nint result = new();
            using var codePin = new PinnedArray<byte>(code);
            var instructions = Capstone.cs_disasm(Address, codePin.Address, (uint)code.Length, 0, 0, ref result);
            if (instructions == 0)
            {
                CapstoneException.ThrowLastError(Address);
            }

            using var buffer = new CapstoneBuffer(result, instructions);
            return Marshal.PtrToStructure<cs_insn>(result);
        }
    }
}
