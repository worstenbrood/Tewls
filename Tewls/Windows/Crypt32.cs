using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows
{
    public static class Crypt32
    {
        [DllImport("Crypt32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptUnprotectData(DataBlob pDataIn, IntPtr ppszDataDescr, DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, uint dwFlags, DataBlob pDataOut);

    }
}
