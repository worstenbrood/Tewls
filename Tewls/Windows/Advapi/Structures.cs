using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public enum CredType : uint
    {
        Generic = 1,
        DomainPassword,
        DomainCertificate,
        VisiblePassword,
        GenericCertificate,
        DomainExtended,
        Maximum,
        MaximumEx = Maximum + 1000,
    }

    [Flags]
    public enum CredEnumFlags : uint
    {
        AllCredentials = 1
    }

    [Flags]
    public enum CredFlags : uint
    {
        None = 0,
        PromptNow = 2,
        UsernameTarget = 4,
    }

    public enum CredPersist : uint
    {
        Session = 1,
        LocalMachine,
        Entreprise
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CredentialTargetInformation
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TargetName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string NetbiosServerName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DnsServerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetbiosDomainName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DnsDomainName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string DnsTreeName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string PackageName;
        public uint Flags;
        public uint CredTypeCount;
        public IntPtr CredTypes;
    };

    public enum TargetFlags: uint
    {
        None = 0,
        AllowNameResolution 
    }

    [Flags]
    public enum CredWriteFlags: uint
    {
        None = 0,
        PreserveCredentialBlob = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Luid
    {
        public int LowPart;
        public int HighPart;
    }

    [Flags]
    public enum PrivilegeAttributes : uint
    {
        Disabled = 0,
        EnabledByDefault = 1,
        Enabled = 2,
        Removed = 4,
        UseForAccess = 8
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LuidAndAttributes
    {
        public Luid Luid;
        public PrivilegeAttributes Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TokenPrivileges
    {
        public uint PrivilegeSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]   
        public LuidAndAttributes []Privileges;
    }

    [Flags]
    public enum TokenAccess : uint
    {
        AssignPrimary = 1,
        Duplicate = 2,
        Impersonate = 4,
        Query = 8,
        QuerySource = 16,
        AdjustPrivileges = 32,
        AdjustGroups = 64,
        AdjustDefault = 128,
        AdjustSessionId = 256,
        StandardRightsRequired = 0x000F0000,
        AccessAll = StandardRightsRequired | AssignPrimary | Duplicate | Impersonate | Query | QuerySource | AdjustPrivileges | AdjustGroups | AdjustDefault,
    }

    public enum LogonType : uint
    {
        Batch = 1,
        Interactive = 2,
        Network = 3,
        Service = 5,
        Unlock =  7,
        NetworkClearText = 8,
        NewCredentials = 9
    }

    public enum LogonProvider : uint
    {
        Default = 0,
        WinNT35,
        WinNT40,
        WinNT50,
        Virtual
    }

    public enum SecurityImpersonationLevel : uint
    {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    };

    public enum TokenType : uint
    {
        TokenPrimary = 1,
        TokenImpersonation
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SecurityAttributes
    {
        public uint Length;
        public IntPtr SecurityDescriptor;
        public bool InheritHandle;
    };

    [Flags]
    public enum MemAllocations : uint
    {
        None = 0,
        Commit = 0x00001000,
        Reserve = 0x00002000,
        Reset = 0x00080000,
        Free = 0x000010000,
        ResetUndo = 0x1000000,
        LargePages = 0x20000000,
        Physical = 0x00400000,
        TopDown = 0x00100000
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
    public enum MemType: uint
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
        public MemAllocations State;
        public MemAllocations Protect;
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
}
