using System;
using Tewls.Windows.Kernel;

namespace Tewls.Kit.Asm
{
    public class StubFactory(NativeProcess process)
    {
        /// <summary>
        /// <see cref="SystemInfo"/>
        /// </summary>
        public static readonly SystemInfo SystemInfo = SystemInfo.Get();

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
            mbi = process.VirtualQueryEx(address);
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
                    process.Handle,
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
                MemType.Image => mbi.AllocationBase.ToInt64(),
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
                    IntPtr allocated = TryAllocate(new IntPtr(currentPositive), alignedSize, out var mbi);
                    if (allocated != IntPtr.Zero)
                    {
                        return allocated;
                    }
                    currentPositive = GetNextPositive(currentPositive, mbi);
                }

                if (currentNegative > min)
                {
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
    }
}
