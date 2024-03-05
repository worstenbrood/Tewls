using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetUse
    {
        public void Add<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<UseLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info)) 
            {
                uint paramIndex = 0;
                var result = Netapi32.NetUseAdd(serverName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public void Del(string serverName, string useName, ForceLevel forceLevel)
        {
            var result = Netapi32.NetUseDel(serverName, useName, forceLevel);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string serverName = null)
           where TStruct : class, IInfo<UseLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                uint entriesRead = 0;
                uint totalEntries = 0;

                var info = new TStruct();
                var result = Netapi32.NetUseEnum(serverName, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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

        public static TStruct GetInfo<TStruct>(string serverName, string useName)
            where TStruct : class, IInfo<UseLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = Netapi32.NetUseGetInfo(serverName, useName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }
    }
}
