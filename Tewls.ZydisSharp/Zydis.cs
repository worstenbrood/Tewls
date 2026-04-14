using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp
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
        internal static extern int ZydisDecoderInit(ref ZydisDecoder decoder, ZydisMachineMode machineMode, ZydisStackWidth stackWidth);

        public static ZydisDecoder CreateDecoder(ZydisMachineMode machineMode, ZydisStackWidth stackWidth)
        {
            var decoder = new ZydisDecoder();
            ZyanStatus.ThrowIfFailed(ZydisDecoderInit(ref decoder, machineMode, stackWidth), nameof(ZydisDecoderInit));
            return decoder;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern int ZydisDecoderDecodeFull(ref ZydisDecoder decoder, IntPtr buffer, uint length, ref ZydisDecodedInstruction instruction, IntPtr operands);

        public static DecodeResult DecodeFull(ref ZydisDecoder decoder, byte[] buffer)
        {
            var instruction = new ZydisDecodedInstruction();
            var operands = new ZydisDecodedOperand[ZYDIS_MAX_OPERAND_COUNT];
            using var bufferPin = new Pinned<byte[]>(buffer);
            using var opsPin = new Pinned<ZydisDecodedOperand[]>(operands);
            ZyanStatus.ThrowIfFailed(ZydisDecoderDecodeFull(ref decoder, bufferPin.Address, (uint)buffer.Length, ref instruction, opsPin.Address),
                nameof(ZydisDecoderDecodeFull));
            return new DecodeResult(instruction, operands);
        }
    }
}
