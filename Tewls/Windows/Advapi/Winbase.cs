using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Advapi
{
    public class Winbase
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref Luid lpLuid);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)] 
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokenPrivileges NewState, uint BufferLength, IntPtr PreviousState, ref uint ReturnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccess DesiredAccess, ref IntPtr TokenHandle);

        public const string SeTcbPrivilege = "SeTcbPrivilege";

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

        public static IntPtr OpenProcessToken(IntPtr processHandle, TokenAccess desiredAccess)
        {
            IntPtr tokenHandle = IntPtr.Zero;
            var result = OpenProcessToken(processHandle, desiredAccess, ref tokenHandle);
            if (!result)
            {
                throw new Win32Exception();
            }
            return tokenHandle;
        }

        public static void GetDebugPrivilege(IntPtr processHandle = default)
        {
            var token = OpenProcessToken(processHandle != IntPtr.Zero ? processHandle : Process.GetCurrentProcess().Handle, TokenAccess.AdjustPrivileges);
            var debugPrivilege = LookupPrivilege(SeTcbPrivilege);
            
            var privilege = new TokenPrivileges { Privileges = new LuidAndAttributes[1] };
            privilege.Privileges[0].Attributes = PrivilegeAttributes.Enabled;
            privilege.Privileges[0].Luid = debugPrivilege;
            privilege.PrivilegeSize = 1;

            AdjustTokenPrivileges(token, privilege);
        }
    }
}
