using System;

namespace Tewls.Kit.Asm.Stubs
{
    /// <summary>
    /// Stub type enum
    /// </summary>
    public enum StubType
    {
        IndirectEax,
        IndirectRax,
        Relative
    }

    /// <summary>
    /// Stub factory for creating different types of stubs based on the specified stub type.
    /// </summary>
    public static class StubFactory
    {
        /// <summary>
        /// Create a stub generator based on the specified stub type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IStubGenerator Get(StubType type)
        {
            return type switch
            {
                StubType.IndirectEax => IndirectJumpEaxStub.Instance,
                StubType.IndirectRax => IndirectJumpRaxStub.Instance,
                StubType.Relative => RelativeJumpStub.Instance,
                _ => throw new ArgumentException($"Unsupported stub type: {type}")
            };
        }
    }
}
