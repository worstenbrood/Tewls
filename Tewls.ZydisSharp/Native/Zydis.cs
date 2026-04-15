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
            var r = ZydisDecoderInit(ref decoder, machineMode, stackWidth);
            return decoder;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ZyanStatusMarshaller))]
        internal static extern ZyanStatus ZydisDecoderDecodeFull(ref ZydisDecoder decoder, IntPtr buffer, uint length, 
            ref ZydisDecodedInstruction instruction, IntPtr operands);

        public static ZDecodeResult DecodeFull(ref ZydisDecoder decoder, byte[] buffer)
        {
            var instruction = new ZydisDecodedInstruction();
            var operands = new ZydisDecodedOperand[ZYDIS_MAX_OPERAND_COUNT];
            using var bufferPin = new PinnedArray<byte>(buffer);
            using var opsPin = new PinnedArray<ZydisDecodedOperand>(operands);
            var result = ZydisDecoderDecodeFull(ref decoder, bufferPin.Address, (uint)buffer.Length, ref instruction, 
                opsPin.Address);
            result.ThrowIfFailed(nameof(ZydisDecoderDecodeFull));
            return new ZDecodeResult(instruction, operands);
        }
    }
}
