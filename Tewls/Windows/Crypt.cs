using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    public class DataBlob
    {
        public uint Size;
        public IntPtr Data;
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
        [DllImport("Crypt32.dll", EntryPoint = "CryptUnprotectData", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CryptUnprotectData(DataBlob pDataIn, IntPtr ppszDataDescr, DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, DataBlob pDataOut);

        public static string UnprotectData(DataBlob dataIn, DataBlob optionalEntropy = null)
        {
            using (var buffer = new HGlobalBuffer())
            {
                var dataOut = new DataBlob();
                var result = CryptUnprotectData(dataIn, IntPtr.Zero, optionalEntropy, IntPtr.Zero, IntPtr.Zero, 0, dataOut);
                
                // Free buffer
                buffer.Buffer = dataOut.Data;
                buffer.Size = (IntPtr) dataOut.Size;

                if (!result)
                {
                    throw new Win32Exception();
                }

                return Marshal.PtrToStringAuto(dataOut.Data, (int) dataOut.Size / sizeof(char));
            }
        }
    }
}
