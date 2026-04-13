using IlDasm_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TewlKit.Asm.Stubs;
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
    public abstract class Hook<T> : IHook, IDisposable
        where T : Delegate
    {
        /// <summary>
        /// GCHandle to prevent the hook from being garbage collected. Freed in the finalizer.
        /// </summary>
        protected GCHandle Handle;

        /// <summary>
        /// Module name of the function to hook.
        /// </summary>
        public string ModuleName { get; }

        /// <summary>
        /// Function name to hook.
        /// </summary>
        public string ProcName { get; }

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

        /// <param name="moduleName"></param>
        /// <param name="procName"></param>
        public Hook(string moduleName, string procName)
        {
            ModuleName = moduleName;
            ProcName = procName;
            Handle = GCHandle.Alloc(this);
        }

        /// <summary>
        /// Install the hook
        /// </summary>
        /// <param name="process"></param>
        /// <param name="module"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        public bool Install(NativeProcess process, NativeModule module, IList<NativeExport> exports)
        {
            if (!module.Name.Equals(ModuleName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var export = exports.FirstOrDefault(e => e.Name.Equals(ProcName, StringComparison.OrdinalIgnoreCase));
            if (export == null)
            {
                return false;
            }

            // Read the first 16 bytes of the original method to create the trampoline.
            var buffer = process.ReadBytes(export.Address, 16);

            // Calculate the length of the instructions to overwrite, which should be at least the size of the stub.
            var length = buffer.GetASMLength(0, IndirectJumpRaxStub.Instance.Size);
            if (length < IndirectJumpRaxStub.Instance.Size)
            {
                return false;
            }

            // Allocate memory for the trampoline, which will contain the original method, the stub, and the replacement method.
            var remote = process.VirtualAllocEx((IntPtr)length + IndirectJumpRaxStub.Instance.Size, AllocationType.Commit, MemProtections.ExecuteReadWrite);

            // Set trampoline.
            Trampoline = new Trampoline<T>(export.Address, remote, Replacement);

            // Write the original method to the trampoline.
            process.WriteBytes(remote, [.. buffer.Take(length)]);

            // Write the stub to the trampoline, which will jump original method.
            var stub = IndirectJumpRaxStub.Instance.GetBuffer(export.Address + length);
            process.WriteBytes(remote + length, stub);

            // Change protection
            var prevProtection = process.VirtualProtectEx(export.Address, (IntPtr)IndirectJumpRaxStub.Instance.Size, MemProtections.ExecuteReadWrite);
            
            // Write the jump to the replacement method to the original method.
            var jump = IndirectJumpRaxStub.Instance.GetBuffer(Trampoline.Replacement.Address);
            process.WriteBytes(export.Address, jump);

            // Restore protection
            process.VirtualProtectEx(export.Address, (IntPtr)IndirectJumpRaxStub.Instance.Size, prevProtection);

            return true;
        }

        public void Dispose()
        {
            Handle.Free();
        }
    }
}
