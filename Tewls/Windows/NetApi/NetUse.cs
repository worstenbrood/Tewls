using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetUse
    {
        [DllImport("netapi32.dll", EntryPoint = "NetUseAdd", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetUseAdd(string servername, UseLevel LevelFlags, IntPtr buf, ref uint parm_err);

        [DllImport("netapi32.dll", EntryPoint = "NetUseDel", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetUseDel(string UncServerName, string UseName, ForceLevel ForceLevelFlags);

        [DllImport("netapi32.dll", EntryPoint = "NetUseEnum", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetUseEnum(string UncServerName, UseLevel LevelFlags,ref IntPtr BufPtr, int PreferedMaximumSize, ref uint EntriesRead, ref uint TotalEntries, IntPtr ResumeHandle);

        [DllImport("netapi32.dll", EntryPoint = "NetUseGetInfo", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetUseGetInfo(string UncServerName, string UseName, UseLevel LevelFlags,ref IntPtr bufptr);

        public void Add<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<UseLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info)) 
            {
                uint paramIndex = 0;
                var result = NetUseAdd(serverName, info.GetLevel(), buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public void Del(string serverName, string useName, ForceLevel forceLevel)
        {
            var result = NetUseDel(serverName, useName, forceLevel);
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
                var result = NetUseEnum(serverName, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
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
                var result = NetUseGetInfo(serverName, useName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                buffer.PtrToStructure(info);
                return info;
            }
        }
    }
}
