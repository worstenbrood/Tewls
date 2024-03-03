using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;
using Tewls.Windows.Utils;

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

    public class Crypt
    {
        [DllImport("Crypt32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptUnprotectData(DataBlob pDataIn, IntPtr ppszDataDescr, DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, DataBlob pDataOut);

        public static string UnprotectData(DataBlob dataIn, DataBlob optionalEntropy = null)
        {
            using (var buffer = new LocalBuffer())
            {
                var dataOut = new DataBlob();
                var result = CryptUnprotectData(dataIn, IntPtr.Zero, optionalEntropy, IntPtr.Zero, IntPtr.Zero, 0, dataOut);
                
                // Free buffer
                buffer.Set(dataOut.Data,(IntPtr) dataOut.Size);

                if (!result)
                {
                    throw new Win32Exception();
                }

                return Marshal.PtrToStringAuto(dataOut.Data, (int) dataOut.Size / sizeof(char));
            }
        }
    }
}
