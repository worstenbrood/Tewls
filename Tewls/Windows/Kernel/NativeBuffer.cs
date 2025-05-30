﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class NativeBuffer
    {
        public static IntPtr lstrcpyn(IntPtr lpString1, string lpString2)
        {
            return Kernel32.lstrcpyn(lpString1, lpString2, lpString2.Length + 1);
        }

        private static readonly Cache<Type, FieldInfo[]> FieldCache = new Cache<Type, FieldInfo[]>(t => t.GetFields());

        public static int GetObjectSize(object @object)
        {
            var type = @object.GetType();
            var size = Marshal.SizeOf(type);

            foreach (var field in FieldCache[type])
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
            foreach (var field in FieldCache[type])
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
               
        public static void CopyCollectionToBuffer<TStruct>(IEnumerable<TStruct> structures, IntPtr buffer)
            where TStruct : class
        {
            var offset = 0;
            var list = structures.ToList();

            foreach(var structure in list)
            {
                // Copy marshalled structure to our own buffer
                using (var temp = new HGlobalBuffer<TStruct>(structure))
                {
                    Kernel32.CopyMemory(buffer + offset, temp.Buffer, temp.Size);
                }

                // Next record
                offset += Marshal.SizeOf(structure);
            }

            for (int index = 0; index < list.Count; index++)
            {
                var structure = list[index];
                var type = structure.GetType();

                foreach (var field in FieldCache[type])
                {
                    // Ref types
                    if (field.FieldType.IsValueType)
                    {
                        continue;
                    }

                    var value = field.GetValue(list[index]);
                    if (value == null)
                    {
                        continue;
                    }

                    IntPtr destination = IntPtr.Zero;
                    int fieldSize = 0;

                    if (field.FieldType == typeof(string))
                    {
                        var @string = (string)value;
                        fieldSize = (@string.Length + 1) * sizeof(char);

                        // Copy string to buffer
                        destination = IntPtr.Add(buffer, offset);
                        destination = lstrcpyn(destination, @string);
                    }
                    else
                    {
                        // Recursive
                        fieldSize = CopyToBuffer(structure, buffer, offset);
                        destination = IntPtr.Add(buffer, offset);
                    }

                    // Update pointer
                    var fieldOffset = IntPtr.Add(Marshal.OffsetOf(type, field.Name), index * Marshal.SizeOf(structure));
                    Marshal.WriteIntPtr(buffer + (int) fieldOffset, destination);

                    offset += fieldSize;
                }
            }
        }

        public static int CopyToBuffer<TStruct>(TStruct structure, IntPtr buffer, int offset = 0)
            where TStruct: class
        {
            // Copy marshalled structure to our own buffer
            using (var temp = new HGlobalBuffer<TStruct>(structure))
            {
                Kernel32.CopyMemory(buffer + offset, temp.Buffer, temp.Size);
            }

            // Calculate offset
            offset += Marshal.SizeOf(structure);

            var type = structure.GetType();

            foreach (var field in FieldCache[type])
            {
                // Ref types
                if (field.FieldType.IsValueType)
                {
                    continue;
                }

                var value = field.GetValue(structure);
                if (value == null)
                {
                    continue;
                }

                IntPtr destination = IntPtr.Zero;
                int fieldSize = 0;

                if (field.FieldType == typeof(string))
                {
                    var @string = (string)value;
                    fieldSize = (@string.Length + 1) * sizeof(char);

                    // Copy string to buffer
                    destination = IntPtr.Add(buffer, offset);
                    destination = lstrcpyn(destination, @string);
                }
                else
                {
                    // Recursive
                    fieldSize = CopyToBuffer(structure, buffer, offset);
                    destination = IntPtr.Add(buffer, offset);
                }

                // Update pointer
                var fieldOffset = Marshal.OffsetOf(type, field.Name);
                Marshal.WriteIntPtr(buffer + (int)fieldOffset, destination);

                offset += fieldSize;
            }

            return offset;
        }
    }

    public class NativeBuffer<TStruct> : BufferBase<HGlobalBuffer.Allocator>
        where TStruct: class
    {                              
        private int AllocAndCopyToBuffer(TStruct structure)
        {
            Size = (IntPtr) NativeBuffer.GetObjectSize(structure);
            Buffer = Marshal.AllocHGlobal(Size);

            // Build buffer
            return NativeBuffer.CopyToBuffer(structure, Buffer);
        }

        private void AllocAndCopyCollectionToBuffer(IEnumerable<TStruct> structures)
        {
            var list = structures.ToList();
            Size = (IntPtr)list.Aggregate(0, (s, a) => s + NativeBuffer.GetObjectSize(a));
            Buffer = Marshal.AllocHGlobal(Size);

            // Build buffer
            NativeBuffer.CopyCollectionToBuffer(list, Buffer);
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
            AllocAndCopyToBuffer(structure);
        }
        public NativeBuffer(IEnumerable<TStruct> structures)
        {
            AllocAndCopyCollectionToBuffer(structures);
        }
    }
}
