using System.Runtime.InteropServices;
using System.Text;
using Tewls.ZydisSharp.Native;

namespace Tewls.ZydisSharp
{
    public class ZFormatter : IDisposable
    {
        /// <summary>
        /// Size of the formatter structure in bytes
        /// </summary>
        private const int FormatterSize = 1024;

        /// <summary>
        /// Size of the buffer for formatted instruction
        /// </summary>
        private const int BufferSize = 256;

        /// <summary>
        /// We just allocate a block of memory for the formatter and pass it to the native code. 
        /// The formatter structure is opaque to us, so we don't need to know its internal layout. 
        /// We just need to ensure that we allocate enough memory for it and free it when we're done.
        /// You can use <see cref="SetProperty"/> to configure the formatter.
        /// </summary>
        private readonly IntPtr _formatter;

        /// <summary>
        /// Constructor that initializes the formatter with the specified style.
        /// </summary>
        /// <param name="style"></param>
        public ZFormatter(ZydisFormatterStyle style = ZydisFormatterStyle.ZYDIS_FORMATTER_STYLE_MASM) 
        { 
            _formatter = Marshal.AllocHGlobal(FormatterSize);
            var status = Zydis.ZydisFormatterInit(_formatter, style);
            if (status.Failed)
            {
                Marshal.FreeHGlobal(_formatter);
                status.Throw(nameof(Zydis.ZydisFormatterInit));
            }
        }

        /// <summary>
        /// Set a property of the formatter. The value is passed as an IntPtr, 
        /// which can be used to pass different types of values depending on the property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetProperty(ZydisFormatterProperty property, IntPtr value)
        {
            var status = Zydis.ZydisFormatterSetProperty(_formatter, property, value);
            status.ThrowIfFailed(nameof(Zydis.ZydisFormatterSetProperty));
        }

        /// <summary>
        /// Format an instruction using the formatter. The result is written to a buffer and returned as a string.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="runtimeAddress"></param>
        /// <returns></returns>
        public string FormatInstruction(ZDecodeResult result, ulong runtimeAddress = 0)
        {
            var buffer = new byte[BufferSize];
                     
            // Format instruction
            var status = Zydis.ZydisFormatterFormatInstruction(_formatter, ref result.Instruction, result.Operands, 
                result.Instruction.OperandCountVisible, buffer, (uint)buffer.Length, runtimeAddress, IntPtr.Zero);
            status.ThrowIfFailed(nameof(Zydis.ZydisFormatterFormatInstruction));

            // Return ascii string from the buffer
            return Encoding.ASCII.GetString(buffer);
        }

        /// <summary>
        /// Dispose method to free the allocated memory for the formatter. 
        /// This should be called when the formatter is no longer needed to avoid memory leaks.
        /// </summary>
        public void Dispose() => Marshal.FreeHGlobal(_formatter);
    }
}
