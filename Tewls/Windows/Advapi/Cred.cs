using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class Cred
    {
        public static Credential Read(string targetName, CredType type)
        {
            using (var buffer = new CredBuffer())
            {
                var result = Advapi32.CredRead(targetName, type, 0, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }

                return buffer.PtrToStructure<Credential>();
            }
        }

        public static void Write(Credential credential, CredWriteFlags flags = CredWriteFlags.None)
        {
            var result = Advapi32.CredWrite(ref credential, flags);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public static void Rename(string oldTargetName, string newTargetName, CredType type)
        {
            var result = Advapi32.CredRename(oldTargetName, newTargetName, type);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public static void Delete(string targetName, CredType type)
        {
            var result = Advapi32.CredDelete(targetName, type);
            if (!result) 
            { 
                throw new Win32Exception();
            }
        }

        public static IEnumerable<Credential> Enumerate(string filter = null)
        {
            // CredFree is needed to free the allocated buffer
            using (var buffer = new CredBuffer())
            {
                uint count = 0;
                var result = Advapi32.CredEnumerate(filter, CredEnumFlags.AllCredentials, ref count, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }
                             
                var credential = new Credential();
                
                for (var i = 0; i < count; i++)
                {
                    // Result buffer contains array of pointers to Credential records
                    var record = Marshal.ReadIntPtr(buffer.Buffer, i * IntPtr.Size);
                    yield return BufferBase.PtrToStructure(record, credential);
                }
            }
        }

        public static CredentialTargetInformation GetTargetInfo(string targetName, TargetFlags targetFlags = TargetFlags.AllowNameResolution)
        {
            using (var buffer = new CredBuffer())
            {
                var result = Advapi32.CredGetTargetInfo(targetName, targetFlags, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }

                return buffer.PtrToStructure<CredentialTargetInformation>();
            }
        }
    }
}
