using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class NativeProcess : NativeHandle
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccess DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationTypes flAllocationType, Pages flProtect);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern UIntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, MemoryBasicInformation lpBuffer, IntPtr dwLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemFreeType dwFreeType);

        // Static

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

        public static IntPtr ReadProcessMemory(IntPtr process, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            IntPtr bytesRead = IntPtr.Zero;
            var result = ReadProcessMemory(process, baseAddress, buffer, size, ref bytesRead);
            if (!result)
            {
                throw new Win32Exception();
            }
            return bytesRead;
        }

        public static IntPtr WriteProcessMemory(IntPtr process, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            IntPtr bytesRead = IntPtr.Zero;
            var result = WriteProcessMemory(process, baseAddress, buffer, size, ref bytesRead);
            if (!result)
            {
                throw new Win32Exception();
            }
            return bytesRead;
        }

        public static UIntPtr VirtualQueryEx(IntPtr process, IntPtr address, MemoryBasicInformation buffer)
        {
            var result = VirtualQueryEx(process, address, buffer, (IntPtr) Marshal.SizeOf(buffer));
            if (result == UIntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return result;
        }

        // Class

        public NativeProcess(IntPtr processHandle, bool dispose = false) : base(processHandle, dispose)
        { 
        }

        public NativeToken OpenProcessToken(TokenAccess desiredAccess)
        {
            return new NativeToken(OpenProcessToken(Handle, desiredAccess));
        }

        public IntPtr VirtualAllocEx(IntPtr size, AllocationTypes allocationType, Pages protect, IntPtr address = default)
        {
            var result = VirtualAllocEx(Handle, address, size, allocationType, protect);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public IntPtr VirtualAllocEx(uint size, AllocationTypes allocationType, Pages protect, IntPtr address = default)
        {
            return VirtualAllocEx((IntPtr) size, allocationType, protect, address);
        }

        public MemoryBasicInformation VirtualQueryEx(IntPtr baseAddress)
        {
            var info = new MemoryBasicInformation();
            VirtualQueryEx(Handle, baseAddress, info);
            return info;
        }

        public IntPtr ReadProcessMemory(IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            return ReadProcessMemory(Handle, baseAddress, buffer, size);
        }

        public IntPtr ReadProcessMemory(IntPtr baseAddress, IntPtr buffer, uint size)
        {
            return ReadProcessMemory(baseAddress, buffer, (IntPtr) size);
        }
                
        public TStruct ReadProcessMemory<TStruct>(IntPtr baseAddress)
            where TStruct : class, new()
        {
            var query = VirtualQueryEx(baseAddress);
            using (var buffer = new NativeBuffer<TStruct>(query.RegionSize))
            {
                ReadProcessMemory(baseAddress, buffer, (uint) buffer.Size);
                buffer.Rebase(baseAddress, buffer.Buffer);
                return NativeBuffer<TStruct>.PtrToStructure<TStruct>(buffer);
            }
        }
        
        public IntPtr WriteProcessMemory(IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            return WriteProcessMemory(Handle, baseAddress, buffer, size);
        }

        public IntPtr WriteProcessMemory(IntPtr baseAddress, IntPtr buffer, uint size)
        {
            return WriteProcessMemory(baseAddress, buffer, (IntPtr) size);
        }
               

        public IntPtr WriteProcessMemory<TStruct>(TStruct structure)
            where TStruct : class
        {
            using (var buffer = new NativeBuffer<TStruct>(structure))
            {
                var baseAddress = VirtualAllocEx(buffer.Size, AllocationTypes.Commit, Pages.ReadWrite);
                buffer.Rebase(buffer, baseAddress);
                WriteProcessMemory(baseAddress, buffer, (uint) buffer.Size);
                
                return baseAddress;
            }
        }

        public void VirtualFreeEx(IntPtr address, MemFreeType freeType = MemFreeType.Release, IntPtr size = default)
        {
            var result = VirtualFreeEx(Handle, address, size, freeType);
            if (!result)
            {
                throw new Win32Exception();
            }
        }
    }
}
