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
        private readonly NativeProcess _process;
        private readonly StubAllocator _allocator;

        public StubFactory(NativeProcess process)
        {
            _process = process;
            _allocator = new StubAllocator(process);
            Formatter.SetProperty(ZydisFormatterProperty.ZYDIS_FORMATTER_PROP_DETAILED_PREFIXES, new(1));
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
            var buffer = new byte[list.Count * Zydis.ZYDIS_MAX_INSTRUCTION_LENGTH];
            /*
            
            ulong length = (ulong)buffer.Length;
            var result = Zydis.ZydisEncoderEncodeInstructionAbsolute(ref _request, buffer, ref length, destination);
            result.ThrowIfFailed(nameof(Zydis.ZydisEncoderEncodeInstructionAbsolute));
            Array.Resize(ref buffer, (int)length);
            return buffer;*/
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

                var remote = _allocator.AllocateRelativeAddress(sourceAddress, length + stubGenerator.Size);

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
