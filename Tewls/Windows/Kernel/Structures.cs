using System;

namespace Tewls.Windows.Kernel
{
    [Flags]
    public enum LocalMemFlags : uint
    {
        Fixed = 0,
        Movable = 2,
        ZeroInit = 4,
        Ptr = 0x0040,
        Hnd = 0x0042,
    }
}
