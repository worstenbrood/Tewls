using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;
using Tewls.Windows.Utils;

namespace Tewls.Windows.NetApi
{
    public class NetBase
    {
        protected const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TBuffer, TStruct>(Func<IInfo<ConnectionLevel>, TBuffer, uint, uint, Error> func)
            where TBuffer : BufferBase, new()
            where TStruct : class, IInfo<ConnectionLevel>, new()
        {
            using (var buffer = new TBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = func(info, buffer, entriesRead, totalEntries);
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
