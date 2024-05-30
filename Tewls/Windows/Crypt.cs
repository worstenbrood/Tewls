using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Kernel;

namespace Tewls.Windows
{
    public class Crypt
    {
        public static DataBlob ProtectData(DataBlob dataIn, string description, DataBlob optionalEntropy = null)
        {
            var dataOut = new DataBlob();
            var prompt = new PromptStruct();
            var result = Crypt32.CryptProtectData(dataIn, description, optionalEntropy, IntPtr.Zero, prompt, 0, ref dataOut);
            if (!result)
            {
                throw new Win32Exception();
            }

            return dataOut;
        }

        public static string UnprotectData(DataBlob dataIn, CryptProtect flags = 0, DataBlob optionalEntropy = null)
        {
            using (var buffer = new LocalBuffer())
            {
                var dataOut = new DataBlob();
                var result = Crypt32.CryptUnprotectData(dataIn, IntPtr.Zero, optionalEntropy, IntPtr.Zero, IntPtr.Zero, flags, dataOut);
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
