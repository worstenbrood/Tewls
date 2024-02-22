using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tewls.Windows;

namespace Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var r in WNet.EnumResources(WNet.ResourceScope.Remembered, WNet.ResourceType.Any, WNet.ResourceUsage.Connectable))
            {
                var parent = r.GetParent();
            }
        }
    }
}
