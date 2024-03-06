using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Kernel.Nt
{
    [StructLayout(LayoutKind.Sequential)]
    public class UnicodeString
    {
        public ushort Length;
        public ushort MaximumLength;
        public IntPtr Buffer;

        public override string ToString()
        {
            return Marshal.PtrToStringUni(Buffer, Length);
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ObjectAttributes
    {
        public IntPtr Length;
        public IntPtr RootDirectory;
        public UnicodeString ObjectName;
        public IntPtr Attributes;
        public IntPtr SecurityDescriptor;
        public IntPtr SecurityQualityOfService;

        public ObjectAttributes()
        {
            Length = (IntPtr) Marshal.SizeOf(typeof(ObjectAttributes));
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ClientId
    {
        public IntPtr UniqueProcess;
        public IntPtr UniqueThread;

        public override string ToString()
        {
            return $"Process: {UniqueProcess} - Thread: {UniqueThread}";
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ListEntry
    {
        public ListEntry Flink;
        public ListEntry Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class LDR_DATA_TABLE_ENTRY
    {
        public ListEntry InLoadOrderModuleList;
        public ListEntry InMemoryOrderModuleList;
        public ListEntry InInitializationOrderModuleList;
        IntPtr DllBase;
        IntPtr EntryPoint;
        ulong SizeOfImage;  // in bytes
        UnicodeString FullDllName;
        UnicodeString BaseDllName;
        public ulong Flags;            // LDR_*
        public ushort LoadCount;
        public ushort TlsIndex;
        public ListEntry HashLinks;
        public IntPtr SectionPointer;
        public ulong CheckSum;
        public ulong TimeDateStamp;
        //    PVOID			LoadedImports;					// seems they are exist only on XP !!!
        //    PVOID			EntryPointActivationContext;	// -same-
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PebLdrData
    {
        public ulong Length;
        public bool Initialized;
        public IntPtr SsHandle;
        public ListEntry InLoadOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InLoadOrderModuleList
        public ListEntry InMemoryOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InMemoryOrderModuleList
        public ListEntry InInitializationOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InInitializationOrderModuleList
    };
        
    public enum ProcessInformationClass
    {
        ProcessBasicInformation = 0,
        ProcessQuotaLimits = 1,
        ProcessIoCounters = 2,
        ProcessVmCounters = 3,
        ProcessTimes = 4,
        ProcessBasePriority = 5,
        ProcessRaisePriority = 6,
        ProcessDebugPort = 7,
        ProcessExceptionPort = 8,
        ProcessAccessToken = 9,
        ProcessLdtInformation = 10,
        ProcessLdtSize = 11,
        ProcessDefaultHardErrorMode = 12,
        ProcessIoPortHandlers = 13,
        ProcessPooledUsageAndLimits = 14,
        ProcessWorkingSetWatch = 15,
        ProcessUserModeIOPL = 16,
        ProcessEnableAlignmentFaultFixup = 17,
        ProcessPriorityClass = 18,
        ProcessWx86Information = 19,
        ProcessHandleCount = 20,
        ProcessAffinityMask = 21,
        ProcessPriorityBoost = 22,
        ProcessDeviceMap = 23,
        ProcessSessionInformation = 24,
        ProcessForegroundInformation = 25,
        ProcessWow64Information = 26,
        ProcessImageFileName = 27,
        ProcessLUIDDeviceMapsEnabled = 28,
        ProcessBreakOnTermination = 29,
        ProcessDebugObjectHandle = 30,
        ProcessDebugFlags = 31,
        ProcessHandleTracing = 32,
        ProcessIoPriority = 33,
        ProcessExecuteFlags = 34,
        ProcessTlsInformation = 35,
        ProcessCookie = 36,
        ProcessImageInformation = 37,
        ProcessCycleTime = 38,
        ProcessPagePriority = 39,
        ProcessInstrumentationCallback = 40,
        ProcessThreadStackAllocation = 41,
        ProcessWorkingSetWatchEx = 42,
        ProcessImageFileNameWin32 = 43,
        ProcessImageFileMapping = 44,
        ProcessAffinityUpdateMode = 45,
        ProcessMemoryAllocationMode = 46,
        ProcessGroupInformation = 47,
        ProcessTokenVirtualizationEnabled = 48,
        ProcessOwnerInformation = 49,
        ProcessWindowInformation = 50,
        ProcessHandleInformation = 51,
        ProcessMitigationPolicy = 52,
        ProcessDynamicFunctionTableInformation = 53,
        ProcessHandleCheckingMode = 54,
        ProcessKeepAliveCount = 55,
        ProcessRevokeFileHandles = 56,
        ProcessWorkingSetControl = 57,
        ProcessHandleTable = 58,
        ProcessCheckStackExtentsMode = 59,
        ProcessCommandLineInformation = 60,
        ProcessProtectionInformation = 61,
        ProcessMemoryExhaustion = 62,
        ProcessFaultInformation = 63,
        ProcessTelemetryIdInformation = 64,
        ProcessCommitReleaseInformation = 65,
        ProcessReserved1Information = 66,
        ProcessReserved2Information = 67,
        ProcessSubsystemProcess = 68,
        ProcessInPrivate = 70,
        ProcessRaiseUMExceptionOnInvalidHandleClose = 71,
        ProcessSubsystemInformation = 75,
        ProcessWin32kSyscallFilterInformation = 79,
        ProcessEnergyTrackingState = 82,
        MaxProcessInfoClass,
    }

    public interface IProcessQueryInfoClass
    {
        ProcessInformationClass GetClass();
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ProcessBasicInformation : IProcessQueryInfoClass
    {
        public IntPtr ExitStatus;
        public IntPtr PebBaseAddress;
        public IntPtr AffinityMask;
        public IntPtr BasePriority;
        public IntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessBasicInformation;
        }
    };
}
