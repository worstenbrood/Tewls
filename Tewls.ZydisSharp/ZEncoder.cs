using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZEncoder
    {
        private ZydisEncoderRequest _request;

        internal ZEncoder(ref ZydisEncoderRequest request)
        {
            _request = request;
        }

        public static ZEncoder Create(ZDecodeResult decodeResult)
        {
            var request = new ZydisEncoderRequest();
            var result = Zydis.ZydisEncoderDecodedInstructionToEncoderRequest(ref decodeResult.Instruction, decodeResult.Operands,
                decodeResult.Instruction.OperandCountVisible, ref request);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderDecodedInstructionToEncoderRequest));
            return new ZEncoder(ref request);
        }

        public byte[] Encode()
        {
            byte[] buffer = new byte[Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH];
            ulong length = (ulong)buffer.Length;
            var result = Zydis.ZydisEncoderEncodeInstruction(ref _request, buffer, ref length);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstruction));
            Array.Resize(ref buffer, (int)length);
            return buffer;
        }

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
