using System;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class NativeBuffer
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr lstrcpyn(IntPtr lpString1, string lpString2, int iMaxLength);

        public static IntPtr lstrcpyn(IntPtr lpString1, string lpString2)
        {
            return lstrcpyn(lpString1, lpString2, lpString2.Length + 1);
        }
    }

    public class NativeBuffer<TStruct> : BufferBase<HGlobalBuffer.Allocator>
        where TStruct: class
    {
        public class Allocator : IAllocator
        {
            public IntPtr Alloc(IntPtr size)
            {
                return Marshal.AllocHGlobal(size);
            }

            public void Free(IntPtr buffer)
            {
                Marshal.FreeHGlobal(buffer);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                return Marshal.ReAllocHGlobal(buffer, size);
            }
        }

        public void ToBuffer(TStruct structure)
        {
            var classType = typeof(TStruct);
            var size = Marshal.SizeOf(structure);
            Marshal.StructureToPtr(structure, Buffer, false);

            foreach (var field in classType.GetFields())
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                if (field.FieldType == typeof(string))
                {
                    var value = (string)field.GetValue(structure);
                    if (value == null)
                    {
                        continue;
                    }

                    var fieldSize = (value.Length + 1) * sizeof(char);
                    Buffer = Marshal.ReAllocHGlobal(Buffer, (IntPtr)size + fieldSize);
                    var destination = Buffer + size;
                    var r = NativeBuffer.lstrcpyn(destination, value);
                    var fieldOffset = Marshal.OffsetOf(classType, field.Name);
                    Marshal.WriteIntPtr(Buffer + (int)fieldOffset, r);

                    // Increase size                    
                    size += fieldSize;
                }
            }
        }

        public void Rebase(IntPtr baseAddress)
        {
            var classType = typeof(TStruct);

            foreach (var field in classType.GetFields())
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                // Offset of field in Buffer
                var fieldOffset = Marshal.OffsetOf(classType, field.Name);
                // Pointer to ref type
                var value = Marshal.ReadIntPtr(IntPtr.Add(Buffer, fieldOffset.ToInt32()));

                if (value == IntPtr.Zero)
                {
                    continue;
                }

                var offset = (int)(value.ToInt64() - Buffer.ToInt64());
                var destination = IntPtr.Add(baseAddress, offset);
                Marshal.WriteIntPtr(IntPtr.Add(Buffer, fieldOffset.ToInt32()), destination);
            }
        }

        public int GetSize(TStruct structure)
        {
            var classType = typeof(TStruct);
            var size = Marshal.SizeOf(classType);

            foreach (var field in classType.GetFields())
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                if (field.FieldType == typeof(string))
                {
                    var value = (string)field.GetValue(structure);
                    if (value == null)
                    {
                        continue;
                    }

                    size += (value.Length + 1) * sizeof(char);
                }
            }

            return size;
        }



        public NativeBuffer(IntPtr size) : base(size)
        {
        }

        public NativeBuffer(TStruct structure) : base ((IntPtr) Marshal.SizeOf(typeof(TStruct)))
        {
            ToBuffer(structure);
        }
    }
}
