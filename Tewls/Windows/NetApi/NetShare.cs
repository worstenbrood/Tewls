using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetShare
    {
        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareEnum(string servername, ShareLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string computerName)
            where TStruct : class, IInfo<ShareLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = NetShareEnum(computerName, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                foreach(var structure in buffer.EnumStructure<TStruct>(totalEntries))
                {
                    yield return structure;
                }
            }
        }
    }
}
