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
                throw new Exception($"Process {name} not found.");
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

        public MemoryBasicInformation VirtualQueryEx(IntPtr remoteBuffer)
        {
            var info = new MemoryBasicInformation();
            VirtualQueryEx(Handle, remoteBuffer, info);
            return info;
        }

        public IntPtr ReadProcessMemory(IntPtr remoteBuffer, IntPtr localBuffer, IntPtr size)
        {
            return ReadProcessMemory(Handle, remoteBuffer, localBuffer, size);
        }

        public IntPtr ReadProcessMemory(IntPtr remoteBuffer, IntPtr localBuffer, uint size)
        {
            return ReadProcessMemory(remoteBuffer, localBuffer, (IntPtr) size);
        }
                
        public TStruct ReadProcessMemory<TStruct>(IntPtr remoteBuffer)
            where TStruct : class, new()
        {
            var query = VirtualQueryEx(remoteBuffer);
            using (var localBuffer = new NativeBuffer<TStruct>(query.RegionSize))
            {
                ReadProcessMemory(remoteBuffer, localBuffer, (uint)localBuffer.Size);
                localBuffer.Rebase(remoteBuffer, localBuffer.Buffer);
                return NativeBuffer<TStruct>.PtrToStructure<TStruct>(localBuffer);
            }
        }
        
        public IntPtr WriteProcessMemory(IntPtr remoteBuffer, IntPtr localBuffer, IntPtr size)
        {
            return WriteProcessMemory(Handle, remoteBuffer, localBuffer, size);
        }

        public IntPtr WriteProcessMemory(IntPtr remoteBuffer, IntPtr localBuffer, uint size)
        {
            return WriteProcessMemory(remoteBuffer, localBuffer, (IntPtr) size);
        }
               
        public IntPtr WriteProcessMemory<TStruct>(TStruct structure)
            where TStruct : class
        {
            using (var localBuffer = new NativeBuffer<TStruct>(structure))
            {
                var remoteBuffer = VirtualAllocEx(localBuffer.Size, MemAllocations.Commit, MemProtections.ReadWrite);
                localBuffer.Rebase(localBuffer, remoteBuffer);
                WriteProcessMemory(remoteBuffer, localBuffer, (uint) localBuffer.Size);
                
                return remoteBuffer;
            }
        }

        public MemProtections VirtualProtectEx(IntPtr remoteBuffer, IntPtr size, MemProtections protection)
        {
            return VirtualProtectEx(Handle, remoteBuffer, size, protection);
        }

        public MemProtections VirtualProtectEx(IntPtr remoteBuffer, MemProtections protection)
        {
            var query = VirtualQueryEx(remoteBuffer);    
            return VirtualProtectEx(remoteBuffer, query.RegionSize, protection);
        }

        public void VirtualFreeEx(IntPtr remoteBuffer, MemFreeType freeType = MemFreeType.Release, IntPtr size = default)
        {
            var result = VirtualFreeEx(Handle, remoteBuffer, size, freeType);
            if (!result)
            {
                throw new Win32Exception();
            }
        }
    }
}
