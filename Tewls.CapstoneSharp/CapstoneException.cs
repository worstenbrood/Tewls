using Tewls.CapstoneSharp.Native;

namespace Tewls.CapstoneSharp
{
    /// <summary>
    /// Capstone exception that is thrown when a Capstone API call returns an error code. 
    /// The exception message includes the error code and the corresponding error message from Capstone.
    /// </summary>
    public class CapstoneException : Exception
    {
        public CapstoneException(cs_err err) : base(Capstone.GetErrorMessage(err)) { }
        public CapstoneException(cs_err err, string message) : base($"{message} (Error({(int)err}): {Capstone.GetErrorMessage(err)})") { }
        public CapstoneException(cs_err err, Exception innerException) : base(Capstone.GetErrorMessage(err), innerException) { }
        public CapstoneException(cs_err err, string message, Exception innerException) :
            base($"{message} (Error({(int)err}): {Capstone.GetErrorMessage(err)})", innerException) { }

        public static void ThrowLastError(nint handle) => throw new CapstoneException(Capstone.cs_errno(handle));   
    } 
}