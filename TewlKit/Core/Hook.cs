using System;

namespace TewlKit.Core
{
    public class Hook<T> : IHook
        where T : Delegate
    {
        public string ModuleName { get; }

        public string ProcName { get; }

        public Trampoline<T> Trampoline { get; protected set; }

        public Hook(string moduleName, string procName)
        {
            ModuleName = moduleName;
            ProcName = procName;
        }
    }
}
