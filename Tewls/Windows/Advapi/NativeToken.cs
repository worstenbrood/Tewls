using System;
using System.ComponentModel;
using Tewls.Windows.Kernel;

namespace Tewls.Windows.Advapi
{
    public class NativeToken : NativeHandle
    {
        // Static

        public static void Impersonate(IntPtr token)
        {
            var result = Advapi32.ImpersonateLoggedOnUser(token);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public static IntPtr DuplicateToken(IntPtr token, SecurityImpersonationLevel impersonationLevel)
        {
            IntPtr duplicateToken = IntPtr.Zero;
            var result = Advapi32.DuplicateToken(token, impersonationLevel, ref duplicateToken);
            if (!result)
            {
                throw new Win32Exception();
            }

            return duplicateToken;
        }

        public static IntPtr DuplicateTokenEx(IntPtr token, TokenAccess desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType)
        {
            IntPtr duplicateToken = IntPtr.Zero;
            var result = Advapi32.DuplicateTokenEx(token, desiredAccess, null, impersonationLevel, tokenType, ref duplicateToken);
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
