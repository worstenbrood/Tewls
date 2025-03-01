﻿using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Kernel.Nt
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UnicodeString
    {
        public ushort Length;
        public ushort MaximumLength;
        public IntPtr Buffer;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct UnicodeString32
    {
        public ushort Length;
        public ushort MaximumLength;
        public uint Buffer;
    };

    [Flags]
    public enum ObjectFlags : uint
    {
        Inherit = 0x00000002,
        Permanent = 0x00000010,
        Exclusive = 0x00000020,
        CaseInsensitive = 0x00000040,
        OpenIf = 0x00000080,
        OpenLink = 0x00000100,
        ValidAttributes = 0x000001F2
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ObjectAttributes
    {
        public uint Length;
        public IntPtr RootDirectory;
        public IntPtr ObjectName;
        public ObjectFlags Attributes;
        public IntPtr SecurityDescriptor;
        public IntPtr SecurityQualityOfService;

        public ObjectAttributes()
        {
            Length = (uint) Marshal.SizeOf(typeof(ObjectAttributes));
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

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ListEntry
    {
        public IntPtr Flink;
        public IntPtr Blink;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct ListEntry32
    {
        public uint Flink;
        public uint Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PebLdrData
    {
        public uint Length;
        public bool Initialized;
        public IntPtr SsHandle;
        public ListEntry InLoadOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InLoadOrderModuleList
        public ListEntry InMemoryOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InMemoryOrderModuleList
        public ListEntry InInitializationOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InInitializationOrderModuleList
        public IntPtr DllBase;
        public IntPtr EntryPoint;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class PebLdrData32
    {
        public uint Length;
        public bool Initialized;
        public uint SsHandle;
        public ListEntry32 InLoadOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InLoadOrderModuleList
        public ListEntry32 InMemoryOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InMemoryOrderModuleList
        public ListEntry32 InInitializationOrderModuleList; // ref. to PLDR_DATA_TABLE_ENTRY->InInitializationOrderModuleList
        public uint DllBase;
        public uint EntryPoint;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class Peb
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte []Reserved1;
        public byte BeingDebugged;
        public byte Reserved2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public IntPtr []Reserved3;
        public IntPtr Ldr;
        public IntPtr ProcessParameters;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public IntPtr []Reserved4;
        public IntPtr AtlThunkSListPtr;
        public IntPtr Reserved5;
        public uint Reserved6;
        public IntPtr Reserved7;
        public uint Reserved8;
        public uint AtlThunkSListPtr32;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public IntPtr []Reserved9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        public byte []Reserved10;
        public IntPtr PostProcessInitRoutine;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte []Reserved11;
        public IntPtr Reserved12;
        public uint SessionId;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class Peb32
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Reserved1;
        public byte BeingDebugged;
        public byte Reserved2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] Reserved3;
        public uint Ldr;
        public uint ProcessParameters;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] Reserved4;
        public uint AtlThunkSListPtr;
        public uint Reserved5;
        public uint Reserved6;
        public uint Reserved7;
        public uint Reserved8;
        public uint AtlThunkSListPtr32;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public uint[] Reserved9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        public byte[] Reserved10;
        public uint PostProcessInitRoutine;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] Reserved11;
        public uint Reserved12;
        public uint SessionId;
    };  

    [StructLayout(LayoutKind.Sequential)]
    public class LdrModule
    {
        public ListEntry InLoadOrderModuleList;
        public ListEntry InMemoryOrderModuleList;
        public ListEntry InInitializationOrderModuleList;
        public IntPtr BaseAddress;
        public IntPtr EntryPoint;
        public uint SizeOfImage;
        public UnicodeString FullDllName;
        public UnicodeString BaseDllName;
        public uint Flags;
        public ushort LoadCount;
        public ushort TlsIndex;
        public ListEntry HashTableEntry;
        public IntPtr TimeDateStamp;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class LdrModule32
    {
        public ListEntry32 InLoadOrderModuleList;
        public ListEntry32 InMemoryOrderModuleList;
        public ListEntry32 InInitializationOrderModuleList;
        public uint BaseAddress;
        public uint EntryPoint;
        public uint SizeOfImage;
        public UnicodeString32 FullDllName;
        public UnicodeString32 BaseDllName;
        public uint Flags;
        public ushort LoadCount;
        public ushort TlsIndex;
        public ListEntry32 HashTableEntry;
        public uint TimeDateStamp;
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
       
    [StructLayout(LayoutKind.Sequential)]
    public class ProcessBasicInformation : IClass<ProcessInformationClass>
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

    [StructLayout(LayoutKind.Sequential)]
    public class ProcessWow64Information : IClass<ProcessInformationClass>
    {
        public IntPtr PebBaseAddress;

        public ProcessInformationClass GetClass()
        {
            return ProcessInformationClass.ProcessWow64Information;
        }
    }

    public enum SeGroup : uint
    {
        Mandatory = 0x00000001,
        EnabledByDefault = 0x00000002,
        Enabled = 0x00000004,
        Owner = 0x00000008,
        UseForDenyOnly = 0x00000010,
        Integrity = 0x00000020,
        IntegrityEnabled = 0x00000040,
        LogonId = 0xC0000000,
        Resource = 0x20000000,
    }
}
