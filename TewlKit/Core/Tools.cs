using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Tewls.Windows.Kernel;
using static System.Collections.Specialized.BitVector32;

namespace TewlKit.Core
{
    /// <summary>
    /// Core Tools
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Enum all running processes
        /// </summary>
        /// <param name="action">Delegate executed for every <see cref="Process"/> and <see cref="NativeProcess"/> pair.</param>
        /// <param name="rights"><see cref="ProcessAccessRights"/> used to open the <see cref="NativeProcess"/>.</param>
        public static void Enum(Action<Process, NativeProcess> action, ProcessAccessRights rights = ProcessAccessRights.AllAccess)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    using (var nativeProcess = new NativeProcess(process.Id, rights))
                    {
                        action.Invoke(process, nativeProcess);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    process.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rights"></param>
        /// <returns></returns>
        public static IEnumerable<NativeProcess> Enum(ProcessAccessRights rights = ProcessAccessRights.AllAccess)
        {
            foreach (var process in Process.GetProcesses())
            {
                NativeProcess nativeProcess;
                try
                {
                    nativeProcess = new NativeProcess(process.Id, rights);
                }
                catch 
                {
                    process.Dispose();
                    continue; 
                }

                yield return nativeProcess;
                nativeProcess.Dispose();
                process.Dispose();
            }
        }

        /// <summary>
        /// Same as <see cref="EnumSuspended"/> but all processes are suspended first and resumed after
        /// </summary>
        /// <param name="action">>Delegate executed for every <see cref="Process"/> and <see cref="NativeProcess"/> pair.</param>
        public static void EnumSuspended(Action<Process, NativeProcess> action)
        {
            // First suspend all threads in all processes
            Enum((p, n) => p.SuspendProcess());
            
            try
            {
                // Execute action for every process
                Enum(action);
            }
            finally
            {
                // Resume all processes
                Enum((p, n) => p.ResumeProcess());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rights"></param>
        /// <returns></returns>
        public static IEnumerable<NativeProcess> EnumSuspended(ProcessAccessRights rights = ProcessAccessRights.AllAccess)
        {
            // First suspend all threads in all processes
            Enum((p, n) => p.SuspendProcess());

            try
            {
                // Execute action for every process
                foreach(var process in Enum(rights))
                {
                    yield return process;
                }
            }
            finally
            {
                // Resume all processes
                Enum((p, n) => p.ResumeProcess());
            }
        }

        /// <summary>
        /// Enum 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="method"></param>
        public static void EnumThreads(Process process, Func<IntPtr, uint> method)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var handle = Kernel32.OpenThread(ThreadAccess.SuspendResume, false, thread.Id);
                if (handle == IntPtr.Zero)
                {
                    continue;
                }

                try
                {
                    if (method(handle) != 0)
                    {
                        throw new Win32Exception();
                    }

                }
#if DEBUG
                catch (Exception ex)
                {
                    Debug.WriteLine("SuspendThread: " + ex.Message);
                }
#endif
                finally
                {
                    Kernel32.CloseHandle(handle);
                }
            }
        }
        
        /// <summary>
        /// Suspend process
        /// </summary>
        /// <param name="process"></param>
        public static void SuspendProcess(this Process process) => EnumThreads(process, Kernel32.SuspendThread);

        /// <summary>
        /// Resume process
        /// </summary>
        /// <param name="process"></param>
        public static void ResumeProcess(this Process process) => EnumThreads(process, Kernel32.ResumeThread);
    }
}
