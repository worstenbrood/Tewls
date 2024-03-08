using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.NetApi.Structures
{
    public enum PlatformId : uint
    {
        Dos = 300,
        Os2 = 400,
        Nt = 500,
        Osf = 600,
        Vms = 700
    }

    public enum InfoLevel : uint
    {
        Info100 = 100,
        Info101 = 101,
        Info102 = 102
    };  

    [StructLayout(LayoutKind.Sequential)]
    public class ServerInfo100 : IInfo<InfoLevel>
    {
        public PlatformId PlatformId;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        public InfoLevel GetLevel()
        {
            return InfoLevel.Info100;
        }
    };

    [Flags]
    public enum ServerType : uint
    {
        Workstation = 0x1,
        Server = 0x2,
        SQLServer = 0x4,
        DomainController = 0x8,
        DomainBackupController = 0x10,
        TimeSource = 0x20,
        Afp = 0x40,
        Novell = 0x80,
        DomainMember = 0x100,
        PrintQueueServer = 0x200,
        DialinServer = 0x400,
        XenixServer = 0x800,
        Nt = 0x1000,
        Wfw = 0x2000,
        ServerMfpn = 0x4000,
        ServerNt = 0x8000,
        PotentialBrowser = 0x10000,
        BackupBrowser = 0x20000,
        MasterBrowser = 0x40000,
        DomainMaster = 0x80000,
        ServerOsf = 0x100000,
        ServerVms = 0x200000,
        Windows = 0x400000,
        Dfs = 0x800000,
        ClusterNt = 0x1000000,
        TerminalServer = 0x2000000,
        ClusterVirtualServerNt = 0x4000000,
        Dce = 0x10000000,
        AlternateXport = 0x20000000,
        LocalListOnly = 0x40000000,
        DomainEnum = 0x80000000,
        All = 0xFFFFFFFF
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ServerInfo101 : IInfo<InfoLevel>
    {
        public PlatformId PlatformId;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        public uint VersionMajor;
        public uint VersionMinor;
        public ServerType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;

        public InfoLevel GetLevel()
        {
            return InfoLevel.Info101;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ServerInfo102 : IInfo<InfoLevel>
    {
        public PlatformId PlatformId;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        public uint VersionMajor;
        public uint VersionMinor;
        public ServerType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        public uint Users;
        public long Disc;
        public bool Hidden;
        public uint Announce;
        public uint Anndelta;
        public uint Licenses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Userpath;

        public InfoLevel GetLevel()
        {
            return InfoLevel.Info102;
        }
    };

    public enum TransportLevel: uint
    {
        Transport0 = 0,
        Transport1,
        Transport2,
        Transport3,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ServerTransportInfo0 : IInfo<TransportLevel>
    {
        public uint NumberOfVcs;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TransportName;
        public IntPtr TransportAddress;
        public uint TransportAddressLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetworkAddress;

        public TransportLevel GetLevel()
        {
            return TransportLevel.Transport0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ServerTransportInfo1 : IInfo<TransportLevel>
    {
        public uint NumberOfVcs;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TransportName;
        public IntPtr TransportAddress;
        public uint TransportAddressLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetworkAddress;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;

        public TransportLevel GetLevel()
        {
            return TransportLevel.Transport1;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ServerTransportInfo2 : IInfo<TransportLevel>
    {
        public uint NumberOfVcs;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TransportName;
        public IntPtr TransportAddress;
        public uint TransportAddressLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetworkAddress;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;
        public ulong Flags;

        public TransportLevel GetLevel()
        {
            return TransportLevel.Transport2;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ServerTransportInfo3 : IInfo<TransportLevel>
    {
        public uint NumberOfVcs;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string TransportName;
        public IntPtr TransportAddress;
        public uint TransportAddressLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetworkAddress;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domain;
        public ulong Flags;
        public uint PasswordLength;
        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 128)]
        public string Password;

        public TransportLevel GetLevel()
        {
            return TransportLevel.Transport3;
        }
    };

    public enum UseLevel : uint
    {
        Use0 = 0,
        Use1,
        Use2,
    }

    public enum UseStatus : uint
    {
        Ok,
        Paused,
        SessionLost,
        Disconnected,
        NetworkError,
        Connecting,
        ReConnecting
    }

    public enum AsgType : uint
    {
        Wildcard,
        DiskDevice,
        SpoolDevice,
        Ipc
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UseInfo0 : IInfo<UseLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Local;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remote;

        public UseLevel GetLevel()
        {
            return UseLevel.Use0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UseInfo1 : IInfo<UseLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Local;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remote;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public UseStatus Status;
        public AsgType AsgType;
        public uint RefCount;
        public uint UseCount;

        public UseLevel GetLevel()
        {
            return UseLevel.Use1;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UseInfo2 : IInfo<UseLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Local;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remote;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public UseStatus Status;
        public AsgType AsgType;
        public uint RefCount;
        public uint UseCount;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Username;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Domainname;

        public UseLevel GetLevel()
        {
            return UseLevel.Use2;
        }
    }

    public enum ForceLevel : uint
    { 
        NoForce = 0,
        Force,
        LotsOfForce
    }

    [Flags]
    public enum Supports : uint
    {
        RemoteAdminProtocol = 2,
        Rpc = 4,
        SamProtocol = 8,
        Unicode = 16,
        Local = 32,
        Any = uint.MaxValue,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class TimeOfDayInfo
    {
        public uint ElapsedT;
        public uint Msecs;
        public uint Hours;
        public uint Mins;
        public uint Secs;
        public uint Hunds;
        public long Timezone;
        public uint Day;
        public uint Month;
        public uint Year;
        public DayOfWeek WeekDay;
        public uint Tinterval;
    };

    public enum ShareLevel
    {
        Share0 = 0,
        Share1 = 1,
        Share2 = 2,
        Share502 = 502,
        Share503 = 503
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ShareInfo0 : IInfo<ShareLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;

        public ShareLevel GetLevel()
        {
            return ShareLevel.Share0;
        }
    };

    public enum ShareType : uint
    {
        DiskTree = 0,
        PrintQueue,
        Device,
        Ipc,
        Temporary = 0x40000000,
        Special = 0x80000000
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ShareInfo1 : IInfo<ShareLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;
        public ShareType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;

        public ShareLevel GetLevel()
        {
            return ShareLevel.Share1;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ShareInfo2 : IInfo<ShareLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;
        public ShareType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        public AccessFlags Permissions;
        public uint MaxUses;
        public uint CurrentUses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;

        public ShareLevel GetLevel()
        {
            return ShareLevel.Share2;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ShareInfo502 : IInfo<ShareLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;
        public ShareType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        public AccessFlags Permissions;
        public uint MaxUses;
        public uint CurrentUses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public uint Reserved;
        public IntPtr SecurityDescriptor;

        public ShareLevel GetLevel()
        {
            return ShareLevel.Share502;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ShareInfo503 : IInfo<ShareLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;
        public ShareType Type;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        public AccessFlags Permissions;
        public uint MaxUses;
        public uint CurrentUses;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Servername;
        public uint Reserved;
        public IntPtr SecurityDescriptor;

        public ShareLevel GetLevel()
        {
            return ShareLevel.Share503;
        }
    };

    public enum ConnectionLevel
    {
        Connection0 = 0,
        Connection1 = 1,
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ConnectionInfo0 : IInfo<ConnectionLevel>
    {
        public uint Id;

        public ConnectionLevel GetLevel()
        {
            return ConnectionLevel.Connection0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class ConnectionInfo1 : IInfo<ConnectionLevel>
    {
        public uint Id;
        public ShareType Type;
        public uint NumOpens;
        public uint NumUsers;
        public uint Time;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Username;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Netname;

        public ConnectionLevel GetLevel()
        {
            return ConnectionLevel.Connection1;
        }
    }

    public enum SessionLevel : uint
    {
        Session0 = 0,
        Session1 = 1,
        Session2 = 2,
    }

    [Flags]
    public enum SessionFlags : uint
    {
        Guest = 1,
        NoEncryption = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SessionInfo0 : IInfo<SessionLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CName;

        public SessionLevel GetLevel()
        {
            return SessionLevel.Session0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SessionInfo1 : IInfo<SessionLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Username;
        public uint NumOpens;
        public uint Time;
        public uint IdleTime;
        public SessionFlags UserFlags;
      
        public SessionLevel GetLevel()
        {
            return SessionLevel.Session1;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SessionInfo2 : IInfo<SessionLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Username;
        public uint NumOpens;
        public uint Time;
        public uint IdleTime;
        public SessionFlags UserFlags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string CLTypeName;

        public SessionLevel GetLevel()
        {
            return SessionLevel.Session2;
        }
    };

    public enum UserLevel : uint
    {
        User0,
        User1,
        User2,
        User3,
        User10 = 10,
    };

    [Flags]
    public enum NetUserFilter : uint
    {
        TempDuplicateAccount = 1,
        NormalAccount = 2,
        InterDomainTrustAccount = 8,
        WorkstationTrustAccount = 16,
        ServerTrustAccount = 32
    }

    public class UserInfo0 : IInfo<UserLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;

        public UserLevel GetLevel()
        {
            return UserLevel.User0;
        }
    };

    public enum UserPrivilege : uint
    {
        Guest = 0,
        User = 1,
        Admin = 2
    }

    [Flags]
    public enum UserFlags : uint
    {
        Script = 0x0001,
        Accountdisable = 0x0002,
        HomedirRequired = 0x0008,
        Lockout = 0x0010,
        PasswdNotreqd = 0x0020,
        PasswdCantChange = 0x0040,
        EncryptedTextPasswordAllowed = 0x0080,
        TempDuplicateAccount = 0x0100,
        NormalAccount = 0x0200,
        InterdomainTrustAccount = 0x0800,
        WorkstationTrustAccount = 0x1000,
        ServerTrustAccount = 0x2000,
        DontExpirePasswd = 0x10000,
        MnsLogonAccount = 0x20000,
        SmartcardRequired = 0x40000,
        TrustedForDelegation = 0x80000,
        NotDelegated = 0x100000,
        UseDesKeyOnly = 0x200000,
        DontRequirePreauth = 0x400000,
        PasswordExpired = 0x800000,
        TrustedToAuthenticateForDelegation = 0x1000000,
        NoAuthDataRequired = 0x2000000,
        MachineAccountMask = InterdomainTrustAccount | WorkstationTrustAccount | ServerTrustAccount,
        AccountTypeMask = TempDuplicateAccount | NormalAccount | InterdomainTrustAccount | WorkstationTrustAccount | ServerTrustAccount
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfo1 : IInfo<UserLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public uint PasswordAge;
        public UserPrivilege Privilege;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDirectory;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        public UserFlags Flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ScriptPath;

        public UserLevel GetLevel()
        {
            return UserLevel.User1;
        }
    }

    [Flags]
    public enum UserOperationFlags : uint
    {
        Print = 1,
        Comm = 2,
        Server = 4,
        Accounts = 8
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfo2 : IInfo<UserLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public uint PasswordAge;
        public UserPrivilege Privilege;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDirectory;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        public UserFlags Flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ScriptPath;
        public UserOperationFlags AuthFlags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Fullname;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string UserComment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Parameters;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Workstations;
        public uint LastLogon;
        public uint LastLogoff;
        public uint AccountExpires;
        public uint MaxStorage;
        public uint UnitsPerWeek;
        public IntPtr LogonHours;
        public uint BadPasswordCount;
        public uint NumberLogons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LogonServer;
        public uint CountryCode;
        public uint CodePage;

        public UserLevel GetLevel()
        {
            return UserLevel.User2;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfo3 : IInfo<UserLevel>
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
        public uint PasswordAge;
        public UserPrivilege Privilege;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDirectory;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        public UserFlags Flags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string ScriptPath;
        public UserOperationFlags AuthFlags;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Fullname;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string UserComment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Parameters;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Workstations;
        public uint LastLogon;
        public uint LastLogoff;
        public uint AccountExpires;
        public uint MaxStorage;
        public uint UnitsPerWeek;
        public IntPtr LogonHours;
        public uint BadPasswordCount;
        public uint NumberLogons;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string LogonServer;
        public uint CountryCode;
        public uint CodePage;
        public uint UserId;
        public uint PrrimaryGroupId;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Profile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string HomeDirDrive;
        public uint PasswordExpired;

        public UserLevel GetLevel()
        {
            return UserLevel.User3;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfo10 : IInfo<UserLevel>
    {

        [MarshalAs(UnmanagedType.LPWStr)]
        public string Name;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Comment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string UserComment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Fullname;

        public UserLevel GetLevel()
        {
            return UserLevel.User10;
        }
    }

    public enum GroupLevel : uint
    {
        Group0
    }

    public enum GroupUsersLevel : uint
    {
        GroupUsers0 = 0,
    }
}
