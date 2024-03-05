using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetSession
    {
        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null, string uncClientName = null, string username = null)
            where TStruct : class, IInfo<SessionLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetSessionEnum(servername, uncClientName, username, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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
        public static TStruct GetInfo<TStruct>(string servername = null, string uncClientName = null, string username = null)
          where TStruct : class, IInfo<SessionLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = Netapi32.NetSessionGetInfo(servername, uncClientName, username, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public void Del(string servername, string uncClientName, string username = null)
        {
            var result = Netapi32.NetSessionDel(servername, uncClientName, username);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }
    }
}
