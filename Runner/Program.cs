using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using TewlKit.Asm.Stubs;
using TewlKit.Hooks;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp;

namespace Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Zydis Version: {Zydis.GetVersion()}");
            Console.WriteLine(Marshal.SizeOf<ZydisDecoder>());
            var decoder = Zydis.CreateDecoder(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_COMPAT_32, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);
            Console.WriteLine($"Decoder Machine Mode: {decoder.MachineMode}, Stack Width: {decoder.StackWidth}");
            var b = new byte[] { 0x44, 0x39, 0x1D, 0x76, 0x3F, 0x04, 0x00 };

            var instruction = Zydis.DecodeInstruction(ref decoder, b);
            Console.WriteLine($"Decoded Instruction: {instruction.Length}");

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
            /*var currentProcess = Process.GetCurrentProcess();
            var nativeProcess = new NativeProcess(currentProcess.Id, ProcessAccessRights.AllAccess);
            var module = nativeProcess.GetModule("user32.dll");
            var procs = nativeProcess.GetExports(module.Address)
                .ToList();
            
            var hook = new MessageBoxHook();
            hook.Install(nativeProcess, module, procs);
            var st = nativeProcess.ReadBytes(hook.Trampoline.Stub.Address, 24);
            foreach ( var b in st)
            {
                Console.Write($"{b:X2} ");
            }
            Console.WriteLine();
            var p = nativeProcess.VirtualQueryEx(hook.Trampoline.Stub.Address);
            Console.WriteLine($"BaseAddress: {p.BaseAddress.ToInt64():X8}, RegionSize: {p.RegionSize}, State: {p.State}, Protect: {p.Protect}");
            st = nativeProcess.ReadBytes(hook.Trampoline.Stub.Address, 24);
            foreach (var b in st)
            {
                Console.Write($"{b:X2} ");
            }
            Console.WriteLine();

            var result = hook.Trampoline.Original.Method(IntPtr.Zero, "test", "test", 0);*/
        }
    }
}
