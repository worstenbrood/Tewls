using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Tewls.Windows.Utils;

namespace Tewls.Windows
{
    public class RestartManager
    {
        public struct RmUniqueProcess
        {
            public ulong dwProcessId;              // PID
            public FILETIME ProcessStartTime;      // Process creation time
        };

        public enum RmAppType
        {
            RmUnknownApp = 0,   // Application type cannot be classified in
                                // known categories
            RmMainWindow = 1,   // Application is a windows application that
                                // displays a top-level window
            RmOtherWindow = 2,  // Application is a windows app but does not
                                // display a top-level window
            RmService = 3,      // Application is an NT service
            RmExplorer = 4,     // Application is Explorer
            RmConsole = 5,      // Application is Console application
            RmCritical = 1000   // Application is critical system process
                                // where a reboot is required to restart
        }

        public class RmProcessInfo
        {
            public RmUniqueProcess Process;
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = 256)]
            public string strAppName;
            [MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            public string strServiceShortName;
            public RmAppType ApplicationType;
            public ulong AppStatus;
            public ulong TSSessionId;
            public bool bRestartable;
        };

        [Flags]
        public enum RmRebootReason: uint
        {
            None = 0x0,
            PermissionDenied = 0x1,
            SessionMismatch = 0x2,
            CriticalProcess = 0x4,
            CriticalService = 0x8,
            DetectedSelf = 0x10,
        };

        public const int CchRmMaxAppName = 255;
        public const int CchRmMaxServiceName = 63;
        public const int RmSessionKeyLen = 16;
        public const int CchRmSessionKey = RmSessionKeyLen * 2;

        public static int StartSession(ref string sessionKey)
        {
            int sessionHandle = -1;

            using (var buffer = new HGlobalBuffer(CchRmSessionKey + sizeof(char)))
            {
                var result = Rstrtmgr.RmStartSession(ref sessionHandle, 0, buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                sessionKey = Marshal.PtrToStringAuto(buffer, CchRmSessionKey);
                return sessionHandle;
            }
        }

        public static IEnumerable<RmProcessInfo> GetList(int sessionHandle)
        {
            uint total = 0;
            uint entries = 0;
            RmRebootReason reason = RmRebootReason.None;

            var result = Rstrtmgr.RmGetList(sessionHandle, ref total, ref entries, IntPtr.Zero, ref reason);
            if (result == Error.Success)
            {
                yield break;
            }

            if (result != Error.MoreData)
            {
                throw new Win32Exception();
            }

            using (var buffer = new HGlobalBuffer(total * (uint) Marshal.SizeOf(typeof(RmProcessInfo))))
            {
                result = Rstrtmgr.RmGetList(sessionHandle, ref total, ref entries, buffer.Buffer, ref reason);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                foreach (var structure in buffer.EnumStructure<RmProcessInfo>(total))
                {
                    yield return structure;
                }
            }
        }

        public static void EndSession(int sessionHandle)
        {
            var result = Rstrtmgr.RmEndSession(sessionHandle);
            if (result != Error.Success)
            {
                throw new Win32Exception();
            }
        }
    }
}
