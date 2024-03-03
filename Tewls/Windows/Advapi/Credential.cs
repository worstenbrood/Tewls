using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    [StructLayout(LayoutKind.Sequential)]
    public class Credential
    {
        public CredFlags Flags;
        public CredType Type;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TargetName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Comment;
        public FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CredPersist Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string TargetAlias;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string UserName;

        private string DecryptBlob(Guid guid)
        {
            try
            {
                var data = new DataBlob
                {
                    Data = CredentialBlob,
                    Size = CredentialBlobSize
                };

                var array = guid.ToByteArray();
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] *= 4;
                }

                using (var buffer = new HGlobalBuffer(array))
                {

                    var entropy = new DataBlob
                    {
                        Data = buffer.Buffer,
                        Size = (uint)buffer.Size
                    };
                    var result = Crypt.UnprotectData(data, entropy);
                    return result;
                }
            }
            catch
            {
                return Marshal.PtrToStringAuto(CredentialBlob, (int)CredentialBlobSize / sizeof(char));
            }
        }

        public string GetPassword()
        {
            if (CredentialBlob == IntPtr.Zero)
            {
                return null;
            }

            switch (Type)
            {
                case CredType.VisiblePassword:
                    return DecryptBlob(Guid.Parse("82BD0E67-9FEA-4748-8672-D5EFE5B779B0"));

                case CredType.Generic:
                    return DecryptBlob(Guid.Parse("abe2869f-9b47-4cd9-a358-c22904dba7f7"));

                default:
                    return Marshal.PtrToStringAuto(CredentialBlob, (int)CredentialBlobSize / sizeof(char));
            }
        }
    };
}
