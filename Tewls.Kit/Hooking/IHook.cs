namespace Tewls.Kit.Hooking
{
    /// <summary>
    /// Interface for a hook. Contains the module and procedure name to hook.
    /// </summary>
    public interface IHook
    {
        /// <summary>
        /// Module name of the function to hook.
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Function name to hook.
        /// </summary>
        string ProcName { get; }
    }
}