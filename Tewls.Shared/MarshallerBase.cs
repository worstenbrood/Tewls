using System.Runtime.InteropServices;

namespace Tewls.Shared
{
    public class MarshallerBase<T> : ICustomMarshaler
        where T: MarshallerBase<T>, new()
    {
        public virtual void CleanUpManagedData(object ManagedObj)
        {}

        public virtual void CleanUpNativeData(IntPtr pNativeData)
        {}

        public virtual int GetNativeDataSize() => 0;

        public virtual IntPtr MarshalManagedToNative(object ManagedObj) => IntPtr.Zero;

        public virtual object MarshalNativeToManaged(IntPtr pNativeData) => new ();

        private static readonly Lazy<T> _instance = new(() => new());
        
        public static ICustomMarshaler GetInstance(string cookie) => _instance.Value;
    }
}
