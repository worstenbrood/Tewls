using System;
using Tewls.Windows.Kernel;
using TewlKit.Asm.Stubs;
using System.Diagnostics;
using System.Linq;
using TewlKit.Hooks;

namespace Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*var sys = SystemInfo.GetSystemInfo();
            Thread.Sleep(1000);
            foreach (var info in NetGroup.Enum<GroupInfo0>())
            {
                Console.WriteLine($"{info.Name}");
            }

            foreach (var cred in Cred.Enumerate())
            {
                Console.WriteLine($"{cred.UserName} : {cred.TargetName} : {cred.GetPassword()}");
            }*/

            //Engine.Start();
            var addr = new IntPtr(0x11223344);
            var stub = IndirectJumpEaxStub.Instance.GetBuffer(addr);
            foreach (var b in stub)
            {
                Console.Write($"{b:X2} ");
            }
            Console.WriteLine();

            var currentProcess = Process.GetCurrentProcess();
            var nativeProcess = new NativeProcess(currentProcess.Id, ProcessAccessRights.AllAccess);
            var module = nativeProcess.GetModule("user32.dll");
            var procs = nativeProcess.GetExports(module.Address)
                .ToList();
            
            var hook = new MessageBoxHook();
            hook.Install(nativeProcess, module, procs);
            var st = nativeProcess.ReadBytes(hook.Trampoline.Stub.Address, 16);
            foreach ( var b in st)
            {
                Console.Write($"{b:X2} ");
            }
            var p = nativeProcess.VirtualQueryEx(hook.Trampoline.Stub.Address);
            Console.WriteLine($"BaseAddress: {p.BaseAddress}, RegionSize: {p.RegionSize}, State: {p.State}, Protect: {p.Protect}");
            st = nativeProcess.ReadBytes(hook.Trampoline.Stub.Address, 16);
            foreach (var b in st)
            {
                Console.Write($"{b:X2} ");
            }

            var result = hook.Trampoline.Original.Method(IntPtr.Zero, "test", "test", 0);
        }
    }
}
