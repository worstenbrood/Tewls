using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetRemote : NetBase
    {
        public static Supports ComputerSupports(string serverName = null, Supports optionsRequested = Supports.Any)
        {
            Supports optionsSupported = 0;

            var result = Netapi32.NetRemoteComputerSupports(serverName, optionsRequested, ref optionsSupported);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }

            return optionsSupported;
        }

        public static TimeOfDayInfo TimeOfDay(string serverName = null)
        {
            using (var buffer = new NetBuffer())
            {
                var result = Netapi32.NetRemoteTOD(serverName, ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure<TimeOfDayInfo>();
            }
        }
    }
}
