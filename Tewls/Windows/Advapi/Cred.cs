using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    
    public class Cred
    {
        [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error CredDelete(string TargetName, CredType Type, uint Flags = 0);

        [DllImport("advapi32.dll", EntryPoint = "CredFree", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void CredFree(IntPtr Buffer);

        [DllImport("advapi32.dll", EntryPoint = "CredEnumerateW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredEnumerate(string Filter, CredEnumFlags Flags, ref uint Count, ref IntPtr Credential);

        public static void Delete(string targetName, CredType type)
        {
            var result = CredDelete(targetName, type);
            if (result != Error.Success) 
            { 
                throw new Win32Exception((int) result);
            }
        }

        public static IEnumerable<Credential> Enum(string filter = null)
        {
            // CredFree is needed to free the allocated buffer
            using (var buffer = new CredBuffer())
            {
                uint count = 0;
                var result = CredEnumerate(filter, 0, ref count, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }

                var array = new IntPtr[count];

                // Result buffer contains an array of pointers to Credential records
                Marshal.Copy(buffer.Buffer, array, 0, (int) count);

                for(var i = 0; i < count; i++)
                {
                    yield return CredBuffer.PtrToStructure<Credential>(array[i]);
                }
            }
        }
    }
}
