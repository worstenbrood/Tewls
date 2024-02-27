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
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TargetName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        public FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CredPersist Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TargetAlias;
        [MarshalAs(UnmanagedType.LPWStr)]
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
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TargetName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetbiosServerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string DnsServerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetbiosDomainName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string DnsDomainName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string DnsTreeName;
        [MarshalAs(UnmanagedType.LPWStr)]
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
}
