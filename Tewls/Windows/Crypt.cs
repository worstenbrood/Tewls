using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows
{
    public class Crypt
    {
        public static string UnprotectData(DataBlob dataIn, DataBlob optionalEntropy = null)
        {
            using (var buffer = new LocalBuffer())
            {
                var dataOut = new DataBlob();
                var result = Crypt32.CryptUnprotectData(dataIn, IntPtr.Zero, optionalEntropy, IntPtr.Zero, IntPtr.Zero, 0, dataOut);
                if (!result)
                {
                    throw new Win32Exception();
                }

                // Free buffer
                buffer.Set(dataOut.Data, (IntPtr)dataOut.Size);

                return Marshal.PtrToStringAuto(dataOut.Data, (int) dataOut.Size / sizeof(char));
            }
        }
    }
}
