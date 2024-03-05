using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetServer
    {
        private const int EntrySize = 3;
        private const int PrefMaxLength = -1;

        public static string[] GetDiskEnum(string serverName = null)
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetServerDiskEnum(serverName, 0, ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                var drives = new string[entriesRead];
                for (var i = 0; i < entriesRead; i++)
                {
                    var charArray = new char[EntrySize];
                    Marshal.Copy(buffer.Buffer + i * EntrySize, charArray, 0, EntrySize);
                    drives[i] = new string(charArray, 0, 2);
                }

                return drives;
            }
        }

        public static void ComputerNameAdd(string serverName, string emulatedDomainName, string emulatedServerName)
        {
            var result = Netapi32.NetServerComputerNameAdd(serverName, emulatedDomainName, emulatedServerName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void ComputerNameDel(string serverName, string emulatedServerName)
        {
            var result = Netapi32.NetServerComputerNameDel(serverName, emulatedServerName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static IEnumerable<TStruct> ServerEnum<TStruct>(ServerType type, string domainName = null)
             where TStruct : class, IInfo<InfoLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var info = new TStruct();
                var result = Netapi32.NetServerEnum(null, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, (uint) type, domainName, IntPtr.Zero);
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

        public static TStruct GetInfo<TStruct>(string serverName = null)
            where TStruct : class, IInfo<InfoLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = Netapi32.NetServerGetInfo(serverName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int) result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public static void SetInfo<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<InfoLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = Netapi32.NetServerSetInfo(serverName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }       

        public static void TransportAdd<TStruct>(string serverName, TStruct info)
           where TStruct : class, IInfo<TransportLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                var result = Netapi32.NetServerTransportAdd(serverName, info.GetLevel(), buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void TransportAddEx<TStruct>(string serverName, TStruct info)
           where TStruct : class, IInfo<TransportLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                var result = Netapi32.NetServerTransportAddEx(serverName, info.GetLevel(), buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void TransportDel<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<TransportLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                var result = Netapi32.NetServerTransportDel(serverName, info.GetLevel(), buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static IEnumerable<TStruct> TransportEnum<TStruct>(string serverName = null)
            where TStruct : class, IInfo<TransportLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var info = new TStruct();
                var result = Netapi32.NetServerTransportEnum(serverName, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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
