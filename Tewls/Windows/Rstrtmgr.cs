using System;
using System.Runtime.InteropServices;
using static Tewls.Windows.RestartManager;

namespace Tewls.Windows
{
    public static class Rstrtmgr
    {
        [DllImport(nameof(Rstrtmgr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error RmStartSession(ref int pSessionHandle, int dwSessionFlags, IntPtr strSessionKey);

        [DllImport(nameof(Rstrtmgr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error RmEndSession(int dwSessionHandle);

        [DllImport(nameof(Rstrtmgr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error RmGetList(int dwSessionHandle, ref uint pnProcInfoNeeded, ref uint pnProcInfo, IntPtr rgAffectedApps, ref RmRebootReason lpdwRebootReasons);
    }
}
