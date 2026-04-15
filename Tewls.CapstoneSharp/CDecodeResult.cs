using Tewls.CapstoneSharp.Native;

namespace Tewls.CapstoneSharp
{
    /// <summary>
    /// Decoding result that contains the instruction and its detail (if available). 
    /// </summary>
    /// <param name="instruction"></param>
    /// <param name="detail"></param>
    public class CDecodeResult(cs_insn instruction, cs_detail? detail)
    {
        public cs_insn Instruction { get; } = instruction;
        public cs_detail? Detail { get; } = detail;
        public bool HasDetail => Detail.HasValue;
    }
}
