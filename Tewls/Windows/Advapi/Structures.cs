using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

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
    public class Credential
    {
        public CredFlags Flags;
        public CredType Type;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TargetName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Comment;
        public FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CredPersist Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TargetAlias;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string UserName;

        public string GetPassword()
        {
            if (CredentialBlob == IntPtr.Zero)
            {
                return null;
            }

            switch(Type)
            {
                default:
                    return Marshal.PtrToStringUni(CredentialBlob, (int) CredentialBlobSize / sizeof(char));
            }
        }
    };

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
    }
}
