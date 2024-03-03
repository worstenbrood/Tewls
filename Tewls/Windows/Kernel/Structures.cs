using System;
using System.Runtime.InteropServices;

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

    [StructLayout(LayoutKind.Sequential)]
    public class SystemInfo
    {
        public short ProcessorArchitecture;
        public short Reserved;
        public uint PageSize;
        public IntPtr MinimumApplicationAddress;
        public IntPtr MaximumApplicationAddress;
        public IntPtr ActiveProcessorMask;
        public uint NumberOfProcessors;
        public uint ProcessorType;
        public uint AllocationGranularity;
        public short ProcessorLevel;
        public short ProcessorRevision;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void GetSystemInfo(SystemInfo lpSystemInfo);

        public static SystemInfo GetSystemInfo()
        {
            var info = new SystemInfo();
            GetSystemInfo(info);
            return info;
        }
    };
}
