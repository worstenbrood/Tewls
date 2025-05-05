using System;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel;
using Tewls.Windows.NetApi;
using Tewls.Windows.NetApi.Structures;
using TewlKit.Core;

namespace Runner
{
    internal class Program
    {
        private delegate void Pointer(string[] argv);

        static void Main(string[] args)
        {
            var sys = SystemInfo.GetSystemInfo();

            foreach (var info in NetGroup.Enum<GroupInfo0>())
            {
                Console.WriteLine($"{info.Name}");
            }

            foreach (var cred in Cred.Enumerate())
            {
                Console.WriteLine($"{cred.UserName} : {cred.TargetName} : {cred.GetPassword()}");
            }

            Engine.Start();
        }
    }
}
