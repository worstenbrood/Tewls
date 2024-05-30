using System;
using System.Runtime.InteropServices;

namespace Tewls.Windows
{
    public static class Crypt32
    {
        [DllImport(nameof(Crypt32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptProtectData(DataBlob pDataIn, string szDataDescr, DataBlob pOptionalEntropy, IntPtr pvReserved, PromptStruct pPromptStruct, CryptProtect dwFlags, ref DataBlob pDataOut);
        
        [DllImport(nameof(Crypt32), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptUnprotectData(DataBlob pDataIn, IntPtr ppszDataDescr, DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, CryptProtect dwFlags, DataBlob pDataOut);
    }
}
