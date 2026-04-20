using System;
using Tewls.Kit.Asm.Stubs;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Asm
{
    public class StubFactory
    {
        
        private readonly NativeProcess _process;
        private readonly StubAllocator _allocator;

        public StubFactory(NativeProcess process)
        {
            _process = process;
            _allocator = new StubAllocator(process);
        }

        /// <summary>
        /// Create a stub
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="stubGenerator"></param>
        /// <returns></returns>
        public IntPtr Create(IntPtr sourceAddress, IStubGenerator stubGenerator)
        {
            var mbi = _process.VirtualQueryEx(sourceAddress);

            // Change protection
            var prevProtection = _process.VirtualProtectEx(mbi.BaseAddress, mbi.RegionSize, MemProtections.ExecuteReadWrite);

#if DEBUG
            Console.WriteLine($"[DEBUG] Protection: {prevProtection}");
#endif

            try
            {
                var buffer = _process.ReadBytes(sourceAddress, Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH);
                // Read the first ZYDIS_MAX_INSTRUCTION_LENGTH bytes of the original method to create the trampoline.
                var relocator = new InstructionRelocator(sourceAddress, buffer, stubGenerator.Size);

#if DEBUG
                relocator.Print();
                Console.WriteLine($"[DEBUG] ASM length: {relocator.Length}");
#endif          
                // We should allocate before reading the original method. Based on where we are able to allocate memory,
                // we can decide which kind of jump we're going to using the stub.
                // If we can allocate memory within 2GB, we can use a relative jump, which is smaller and faster.
                // This does mean we need to allocate a fixed buffer for the stub, which is the size of the original
                // method + the size of the stub.
                var remote = _allocator.AllocateRelativeAddress(sourceAddress, relocator.Length + stubGenerator.Size);

#if DEBUG
                Console.WriteLine($"[DEBUG] Remote stub address: 0x{remote.ToInt64():X}");
                Console.WriteLine($"[DEBUG] Distance: 0x{remote.ToInt64() - sourceAddress.ToInt64():X}");
#endif

                var copy = relocator.Copy(remote);

                // Write the original method to the trampoline.
                _process.WriteBytes(remote, copy);

                // Write the stub to the trampoline, which will jump original method.
                var stub = stubGenerator.GetBuffer(sourceAddress + copy.Length);
                _process.WriteBytes(remote + copy.Length, stub);

                return remote;
            }
            finally
            {
                // Restore original protection
                _process.VirtualProtectEx(mbi.BaseAddress, mbi.RegionSize, prevProtection);

                // Flush
                _process.FlushInstructionCache(mbi.BaseAddress, mbi.RegionSize);
            }
        }
    }
}
