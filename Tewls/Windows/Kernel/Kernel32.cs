using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Advapi;

namespace Tewls.Windows.Kernel
{
    public static class Kernel32
    {
        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesWritten);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationType flAllocationType, MemProtections flProtect);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, MemoryBasicInformation lpBuffer, IntPtr dwLength);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemProtections flNewProtect, ref MemProtections lpflOldProtect);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemFreeType dwFreeType);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, SecurityAttributes lpThreadAttributes, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, CreationFlags dwCreationFlags, ref uint lpThreadId);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetProcessId(IntPtr Process);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool IsWow64Process(IntPtr hProcess, ref bool Wow64Process);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessRights dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetProcessInformation(IntPtr hProcess, ProcessInformationClass ProcessInformationClass, IntPtr ProcessInformation, uint ProcessInformationSize);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LocalAlloc(LocalMemFlags uFlags, IntPtr uBytes);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LocalReAlloc(IntPtr hMem, IntPtr uBytes, LocalMemFlags uFlags);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr lstrcpyn(IntPtr lpString1, string lpString2, int iMaxLength);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, IntPtr Length);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle, uint dwDesiredAccess, bool bInheritHandle, DuplicateOptions dwOptions);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern void GetSystemInfo(SystemInfo lpSystemInfo);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport(nameof(Kernel32), CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);




    }
}
