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

    public enum ProcessorArchitecture : ushort
    {
        Amd64 = 9,
        Arm = 5,
        Arm64 = 12,
        IA64 = 6,
        Intel = 0,
        Unknown = 0xffff
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SystemInfo
    {
        public ProcessorArchitecture ProcessorArchitecture;
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
