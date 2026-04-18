//The code you've provided is a C# wrapper around **Zydis**, a modern, fast, and portable x86/x64 instruction decoder library. It exposes a simplified interface for decoding binary instruction data into structured information such as instruction mnemonic, operands, addressing modes, etc.
//Let’s walk through this class and understand what it does in detail.
//---
//### 🔧 **Namespace & Imports**
//```csharp
//using Tewls.ZydisSharp.Native;
//```
//This line imports native bindings to the underlying Zydis library (likely implemented via P/Invoke or similar interop), which allows C# code to use the C-based Zydis functionality.
//---
//### 🧠 **Class Overview: `ZDecoder`**
//This class is designed to provide a high-level abstraction over the low-level Zydis decoding engine.
//#### ⚙️ **Fields and Constructors**
//```csharp
//private ZydisDecoder _decoder;
//public static ZDecoder Create(ZydisMachineMode machineMode, ZydisStackWidth stackWidth) =>
//    new(Zydis.CreateDecoder(machineMode, stackWidth));
//```
//- The static factory method `Create(...)` creates a new instance of `ZDecoder` with specific machine mode and stack width settings.
//- These settings define:
//  - `ZydisMachineMode`: Whether to decode in 64-bit (`ZYDIS_MACHINE_MODE_LONG_64`) or 32-bit (`ZYDIS_MACHINE_MODE_LEGACY_32`) mode.
//  - `ZydisStackWidth`: Size of the stack pointer (e.g., 32-bit vs 64-bit), used in some edge cases like compatibility mode.
//##### Predefined Factories
//```csharp
//public static ZDecoder Create64() => Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_64);
//public static ZDecoder Create32() => Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LEGACY_32, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);
//public static ZDecoder Create64With32BitStack() => Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);
//```
//These convenience methods allow users to quickly create decoders for common use cases:
//- `Create64()` for standard x64 code
//- `Create32()` for x86 code
//- `Create64With32BitStack()` for rare edge cases where 64-bit mode but 32-bit stack is desired
//##### Internal Constructor
//```csharp
//internal ZDecoder(ZydisDecoder zydisDecoder)
//{
//    _decoder = zydisDecoder;
//}
//```
//- This internal constructor allows only this assembly to construct a `ZDecoder`, ensuring controlled instantiation using the static factory methods.
//---
//### 🧪 **Decoding Methods**
//#### 🔍 `DisassembleIter(...)`
//```csharp
//public ZDecodeResult? DisassembleIter(byte[] buffer, int index = 0, int length = 0) =>
//    Zydis.DecodeFull(ref _decoder, buffer, index, length);
//```
//- Decodes a single instruction from a byte buffer at the specified index.
//- If no more instructions can be decoded, returns `null`.
//- This is a low-level decoding step used internally.
//#### 🔁 `Disassemble(...)`
//```csharp
//public IEnumerable<ZDecodeResult> Disassemble(byte[] buffer, int index = 0, int length = 0)
//{
//    var currentIndex = index;
//    do
//    {
//        var result = DisassembleIter(buffer, currentIndex, length);
//        if (result == null)
//        {
//            yield break;
//        }
//        yield return result;
//        currentIndex += result.Instruction.Length;
//        length = buffer.Length - currentIndex;
//    } while (length > 0);
//}
//```
//- This method takes a byte array and decodes it iteratively, yielding each instruction one by one.
//- It keeps track of the current position in the buffer (`currentIndex`) and continues decoding until all bytes are consumed.
//- Useful for disassembling a whole block of machine code, e.g., when analyzing a function or binary data.
//---
//### 📦 Summary: Key Features
//| Feature | Description |
//|--------|-------------|
//| **Machine Mode Support** | Supports x86 (32-bit) and x64 (64-bit) modes |
//| **Stack Width Options** | Allows for special-case stack width configurations |
//| **Single Instruction Decode** | `DisassembleIter()` decodes one instruction |
//| **Iterative Decoding** | `Disassemble()` allows iterating over full instruction stream |
//| **High-Level Interface** | Simplifies the complex native Zydis API |
//---
//### 🛠️ Example Usage
//```csharp
//var decoder = ZDecoder.Create64();
//byte[] code = { 0x48, 0x89, 0xC0 }; // MOV RAX, RAX
//foreach (var instruction in decoder.Disassemble(code))
//{
//    Console.WriteLine(instruction.Instruction.Mnemonic); // Outputs: "MOV"
//}
//```
//---
//### ✅ Conclusion
//The `ZDecoder` class abstracts the complexity of working directly with the native Zydis API. It offers:
//- Easy-to-use factory methods for common architectures.
//- Flexible iteration over instruction streams.
//- Clean separation between low-level decoding and high-level logic.
//It’s particularly useful in tools like debuggers, emulators, or JIT compilers where accurate disassembly is needed.
//This C# code defines a class `ZDecoder` that wraps and exposes functionality from the **Zydis** library, which is a fast and lightweight x86/x64 instruction decoder. Here's a breakdown of what this code does:
//---
//### 🔧 **Namespace & Using Statement**
//```csharp
//using Tewls.ZydisSharp.Native;
//```
//- This imports native bindings to the **Zydis** library (likely a .NET wrapper around the C-based Zydis decoder).
//---
//### 📦 **Class: `ZDecoder`**
//A helper class for decoding x86/x64 machine code into human-readable instructions using Zydis.
//#### ✅ Fields:
//```csharp
//private ZydisDecoder _decoder;
//```
//- Internal reference to the actual Zydis decoder object.
//---
//### 🚀 **Static Factory Methods**
//#### `Create(...)`
//```csharp
//public static ZDecoder Create(ZydisMachineMode machineMode, ZydisStackWidth stackWidth)
//```
//- Creates a new `ZDecoder` with a specific machine mode and stack width (used for setting up decoding context like 64-bit vs 32-bit).
//#### `Create64()`
//```csharp
//public static ZDecoder Create64()
//```
//- Convenience method for creating a decoder for standard 64-bit x86 code.
//#### `Create32()`
//```csharp
//public static ZDecoder Create32()
//```
//- For 32-bit x86 code.
//#### `Create64With32BitStack()`
//```csharp
//public static ZDecoder Create64With32BitStack()
//```
//- Special case for decoding 64-bit mode with a 32-bit stack width — typically used in rare or advanced scenarios.
//---
//### 🛠️ **Constructors**
//#### `internal ZDecoder(ZydisDecoder zydisDecoder)`
//```csharp
//internal ZDecoder(ZydisDecoder zydisDecoder)
//{
//    _decoder = zydisDecoder;
//}
//```
//- Private constructor to ensure that only factory methods can create instances of `ZDecoder`.
//---
//### 🔍 **Core Decoding Methods**
//#### `DisassembleIter(...)`
//```csharp
//public ZDecodeResult? DisassembleIter(byte[] buffer, int index = 0, int length = 0)
//```
//- Decodes a **single instruction** from the given byte array buffer starting at `index`, up to `length` bytes.
//- Returns `null` if no valid instruction could be decoded.
//- Uses `Zydis.DecodeFull` under the hood.
//#### `Disassemble(...)`
//```csharp
//public IEnumerable<ZDecodeResult> Disassemble(byte[] buffer, int index = 0, int length = 0)
//```
//- Returns an **enumerable** of all decoded instructions from the input buffer.
//- Uses `DisassembleIter` to decode each instruction in sequence.
//- Advances through the buffer by instruction length to find subsequent instructions.
//💡 **Behavior**:
//- Starts at `index`.
//- Decodes one instruction at a time.
//- Advances `currentIndex` by the length of the decoded instruction.
//- Continues until the buffer is exhausted or no more valid instructions are found.
//---
//### 📌 Summary of Functionality
//| Method | Purpose |
//|-------|---------|
//| `Create*()` | Factory methods to initialize decoder for different modes |
//| `DisassembleIter()` | Decode a single instruction |
//| `Disassemble()` | Iterate over entire buffer and yield all instructions |
//---
//### 🧠 Example Usage
//```csharp
//var decoder = ZDecoder.Create64();
//byte[] code = { 0x48, 0x89, 0xC0 }; // MOV RAX, RAX
//foreach (var instr in decoder.Disassemble(code))
//{
//    Console.WriteLine(instr.Instruction.Mnemonic); // Output: "MOV"
//}
//```
//---
//### 📚 Notes
//- This class is part of a .NET wrapper for the **Zydis** C library.
//- It abstracts low-level interactions with the Zydis API for easier use in managed environments.
//- Designed with performance in mind, especially useful for tools like debuggers, disassemblers, or JIT compilers.
//Let me know if you'd like to see how it integrates with other classes like `ZDecodeResult`, or want help writing tests for this!
using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    /// <summary>
    /// 
    /// </summary>
    public class ZDecoder
    {
        private ZydisDecoder _decoder;

        public static ZDecoder Create(ZydisMachineMode machineMode, ZydisStackWidth stackWidth) =>
            new(Zydis.CreateDecoder(machineMode, stackWidth));

        /// <summary>
        /// Standard decoder for normal 64-bit x86 code.
        /// </summary>
        public static ZDecoder Create64() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_64);

        /// <summary>
        /// Decoder for 32-bit x86 code.
        /// </summary>
        public static ZDecoder Create32() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LEGACY_32, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);

        /// <summary>
        /// Decoder for 64-bit mode with a 32-bit stack width.
        /// Advanced/special-case use only.
        /// </summary>
        public static ZDecoder Create64With32BitStack() =>
            Create(ZydisMachineMode.ZYDIS_MACHINE_MODE_LONG_64, ZydisStackWidth.ZYDIS_STACK_WIDTH_32);

        internal ZDecoder(ZydisDecoder zydisDecoder)
        {
            _decoder = zydisDecoder;
        }

        /// <summary>
        /// Decode a single instruction
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public ZDecodeResult? DisassembleInstruction(byte[] buffer, int index = 0, int length = 0) =>
            Zydis.DecodeFull(ref _decoder, buffer, index, length);

        /// <summary>
        /// Fully decodes an instruction from the given buffer. This method will decode the instruction and all of its operands, 
        /// including any implicit operands that may be present. The resulting DecodeResult will contain the decoded instruction 
        /// and its operands, as well as any relevant information about the instruction, such as whether it has rip-relative 
        /// memory or relative immediates.
        /// </summary>
        /// <param name="buffer">Buffer containing the binary instruction data.</param>
        /// <returns><see cref="ZDecodeResult"/></returns>
        public IEnumerable<ZDecodeResult> Disassemble(byte[] buffer, int index = 0, int length = 0)
        {
            var currentIndex = index;
            do
            {
                var result = DisassembleInstruction(buffer, currentIndex, length);
                // No more data
                if (result == null)
                {
                    yield break;
                }

                // Adjust index and length for the next iteration
                currentIndex += result.Instruction.Length;
                length = buffer.Length - currentIndex;                // Return the decoded instruction
                yield return result;

            } while (length > 0);
        }
    }
}
