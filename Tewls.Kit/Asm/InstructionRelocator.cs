using System;
using System.Collections.Generic;
using System.Linq;
using Tewls.ZydisSharp;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Asm
{
    public class InstructionRelocator
    {
        public static readonly ZDecoder Decoder = ZDecoder.Create64();
        public static readonly ZFormatter Formatter = new();

        public readonly IntPtr SourceAddress;
        public readonly byte[] Buffer;
        public readonly List<ZDecodeResult> Instructions;
        public readonly int Length;

        private int GetStubSize(int requiredSize)
        {
            var length = 0;
            while (length < requiredSize)
            {
                var instruction = Decoder.DisassembleInstruction(Buffer, length);
                if (instruction == null)
                {
                    break;
                }

                Instructions.Add(instruction);
                length += instruction.Instruction.Length;
            }
            return length;
        }

        public InstructionRelocator(IntPtr sourceAddress, byte[] buffer, int requiredSize)
        {
            SourceAddress = sourceAddress;
            Buffer = buffer;
            Instructions = [];
            Length = GetStubSize(requiredSize);
            Formatter.SetProperty(ZydisFormatterProperty.ZYDIS_FORMATTER_PROP_DETAILED_PREFIXES, new(1));
        }

        public void Print()
        {
            var offset = 0;
            var source = (ulong)SourceAddress.ToInt64();
            foreach (var instruction in Instructions)
            {
                var instructionAddress = source + (uint)offset;
                instruction.TryGetAbsoluteTarget(instructionAddress, out var target);
                Console.WriteLine($"[DEBUG] {instructionAddress:X8}: {Formatter.FormatInstruction(instruction, instructionAddress)} ({target:X8})");
                offset += instruction.Instruction.Length;
            }
        }

        /// <summary>
        /// Copy instructions to the destination, adjusting absolute addresses if necessary.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public byte[] Copy(IntPtr destination)
        {
            var sourceAddress = (ulong)SourceAddress.ToInt64();
            var destinationAddress = (ulong)destination.ToInt64();
            var buffer = new List<byte>();
            int sourceIndex = 0;

            foreach (var result in Instructions)
            {
                var currentAddress = sourceAddress + (uint)sourceIndex;
                if (!result.TryGetAbsoluteTarget(currentAddress, out var target))
                {
                    // Copy raw
                    buffer.AddRange(Buffer.Skip(sourceIndex).Take(result.Instruction.Length));
                }
                else
                {
                    // Adjust address
                    buffer.AddRange(result.EncodeAbsolute(currentAddress, destinationAddress + (uint)buffer.Count));
                }
                sourceIndex += result.Instruction.Length;
            }

            return [.. buffer];
        }
    }
}
