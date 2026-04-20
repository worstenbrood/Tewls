using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZEncoder
    {
        private ZydisEncoderRequest _request;

        public ZEncoder(ref ZydisEncoderRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Create a new encoder request based on a decoded instruction. 
        /// This is useful for re-encoding instructions after modifying them.
        /// </summary>
        /// <param name="decodeResult"></param>
        /// <returns></returns>
        public static ZEncoder Create(ZDecodeResult decodeResult)
        {
            var request = new ZydisEncoderRequest();
            var result = Zydis.ZydisEncoderDecodedInstructionToEncoderRequest(ref decodeResult.Instruction, decodeResult.Operands,
                decodeResult.Instruction.OperandCountVisible, ref request);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderDecodedInstructionToEncoderRequest));
            return new ZEncoder(ref request);
        }

        /// <summary>
        /// Encode request into a byte array. The request must be properly initialized before calling this method.
        /// </summary>
        /// <returns></returns>
        public byte[] Encode()
        {
            byte[] buffer = new byte[Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH];
            ulong length = (ulong)buffer.Length;
            var result = Zydis.ZydisEncoderEncodeInstruction(ref _request, buffer, ref length);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstruction));
            Array.Resize(ref buffer, (int)length);
            return buffer;
        }

        /// <summary>
        /// Encode request into a byte array, using the provided runtime address for relative operand encoding. 
        /// The request must be properly initialized before calling this method.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte[] EncodeAbsolute(ulong address)
        {
            byte[] buffer = new byte[Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH];
            ulong length = (ulong)buffer.Length;
            var result = Zydis.ZydisEncoderEncodeInstructionAbsolute(ref _request, buffer, ref length, address);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstructionAbsolute));
            Array.Resize(ref buffer, (int)length);
            return buffer;
        }
    }
}
