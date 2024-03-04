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

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareSetInfo(string servername, string netname, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareGetInfo(string servername, string netname, ShareLevel level, ref IntPtr bufptr);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareAdd(string servername, ShareLevel level, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareDel(string servername, string netname, uint reserved);

        [DllImport("netapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetShareDelEx(string servername, ShareLevel level, IntPtr buf);

        private const int PrefMaxLength = -1;

        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null)
            where TStruct : class, IInfo<ShareLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = NetShareEnum(servername, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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

        public static TStruct GetInfo<TStruct>(string serverName, string netName)
           where TStruct : class, IInfo<ShareLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = NetShareGetInfo(serverName, netName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public static void SetInfo<TStruct>(string serverName, string netName, TStruct info)
            where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = NetShareSetInfo(serverName, netName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Add<TStruct>(string serverName, TStruct info)
           where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = NetShareAdd(serverName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Del(string serverName, string netName)
        {
            var result = NetShareDel(serverName, netName, 0);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void Del<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                var result = NetShareDelEx(serverName, info.GetLevel(), buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }
    }
}
