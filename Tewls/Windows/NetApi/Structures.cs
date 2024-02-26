using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

    public interface IInfo<T>
    {
        T GetLevel();
    }

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

    [StructLayout(LayoutKind.Sequential)]
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
        public uint Tinterval;
        public uint WeekDay;
    };
}
