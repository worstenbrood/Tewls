using System.Runtime.CompilerServices;

namespace Tewls.ZydisSharp.Native
{
    public enum ZyanStatusFlags : uint
    {
        ZYDIS_STATUS_NO_MORE_DATA = 0x80200000,
        ZYDIS_STATUS_DECODING_ERROR = 0x80200001,
        ZYDIS_STATUS_INSTRUCTION_TOO_LONG = 0x80200002,
        ZYDIS_STATUS_BAD_REGISTER = 0x80200003,
        ZYDIS_STATUS_ILLEGAL_LOCK = 0x80200004,
        ZYDIS_STATUS_ILLEGAL_LEGACY_PFX = 0x80200005,
        ZYDIS_STATUS_ILLEGAL_REX = 0x80200006,
        ZYDIS_STATUS_ILLEGAL_REX2 = 0x80200007,
        ZYDIS_STATUS_INVALID_MAP = 0x80200008,
        ZYDIS_STATUS_MALFORMED_EVEX = 0x80200009,
        ZYDIS_STATUS_MALFORMED_MVEX = 0x8020000A,
        ZYDIS_STATUS_INVALID_MASK = 0x8020000B,
        ZYDIS_STATUS_SKIP_TOKEN = 0x8020000C,
        ZYDIS_STATUS_IMPOSSIBLE_INSTRUCTION = 0x8020000D,
    }

    public class ZyanStatus(uint value)
    {
        public ZyanStatusFlags Status { get; private set; } = (ZyanStatusFlags)value;
        public bool Success => (((uint)Status) & 0x80000000u) == 0;
        public bool Failed => (((uint)Status) & 0x80000000u) != 0;

        public void ThrowIfFailed([CallerMemberName] string apiName = "")
        {
            if (Failed)
            {
                throw new InvalidOperationException($"{apiName} failed: {Status.ToString("G")}");
            }
        } 
    }
}
