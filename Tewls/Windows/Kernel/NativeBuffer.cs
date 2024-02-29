using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class NativeBuffer
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr lstrcpyn(IntPtr lpString1, string lpString2, int iMaxLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, IntPtr Length);

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

        private static readonly Type Type = typeof(TStruct);
        private static readonly FieldInfo[] Fields = Type.GetFields();

        public void ToBuffer(TStruct structure)
        {
            var offset = Marshal.SizeOf(structure);

            Size = (IntPtr) GetSize(structure);
            Buffer = Marshal.AllocHGlobal(Size);

            // Copy marshalled structure to our own buffer
            using (var temp = new HGlobalBuffer<TStruct>(structure))
            {
                NativeBuffer.CopyMemory(Buffer, temp, temp.Size);
            }

            foreach (var field in Fields)
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
                    var destination = Buffer + offset;
                    destination = NativeBuffer.lstrcpyn(destination, value);
                    var fieldOffset = Marshal.OffsetOf(Type, field.Name);
                    Marshal.WriteIntPtr(Buffer + (int)fieldOffset, destination);

                    offset += fieldSize;
                }
            }
        }

        public void Rebase(IntPtr from, IntPtr to)
        {
            foreach (var field in Fields)
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                // Offset of field in Buffer
                var fieldOffset = Marshal.OffsetOf(Type, field.Name);
                // Pointer to ref type
                var value = Marshal.ReadIntPtr(IntPtr.Add(Buffer, fieldOffset.ToInt32()));

                if (value == IntPtr.Zero)
                {
                    continue;
                }

                // Calculate offset of pointer in the buffer
                var offset = (int) (value.ToInt64() - from.ToInt64());

                // Rebase
                var destination = IntPtr.Add(to, offset);

                // Write new value
                Marshal.WriteIntPtr(IntPtr.Add(Buffer, fieldOffset.ToInt32()), destination);
            }
        }

        public int GetSize(TStruct structure)
        {
            var size = Marshal.SizeOf(Type);

            foreach (var field in Fields)
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

        public NativeBuffer(TStruct structure) 
        {
            ToBuffer(structure);
        }
    }
}
