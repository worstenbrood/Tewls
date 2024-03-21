﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tewls.Windows.Advapi;
using Tewls.Windows.Kernel.Nt;
using Tewls.Windows.PE;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class NativeProcess : NativeHandle
    {
        // Static

        public static IntPtr OpenProcess(int processId, ProcessAccessRights desiredAccess, bool inheritHandle = true)
        {
            return NtDll.NtOpenProcess(processId, desiredAccess);
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
            var result = Advapi32.OpenProcessToken(processHandle, desiredAccess, ref tokenHandle);
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

        public static IntPtr ReadProcessMemory(IntPtr process, IntPtr baseAddress, IntPtr buffer, uint size)
        {
            return ReadProcessMemory(process, baseAddress, buffer, (IntPtr) size);
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

        public NativeProcess() : base(Kernel32.GetCurrentProcess(), false)
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

        public IntPtr VirtualAllocEx(IntPtr size, AllocationType allocationType, MemProtections protect, IntPtr address = default)
        {
            var result = Kernel32.VirtualAllocEx(Handle, address, size, allocationType, protect);
            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return result;
        }

        public RemoteBuffer VirtualAllocExBuffer(IntPtr size, AllocationType allocationType, MemProtections protect, IntPtr address = default)
        {
            return new RemoteBuffer(this, VirtualAllocEx(size, allocationType, protect, address), size);
        }

        public RemoteBuffer VirtualAllocExBuffer(uint size, AllocationType allocationType, MemProtections protect, IntPtr address = default)
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

        public TStruct ReadProcessMemory<TStruct>(IntPtr remoteBuffer, IntPtr size, bool rebase = false)
           where TStruct : class, new()
        {
            using (var localBuffer = new NativeBuffer<TStruct>(size))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, size);
                if (rebase)
                {
                    localBuffer.Rebase(remoteBuffer, localBuffer.Buffer);
                }
                return BufferBase.PtrToStructure<TStruct>(localBuffer.Buffer);
            }
        }

        public TStruct ReadProcessMemory<TStruct>(IntPtr remoteBuffer, bool rebase = false)
            where TStruct : class, new()
        {
            return ReadProcessMemory<TStruct>(remoteBuffer, (IntPtr)Marshal.SizeOf(typeof(TStruct)), rebase);
        }

        public TStruct ReadProcessMemory<TStruct>(uint remoteBuffer, bool rebase = false)
            where TStruct : class, new()
        {
            return ReadProcessMemory<TStruct>((IntPtr) remoteBuffer, (IntPtr) Marshal.SizeOf(typeof(TStruct)), rebase);
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

        public IntPtr WriteProcessMemory<TStruct>(TStruct structure, IntPtr remoteBuffer, bool rebase = true)
            where TStruct : class
        {
            using (var localBuffer = new NativeBuffer<TStruct>(structure))
            {
                if (rebase)
                {
                    localBuffer.Rebase(localBuffer.Buffer, remoteBuffer);
                }
                return WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
        }

        public RemoteBuffer WriteProcessMemory<TStruct>(TStruct structure, RemoteBuffer remoteBuffer)
            where TStruct : class
        {
            return WriteProcessMemory(structure, remoteBuffer);
        }

        public RemoteBuffer WriteProcessMemory<TStruct>(TStruct structure, bool rebase = true)
            where TStruct : class
        {
            using (var localBuffer = new NativeBuffer<TStruct>(structure))
            {
                var remoteBuffer = new RemoteBuffer(this, localBuffer.Size);
                if (rebase)
                {
                    localBuffer.Rebase(localBuffer.Buffer, remoteBuffer.Buffer);
                }
                return WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
        }

        public string ReadString(IntPtr remoteBuffer, uint size, int charsize = sizeof(char))
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr) size))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, (IntPtr) size);
                return Marshal.PtrToStringAuto(localBuffer.Buffer, (int) size / charsize);
            }
        }

        public string ReadString(uint remoteBuffer, uint size, int charsize = sizeof(char))
        {
            return ReadString((IntPtr) remoteBuffer, size, charsize);
        }

        public string ReadStringA(IntPtr remoteBuffer, uint size)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr)size))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, (IntPtr)size);
                return Marshal.PtrToStringAnsi(localBuffer.Buffer);
            }
        }

        public string ReadStringA(uint remoteBuffer, uint size)
        {
            return ReadStringA((IntPtr)remoteBuffer, size);
        }

        public string ReadString(RemoteBuffer remoteBuffer)
        {
            var query = remoteBuffer.GetInformation();
            using (var localBuffer = new HGlobalBuffer(query.RegionSize))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                return Marshal.PtrToStringAuto(localBuffer.Buffer);
            }
        }

        public IntPtr ReadIntPtr(IntPtr remoteBuffer)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr) Marshal.SizeOf(typeof(IntPtr))))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                return Marshal.ReadIntPtr(localBuffer.Buffer);
            }
        }

        public int ReadInt(IntPtr remoteBuffer)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr)Marshal.SizeOf(typeof(int))))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                return Marshal.ReadInt32(localBuffer.Buffer);
            }
        }

        public int ReadInt(uint remoteBuffer)
        {
            return ReadInt((IntPtr)remoteBuffer);
        }

        public short ReadInt16(IntPtr remoteBuffer)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr)Marshal.SizeOf(typeof(short))))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                return Marshal.ReadInt16(localBuffer.Buffer);
            }
        }

        public short ReadInt16(uint remoteBuffer)
        {
            return ReadInt16((IntPtr)remoteBuffer);
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
            var remoteBuffer = VirtualAllocExBuffer((IntPtr)size, AllocationType.Commit | AllocationType.Reserve | AllocationType.TopDown, MemProtections.ReadWrite);
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
            where TStruct : class, IClass<ProcessInformationClass>, new()
        {
            var info = new TStruct();
            using (var buffer = new HGlobalBuffer<TStruct>(info))
            {
                // This fails for ProcessLeapSecondInfo and ProcessPowerThrottlingState for some reason
                var result = Kernel32.GetProcessInformation(Handle, info.GetClass(), buffer.Buffer, (uint) buffer.Size);
                if (!result)
                {
                    throw new Win32Exception();
                }
                                
                return buffer.PtrToStructure(info);
            }
        }

        public TStruct QueryProcessInformation<TStruct>()
            where TStruct : class, IClass<Nt.ProcessInformationClass>, new()
        {
            return NtDll.NtQueryInformationProcess<TStruct>(Handle);
        }

        public LdrModule GetModuleHandle(string name)
        {
            var pbi = QueryProcessInformation<ProcessBasicInformation>();
            var peb = ReadProcessMemory<Peb>(pbi.PebBaseAddress);
            var ldr = ReadProcessMemory<PebLdrData>(peb.Ldr);
            var current = ldr.InLoadOrderModuleList.Flink;

            do
            {
                var module = ReadProcessMemory<LdrModule>(current);
                if (module.BaseDllName.Buffer != IntPtr.Zero)
                {
                    var baseDllName = ReadString(module.BaseDllName.Buffer, module.BaseDllName.Length);
                    if (baseDllName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return module;
                    }
                }

                current = module.InLoadOrderModuleList.Flink;
            }
            while (current != ldr.InLoadOrderModuleList.Flink);

            return null;
        }

        public LdrModule32 GetModuleHandleWow64(string name)
        {
            var wow64info = QueryProcessInformation<ProcessWow64Information>();
            var peb = ReadProcessMemory<Peb32>(wow64info.PebBaseAddress);
            var ldr = ReadProcessMemory<PebLdrData32>(peb.Ldr);
            var current = ldr.InLoadOrderModuleList.Flink;

            do
            {
                var module = ReadProcessMemory<LdrModule32>(current);
                if (module.BaseDllName.Buffer != 0)
                {
                    var baseDllName = ReadString(module.BaseDllName.Buffer, module.BaseDllName.Length);
                    if (baseDllName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return module;
                    }
                }

                current = module.InLoadOrderModuleList.Flink;
            }
            while (current != ldr.InLoadOrderModuleList.Flink);

            return null;
        }

        public class Export
        {
            public string Name { get; }
            public short Ordinal { get; }
            public IntPtr Address { get; }

            public Export(string name, short ordinal, IntPtr address)
            {
                Name = name;
                Ordinal = ordinal;
                Address = address;
            }
        }

        public IEnumerable<Export> GetExports(IntPtr baseAddress)
        {
            var pe = ReadProcessMemory<ImageDosHeader>(baseAddress);
            if (pe.e_magic == ImageSignature.Dos)
            {
                var ntheader = ReadProcessMemory<ImageNtHeaders>(baseAddress + pe.e_lfanew);

                var directoryAddress = IntPtr.Add(baseAddress, (int)ntheader.OptionalHeader.DataDirectory[(int)ImageDirectoryEntry.EXPORT].VirtualAddress);
                var imageExportDirectory = ReadProcessMemory<ImageExportDirectory>(directoryAddress);
                var table = IntPtr.Add(baseAddress, (int)imageExportDirectory.AddressOfNames);

                for (int index = 0; index < imageExportDirectory.NumberOfFunctions; index++)
                {
                    var functionName = string.Empty;
                    //if (function != 0)
                    {
                        // Read offset of name
                        var offset = ReadInt(table + (index * Marshal.SizeOf(typeof(int))));

                        // Read name
                        functionName = ReadStringA(IntPtr.Add(baseAddress, offset), 128);
                    }

                    // Calculate address of ordinal
                    var ordinalAddress = IntPtr.Add(baseAddress, (int) imageExportDirectory.AddressOfNameOrdinals + (index * Marshal.SizeOf(typeof(ushort))));

                    // Read ordinal
                    var ordinal = ReadInt16(ordinalAddress);
                                        
                    // Calculate address of function
                    var functionAddress = IntPtr.Add(baseAddress, (int) imageExportDirectory.AddressOfFunctions + (ordinal * Marshal.SizeOf(typeof(uint))));

                    // Read function offset
                    var function = ReadInt(functionAddress);

                    // Result
                    var address = IntPtr.Add(baseAddress, function);

                    yield return new Export(functionName, ordinal, address);
                }
            }
        }

        public IEnumerable<Export> GetExportsWow64(uint baseAddress)
        {
            var pe = ReadProcessMemory<ImageDosHeader>(baseAddress);
            if (pe.e_magic == ImageSignature.Dos)
            {
                var ntheader = ReadProcessMemory<ImageNtHeaders32>(baseAddress + (uint) pe.e_lfanew);

                var directoryAddress = baseAddress + ntheader.OptionalHeader.DataDirectory[(int)ImageDirectoryEntry.EXPORT].VirtualAddress;
                var imageExportDirectory = ReadProcessMemory<ImageExportDirectory>(directoryAddress);
                var table = baseAddress + imageExportDirectory.AddressOfNames;

                for (int index = 0; index < imageExportDirectory.NumberOfFunctions; index++)
                {
                    
                    // Calculate address of ordinal
                    var ordinalAddress = baseAddress + imageExportDirectory.AddressOfNameOrdinals + (uint) (index * Marshal.SizeOf(typeof(ushort)));

                    // Read ordinal
                    var ordinal = ReadInt16(ordinalAddress);

                    // Calculate address of function
                    var functionAddress = baseAddress + imageExportDirectory.AddressOfFunctions + (uint)(ordinal * Marshal.SizeOf(typeof(uint)));

                    // Read function offset
                    var function = ReadInt(functionAddress);

                    // Result
                    var address = baseAddress + (uint) function;

                    var functionName = string.Empty;
                    if (function != 0)
                    {
                        // Read offset of name
                        var offset = ReadInt(table + (uint)(index * Marshal.SizeOf(typeof(int))));

                        // Read name
                        functionName = ReadStringA(baseAddress + (uint)offset, 128);
                    }

                    yield return new Export(functionName, ordinal, (IntPtr) address);
                }
            }
        }

        public IntPtr GetProcAddress(LdrModule module, string name)
        {
            foreach(var proc in GetExports(module.BaseAddress))
            { 
                if (proc.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return proc.Address;
                }
            }

            return IntPtr.Zero;
        }

        public IntPtr GetProcAddressWow64(LdrModule32 module, string name)
        {
            foreach (var proc in GetExportsWow64(module.BaseAddress))
            {
                if (proc.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return proc.Address;
                }
            }

            return IntPtr.Zero;
        }

        public override int GetHashCode()
        {
            return (int)GetProcessId();
        }
    }
}
