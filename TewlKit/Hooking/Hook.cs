using System;

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
    public class Hook<T>(string moduleName, string procName) : IHook
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

        //public void Hook()
    }
}
