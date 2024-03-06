using System;
using Tewls.Windows.Utils;

namespace Tewls.Windows.Kernel
{
    public class RemoteBuffer : BufferBase
    {
        public class ProcessAllocator : IMemory
        {
            private readonly NativeProcess _process;

            public IntPtr Alloc(IntPtr size)
            {
                return _process.VirtualAllocEx(size, AllocationType.Commit | AllocationType.TopDown, MemProtections.ReadWrite);
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

        protected readonly NativeProcess _process;
        private readonly IMemory _memory;

        public override IMemory Memory => _memory;

        protected RemoteBuffer(NativeProcess process)
        {
            _process = process;
            _memory = new ProcessAllocator(_process);
        }

        public RemoteBuffer(NativeProcess process, IntPtr size) : this(process)
        {
            Set(Alloc(size), size);
        }

        public RemoteBuffer(NativeProcess process, IntPtr buffer, IntPtr size) : this(process)
        {
            Set(buffer, size);
        }

        public RemoteBuffer(NativeProcess process, string s) : this(process, (IntPtr)((s.Length + 1) * sizeof(char)))
        {
            process.WriteString(this, s);
        }

        public RemoteBuffer(NativeProcess process, byte[] bytes) : this(process)
        {
            using (var localBuffer = new HGlobalBuffer(bytes))
            {
                Size = localBuffer.Size;
                Buffer = Alloc(Size);
                process.WriteProcessMemory(Buffer, localBuffer.Buffer, Size);
            }
        }

        public RemoteBuffer Write<TStruct>(TStruct structure, int offset = 0)
            where TStruct : class
        {
            _process.WriteProcessMemory(structure, Buffer + offset);
            return this;
        }

        public TStruct Read<TStruct>(int offset = 0)
            where TStruct : class, new()
        {
            return _process.ReadProcessMemory<TStruct>(Buffer + offset);
        }

        public string ReadString()
        {
            return _process.ReadString(this);
        }

        public RemoteBuffer WriteString(string s)
        {
            return _process.WriteString(this, s);
        }

        public MemoryBasicInformation GetInformation()
        {
            return _process.VirtualQueryEx(Buffer);
        }

        public RemoteBuffer Protect(MemProtections protection)
        {
            _process.VirtualProtectEx(this, protection);
            return this;
        }
    }

    public class RemoteBuffer<TStruct> : RemoteBuffer
        where TStruct : class, new()
    {
        public RemoteBuffer(NativeProcess process, TStruct structure) : base(process, (IntPtr)NativeBuffer.GetObjectSize(structure))
        {
            process.WriteProcessMemory(structure, Buffer);
        }

        public TStruct Read(int offset = 0)
        {
            return _process.ReadProcessMemory<TStruct>(Buffer + offset);
        }

        public RemoteBuffer<TStruct> Write(TStruct structure, int offset = 0)
        {
            _process.WriteProcessMemory(structure, Buffer + offset);
            return this;
        }

        public new RemoteBuffer<TStruct> Protect(MemProtections protection)
        {
            _process.VirtualProtectEx(this, protection);
            return this;
        }
    }
}
