using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetSession
    {
        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetSessionEnum(string servername, string UncClientName, string username, SessionLevel level, ref IntPtr bufptr, int prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resume_handle);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetSessionGetInfo(string servername, string UncClientName, string username, SessionLevel level,ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetSessionDel(string servername, string UncClientName, string username);

        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null, string uncClientName = null, string username = null)
            where TStruct : class, IInfo<SessionLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = NetSessionEnum(servername, uncClientName, username, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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
                var result = NetSessionGetInfo(servername, uncClientName, username, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public void Del(string servername, string uncClientName, string username = null)
        {
            var result = NetSessionDel(servername, uncClientName, username);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }
    }
}
