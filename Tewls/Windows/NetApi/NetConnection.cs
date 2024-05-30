using System;
using System.Collections.Generic;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetConnection : NetBase
    {
        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null, string qualifier = null)
            where TStruct : class, IInfo<ConnectionLevel>, new()
        {
            return Enum<NetBuffer, TStruct>((i, b, r, t) => Netapi32.NetConnectionEnum(servername,
                qualifier, i.GetLevel(), ref b.Buffer, PrefMaxLength, ref r, ref t, IntPtr.Zero));
        }
    }
}