namespace TewlKit.Core
{
    /// <summary>
    /// 
    /// </summary>

    public class Hook<TFunction> : IHook<TFunction>
    {
        /// <summary>
        /// Dll name
        /// </summary>
        public string ModuleName { get; }

        /// <summary>
        /// Function name
        /// </summary>
        public string ProcName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="procName"></param>
        public Hook(string moduleName, string procName)
        {
            ModuleName = moduleName;
            ProcName = procName;
        }

        // Original method
        public TFunction Original { get; protected set; }
    }
}
