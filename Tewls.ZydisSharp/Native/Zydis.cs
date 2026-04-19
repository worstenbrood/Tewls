using System.Runtime.InteropServices;
using Tewls.Shared;

namespace Tewls.ZydisSharp.Native
{
    public class Zydis
    {
        public const string LibraryName = "Zydis";
        public const int ZYDIS_MAX_OPERAND_COUNT = 10;
        public const int ZYDIS_ENCODER_MAX_OPERANDS = 5;
        public const int ZYDIS_MAX_INSTRUCTION_LENGTH = 15;

        static Zydis()
        {
            NativeLoader.Load<Zydis>(LibraryName);
#if DEBUG
            Console.WriteLine($"Zydis version: {GetVersion()}");
#endif
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong ZydisGetVersion();

        public static Version GetVersion()
        {
            ulong version = ZydisGetVersion();
            int major = (int)(((version) & 0xFFFF000000000000) >> 48);
            int minor = (int)(((version) & 0x0000FFFF00000000) >> 32);
            int patch = (int)(((version) & 0x00000000FFFF0000) >> 16);
            int build = (int)((version) & 0x000000000000FFFF);
            return new Version(major, minor, build, patch);
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisDecoderInit(ref ZydisDecoder decoder, ZydisMachineMode machineMode, ZydisStackWidth stackWidth);

        public static ZydisDecoder CreateDecoder(ZydisMachineMode machineMode, ZydisStackWidth stackWidth)
        {
            var decoder = new ZydisDecoder();
            ZyanStatus result = ZydisDecoderInit(ref decoder, machineMode, stackWidth);
            result.ThrowIfFailed(nameof(ZydisDecoderInit));
            return decoder;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisDecoderDecodeFull(ref ZydisDecoder decoder, IntPtr buffer, uint length, 
            ref ZydisDecodedInstruction instruction, IntPtr operands);

        public static ZDecodeResult? DecodeFull(ref ZydisDecoder decoder, byte[] buffer, int index, int byteCount)
        {
            // If length is 0, use length of the buffer starting from index
            if (byteCount == 0)
            {
                byteCount = buffer.Length - index;
            }

            // Initialize the instruction and operands structures
            var instruction = new ZydisDecodedInstruction();
            var operands = new ZydisDecodedOperand[ZYDIS_MAX_OPERAND_COUNT];

            // Pin the buffer and operands array to get their addresses
            using var bufferPin = new PinnedArray<byte>(buffer, index);
            using var opsPin = new PinnedArray<ZydisDecodedOperand>(operands);

            // Call the native function to decode the instruction
            ZyanStatus result = ZydisDecoderDecodeFull(ref decoder, bufferPin.Address, (uint)byteCount,
                ref instruction, opsPin.Address);

            if (result.Status != ZyanStatusFlags.ZYDIS_STATUS_NO_MORE_DATA)
            {
                // Check the result and throw an exception if it failed
                result.ThrowIfFailed(nameof(ZydisDecoderDecodeFull));
                
                // Return the decoded instruction and operands as a ZDecodeResult
                return new ZDecodeResult(instruction, operands);
            }

            return null;
        }

        /// <summary>
        /// Calculates the absolute address value for the given instruction operand. 
        /// </summary>
        /// <param name="instruction">A pointer to the ZydisDecodedInstruction struct. </param>
        /// <param name="operand">A pointer to the ZydisDecodedOperand struct. </param>
        /// <param name="runtimeAddress">The runtime address of the instruction.</param>
        /// <param name="resultAddress">A pointer to the memory that receives the absolute address.</param>
        /// <returns></returns>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisCalcAbsoluteAddress(ref ZydisDecodedInstruction instruction,
                ref ZydisDecodedOperand operand, ulong runtimeAddress, out ulong resultAddress);

        /// <summary>
        /// Calculates the absolute address value for the given instruction operand. 
        /// </summary>
        /// <param name="instruction">A reference to the ZydisDecodedInstruction struct. </param>
        /// <param name="operand">A reference to the ZydisDecodedOperand struct.</param>
        /// <param name="runtimeAddress">The runtime address of the instruction.</param>
        /// <returns>The absolute address.</returns>
        public static ulong CalcAbsoluteAddress(ref ZydisDecodedInstruction instruction,
            ref ZydisDecodedOperand operand, ulong runtimeAddress)
        {
            ZyanStatus result = ZydisCalcAbsoluteAddress(ref instruction, ref operand, runtimeAddress, out ulong address);
            result.ThrowIfFailed(nameof(ZydisCalcAbsoluteAddress));
            return address;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisFormatterInit(IntPtr formatter, ZydisFormatterStyle style);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisFormatterSetProperty(IntPtr formatter, ZydisFormatterProperty property, IntPtr value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisFormatterFormatInstruction(IntPtr formatter, 
            ref ZydisDecodedInstruction instruction, ZydisDecodedOperand[] operands, byte operandCount, byte[] buffer, uint length, ulong runtimeAddress,
            IntPtr userData);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisEncoderDecodedInstructionToEncoderRequest(ref ZydisDecodedInstruction instruction, ZydisDecodedOperand[] operands, byte operand_count_visible, ref ZydisEncoderRequest request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisEncoderEncodeInstruction(ref ZydisEncoderRequest request, byte[] buffer, ref ulong length);
    }
}
