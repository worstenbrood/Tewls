using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetLocalGroup : NetBase
    {
        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null)
            where TStruct : class, IInfo<LocalGroupLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetLocalGroupEnum(servername, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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
