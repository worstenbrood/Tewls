using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    public class DataBlob
    {
        public uint Size;
        public IntPtr Data;

        public DataBlob()
        {
        }

        public DataBlob(IntPtr data, uint size)
        {
            Size = size;
            Data = data;
        }
    }

    [Flags]
    public enum CryptProtectPrompt : uint
    {
        OnProtect = 0x1,
        OnUnprotect = 0x2,
        Reserved = 0x4,
        Strong = 0x8,
        RequireStrong = 0x10
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PromptStruct
    {
        public uint Size;
        public CryptProtectPrompt PromptFlags;
        public IntPtr HwndApp;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Prompt;
    }

    [Flags]
    public enum CryptProtect : uint
    {
        UiForbidden = 0x1,
        LocalMachine = 0x4,
        CredSync = 0x8,
        Audit = 0x10,
        NoRecovery = 0x20,
        VerifyProtection = 0x40,
        CredRegenerate = 0x80,
    }

    public interface IInfo<T>
    {
        T GetLevel();
    }

    public interface IClass<T>
    {
        T GetClass();
    }    
}
