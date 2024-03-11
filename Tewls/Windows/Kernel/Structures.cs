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

    public enum ProcessorType : ushort
    {
        Intel386 = 386,
        Intel486 = 486,
        IntelPentium = 586,
        IntelIA64 = 2200,
        IntelAMDX8664 = 8664,
        Arm = 0
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
        public ProcessorType ProcessorType;
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

    [Flags]
    public enum AccessFlags : uint
    {
        None = 0,
        Read = 0x00000001,
        Write = 0x00000002,
        Create = 0x00000004,
        Execute = 0x00000008,
        Delete = 0x00000010,
        Attributes = 0x00000020,
        Permissions = 0x00000040,
        All = 0x00008000
    }

    [Flags]
    public enum AllocationType : uint
    {
        None = 0,
        Commit = 0x00001000,
        Reserve = 0x00002000,
        Reset = 0x00080000,
        Free = 0x000010000,
        ResetUndo = 0x1000000,
        LargePages = 0x20000000,
        Physical = 0x00400000,
        TopDown = 0x00100000,
        FourMbPages = 0x80000000
    }

    [Flags]
    public enum MemProtections : uint
    {
        NoAccess = 0x1,
        ReadOnly = 0x2,
        ReadWrite = 0x4,
        WriteCopy = 0x8,
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400,
        TargetsInvalid = 0x40000000,
    }

    [Flags]
    public enum MemType : uint
    {
        None = 0,
        Image = 0x1000000,
        Mapped = 0x40000,
        Private = 0x20000,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MemoryBasicInformation<TPointer>
    {
        public TPointer BaseAddress;
        public TPointer AllocationBase;
        public MemProtections AllocationProtect;
        public short PartitionId;
        public TPointer RegionSize;
        public AllocationType State;
        public AllocationType Protect;
        public MemType Type;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class MemoryBasicInformation : MemoryBasicInformation<IntPtr>
    {
    }

    public enum MemFreeType : uint
    {
        Decommit = 0x00004000,
        Release = 0x00008000,
        CoalescePlaceholder = 0x00000001,
        PreserverPlaceHolder = 0x00000002
    }

    [Flags]
    public enum CreationFlags : uint
    {
        None = 0,
        Suspended = 4,
        StackSizeParamIsAReservation = 0x00010000
    }

    [Flags]
    public enum ProcessAccessRights : uint
    {
        Terminate = 0x0001,
        CreateThread = 0x0002,
        VmOperation = 0x0008,
        VmRead = 0x0010,
        VmWrite = 0x0020,
        DuplicateHandle = 0x0040,
        CreateProcess = 0x0080,
        SetQuota = 0x0100,
        SetInformation = 0x0200,
        QueryInformation = 0x0400,
        SuspendResume = 0x0800,
        QueryLimitedInformation = 0x1000,
        Delete = 0x00010000,
        ReadControl = 0x00020000,
        WriteDac = 0x00040000,
        WriteOwner = 0x00080000,
        Synchronize = 0x00100000,
        StandardRightsRequired = 0x000F0000,
        AllAccess = StandardRightsRequired | Synchronize | 0xFFF
    }

    public enum ProcessInformationClass
    {
        ProcessMemoryPriority,
        ProcessMemoryExhaustionInfo,
        ProcessAppMemoryInfo,
        ProcessInPrivateInfo,
        ProcessPowerThrottling,
        ProcessReservedValue1,
        ProcessTelemetryCoverageInfo,
        ProcessProtectionLevelInfo,
        ProcessLeapSecondInfo,
        ProcessMachineTypeInfo,
        ProcessOverrideSubsequentPrefetchParameter,
        ProcessMaxOverridePrefetchParameter,
        ProcessInformationClassMax
    };
    
    public enum MemoryPriority : uint
    {
        VeryLow = 1,
        Low,
        Medium,
        BelowNormal,
        Normal
    }
      
    [StructLayout(LayoutKind.Sequential)]
    public class MemoryPriorityInformation : IClass<ProcessInformationClass>
    {
        public MemoryPriority MemoryPriority;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessMemoryPriority;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ProcessPowerThrottlingState : IClass<ProcessInformationClass>
    {
        public uint Version;
        public uint ControlMask;
        public uint StateMask;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessPowerThrottling;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ProcessProtectionLevelInformation : IClass<ProcessInformationClass>
    {
        public uint ProtectionLevel;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessProtectionLevelInfo;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ProcessLeapSecondInfo : IClass<ProcessInformationClass>
    {
        public uint Flags;
        public uint Reserved;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessLeapSecondInfo;
        }
    }

    [Flags]
    public enum DuplicateOptions : uint 
    { 
        CloseSource = 1,
        SameAccess = 2
    }
}
