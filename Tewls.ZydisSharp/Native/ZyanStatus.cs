namespace Tewls.ZydisSharp
{
    public static class ZyanStatus
    {
        public static bool Success(int status) => status >= 0;
        public static bool Failed(int status) => status < 0;

        public static void ThrowIfFailed(int status, string apiName)
        {
            if (Failed(status))
            {
                throw new InvalidOperationException($"{apiName} failed: 0x{status:X8}");
            }
        }
    }
}
