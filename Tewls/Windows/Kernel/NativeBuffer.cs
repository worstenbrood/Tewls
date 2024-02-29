using System;
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

        public static int GetObjectSize(object @object)
        {
            var type = @object.GetType();
            var size = Marshal.SizeOf(type);

            foreach (var field in type.GetFields())
            {
                // Ref types, value types are included in Marshal.SizeOf
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                // Check for null
                var value = field.GetValue(@object);
                if (value == null)
                {
                    continue;
                }

                if (field.FieldType == typeof(string))
                {
                    var @string = (string) value;
                    size += (@string.Length + 1) * sizeof(char);
                }
                else
                {
                    // Recursive
                    size += GetObjectSize(value);
                }
            }

            return size;
        }

        public static void Rebase(Type type, IntPtr buffer, IntPtr from, IntPtr to)
        {
            foreach (var field in type.GetFields())
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                // Offset of field in Buffer
                var fieldOffset = Marshal.OffsetOf(type, field.Name);

                // Pointer to reference in buffer
                var fieldPtr = IntPtr.Add(buffer, fieldOffset.ToInt32());

                // Pointer to ref type
                var ptr = Marshal.ReadIntPtr(fieldPtr);

                // null
                if (ptr == IntPtr.Zero)
                {
                    continue;
                }

                // Calculate offset of pointer in the buffer
                var offset = (int) (ptr.ToInt64() - from.ToInt64());

                // Rebase
                var destination = IntPtr.Add(to, offset);

                // Write new value
                Marshal.WriteIntPtr(fieldPtr, destination);

                if (field.FieldType != typeof(string))
                {
                    // Recursive
                    Rebase(field.FieldType, buffer + offset, from, to);
                }
            }
        }

        public static int ToBuffer(object @object, IntPtr buffer, int offset = 0)
        {
            // Copy marshalled structure to our own buffer
            using (var temp = new HGlobalBuffer<object>(@object))
            {
                CopyMemory(buffer + offset, temp, temp.Size);
            }

            // Calculate offset
            offset += Marshal.SizeOf(@object);

            var type = @object.GetType();

            foreach (var field in type.GetFields())
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                var value = field.GetValue(@object);
                if (value == null)
                {
                    continue;
                }

                if (field.FieldType == typeof(string))
                {
                    var @string = (string)value;
                    var fieldSize = (@string.Length + 1) * sizeof(char);

                    // Copy string to buffer
                    var destination = buffer + offset;
                    destination = lstrcpyn(destination, @string);

                    // Update pointer
                    var fieldOffset = Marshal.OffsetOf(type, field.Name);
                    Marshal.WriteIntPtr(buffer + (int)fieldOffset, destination);

                    offset += fieldSize;
                }
                else
                {
                    // Recursive
                    offset += ToBuffer(@object, buffer, offset);
                }
            }

            return offset;
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
                               
        private int ToBuffer(TStruct structure)
        {
            Size = (IntPtr) NativeBuffer.GetObjectSize(structure);
            Buffer = Marshal.AllocHGlobal(Size);

            // Build buffer
            return NativeBuffer.ToBuffer(structure, Buffer);
        }

        public void Rebase(IntPtr from, IntPtr to)
        {
            NativeBuffer.Rebase(typeof(TStruct), Buffer, from, to);
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
