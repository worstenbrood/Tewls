using System;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public static class Netapi32
    {
        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferAllocate(uint ByteCount, ref IntPtr Buffer);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferReallocate(IntPtr OldBuffer, uint NewByteCount, ref IntPtr NewBuffer);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferSize(IntPtr Buffer, ref uint ByteCount);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferFree(IntPtr Buffer);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetConnectionEnum(string servername, string qualifier, ConnectionLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetRemoteComputerSupports(string UncServerName, Supports OptionsWanted, ref Supports OptionsSupported);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetRemoteTOD(string UncServerName, ref IntPtr BufferPtr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerEnum(string servername, InfoLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, uint servertype, string domain, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerGetInfo(string servername, InfoLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerSetInfo(string servername, InfoLevel level, IntPtr buf, ref uint ParmError);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerDiskEnum(string servername, InfoLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportAdd(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportAddEx(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportDel(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportEnum(string servername, TransportLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerComputerNameAdd(string ServerName, string EmulatedDomainName, string EmulatedServerName);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerComputerNameDel(string ServerName, string EmulatedServerName);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionEnum(string servername, string UncClientName, string username, SessionLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionGetInfo(string servername, string UncClientName, string username, SessionLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionDel(string servername, string UncClientName, string username);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareEnum(string servername, ShareLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareSetInfo(string servername, string netname, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareGetInfo(string servername, string netname, ShareLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareAdd(string servername, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareDel(string servername, string netname, uint reserved);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareDelEx(string servername, ShareLevel level, IntPtr buf);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseAdd(string servername, UseLevel LevelFlags, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseDel(string UncServerName, string UseName, ForceLevel ForceLevelFlags);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseEnum(string UncServerName, UseLevel LevelFlags, ref IntPtr BufPtr, int PreferedMaximumSize, ref uint EntriesRead, ref uint TotalEntries, IntPtr ResumeHandle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseGetInfo(string UncServerName, string UseName, UseLevel LevelFlags, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserEnum(string servername, UserLevel level, NetUserFilter filter, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserGetInfo(string servername, string username, UserLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserSetInfo(string servername, string username, UserLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserDel(string servername, string username);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserAdd(string servername, UserLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupEnum(string servername, GroupLevel level, ref IntPtr bufptr, int prefmaxlen,ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupGetInfo(string servername, string groupname, GroupLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupSetInfo(string servername, string groupname, GroupLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupAdd(string servername, GroupLevel level, IntPtr buf,ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupDel(string servername, string groupname);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupAddUser(string servername, string GroupName, string Username);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupDelUser(string servername, string GroupName, string Username);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetGroupGetUsers(string servername, string groupname, GroupUsersLevel level, ref IntPtr bufptr, int prefmaxlen,ref uint entriesread, ref uint totalentries, IntPtr ResumeHandle);
        
        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetLocalGroupEnum(string servername, LocalGroupLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumehandle);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetLocalGroupGetInfo(string servername, string groupname, LocalGroupLevel level, ref IntPtr bufptr);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetLocalGroupSetInfo(string servername, string groupname, LocalGroupLevel level, IntPtr buf, ref uint parm_err);

        [DllImport(nameof(Netapi32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetLocalGroupGetMembers(string servername, string localgroupname, MemberLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumehandle);
    }
}
