using System.Runtime.InteropServices;
using Tewls.Shared;

namespace Tewls.ZydisSharp.Native
{
    public class ZyanStatusMarshaller : MarshallerBase<ZyanStatusMarshaller>
    {
        public override int GetNativeDataSize() => Marshal.SizeOf<int>();

        public override IntPtr MarshalManagedToNative(object managedObj) =>
            managedObj switch
            {
                null => IntPtr.Zero,
                ZyanStatus zyanStatus => new IntPtr(zyanStatus.Status),
                _ => throw new MarshalDirectiveException("Managed object must be of type ZyanStatus.")
            };

        public override object MarshalNativeToManaged(IntPtr pNativeData) => new ZyanStatus((uint)pNativeData);
    }
}
