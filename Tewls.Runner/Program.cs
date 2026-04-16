using System;
using System.Diagnostics;
using System.Linq;
using Tewls.CapstoneSharp;
using Tewls.Kit.Hooks;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp;
using Tewls.ZydisSharp.Native;

namespace Tewls.Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var decoder = ZDecoder.Create64();
            byte[] jumpCode =
            [
                0x48, 0xB8, // MOV RAX, imm64
                0x55, 0x44, 0x33, 0x22, 0x11, 0x00, 0x00, 0x00, // 0x1122334455 (little-endian)
                0xFF, 0xE0  // JMP RAX
            ];
            var formatter = new ZFormatter(ZydisFormatterStyle.ZYDIS_FORMATTER_STYLE_MASM);
            var result = decoder.Disassemble(jumpCode);
            foreach (var entry in result)
            {
                Console.WriteLine($"Decoded Instruction: {entry.Instruction.Length}");
                Console.WriteLine($"Formatted Instruction: {formatter.FormatInstruction(ref entry.Instruction)}");
            }
            

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
            /*
            var currentProcess = Process.GetCurrentProcess();
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

            var result = hook.Trampoline.Original.Method(IntPtr.Zero, "test", "test", 0);
            */
        }
    }
}
