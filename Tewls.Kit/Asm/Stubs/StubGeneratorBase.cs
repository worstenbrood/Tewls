using System;

namespace Tewls.Kit.Asm.Stubs
{
    /// <summary>
    /// Base class for stub generators that provides a singleton instance of the generator. 
    /// This class is generic and requires the derived class to specify itself as the type parameter T. 
    /// The derived class must also have a parameterless constructor to allow for instantiation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class StubGeneratorBase<T> : IStubGenerator
        where T : StubGeneratorBase<T>, new()
    {
        private static readonly Lazy<T> _instance = new(() => new T());

        /// <summary>
        /// Gets the singleton instance of the stub generator.
        /// </summary>
        public static T Instance => _instance.Value;

        /// <inheritdoc />
        public abstract int Size { get; }

        /// <summary>
        /// Gets a new instance of the StubWriter class initialized with the size of the stub.
        /// </summary>
        /// <returns></returns>
        protected StubWriter GetWriter() => new(Size);

        /// <inheritdoc />
        public abstract byte[] GetBuffer(nint address);
    }
}
