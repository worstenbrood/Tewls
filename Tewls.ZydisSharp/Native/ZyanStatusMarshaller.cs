using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp.Native
{
    public class ZyanStatusMarshaller : ICustomMarshaler
    {
        public void CleanUpManagedData(object ManagedObj)
        {
            // Its an int, so we don't need to do anything to clean it up.
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            // Its an int, so we don't need to do anything to clean it up.
        }

        public int GetNativeDataSize() => Marshal.SizeOf<int>();

        public IntPtr MarshalManagedToNative(object managedObj) =>
            managedObj switch
            {
                null => IntPtr.Zero,
                ZyanStatus zyanStatus => new IntPtr(zyanStatus.Status),
                _ => throw new MarshalDirectiveException("Managed object must be of type ZyanStatus.")
            };

        public object MarshalNativeToManaged(IntPtr pNativeData) => new ZyanStatus(pNativeData.ToInt32());

        private static readonly Lazy<ZyanStatusMarshaller> _instance = new(() => new());
        public static ICustomMarshaler GetInstance(string cookie) => _instance.Value;
    }
}
