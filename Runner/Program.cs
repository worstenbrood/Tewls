using System;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel;
using Tewls.Windows.NetApi;
using Tewls.Windows.NetApi.Structures;
using TewlKit.Core;
using System.Diagnostics;
using System.Linq;
using TewlKit.Hooks;
using TewlKit.Core.Functions;

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

            //Engine.Start();
            var currentProcess = Process.GetCurrentProcess();
            var nativeProcess = new NativeProcess(currentProcess.Id, ProcessAccessRights.AllAccess);
            var module = nativeProcess.GetModule("user32.dll");
            var proc = nativeProcess.GetExports(module.Address).First(f => f.Name == "MessageBoxA");
            
            var hook = new MessageBoxHook(proc.Address);
            var handle = hook.Trampoline.Replacement.Method(IntPtr.Zero, "test", "test", 0);
        }
    }
}
