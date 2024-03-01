using System;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class RemoteBuffer : BufferBase
    {
        public class ProcessAllocator : IAllocator
        {
            private readonly NativeProcess _process;

            public IntPtr Alloc(IntPtr size)
            {
                return _process.VirtualAllocEx(size, MemAllocations.Commit | MemAllocations.TopDown, MemProtections.ReadWrite);
            }

            public void Free(IntPtr buffer)
            {
                _process.VirtualFreeEx(buffer, MemFreeType.Release);
            }

            public IntPtr ReAlloc(IntPtr buffer, IntPtr size)
            {
                throw new NotImplementedException();
            }

            public ProcessAllocator(NativeProcess process) 
            {
                _process = process;
            }
        }

        private readonly NativeProcess _process;
        private readonly IAllocator _allocator;
        public override IAllocator GetAllocator => _allocator;

        public RemoteBuffer(NativeProcess process, IntPtr size)
        {
            _process = process;
            _allocator = new ProcessAllocator(_process);
            Buffer = _allocator.Alloc(size);
            Size = size;
        }

        public RemoteBuffer(NativeProcess process, IntPtr buffer, IntPtr size)
        {
            _process = process;
            _allocator = new ProcessAllocator(_process);
            Buffer = buffer;
            Size = size;
        }

        public void Write<TStruct>(TStruct structure, int offset = 0)
            where TStruct : class
        {
            _process.WriteProcessMemory(structure, Buffer + offset);
        }

        public TStruct Read<TStruct>(int offset = 0)
            where TStruct : class, new()
        {
            return _process.ReadProcessMemory<TStruct>(Buffer + offset);
        }
    }
}
