using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class ZDecoder
    {
        private ZydisDecoder _decoder;

        public static ZDecoder Create(ZydisMachineMode machineMode, ZydisStackWidth stackWidth) =>
            new(Zydis.CreateDecoder(machineMode, stackWidth));

        /// <summary>
        /// Standard decoder for normal 64-bit x86 code.
        /// </summary>
        public static ZDecoder Create64() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_64);

        /// <summary>
        /// Decoder for 32-bit x86 code.
        /// </summary>
        public static ZDecoder Create32() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LEGACY_32, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);

        /// <summary>
        /// Decoder for 64-bit mode with a 32-bit stack width.
        /// Advanced/special-case use only.
        /// </summary>
        public static ZDecoder Create64With32BitStack() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);

        internal ZDecoder(ZydisDecoder zydisDecoder)
        {
            _decoder = zydisDecoder;
        }

        /// <summary>
        /// Decode a single instruction
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public ZDecodeResult? DisassembleIter(byte[] buffer, int index = 0, int length = 0) =>
            Zydis.DecodeFull(ref _decoder, buffer, index, length);

        /// <summary>
        /// Fully decodes an instruction from the given buffer. This method will decode the instruction and all of its operands, 
        /// including any implicit operands that may be present. The resulting DecodeResult will contain the decoded instruction 
        /// and its operands, as well as any relevant information about the instruction, such as whether it has rip-relative 
        /// memory or relative immediates.
        /// </summary>
        /// <param name="buffer">Buffer containing the binary instruction data.</param>
        /// <returns><see cref="ZDecodeResult"/></returns>
        public IEnumerable<ZDecodeResult> Disassemble(byte[] buffer, int index = 0, int length = 0)
        {
            var currentIndex = index;
            do
            {
                var result = DisassembleIter(buffer, currentIndex, length);
                // No more data
                if (result == null)
                {
                    yield break;
                }

                // Return the decoded instruction
                yield return result;

                // Adjust index and length for the next iteration
                currentIndex += result.Instruction.Length;
                length = buffer.Length - currentIndex;
            } while (length > 0);
        }
    }
}
