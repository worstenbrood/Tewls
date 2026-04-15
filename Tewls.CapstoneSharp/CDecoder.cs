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
        public static CDecoder Create(cs_arch arch, cs_mode mode, bool detail = true)
        {
            nint handle = new ();
            var result = Capstone.cs_open(arch, mode, ref handle);
            if (result != cs_err.CS_ERR_OK)
            {
                throw new CapstoneException(result);
            }

            var decoder = new CDecoder(handle);
            try
            {
                decoder.SetOption(cs_opt_type.CS_OPT_DETAIL, detail);
                decoder.SetOption(cs_opt_type.CS_OPT_SKIPDATA, false);
            }
            catch
            {
                decoder.Dispose();
                throw;
            }
            return decoder;
        }

        private void SetOption(cs_opt_type type, bool value)
        {
            var result = Capstone.cs_option(Address, type, value ? 1 : 0);
            if (result != cs_err.CS_ERR_OK)
            {
                throw new CapstoneException(result);
            }
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

        /// <summary>
        /// Throws a CapstoneException with the last error code from the Capstone library. 
        /// This method is used to handle errors that occur during disassembly.
        /// </summary>
        private void ThrowLastError() => CapstoneException.ThrowLastError(Address);

        /// <summary>
        /// Create buffer for the current decoder. This buffer will be used to store the disassembled instructions. 
        /// The buffer will be automatically freed when it is disposed.
        /// </summary>
        /// <returns></returns>
        public CapstoneBuffer CreateBuffer() => CapstoneBuffer.Create(Address);

        public CDecodeResult DecodeFull(byte[] code, int index = 0, int length = 0)
        {
            if (length == 0)
            {
                length = code.Length - index;
            }

            nint result = new();
   
            // Pin buffer array
            using var codePin = new PinnedArray<byte>(code, index);

            // Disassemble the instruction at the specified index and length.
            // If length is 0, it will disassemble until the end of the buffer.
            var instructions = Capstone.cs_disasm(Address, codePin.Address, (uint)length, 0, 0, ref result);
            if (instructions == 0)
            {
                ThrowLastError();
            }

            using var buffer = new CapstoneBuffer(result, instructions);
            var instruction = Marshal.PtrToStructure<cs_insn>(result);

            return instruction.Detail != 0 ?
                new CDecodeResult(instruction, Marshal.PtrToStructure<cs_detail>(instruction.Detail)) : 
                new CDecodeResult(instruction, null);
        }
    }
}
