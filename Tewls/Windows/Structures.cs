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

    [StructLayout(LayoutKind.Sequential)]
    public class PromptStruct
    {
        public uint Size;
        public uint PromptFlags;
        public IntPtr HwndApp;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Prompt;
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
