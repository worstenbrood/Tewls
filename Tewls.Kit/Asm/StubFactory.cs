using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tewls.Kit.Asm.Stubs;
using Tewls.Windows.Kernel;
using Tewls.ZydisSharp;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Asm
{
    public class StubFactory
    {
        protected static readonly ZDecoder Decoder = ZDecoder.Create64();
        protected static readonly ZFormatter Formatter = new();

        /// <summary>
        /// <see cref="SystemInfo"/>
        /// </summary>
        public static readonly SystemInfo SystemInfo = SystemInfo.Get();

        private readonly NativeProcess _process;

        public StubFactory(NativeProcess process)
        {
            _process = process;
            Formatter.SetProperty(ZydisFormatterProperty.ZYDIS_FORMATTER_PROP_DETAILED_PREFIXES, new(1));
        }

        /// <summary>
        /// Gets the memory aligned size for a given required size.
        /// </summary>
        /// <param name="requiredSize"></param>
        /// <returns></returns>
        public static long GetAlignedSize(long requiredSize) =>
                (requiredSize + SystemInfo.PageSize - 1) & ~(SystemInfo.PageSize - 1);

        /// <summary>
        /// Try to allocate memory at a specific address in the target process. 
        /// This method checks if the memory region at the given address is free and large enough to accommodate 
        /// the requested size. If it is, it attempts to allocate memory there using VirtualAllocEx. It returns the 
        /// address of the allocated memory if successful, or IntPtr.Zero if it fails. 
        /// The MemoryBasicInformation structure is also returned as an out parameter for debugging purposes.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="mbi"></param>
        /// <returns></returns>
        private IntPtr TryAllocate(IntPtr address, long size, out MemoryBasicInformation mbi)
        {
            // Implementation for trying to allocate memory
            mbi = _process.VirtualQueryEx(address);
            if (mbi.RegionSize == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            long regionBase = mbi.BaseAddress.ToInt64();
            long regionSize = mbi.RegionSize.ToInt64();
#if DEBUG
            Console.WriteLine($"[DEBUG] Checking memory at 0x{address.ToInt64():X}");
            Console.WriteLine($"[DEBUG] Base: 0x{regionBase:X} Size: 0x{regionSize:X}");
#endif

            if (mbi.State == AllocationType.Free && regionSize > size)
            {
#if DEBUG
                Console.WriteLine($"[DEBUG] Found free memory at 0x{regionBase:X} with size 0x{regionSize:X}");
#endif

                var allocated = Kernel32.VirtualAllocEx(
                    _process.Handle,
                    new IntPtr(regionBase),
                    new IntPtr(size),
                    AllocationType.Reserve | AllocationType.Commit,
                    MemProtections.ExecuteReadWrite
                );

                if (allocated != IntPtr.Zero)
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG] Successfully allocated memory at 0x{allocated.ToInt64():X}");
#endif
                    return allocated;
                }
#if DEBUG
                Console.WriteLine($"[DEBUG] Failed to allocate memory at 0x{regionBase:X}");
#endif
            }

            return IntPtr.Zero;
        }
        /// <summary>
        /// Get next positive address to check based on the current memory region information.
        /// If the region size is zero, it simply moves to the next page. Otherwise, it moves to the end of the current region.
        /// </summary>
        /// <param name="currentPositive"></param>
        /// <param name="mbi"></param>
        /// <returns></returns>
        private static long GetNextPositive(long currentPositive, MemoryBasicInformation mbi) =>
            mbi == null ?
            currentPositive + SystemInfo.PageSize :
            mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();

        /// <summary>
        /// Get next negative address to check based on the current memory region information.
        /// </summary>
        /// <param name="currentNegative"></param>
        /// <param name="mbi"></param>
        /// <returns></returns>
        private static long GetNextNegative(long currentNegative, MemoryBasicInformation mbi)
        {
            if (mbi == null)
                return currentNegative - SystemInfo.PageSize;

            long baseToUse = mbi.Type switch
            {
                // Use allocation base for images to avoid skipping large regions that may be reserved
                // for the image but not yet committed.
                MemType.Image => mbi.AllocationBase.ToInt64(),
                // Use the end of the region for mapped and private memory to skip past it.
                _ => mbi.BaseAddress.ToInt64()
            };

            return baseToUse - SystemInfo.PageSize;
        }

        /// <summary>
        /// Allocates memory in the target process within a specified range relative to a base address. 
        /// This is useful for scenarios like code injection where you want the allocated memory to be close
        /// to the target function or module. The method iterates through the memory regions of the target process, 
        /// looking for free space that can accommodate the required size. If a suitable region is found, it 
        /// attempts to allocate memory there and returns the address of the allocated memory. 
        /// If no suitable region is found, it returns IntPtr.Zero.
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="requiredSize"></param>
        /// <param name="minOffset"></param>
        /// <param name="maxOffset"></param>
        /// <returns></returns>
        public IntPtr AllocateRelativeAddress(IntPtr baseAddress, long requiredSize, long minOffset = int.MaxValue,
            long maxOffset = int.MaxValue)
        {
            long alignedSize = GetAlignedSize(requiredSize);
            long start = baseAddress.ToInt64();
            long min = Math.Max(0, start - minOffset);
            long max = start > long.MaxValue - maxOffset ? long.MaxValue : start + maxOffset;

            long currentPositive = start;
            long currentNegative = start - SystemInfo.PageSize;

            while (currentPositive < max || currentNegative >= min)
            {
                if (currentPositive < max)
                {
                    // Allocate in the positive direction first, then negative.
                    // This is because it's more common to find free memory after the base address than before it,
                    // especially if the base address is near the lower end of the address space.
                    IntPtr allocated = TryAllocate(new IntPtr(currentPositive), alignedSize, out var mbi);
                    if (allocated != IntPtr.Zero)
                    {
                        return allocated;
                    }
                    currentPositive = GetNextPositive(currentPositive, mbi);
                }

                if (currentNegative > min)
                {
                    // Allocate in the negative direction if positive allocation fails or if the positive address is out of range.
                    IntPtr allocated = TryAllocate(new IntPtr(currentNegative), alignedSize, out var mbi);
                    if (allocated != IntPtr.Zero)
                    {
                        return allocated;
                    }
                    currentNegative = GetNextNegative(currentNegative, mbi);
                }
            }

            return IntPtr.Zero;
        }

        private static int GetStubSize(byte[]buffer, IStubGenerator stubGenerator, List<ZDecodeResult> result)
        {
            var length = 0;
            while (length < stubGenerator.Size)
            {
                var instruction = Decoder.DisassembleInstruction(buffer, length);
                if (instruction == null)
                {
                    break;
                }
                
                result?.Add(instruction);
                length += instruction.Instruction.Length;
            }
            return length;
        }

        private void CopyInstructions(ulong source, ulong destination, List<ZDecodeResult> list)
        {
            
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
            _process.FlushInstructionCache(mbi.BaseAddress, mbi.RegionSize);
#if DEBUG
            Console.WriteLine($"[DEBUG] Protection: {prevProtection}");
#endif

            try
            {
                // Read the first ZYDIS_MAX_INSTRUCTION_LENGTH bytes of the original method to create the trampoline.
                var buffer = _process.ReadBytes(sourceAddress, Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH);
                var instructions = new List<ZDecodeResult>();
                var length = GetStubSize(buffer, stubGenerator, instructions);

#if DEBUG
                var offset = 0;
                foreach (var instruction in instructions)
                {
                    var instructionAddress = (ulong)(sourceAddress.ToInt64() + offset);
                    var absAddress = instruction.TryGetAbsoluteTarget(instructionAddress, out var target) ? target : 0;
                    Console.WriteLine($"[DEBUG] {instructionAddress:X8}: {Formatter.FormatInstruction(instruction, instructionAddress)} ({target:X8})");
                    offset += instruction.Instruction.Length;
                }

                Console.WriteLine($"[DEBUG] ASM length: {length}");
#endif

                var remote = AllocateRelativeAddress(sourceAddress, length + stubGenerator.Size);

#if DEBUG
                Console.WriteLine($"[DEBUG] Remote stub address: 0x{remote.ToInt64():X}");
                Console.WriteLine($"[DEBUG] Distance: 0x{remote.ToInt64() - sourceAddress.ToInt64():X}");
#endif

                // Write the original method to the trampoline.
                _process.WriteBytes(remote, [.. buffer.Take(length)]);

                // Write the stub to the trampoline, which will jump original method.
                var stub = stubGenerator.GetBuffer(sourceAddress + length);
                _process.WriteBytes(remote + length, stub);

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
