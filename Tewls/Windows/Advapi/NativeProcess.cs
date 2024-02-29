using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

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
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemAllocations flAllocationType, MemProtections flProtect);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern UIntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, MemoryBasicInformation lpBuffer, IntPtr dwLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemProtections flNewProtect, ref MemProtections lpflOldProtect);

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

        public static MemProtections VirtualProtectEx(IntPtr process, IntPtr address, IntPtr size, MemProtections protection)
        {
            MemProtections previous = 0;

            var result = VirtualProtectEx(process, address, size, protection, ref previous);
            if (!result)
            {
                throw new Win32Exception();
            }
            return previous;
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

        public NativeProcess(string name)
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            { 
                throw new Exception();
            }

            Handle = processes[0].Handle;
        }

        public NativeProcess(IntPtr processHandle, bool dispose = false) : base(processHandle, dispose)
        { 
        }

        public NativeToken OpenProcessToken(TokenAccess desiredAccess)
        {
            return new NativeToken(OpenProcessToken(Handle, desiredAccess));
        }

        public IntPtr VirtualAllocEx(IntPtr size, MemAllocations allocationType, MemProtections protect, IntPtr address = default)
        {
            var result = VirtualAllocEx(Handle, address, size, allocationType, protect);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public IntPtr VirtualAllocEx(uint size, MemAllocations allocationType, MemProtections protect, IntPtr address = default)
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
                var baseAddress = VirtualAllocEx(buffer.Size, MemAllocations.Commit, MemProtections.ReadWrite);
                buffer.Rebase(buffer, baseAddress);
                WriteProcessMemory(baseAddress, buffer, (uint) buffer.Size);
                
                return baseAddress;
            }
        }

        public MemProtections VirtualProtectEx(IntPtr address, IntPtr size, MemProtections protection)
        {
            return VirtualProtectEx(Handle, address, size, protection);
        }

        public MemProtections VirtualProtectEx(IntPtr address, MemProtections protection)
        {
            var query = VirtualQueryEx(address);    
            return VirtualProtectEx(Handle, address, query.RegionSize, protection);
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
