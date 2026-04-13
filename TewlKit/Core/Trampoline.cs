using System;

namespace TewlKit.Core
{
    /// <summary>
    /// 
    /// </summary>

    public class Trampoline<TFunction>
        where TFunction : Delegate
    {
        // Original method
        public Procedure<TFunction> Original { get; protected set; }

        // Replacement method
        public Procedure<TFunction> Replacement { get; protected set; }

        // Trampoline method

        public Trampoline(IntPtr original, TFunction function)
        {
            Original = new Procedure<TFunction>(original);
            Replacement = new Procedure<TFunction>(function);
        }
    }
}
