using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetGroup
    {
        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null)
            where TStruct : class, IInfo<GroupLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetGroupEnum(servername, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                foreach (var structure in buffer.EnumStructure<TStruct>(totalEntries))
                {
                    yield return structure;
                }
            }
        }

        public static TStruct GetInfo<TStruct>(string serverName, string groupName)
            where TStruct : class, IInfo<GroupLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = Netapi32.NetGroupGetInfo(serverName, groupName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public static void SetInfo<TStruct>(string serverName, string groupName, TStruct info)
           where TStruct : class, IInfo<GroupLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = Netapi32.NetGroupSetInfo(serverName, groupName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Add<TStruct>(string serverName, TStruct info)
           where TStruct : class, IInfo<GroupLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = Netapi32.NetGroupAdd(serverName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Del(string serverName, string groupName)
        {
            var result = Netapi32.NetGroupDel(serverName, groupName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void AddUser(string serverName, string groupName, string userName)
        {
            var result = Netapi32.NetGroupAddUser(serverName, groupName, userName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void DelUser(string serverName, string groupName, string userName)
        {
            var result = Netapi32.NetGroupDelUser(serverName, groupName, userName);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static IEnumerable<TStruct> GetUsers<TStruct>(string servername, string groupName)
            where TStruct : class, IInfo<GroupUsersLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetGroupGetUsers(servername, groupName, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                foreach (var structure in buffer.EnumStructure<TStruct>(totalEntries))
                {
                    yield return structure;
                }
            }
        }
    }
}
