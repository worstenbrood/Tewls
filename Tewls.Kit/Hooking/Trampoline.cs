using System;
using Tewls.Kit.Utils;

namespace Tewls.Kit.Hooking
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="original"></param>
    /// <param name="stub"></param>
    /// <param name="replacement"></param>

    public class Trampoline<TFunction>(nint original, nint stub, TFunction replacement) : GCHandled
        where TFunction : Delegate
    {
        /// <summary>
        /// Original method and delegate.
        /// </summary>
        public Procedure<TFunction> Original { get; protected set; } = new Procedure<TFunction>(original);

        /// <summary>
        /// Stub method and delegate.
        /// </summary>
        public Procedure<TFunction> Stub { get; protected set; } = new Procedure<TFunction>(stub);

        /// <summary>
        /// Replacement method and delegate.
        /// </summary>
        public Procedure<TFunction> Replacement { get; protected set; } = new Procedure<TFunction>(replacement);
    }
}
