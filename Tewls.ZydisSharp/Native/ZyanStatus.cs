using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tewls.Shared;

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
        ZYAN_STATUS_SUCCESS = 0x80100000,
        ZYAN_STATUS_FAILED = 0x80100001,
        ZYAN_STATUS_TRUE = 0x80100002,
        ZYAN_STATUS_FALSE = 0x80100003,
        ZYAN_STATUS_INVALID_ARGUMENT = 0x80100004,
        ZYAN_STATUS_INVALID_OPERATION = 0x80100005,
        ZYAN_STATUS_ACCESS_DENIED = 0x80100006,
        ZYAN_STATUS_NOT_FOUND = 0x80100007,
        ZYAN_STATUS_OUT_OF_RANGE = 0x80100008,
        ZYAN_STATUS_INSUFFICIENT_BUFFER_SIZE = 0x80100009,
        ZYAN_STATUS_NOT_ENOUGH_MEMORY = 0x8010000A,
        ZYAN_STATUS_BAD_SYSTEMCALL = 0x8010000B,
        ZYAN_STATUS_OUT_OF_RESOURCES = 0x8010000C,
        ZYAN_STATUS_MISSING_DEPENDENCY = 0x8010000D,
        ZYAN_STATUS_ARG_NOT_UNDERSTOOD = 0x80300000,
        ZYAN_STATUS_TOO_FEW_ARGS = 0x80300001,
        ZYAN_STATUS_TOO_MANY_ARGS = 0x80300002,
        ZYAN_STATUS_ARG_MISSES_VALUE = 0x80300003,
        ZYAN_STATUS_REQUIRED_ARG_MISSING = 0x80300004
    }

    public class ZyanStatus(uint value)
    {
        public ZyanStatusFlags Status { get; private set; } = (ZyanStatusFlags)value;
        public bool Success => (((uint)Status) & 0x80000000u) == 0;
        public bool Failed => (((uint)Status) & 0x80000000u) != 0;

        public void Throw([CallerMemberName] string apiName = "") =>
            throw new InvalidOperationException($"{apiName} failed: {Status}");

        public void ThrowIfFailed([CallerMemberName] string apiName = "")
        {
            if (Failed)
            {
                Throw(apiName);
            }
        }
    }

    public class ZyanStatusMarshaller : MarshallerBase<ZyanStatusMarshaller>
    {
        public override int GetNativeDataSize() => Marshal.SizeOf<int>();

        public override IntPtr MarshalManagedToNative(object managedObj) =>
            managedObj switch
            {
                null => IntPtr.Zero,
                ZyanStatus zyanStatus => new IntPtr((int)zyanStatus.Status),
                _ => throw new MarshalDirectiveException("Managed object must be of type ZyanStatus.")
            };

        public override object MarshalNativeToManaged(IntPtr pNativeData) => new ZyanStatus((uint)pNativeData);
    }
}
