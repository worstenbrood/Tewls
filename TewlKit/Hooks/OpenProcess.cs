using System;
using TewlKit.Core;

namespace TewlKit.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenProcess : Hook<OpenProcess.Function>
    {
        public delegate IntPtr Function(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        /// <summary>
        /// 
        /// </summary>
        public unsafe OpenProcess() : base("kernel32.dll", "OpenProccess")
        {
            Original = *(Function*)0;
        }
    }
}
