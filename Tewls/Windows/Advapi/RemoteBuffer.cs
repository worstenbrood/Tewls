using System;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Advapi
{
    public class RemoteBuffer : BufferBase
    {
        public class ProcessAllocator : IAllocator
        {
            private NativeProcess _process;

            public IntPtr Alloc(IntPtr size)
            {
                throw new NotImplementedException();
            }

            public void Free(IntPtr buffer)
            {
                _process.VirtualFreeEx(buffer);
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

        private readonly IAllocator _allocator;
        public override IAllocator GetAllocator => _allocator;

        public RemoteBuffer(NativeProcess process, IntPtr buffer)
        {
            _allocator = new ProcessAllocator(process);
            Buffer = buffer;
        }
    }
}
