using System;
using System.Collections.Generic;
using System.Linq;
using Tewls.ZydisSharp;
using Tewls.ZydisSharp.Native;

namespace Tewls.Kit.Asm
{
    /// <summary>
    /// Instrcution relocator
    /// </summary>
    public class InstructionRelocator
    {
        public static readonly ZDecoder Decoder = ZDecoder.Create64();
        public static readonly ZFormatter Formatter = new();

        /// <summary>
        /// Source address of the instructions to be relocated. 
        /// This is the address of the original method that we want to hook.
        /// </summary>
        public readonly IntPtr SourceAddress;

        /// <summary>
        /// Buffer 
        /// </summary>
        public readonly byte[] Buffer;

        /// <summary>
        /// Decoded instructions of <see cref="Buffer"/>
        /// </summary>
        public readonly List<ZDecodeResult> Instructions;

        /// <summary>
        /// Total instruction length
        /// </summary>
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

        /// <summary>
        /// Constructor for the instruction relocator. 
        /// It disassembles the instructions at the source address and calculates the total length of the 
        /// instructions to be relocated, which should be at least the size of the stub. 
        /// The instructions are stored in a list for later use when copying to the destination.
        /// </summary>
        /// <param name="sourceAddress"></param>
        /// <param name="buffer"></param>
        /// <param name="requiredSize"></param>
        public InstructionRelocator(IntPtr sourceAddress, byte[] buffer, int requiredSize)
        {
            SourceAddress = sourceAddress;
            Buffer = buffer;
            Instructions = [];
            Length = GetStubSize(requiredSize);
            Formatter.SetProperty(ZydisFormatterProperty.ZYDIS_FORMATTER_PROP_DETAILED_PREFIXES, new(1));
        }

        /// <summary>
        /// Print the original instructions
        /// </summary>
        public void Print()
        {
            var offset = 0;
            var source = (ulong)SourceAddress.ToInt64();
            foreach (var instruction in Instructions)
            {
                var instructionAddress = source + (uint)offset;
                Console.WriteLine($"[DEBUG] {instructionAddress:X}: {Formatter.FormatInstruction(instruction, instructionAddress)}");
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
