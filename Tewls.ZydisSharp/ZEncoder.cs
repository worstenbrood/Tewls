using Tewls.Shared;
using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZEncoder
    {
        public ZydisEncoderRequest Request;

        public ZEncoder(ref ZydisEncoderRequest request)
        {
            Request = request;
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
            var result = Zydis.ZydisEncoderEncodeInstruction(ref Request, buffer, ref length);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstruction));
            Array.Resize(ref buffer, (int)length);
            return buffer;
        }

        /// <summary>
        /// Set absolute addresses for all relative operands in the request.
        /// This is useful for encoding instructions with RIP-relative addressing or immediate operands that represent addresses.
        /// </summary>
        /// <param name="absoluteAddress"></param>
        /// <returns></returns>
        public ZEncoder SetAbsoluteAddress(ulong absoluteAddress)
        {
            int count = Math.Min(Request.OperandCount, Request.Operands.Length);

            for (int i = 0; i < count; i++)
            {
                ref var op = ref Request.Operands[i];

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_MEMORY &&
                    (op.Mem.Base == ZydisRegister.ZYDIS_REGISTER_RIP ||
                    op.Mem.Base == ZydisRegister.ZYDIS_REGISTER_EIP ||
                    op.Mem.Base == ZydisRegister.ZYDIS_REGISTER_IP))
                {
                    op.Mem.Displacement = unchecked((long)absoluteAddress);
                }

                if (op.Type == ZydisOperandType.ZYDIS_OPERAND_TYPE_IMMEDIATE)
                {
                    op.Imm.U = absoluteAddress;
                }
            }
            return this;
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
            IntPtr length = new(buffer.Length);
            var result = Zydis.ZydisEncoderEncodeInstructionAbsolute(ref Request, buffer, ref length, address);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstructionAbsolute));
            Array.Resize(ref buffer, length.ToInt32());
            return buffer;
        }

        /// <summary>
        /// Fill a buffer with nops. This is useful for padding instructions or filling gaps in code.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public static void NopFill(byte[] buffer, int index = 0, int length = 0)
        {
            if (length == 0 || length > buffer.Length - index)
            {
                length = buffer.Length - index;
            }

            using var bufferPin = new PinnedArray<byte>(buffer, index);
            var result = Zydis.ZydisEncoderNopFill(bufferPin.Address, new IntPtr(length));
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderNopFill));
        }
    }
}
