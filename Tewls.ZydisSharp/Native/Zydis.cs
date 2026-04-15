using System.Runtime.InteropServices;
using Tewls.Shared;

namespace Tewls.ZydisSharp.Native
{
    public class Zydis
    {
        public const string LibraryName = "Zydis";
        public const int ZYDIS_MAX_OPERAND_COUNT = 10;

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
            var result = ZydisDecoderInit(ref decoder, machineMode, stackWidth);
            result.ThrowIfFailed(nameof(ZydisDecoderInit));
            return decoder;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisDecoderDecodeFull(ref ZydisDecoder decoder, IntPtr buffer, uint length, 
            ref ZydisDecodedInstruction instruction, IntPtr operands);

        public static ZDecodeResult DecodeFull(ref ZydisDecoder decoder, byte[] buffer, int index, int byteCount)
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
            var result = ZydisDecoderDecodeFull(ref decoder, bufferPin.Address, (uint)byteCount, ref instruction,
                opsPin.Address);

            // Check the result and throw an exception if it failed
            result.ThrowIfFailed(nameof(ZydisDecoderDecodeFull));

            // Return the decoded instruction and operands as a ZDecodeResult
            return new ZDecodeResult(instruction, operands);
        }
    }
}
