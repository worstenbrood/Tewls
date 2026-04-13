using System;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel;
using Tewls.Windows.NetApi;
using Tewls.Windows.NetApi.Structures;
using TewlKit.Asm.Stubs;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using TewlKit.Hooks;
using IlDasm_CSharp;

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
            var proc = nativeProcess.GetExports(module.Address).First(f => f.Name == "MessageBoxA");
            var buffer = nativeProcess.ReadBytes(proc.Address, 16);
            
            var length = buffer.GetASMLength(0, 12, module.Is64Bit);
            Console.WriteLine($"Length: {length}");
            var remote = nativeProcess.VirtualAllocEx((IntPtr)length, AllocationType.TopDown|AllocationType.Commit, MemProtections.ExecuteReadWrite);
            nativeProcess.WriteBytes(remote, buffer.Take(length).ToArray());
            Console.WriteLine($"Remote Address: {remote.ToInt64():X}");
            Console.WriteLine($"Original Address: {proc.Address.ToInt64():X}");
            Console.WriteLine($"Distance: {proc.Address.ToInt64() - remote.ToInt64():X}");

            var hook = new MessageBoxHook(proc.Address);
            var handle = hook.Trampoline.Replacement.Method(IntPtr.Zero, "test", "test", 0);
        }
    }
}
