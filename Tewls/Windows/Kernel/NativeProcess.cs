using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tewls.Windows.Advapi;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class NativeProcess : NativeHandle
    {
        
        // Static

        public static IntPtr OpenProcess(int processId, ProcessAccessRights desiredAccess, bool inheritHandle = true)
        {
            var result = Kernel32.OpenProcess(desiredAccess, inheritHandle, processId);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return result;
        }

        public static void TerminateProcess(IntPtr hProcess, int uExitCode = 0)
        {
            if (!Kernel32.TerminateProcess(hProcess, (uint)uExitCode))
            {
                throw new Win32Exception();
            }
        }

        public static IntPtr OpenProcessToken(IntPtr processHandle, TokenAccess desiredAccess)
        {
            IntPtr tokenHandle = IntPtr.Zero;
            var result = Kernel32.OpenProcessToken(processHandle, desiredAccess, ref tokenHandle);
            if (!result)
            {
                throw new Win32Exception();
            }
            return tokenHandle;
        }

        public static IntPtr ReadProcessMemory(IntPtr process, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            IntPtr bytesRead = IntPtr.Zero;
            var result = Kernel32.ReadProcessMemory(process, baseAddress, buffer, size, ref bytesRead);
            if (!result)
            {
                throw new Win32Exception();
            }
            return bytesRead;
        }

        public static IntPtr WriteProcessMemory(IntPtr process, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            IntPtr bytesRead = IntPtr.Zero;
            var result = Kernel32.WriteProcessMemory(process, baseAddress, buffer, size, ref bytesRead);
            if (!result)
            {
                throw new Win32Exception();
            }
            return bytesRead;
        }

        public static MemProtections VirtualProtectEx(IntPtr process, IntPtr address, IntPtr size, MemProtections protection)
        {
            MemProtections previous = 0;

            var result = Kernel32.VirtualProtectEx(process, address, size, protection, ref previous);
            if (!result)
            {
                throw new Win32Exception();
            }
            return previous;
        }

        public static IntPtr VirtualQueryEx(IntPtr process, IntPtr address, MemoryBasicInformation buffer)
        {
            var result = Kernel32.VirtualQueryEx(process, address, buffer, (IntPtr)Marshal.SizeOf(buffer));
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return result;
        }

        public static IntPtr CreateRemoteThread(IntPtr process, IntPtr stackSize, IntPtr startAddress, IntPtr parameter, CreationFlags creationFlags = 0)
        {
            var securityAttributes = new SecurityAttributes { InheritHandle = true };
            uint threadId = 0;

            var result = Kernel32.CreateRemoteThread(process, securityAttributes, stackSize, startAddress, parameter, creationFlags, ref threadId);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public static bool IsWow64Process(IntPtr hProcess)
        {
            var result = false;
            if (!Kernel32.IsWow64Process(hProcess, ref result))
            {
                throw new Win32Exception();
            }
            return result;
        }

        // Class

        public NativeProcess() : base(Kernel32.GetCurrentProcess())
        {
        }

        public NativeProcess(string name, ProcessAccessRights accessRights)
        {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                throw new Exception($"Process {name} not found.");
            }

            Handle = OpenProcess(processes[0].Id, accessRights);
        }

        public NativeProcess(int processId, ProcessAccessRights accessRights)
        {
            Handle = OpenProcess(processId, accessRights);
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
            var result = Kernel32.VirtualAllocEx(Handle, address, size, allocationType, protect);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public RemoteBuffer VirtualAllocExBuffer(IntPtr size, MemAllocations allocationType, MemProtections protect, IntPtr address = default)
        {
            return new RemoteBuffer(this, VirtualAllocEx(size, allocationType, protect, address), size);
        }

        public RemoteBuffer VirtualAllocExBuffer(uint size, MemAllocations allocationType, MemProtections protect, IntPtr address = default)
        {
            return VirtualAllocExBuffer((IntPtr)size, allocationType, protect, address);
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

        public RemoteBuffer ReadProcessMemory(RemoteBuffer remoteBuffer, IntPtr localBuffer, IntPtr size)
        {
            ReadProcessMemory(Handle, remoteBuffer.Buffer, localBuffer, size);
            return remoteBuffer;
        }

        public RemoteBuffer ReadProcessMemory(RemoteBuffer remoteBuffer, IntPtr localBuffer, uint size)
        {
            ReadProcessMemory(remoteBuffer, localBuffer, (IntPtr)size);
            return remoteBuffer;
        }

        public TStruct ReadProcessMemory<TStruct>(IntPtr remoteBuffer)
            where TStruct : class, new()
        {
            var query = VirtualQueryEx(remoteBuffer);
            using (var localBuffer = new NativeBuffer<TStruct>(query.RegionSize))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, query.RegionSize);
                localBuffer.Rebase(remoteBuffer, localBuffer.Buffer);
                return BufferBase.PtrToStructure<TStruct>(localBuffer.Buffer);
            }
        }

        public TStruct ReadProcessMemory<TStruct>(RemoteBuffer remoteBuffer)
            where TStruct : class, new()
        {
            return ReadProcessMemory<TStruct>(remoteBuffer.Buffer);
        }

        public IntPtr WriteProcessMemory(IntPtr remoteBuffer, IntPtr localBuffer, IntPtr size)
        {
            return WriteProcessMemory(Handle, remoteBuffer, localBuffer, size);
        }

        public RemoteBuffer WriteProcessMemory(RemoteBuffer remoteBuffer, IntPtr localBuffer, IntPtr size)
        {
            WriteProcessMemory(Handle, remoteBuffer.Buffer, localBuffer, size);
            return remoteBuffer;
        }

        public RemoteBuffer WriteProcessMemory(RemoteBuffer remoteBuffer, IntPtr localBuffer, uint size)
        {
            return WriteProcessMemory(remoteBuffer, localBuffer, (IntPtr)size);
        }

        public IntPtr WriteProcessMemory<TStruct>(TStruct structure, IntPtr remoteBuffer)
            where TStruct : class
        {
            using (var localBuffer = new NativeBuffer<TStruct>(structure))
            {
                localBuffer.Rebase(localBuffer.Buffer, remoteBuffer);
                return WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
        }

        public RemoteBuffer WriteProcessMemory<TStruct>(TStruct structure, RemoteBuffer remoteBuffer)
            where TStruct : class
        {
            return WriteProcessMemory(structure, remoteBuffer);
        }

        public RemoteBuffer WriteProcessMemory<TStruct>(TStruct structure)
            where TStruct : class
        {
            using (var localBuffer = new NativeBuffer<TStruct>(structure))
            {
                var remoteBuffer = new RemoteBuffer(this, localBuffer.Size);
                localBuffer.Rebase(localBuffer.Buffer, remoteBuffer.Buffer);
                return WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
        }

        public string ReadString(RemoteBuffer remoteBuffer)
        {
            var query = remoteBuffer.GetInformation();
            using (var localBuffer = new HGlobalBuffer(query.RegionSize))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, (uint)localBuffer.Size);
                return Marshal.PtrToStringAuto(localBuffer.Buffer);
            }
        }

        public RemoteBuffer WriteString(RemoteBuffer remoteBuffer, string s)
        {
            using (var localBuffer = new HGlobalBuffer(remoteBuffer.Size))
            {
                NativeBuffer.lstrcpyn(localBuffer.Buffer, s);
                return WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
        }

        public RemoteBuffer WriteString(string s)
        {
            var size = (s.Length + 1) * sizeof(char);
            var remoteBuffer = VirtualAllocExBuffer((IntPtr)size, MemAllocations.Commit | MemAllocations.Reserve | MemAllocations.TopDown, MemProtections.ReadWrite);
            return WriteString(remoteBuffer, s);
        }

        public MemProtections VirtualProtectEx(IntPtr remoteBuffer, IntPtr size, MemProtections protection)
        {
            return VirtualProtectEx(Handle, remoteBuffer, size, protection);
        }

        public MemProtections VirtualProtectEx(RemoteBuffer remoteBuffer, IntPtr size, MemProtections protection)
        {
            return VirtualProtectEx(remoteBuffer.Buffer, size, protection);
        }

        public MemProtections VirtualProtectEx(RemoteBuffer remoteBuffer, MemProtections protection)
        {
            var query = VirtualQueryEx(remoteBuffer.Buffer);
            return VirtualProtectEx(remoteBuffer, query.RegionSize, protection);
        }

        public void VirtualFreeEx(IntPtr remoteBuffer, MemFreeType freeType = MemFreeType.Release, IntPtr size = default)
        {
            var result = Kernel32.VirtualFreeEx(Handle, remoteBuffer, size, freeType);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public void VirtualFreeEx(RemoteBuffer remoteBuffer, MemFreeType freeType = MemFreeType.Release, IntPtr size = default)
        {
            var result = Kernel32.VirtualFreeEx(Handle, remoteBuffer.Buffer, size, freeType);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        public NativeToken CreateRemoteThread(IntPtr stackSize, IntPtr startAddress, IntPtr parameter)
        {
            return new NativeToken(CreateRemoteThread(Handle, stackSize, startAddress, parameter));
        }

        public uint GetProcessId()
        {
            var result = Kernel32.GetProcessId(Handle);
            if (result == 0)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public bool IsWow64Process()
        {
            return IsWow64Process(Handle);
        }

        public void TerminateProcess(int exitCode = 0)
        {
            TerminateProcess(Handle, exitCode);
        }

        public TStruct GetProcessInformation<TStruct>()
            where TStruct : class, IProcessClass, new()
        {
            var t = new TStruct();
            using (var buffer = new HGlobalBuffer<TStruct>(t, false))
            {
                // This fails for ProcessLeapSecondInfo and ProcessPowerThrottlingState for some reason
                var result = Kernel32.GetProcessInformation(Handle, t.GetClass(), buffer.Buffer, (uint) buffer.Size);
                if (!result)
                {
                    throw new Win32Exception();
                }

                return buffer.PtrToStructure(t);
        
            }
        }

        public override int GetHashCode()
        {
            return (int)GetProcessId();
        }
    }
}
