﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    
    public class Cred
    {
        [DllImport("advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredRead(string TargetName, CredType Type, uint Flags,ref IntPtr Credential);

        [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredDelete(string TargetName, CredType Type, uint Flags = 0);

        [DllImport("advapi32.dll", EntryPoint = "CredFree", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern void CredFree(IntPtr Buffer);

        [DllImport("advapi32.dll", EntryPoint = "CredEnumerateW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredEnumerate(string Filter, CredEnumFlags Flags, ref uint Count, ref IntPtr Credential);

        [DllImport("advapi32.dll", EntryPoint = "CredGetTargetInfoW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredGetTargetInfo(string TargetName, TargetFlags Flags, ref IntPtr TargetInfo);

        [DllImport("advapi32.dll", EntryPoint = "CredRenameW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool CredRename(string OldTargetName, string NewTargetName, CredType Type, uint Flags = 0);
               
        public static Credential Read(string targetName, CredType type)
        {
            using (var buffer = new CredBuffer())
            {
                var result = CredRead(targetName, type, 0, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }

                return buffer.PtrToStructure<Credential>();
            }
        }

        public static void Rename(string oldTargetName, string newTargetName, CredType type)
        {
            var result = CredRename(oldTargetName, newTargetName, type);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public static void Delete(string targetName, CredType type)
        {
            var result = CredDelete(targetName, type);
            if (!result) 
            { 
                throw new Win32Exception();
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

        public static CredentialTargetInformation GetTargetInfo(string targetName, TargetFlags targetFlags = TargetFlags.AllowNameResolution)
        {
            using (var buffer = new CredBuffer())
            {
                var result = CredGetTargetInfo(targetName, targetFlags, ref buffer.Buffer);
                if (!result)
                {
                    throw new Win32Exception();
                }

                return buffer.PtrToStructure<CredentialTargetInformation>();
            }
        }
    }
}
