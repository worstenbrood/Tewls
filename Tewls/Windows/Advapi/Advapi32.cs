using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows.Advapi
{
    public static class Advapi32
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredRead(string TargetName, CredType Type, uint Flags, ref IntPtr Credential);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredWrite(ref Credential Credential, CredWriteFlags Flags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredDelete(string TargetName, CredType Type, uint Flags = 0);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredEnumerate(string Filter, CredEnumFlags Flags, ref uint Count, ref IntPtr Credential);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredGetTargetInfo(string TargetName, TargetFlags Flags, ref IntPtr TargetInfo);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern bool CredRename(string OldTargetName, string NewTargetName, CredType Type, uint Flags = 0);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void CredFree(IntPtr Buffer);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateToken(IntPtr ExistingTokenHandle, SecurityImpersonationLevel ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateTokenEx(IntPtr hExistingToken, TokenAccess dwDesiredAccess, SecurityAttributes lpTokenAttributes, SecurityImpersonationLevel ImpersonationLevel, TokenType TokenType, ref IntPtr phNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref Luid lpLuid);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokenPrivileges NewState, uint BufferLength, IntPtr PreviousState, ref uint ReturnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, LogonType dwLogonType, LogonProvider dwLogonProvider, ref IntPtr phToken);
    }
}
