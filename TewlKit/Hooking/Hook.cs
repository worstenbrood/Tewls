using IlDasm_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TewlKit.Asm;
using TewlKit.Asm.Stubs;
using TewlKit.Utils;
using Tewls.Windows.Kernel;

namespace TewlKit.Hooking
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

            // Read the first 16 bytes of the original method to create the trampoline.
            var buffer = process.ReadBytes(export.Address, 16);

            // Calculate the length of the instructions to overwrite, which should be at least the size of the stub.
            var length = buffer.GetASMLength(0, stubGenerator.Size);
            if (length < stubGenerator.Size)
            {
                return HookResult.Empty;
            }

            // Allocate memory for the trampoline, which will contain the original method, the stub, and the replacement method.
            var remote = process.VirtualAllocEx((IntPtr)length + stubGenerator.Size, AllocationType.Commit, MemProtections.ExecuteReadWrite);

            // Set trampoline.
            Trampoline = new Trampoline<T>(export.Address, remote, Replacement);

            // Write the original method to the trampoline.
            process.WriteBytes(remote, [.. buffer.Take(length)]);

            // Write the stub to the trampoline, which will jump original method.
            var stub = stubGenerator.GetBuffer(export.Address + length);
            process.WriteBytes(remote + length, stub);
            process.VirtualProtectEx(remote, (IntPtr)(IntPtr)length + stubGenerator.Size, MemProtections.Execute);

            // Change protection
            var prevProtection = process.VirtualProtectEx(export.Address, (IntPtr)stubGenerator.Size, MemProtections.ExecuteReadWrite);

            // Write the jump to the replacement method to the original method.
            var jump = stubGenerator.GetBuffer(Trampoline.Replacement.Address);
            process.WriteBytes(export.Address, jump);

            // Restore protection
            process.VirtualProtectEx(export.Address, (IntPtr)stubGenerator.Size, prevProtection);

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
            var prevProtection = process.VirtualProtectEx(hookResult.OriginalAddress, (IntPtr)hookResult.StubSize, MemProtections.ExecuteReadWrite);

            // Restore original method
            process.WriteBytes(hookResult.OriginalAddress, restored);

            // Restore protection
            process.VirtualProtectEx(hookResult.OriginalAddress, (IntPtr)hookResult.StubSize, prevProtection);

            // Free trampoline memory
            process.VirtualFreeEx(hookResult.StubAddress);
        }
    }
}
