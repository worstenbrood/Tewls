using System;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Advapi
{
    public class NativeToken : NativeHandle
    {
        public NativeToken(IntPtr handle, bool dispose = true) : base(handle, dispose)
        {
        }
    }
}
