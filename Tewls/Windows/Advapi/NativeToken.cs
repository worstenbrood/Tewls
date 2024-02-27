using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Advapi
{
    public class NativeToken : NativeHandle
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, SecurityImpersonationLevel ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateTokenEx(IntPtr hExistingToken, TokenAccess dwDesiredAccess, SecurityAttributes lpTokenAttributes, SecurityImpersonationLevel ImpersonationLevel, TokenType TokenType, ref IntPtr phNewToken);

        // Static

        public static void Impersonate(IntPtr token)
        {
            var result = ImpersonateLoggedOnUser(token);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public static IntPtr DuplicateToken(IntPtr token, SecurityImpersonationLevel impersonationLevel)
        {
            IntPtr duplicateToken = IntPtr.Zero;
            var result = DuplicateToken(token, impersonationLevel, ref duplicateToken);
            if (!result)
            {
                throw new Win32Exception();
            }

            return duplicateToken;
        }

        public static IntPtr DuplicateTokenEx(IntPtr token, TokenAccess desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType)
        {
            IntPtr duplicateToken = IntPtr.Zero;
            var result = DuplicateTokenEx(token, desiredAccess, null, impersonationLevel, tokenType, ref duplicateToken);
            if (!result)
            {
                throw new Win32Exception();
            }

            return duplicateToken;
        }

        // Class

        public NativeToken(IntPtr handle, bool dispose = true) : base(handle, dispose)
        {
        }

        public void Impersonate()
        {
            Impersonate(Handle);
        }

        public NativeToken DuplicateToken(SecurityImpersonationLevel impersonationLevel)
        {
            return new NativeToken(DuplicateToken(Handle, impersonationLevel));
        }

        public NativeToken DuplicateTokenEx(TokenAccess desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType)
        {
            return new NativeToken(DuplicateTokenEx(Handle, desiredAccess, impersonationLevel, tokenType));
        }
    }
}
