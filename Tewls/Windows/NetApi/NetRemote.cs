using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetRemote
    {
        [DllImport("netapi32.dll", EntryPoint = "NetRemoteComputerSupports", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetRemoteComputerSupports(string UncServerName, Supports OptionsWanted, ref Supports OptionsSupported);

        [DllImport("netapi32.dll", EntryPoint = "NetRemoteTOD", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern Error NetRemoteTOD(string UncServerName,ref IntPtr BufferPtr);

        public static Supports ComputerSupports(string serverName = null, Supports optionsRequested = Supports.Any)
        {
            Supports optionsSupported = 0;

            var result = NetRemoteComputerSupports(serverName, optionsRequested, ref optionsSupported);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }

            return optionsSupported;
        }

        public static TimeOfDayInfo TimeOfDay(string serverName)
        {
            using (var buffer = new NetBuffer())
            {
                var result = NetRemoteTOD(serverName, ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure<TimeOfDayInfo>();
            }
        }
    }
}
