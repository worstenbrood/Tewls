using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Advapi
{
    public class Privileges
    {
        public const string SeTcbPrivilege = "SeTcbPrivilege";

        // Static

        public static Luid LookupPrivilege(string name, string systemName = null)
        {
            var luid = new Luid();
            var result = Advapi32.LookupPrivilegeValue(systemName, name, ref luid);
            if (!result)
            {
                throw new Win32Exception();
            }

            return luid;
        }

        public static void AdjustTokenPrivileges(IntPtr tokenHandle, TokenPrivileges newState, bool disableAllPrivileges = false)
        {
            uint size = 0;
            var result = Advapi32.AdjustTokenPrivileges(tokenHandle, disableAllPrivileges, ref newState, (uint)Marshal.SizeOf(newState), IntPtr.Zero, ref size);
            if (!result) 
            {
                throw new Win32Exception();
            }
        }

        public static NativeToken LogonUser(string username, string domain, string password, LogonType logonType, LogonProvider logonProvider)
        {
            IntPtr token = IntPtr.Zero;
            var result = Advapi32.LogonUser(username, domain, password, logonType, logonProvider, ref token);
            if (!result)
            {
                throw new Win32Exception();
            }

            return new NativeToken(token);
        }

        public static void GetDebugPrivilege(IntPtr processHandle = default)
        {
            var debugPrivilege = LookupPrivilege(SeTcbPrivilege);
            var handle = processHandle != IntPtr.Zero ? processHandle : Process.GetCurrentProcess().Handle;
            
            using (var process = new NativeProcess(handle, processHandle != IntPtr.Zero))
            {
                using (var token = process.OpenProcessToken(TokenAccess.AdjustPrivileges))
                {
                    var privilege = new TokenPrivileges { Privileges = new LuidAndAttributes[1] };
                    privilege.Privileges[0].Attributes = PrivilegeAttributes.Enabled;
                    privilege.Privileges[0].Luid = debugPrivilege;
                    privilege.PrivilegeSize = 1;

                    AdjustTokenPrivileges(token, privilege);
                }
            }
        }
    }
}
