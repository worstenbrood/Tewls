using IlDasm_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Tewls.Kit.Asm;
using Tewls.Kit.Asm.Stubs;
using Tewls.Kit.Utils;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Hooking
{
    /// <summary>
    /// Hook base class. Contains the module and procedure name, as well as the trampoline containing 
    /// the original method, the stub, and the replacement method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Constructor for the hook.
    /// </remarks>
    /// <param name="moduleName"></param>
    /// <param name="procName"></param>
    public abstract class Hook<T>(string moduleName, string procName) : GCHandled, IHook
        where T : Delegate
    {
        /// <summary>
        /// Module name of the function to hook.
        /// </summary>
        public string ModuleName { get; } = moduleName;

        /// <summary>
        /// Function name to hook.
        /// </summary>
        public string ProcName { get; } = procName;

        /// <summary>
        /// Trampoline containing the original method, the stub, and the replacement method.
        /// </summary>
        public Trampoline<T> Trampoline { get; protected set; }

        /// <summary>
        /// Replacement method and delegate.
        /// </summary>
        protected abstract T Replacement { get; }

        /// <summary>
        /// Stub method
        /// </summary>
        protected T Stub => Trampoline.Stub.Method;

        /// <summary>
        /// Decoder
        /// </summary>
        protected static readonly ZDecoder Decoder = ZDecoder.Create64();
        protected static readonly ZFormatter Formatter = new();


        /// <summary>
        /// Install the hook
        /// </summary>
        /// <param name="process"></param>
        /// <param name="module"></param>
        /// <param name="exports"></param>
        /// <param name="stubGenerator"></param>
        /// <returns></returns>
        public HookResult Install(NativeProcess process, NativeModule module, IList<NativeExport> exports,
            IStubGenerator stubGenerator)
        {
            // Check module name
            if (!module.Name.Equals(ModuleName, StringComparison.OrdinalIgnoreCase))
            {
                return HookResult.Empty;
            }

            // Find method
            var export = exports.FirstOrDefault(e => e.Name.Equals(ProcName, StringComparison.OrdinalIgnoreCase));
            if (export == null)
            {
                return HookResult.Empty;
            }

            Console.WriteLine($"[DEBUG] Hooking {ModuleName}!{ProcName} at address 0x{export.Address.ToInt64():X}");

            var stubFactory = new StubFactory(process);
            var remote = stubFactory.Create(export.Address, stubGenerator);

            // Set trampoline.
            Trampoline = new Trampoline<T>(export.Address, remote, Replacement);
                            
            // Write the jump to the replacement method to the original method.
            var jump = stubGenerator.GetBuffer(Trampoline.Replacement.Address);
            process.WriteBytes(export.Address, jump);

            // Return result struct containing the original method address, the trampoline address, and the stub size.
            return new HookResult(export.Address, remote, stubGenerator.Size);
        }

        /// <summary>
        /// Hook using the default stub generator, which is an indirect jump to RAX. 
        /// This is the most compatible stub, but it is also the largest, 
        /// so it may not work for some functions.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="module"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public HookResult Install(NativeProcess process, NativeModule module, IList<NativeExport> exports) =>
            Install(process, module, exports, IndirectJumpRaxStub.Instance);

        /// <summary>
        /// Uninstall an hook by restoring the original method and freeing the trampoline memory.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="hookResult"></param>
        public void Uninstall(NativeProcess process, HookResult hookResult)
        {
            var restored = process.ReadBytes(hookResult.StubAddress, hookResult.StubSize);

            // Make original address writable
            var prevProtection = process.VirtualProtectEx(hookResult.OriginalAddress, (nint)hookResult.StubSize, MemProtections.ExecuteReadWrite);

            // Restore original method
            process.WriteBytes(hookResult.OriginalAddress, restored);

            // Restore protection
            process.VirtualProtectEx(hookResult.OriginalAddress, (nint)hookResult.StubSize, prevProtection);

            // Free trampoline memory
            process.VirtualFreeEx(hookResult.StubAddress);
        }
    }
}
