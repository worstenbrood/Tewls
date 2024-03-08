using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel.Nt
{
    public static class NtDll
    {
        [DllImport(nameof(NtDll))]
        public static extern NtStatus NtOpenProcess(ref IntPtr ProcessHandle, ProcessAccessRights DesiredAccess, ObjectAttributes ObjectAttributes, ClientId ClientId);

        [DllImport(nameof(NtDll))]
        public static extern NtStatus NtQueryInformationProcess(IntPtr ProcessHandle, ProcessInformationClass ProcessInformationClass, IntPtr ProcessInformation, uint ProcessInformationLength, ref uint ReturnLength);

        public static bool NtSucces(NtStatus status)
        {
            return ((int)status) >= 0;
        }

        public static IntPtr NtOpenProcess(int processId, ProcessAccessRights desiredAccess, ObjectFlags flags = ObjectFlags.Inherit)
        {
            var processHandle = IntPtr.Zero;
            var attributes = new ObjectAttributes{ Attributes = flags};
            var clientId = new ClientId { UniqueProcess = (IntPtr) processId };
            var result = NtOpenProcess(ref processHandle, desiredAccess, attributes, clientId);
            if (!NtSucces(result))
            {
                throw new Win32Exception((int)result, result.ToString());
            }
            return processHandle;
        }

        public static TStruct NtQueryInformationProcess<TStruct>(IntPtr processHandle)
            where TStruct: class, IClass<ProcessInformationClass>, new()
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
