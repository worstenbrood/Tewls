using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
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
            _processId = (uint)processId;
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
            WriteProcessMemory(structure, remoteBuffer.Buffer);
            return remoteBuffer;
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

        public ushort ReadInt16(IntPtr remoteBuffer)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr)Marshal.SizeOf(typeof(short))))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                return (ushort) Marshal.ReadInt16(localBuffer.Buffer);
            }
        }

        public ushort ReadInt16(uint remoteBuffer)
        {
            return ReadInt16((IntPtr)remoteBuffer);
        }

        public byte[] ReadBytes(IntPtr remoteBuffer, int size)
        {
            using (var localBuffer = new HGlobalBuffer((IntPtr) size))
            {
                ReadProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
                var result = new byte[size];
                Marshal.Copy(localBuffer.Buffer, result, 0, size);
                return result;
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
            var remoteBuffer = VirtualAllocExBuffer((IntPtr)size, AllocationType.Commit | AllocationType.Reserve | AllocationType.TopDown, MemProtections.ReadWrite);
            return WriteString(remoteBuffer, s);
        }

        public void WriteBytes(IntPtr remoteBuffer, byte[] bytes)
        {
            using (var localBuffer = new HGlobalBuffer(bytes))
            {
                WriteProcessMemory(remoteBuffer, localBuffer.Buffer, localBuffer.Size);
            }
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

        private uint _processId;

        public uint ProcessId => _processId > 0 ? _processId : _processId = GetProcessId();

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
            try
            {
                return IsWow64Process(Handle);
            }
            // Native method not found
            catch (EntryPointNotFoundException)
            { 
                return false; 
            }
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

        public IEnumerable<NativeModule> GetModules()
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
                    yield return new NativeModule(this, baseDllName, module.BaseAddress, module.SizeOfImage, Environment.Is64BitProcess, false);
                }

                current = module.InLoadOrderModuleList.Flink;
            }
            while (current != ldr.InLoadOrderModuleList.Flink);
        }

        public NativeModule GetModule(string name)
        {
            return GetModules()
                .FirstOrDefault(module => module.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<NativeModule> GetModulesWow64()
        {
            if (!IsWow64Process())
            {
                yield break;
            }

            var wow64Info = QueryProcessInformation<ProcessWow64Information>();
            if (wow64Info.PebBaseAddress == IntPtr.Zero)
            {
                yield break;
            }

            var peb = ReadProcessMemory<Peb32>(wow64Info.PebBaseAddress);
            var ldr = ReadProcessMemory<PebLdrData32>(peb.Ldr);
            var current = ldr.InLoadOrderModuleList.Flink;

            do
            {
                var module = ReadProcessMemory<LdrModule32>(current);
                if (module.BaseDllName.Buffer != 0)
                {
                    var baseDllName = ReadString(module.BaseDllName.Buffer, module.BaseDllName.Length);
                    yield return new NativeModule(this, baseDllName, (IntPtr)module.BaseAddress, module.SizeOfImage, false, true);
                }

                current = module.InLoadOrderModuleList.Flink;
            }
            while (current != ldr.InLoadOrderModuleList.Flink);
        }

        public NativeModule GetModuleWow64(string name)
        {
            return GetModulesWow64()
                .FirstOrDefault(module => module.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<NativeModule> GetAllModules()
        {
            return GetModules().Skip(1).Concat(GetModulesWow64().Skip(1));
        }
        
        public IEnumerable<NativeExport> GetExports(IntPtr baseAddress)
        {  
            var pe = ReadProcessMemory<ImageDosHeader>(baseAddress);
            if (pe.e_magic != ImageSignature.Dos)
            {
                throw new Exception($"Invalid dos header signature: 0x{pe.e_magic:x}");
            }

            var ntheader = ReadProcessMemory<ImageNtHeaders>(baseAddress + pe.e_lfanew);
            if (ntheader.Signature != ImageNt.Signature)
            {
                throw new Exception($"Invalid nt header signature: 0x{ntheader.Signature:x}");
            }

            var dataDirectory = ntheader.OptionalHeader.DataDirectory[ImageDirectoryEntry.Export];
            if (dataDirectory.VirtualAddress == 0)
            {
                yield break;
            }

            var directoryAddress = IntPtr.Add(baseAddress, (int)dataDirectory.VirtualAddress);
            var imageExportDirectory = ReadProcessMemory<ImageExportDirectory>(directoryAddress);

            var names = IntPtr.Add(baseAddress, (int)imageExportDirectory.AddressOfNames);
            var ordinals = IntPtr.Add(baseAddress, (int)imageExportDirectory.AddressOfNameOrdinals);
            var functions = IntPtr.Add(baseAddress, (int)imageExportDirectory.AddressOfFunctions);

            for (var index = 0; index < imageExportDirectory.NumberOfFunctions; index++)
            {
                var ordinalIndex = index * Marshal.SizeOf(typeof(short));

                // Calculate address of ordinal
                var ordinalAddress = IntPtr.Add(ordinals, ordinalIndex);

                // Read ordinal
                var ordinal = ReadInt16(ordinalAddress);

                if (ordinal > imageExportDirectory.NumberOfFunctions)
                {
                    // Forward
                    continue;
                }

                var functionIndex = ordinal * Marshal.SizeOf(typeof(int));

                // Calculate address of function
                var functionAddress = IntPtr.Add(functions, functionIndex);              

                // Read function offset
                var function = ReadInt(functionAddress);

                // Result
                var address = IntPtr.Add(baseAddress, function);

                string functionName = null;
                if (index < imageExportDirectory.NumberOfNames)
                {
                    var nameIndex = index * Marshal.SizeOf(typeof(int));

                    // Read offset of name
                    var offset = ReadInt(IntPtr.Add(names, nameIndex));

                    // Read name
                    functionName = ReadStringA(IntPtr.Add(baseAddress, offset), 255);
                }

                yield return new NativeExport(functionName, ordinal, address);
            }
        }

        public IEnumerable<NativeExport> GetExportsWow64(uint baseAddress)
        {
            var pe = ReadProcessMemory<ImageDosHeader>(baseAddress);
            if (pe.e_magic != ImageSignature.Dos)
            {
                throw new Exception($"Invalid dos header signature: 0x{pe.e_magic:x}");
            }

            var ntheader = ReadProcessMemory<ImageNtHeaders32>(baseAddress + (uint) pe.e_lfanew);
            if (ntheader.Signature != ImageNt.Signature)
            {
                throw new Exception($"Invalid nt header signature: 0x{ntheader.Signature:x}");
            }

            var dataDirectory = ntheader.OptionalHeader.DataDirectory[ImageDirectoryEntry.Export];
            if (dataDirectory.VirtualAddress == 0)
            {
                yield break;
            }

            var directoryAddress = baseAddress + dataDirectory.VirtualAddress;
            var imageExportDirectory = ReadProcessMemory<ImageExportDirectory>(directoryAddress);

            var names = baseAddress + imageExportDirectory.AddressOfNames;
            var ordinals = baseAddress + imageExportDirectory.AddressOfNameOrdinals;
            var functions = baseAddress + imageExportDirectory.AddressOfFunctions;

            for (var index = 0; index < imageExportDirectory.NumberOfFunctions; index++)
            {
                var ordinalIndex = index * Marshal.SizeOf(typeof(ushort));

                // Calculate address of ordinal
                var ordinalAddress = ordinals + (uint)ordinalIndex;

                // Read ordinal
                var ordinal = ReadInt16(ordinalAddress);
                if (ordinal > imageExportDirectory.NumberOfFunctions)
                {
                    // Forward
                    continue;
                }

                var functionIndex = ordinal * Marshal.SizeOf(typeof(uint));

                // Calculate address of function
                var functionAddress = functions + (uint)functionIndex;

                // Read function offset
                var function = ReadInt(functionAddress);

                // Result
                var address = baseAddress + (uint) function;

                string functionName = null;
                if (index < imageExportDirectory.NumberOfNames)
                {
                    var nameIndex = index * Marshal.SizeOf(typeof(int));

                    // Read offset of name
                    var offset = ReadInt(names + (uint)nameIndex);

                    // Read name
                    functionName = ReadStringA(baseAddress + (uint)offset, 255);
                }

                yield return new NativeExport(functionName, ordinal, (IntPtr)address);
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

        public override string ToString()
        {
            return $"ProcessId: {ProcessId}";
        }

        public override int GetHashCode()
        {
            return (int)ProcessId;
        }
    }
}
