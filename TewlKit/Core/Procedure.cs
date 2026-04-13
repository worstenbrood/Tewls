using System;
using System.Runtime.InteropServices;

namespace TewlKit.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Procedure<T>
        where T : Delegate
    {
        /// <summary>
        /// Delegate
        /// </summary>
        public readonly T Method;

        /// <summary>
        /// Memory address
        /// </summary>
        public readonly nint Address;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public Procedure(T d)
        {
            Method = d;
            // Get function pointer
            Address = Marshal.GetFunctionPointerForDelegate(Method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        public Procedure(nint d)
        {
            Address = d;
            // Get delegate
            Method = (T)Marshal.GetDelegateForFunctionPointer(d, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public static implicit operator nint(Procedure<T> f) => f.Address;
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public static implicit operator T(Procedure<T> f) => f.Method;
    }
}
