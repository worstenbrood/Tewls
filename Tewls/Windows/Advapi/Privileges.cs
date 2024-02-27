using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Advapi
{
    public class Privileges
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref Luid lpLuid);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)] 
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokenPrivileges NewState, uint BufferLength, IntPtr PreviousState, ref uint ReturnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, LogonType dwLogonType, LogonProvider dwLogonProvider,ref IntPtr phToken);

        public const string SeTcbPrivilege = "SeTcbPrivilege";

        // Static

        public static Luid LookupPrivilege(string name, string systemName = null)
        {
            var luid = new Luid();
            var result = LookupPrivilegeValue(systemName, name, ref luid);
            if (!result)
            {
                throw new Win32Exception();
            }

            return luid;
        }

        public static void AdjustTokenPrivileges(IntPtr tokenHandle, TokenPrivileges newState, bool disableAllPrivileges = false)
        {
            uint size = 0;
            var result = AdjustTokenPrivileges(tokenHandle, disableAllPrivileges, ref newState, (uint)Marshal.SizeOf(newState), IntPtr.Zero, ref size);
            if (!result) 
            {
                throw new Win32Exception();
            }
        }

        public static NativeToken LogonUser(string username, string domain, string password, LogonType logonType, LogonProvider logonProvider)
        {
            IntPtr token = IntPtr.Zero;
            var result = LogonUser(username, domain, password, logonType, logonProvider, ref token);
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
