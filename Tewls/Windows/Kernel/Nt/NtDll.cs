using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel.Nt
{
    public static class NtDll
    {
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenProcess(ref IntPtr ProcessHandle, ProcessAccessRights DesiredAccess, ObjectAttributes ObjectAttributes, ClientId ClientId);

        [DllImport("ntdll.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern NtStatus NtQueryInformationProcess(IntPtr ProcessHandle, ProcessInformationClass ProcessInformationClass, IntPtr ProcessInformation, uint ProcessInformationLength, ref uint ReturnLength);

        public static bool NtSucces(NtStatus status)
        {
            return ((int)status) >= 0;
        }

        public static TStruct NtQueryInformationProcess<TStruct>(IntPtr processHandle)
            where TStruct: class, IProcessInfoClass, new()
        {
            var info = new TStruct();
            using (var buffer = new HGlobalBuffer<TStruct>(info))
            {
                uint size = 0;
                var result = NtQueryInformationProcess(processHandle, info.GetClass(), buffer.Buffer, (uint) buffer.Size, ref size);
                if (!NtSucces(result))
                {
                    throw new Win32Exception((int)result, result.ToString());
                }

                return buffer.PtrToStructure(info);
            }
        }
    }
}
