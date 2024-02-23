using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tewls.Windows;
using Tewls.Windows.NetApi;
using Tewls.Windows.NetApi.Structures;

namespace Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach(var info in NetServer.TransportEnum<ServerTransportInfo1>(null))
            {
                var t = info.Numberofvcs;
            }


            foreach (var resource in WNet.EnumResources(WNet.ResourceScope.Remembered, WNet.ResourceType.Any, WNet.ResourceUsage.Connectable))
            {
                var parent = resource.GetNetworkInformation();
            }
        }
    }
}
