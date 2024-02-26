using System;
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
    }
}
