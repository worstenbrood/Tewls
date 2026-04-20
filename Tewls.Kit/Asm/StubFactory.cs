using System;
using Tewls.Kit.Asm.Stubs;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Asm
{
    public class StubFactory
    {
        
        private readonly NativeProcess _process;
        private readonly Allocator _allocator;

        public StubFactory(NativeProcess process)
        {
            _process = process;
            _allocator = new Allocator(process);
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
                // Read the first ZYDIS_MAX_INSTRUCTION_LENGTH bytes of the original method to create the trampoline.
                var copier = new Copier(_process, sourceAddress, Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH, stubGenerator);
                
#if DEBUG
                copier.Print();
                Console.WriteLine($"[DEBUG] ASM length: {copier.Length}");
#endif

                var remote = _allocator.AllocateRelativeAddress(sourceAddress, copier.Length + stubGenerator.Size);

#if DEBUG
                Console.WriteLine($"[DEBUG] Remote stub address: 0x{remote.ToInt64():X}");
                Console.WriteLine($"[DEBUG] Distance: 0x{remote.ToInt64() - sourceAddress.ToInt64():X}");
#endif

                var copy = copier.Copy(remote);

                // Write the original method to the trampoline.
                _process.WriteBytes(remote, copy);

                // Write the stub to the trampoline, which will jump original method.
                var stub = stubGenerator.GetBuffer(sourceAddress + copier.Length);
                _process.WriteBytes(remote + copier.Length, stub);

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
