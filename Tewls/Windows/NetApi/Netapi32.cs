using System;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public static class Netapi32
    {
        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferAllocate(uint ByteCount, ref IntPtr Buffer);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferReallocate(IntPtr OldBuffer, uint NewByteCount, ref IntPtr NewBuffer);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferSize(IntPtr Buffer, ref uint ByteCount);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error NetApiBufferFree(IntPtr Buffer);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetConnectionEnum(string servername, string qualifier, ConnectionLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetRemoteComputerSupports(string UncServerName, Supports OptionsWanted, ref Supports OptionsSupported);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetRemoteTOD(string UncServerName, ref IntPtr BufferPtr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerEnum(string servername, InfoLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, uint servertype, string domain, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerGetInfo(string servername, InfoLevel level, ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerSetInfo(string servername, InfoLevel level, IntPtr buf, ref uint ParmError);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerDiskEnum(string servername, InfoLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportAdd(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportAddEx(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportDel(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerTransportEnum(string servername, TransportLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerComputerNameAdd(string ServerName, string EmulatedDomainName, string EmulatedServerName);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetServerComputerNameDel(string ServerName, string EmulatedServerName);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionEnum(string servername, string UncClientName, string username, SessionLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionGetInfo(string servername, string UncClientName, string username, SessionLevel level, ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetSessionDel(string servername, string UncClientName, string username);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareEnum(string servername, ShareLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareSetInfo(string servername, string netname, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareGetInfo(string servername, string netname, ShareLevel level, ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareAdd(string servername, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareDel(string servername, string netname, uint reserved);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetShareDelEx(string servername, ShareLevel level, IntPtr buf);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseAdd(string servername, UseLevel LevelFlags, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseDel(string UncServerName, string UseName, ForceLevel ForceLevelFlags);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseEnum(string UncServerName, UseLevel LevelFlags, ref IntPtr BufPtr, int PreferedMaximumSize, ref uint EntriesRead, ref uint TotalEntries, IntPtr ResumeHandle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUseGetInfo(string UncServerName, string UseName, UseLevel LevelFlags, ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Error NetUserEnum(string servername, UserLevel level, NetUserFilter filter, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);
    }
}
