using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetServer
    {
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerEnum(string servername, InfoLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, uint servertype, string domain, IntPtr resume_handle);

        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerGetInfo(string servername, InfoLevel level, ref IntPtr bufptr);

        [DllImport("netapi32.dll", EntryPoint = "NetServerSetInfo", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerSetInfo(string servername, InfoLevel level, IntPtr buf,ref uint ParmError);

        [DllImport("netapi32.dll", EntryPoint = "NetServerDiskEnum", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerDiskEnum(string servername,InfoLevel level,ref IntPtr bufptr, int prefmaxlen,ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", EntryPoint = "NetServerTransportAdd", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerTransportAdd(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", EntryPoint = "NetServerTransportAddEx", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerTransportAddEx(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", EntryPoint = "NetServerTransportDel", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerTransportDel(string servername, TransportLevel level, IntPtr bufptr);

        [DllImport("netapi32.dll", EntryPoint = "NetServerTransportEnum", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerTransportEnum(string servername,TransportLevel level, ref IntPtr bufptr, int prefmaxlen,ref uint entriesread,ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", EntryPoint = "NetServerComputerNameAdd", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerComputerNameAdd(string ServerName, string EmulatedDomainName,string EmulatedServerName);

        [DllImport("netapi32.dll", EntryPoint = "NetServerComputerNameDel", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetServerComputerNameDel(string ServerName,string EmulatedServerName);

        private const int EntrySize = 3;

        public static string[] GetDiskEnum(string serverName = null)
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = NetServerDiskEnum(serverName, 0, ref buffer.Buffer, -1, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                var drives = new string[entriesRead];
                for (var i = 0; i < entriesRead; i++)
                {
                    var charArray = new char[EntrySize];
                    Marshal.Copy(buffer.Buffer, charArray, 0, EntrySize);
                    drives[i] = new string(charArray, 0, 2);
                }

                return drives;
            }
        }

        public static void ComputerNameAdd(string serverName, string emulatedDomainName, string emulatedServerName)
        {
            var result = NetServerComputerNameAdd(serverName, emulatedDomainName, emulatedServerName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void ComputerNameDel(string serverName, string emulatedServerName)
        {
            var result = NetServerComputerNameDel(serverName, emulatedServerName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static IEnumerable<T> ServerEnum<T>(ServerType type, string domainName = null)
             where T : class, IInfo<InfoLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var info = new T();
                var result = NetServerEnum(null, info.GetLevel(), ref buffer.Buffer, -1, ref entriesRead, ref totalEntries, (uint) type, domainName, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int) result);
                }

                foreach (var entry in buffer.EnumStructure(entriesRead, info))
                {
                    yield return entry;
                }
            }
        }

        public static T GetInfo<T>(string serverName = null)
            where T: class, IInfo<InfoLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new T();
                var result = NetServerGetInfo(serverName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int) result);
                }

                Marshal.PtrToStructure(buffer.Buffer, info);
                return info;
            }
        }

        public static void SetInfo<T>(string serverName, T info)
            where T :class, IInfo<InfoLevel>, new()
        {
            using (var buffer = new NetBuffer((uint)Marshal.SizeOf(info)))
            {
                uint paramIndex = 0;

                Marshal.StructureToPtr(info, buffer, false);

                var result = NetServerSetInfo(serverName, info.GetLevel(), buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }       

        public static void TransportAdd<T>(string serverName, T info)
           where T : class, IInfo<TransportLevel>, new()
        {
            using (var buffer = new NetBuffer((uint) Marshal.SizeOf(info)))
            {
                Marshal.StructureToPtr(info, buffer, false);

                var result = NetServerTransportAdd(serverName, info.GetLevel(), buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void TransportAddEx<T>(string serverName, T info)
           where T : class, IInfo<TransportLevel>, new()
        {
            using (var buffer = new NetBuffer((uint)Marshal.SizeOf(info)))
            {
                Marshal.StructureToPtr(info, buffer, false);

                var result = NetServerTransportAddEx(serverName, info.GetLevel(), buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void TransportDel<T>(string serverName, T info)
            where T : class, IInfo<TransportLevel>, new ()
        {
            using (var buffer = new NetBuffer((uint)Marshal.SizeOf(info)))
            {
                Marshal.StructureToPtr(info, buffer, false);

                var result = NetServerTransportDel(serverName, info.GetLevel(), buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static IEnumerable<T> TransportEnum<T>(string serverName = null)
            where T : class, IInfo<TransportLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var info = new T();
                var result = NetServerTransportEnum(serverName, info.GetLevel(), ref buffer.Buffer, -1, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                foreach (var entry in buffer.EnumStructure(entriesRead, info))
                {
                    yield return entry;
                }
            }
        }
    }
}
