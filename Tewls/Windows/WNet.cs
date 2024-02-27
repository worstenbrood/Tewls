using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Tewls.Windows.Utils;

namespace Tewls.Windows
{
    // See WinNetWk.h
    
    public class WNet
    {
        public enum ResourceScope
        {
            Connected = 1,
            GlobalNet,
            Remembered,
            Recent,
            Context
        };

        public enum ResourceType
        {
            Any,
            Disk,
            Print,
            Reserved
        };

        [Flags]
        public enum ResourceUsage
        {
            None = 0,
            Connectable = 0x00000001,
            Container = 0x00000002,
            NoLocalDevice = 0x00000004,
            Sibling = 0x00000008,
            Attached = 0x00000010,
            All = Connectable | Container | Attached,
        };

        public enum ResourceDisplayType
        {
            Generic,
            Domain,
            Server,
            Share,
            File,
            Group,
            Network,
            Root,
            ShareAdmin,
            Directory,
            Tree,
            NdsContainer
        };        

        [Flags]
        public enum Connect
        {
            None = 0,
            UpdateProfile = 1,
            UpdateRecent = 2,
            Temporary = 4,
            Interactive = 8,
            Prompt = 16,
            NeedDrive = 32,
            RefCount = 64,
            Redirect = 128,
            LocalDrive = 256,
            CurrentMedia = 512,
            Deferred = 1024,
            CommandLine = 2048,
            CmdSaveCred = 4096,
            CredReset = 8192,
        }

        [Flags]
        public enum NetInfo
        {
            Dll16 = 1,
            DiskRedirect = 2,
            Printerredirect
        }

        [Flags]
        public enum WnCon
        {
            ForNetCard = 1,
            NotRouted = 2,
            SlowLink = 4,
            Dynamic = 8
        }

        public enum InfoLevel
        {
            UniversalName = 1,
            RemoteName = 2
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType Type;
            public ResourceDisplayType DisplayType;
            public ResourceUsage Usage;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string LocalName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string RemoteName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Comment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Provider;

            public NetResource AddConnection(string password = null)
            {
                WNet.AddConnection(RemoteName, LocalName, password);
                return this;
            }

            public NetResource AddConnection2(string password = null, string userName = null, Connect flags = Connect.UpdateProfile)
            {
                WNet.AddConnection2(this, password, userName, flags);
                return this;
            }

            public NetResource GetParent()
            {
                return WNet.GetResourceParent(this);
            }

            public string GetUser()
            {
                return WNet.GetUser(LocalName);
            }

            public NetResource CancelConnection(bool force = false)
            {
                WNet.CancelConnection(LocalName, force);
                return this;
            }

            public NetResource CancelConnection2(Connect flags, bool force = false)
            {
                WNet.CancelConnection2(LocalName, flags, force);
                return this;
            }

            public NetInfoStruct GetNetworkInformation()
            {
                return WNet.GetNetworkInformation(Provider);
            }

            public string GetConnection()
            {
                return WNet.GetConnection(LocalName);
            }

            public NetConnectionInfoStruct MultinetGetConnectionPerformance()
            {
                return WNet.MultinetGetConnectionPerformance(this);
            }

            public UniversalNameInfo GetUniversalNameInfo()
            {
                return WNet.GetUniversalNameInfo(LocalName);
            }

            public RemoteNameInfo GetRemoteNameInfo()
            {
                return WNet.GetRemoteNameInfo(LocalName);
            }

            public NetResource GetResourceInformation()
            {
                return WNet.GetResourceInformation(this);
            }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class NetInfoStruct
        {
            public uint Structure;
            public uint ProviderVersion;
            public Error Status;
            public NetInfo Characteristics;
            public ulong Handle;
            public short NetType;
            public uint Printers;
            public uint Drives;

            public NetInfoStruct()
            {
                Structure = (uint)Marshal.SizeOf(typeof(NetInfoStruct));
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public class NetConnectionInfoStruct
        {
            public uint Structure;
            public WnCon Flags;
            public uint Speed;
            public uint Delay;
            public uint OptDataSize;

            public NetConnectionInfoStruct()
            {
                Structure = (uint)Marshal.SizeOf(typeof(NetConnectionInfoStruct));
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public class UniversalNameInfo
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string UniversalName;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class RemoteNameInfo
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string UniversalName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string ConnectionName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string RemainingPath;
        };

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetOpenEnum(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage, NetResource p, out IntPtr lphEnum);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern Error WNetCloseEnum(IntPtr hEnum);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetEnumResource(IntPtr hEnum, ref uint lpcCount, IntPtr buffer, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetAddConnection(string lpRemoteName, string lpPassword, string lpLocalName);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetLastError(ref uint lpError, IntPtr lpErrorBuf, uint nErrorBufSize, IntPtr lpNameBuf, uint nNameBufSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUserName, uint dwFlags);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetNetworkInformation(string lpProvider, NetInfoStruct lpNetInfoStruct);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetCancelConnection(string lpName, bool fForce);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetConnection(string lpLocalName, IntPtr lpRemoteName, ref uint lpnLength);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error MultinetGetConnectionPerformance(NetResource lpNetResource, NetConnectionInfoStruct lpNetConnectInfoStruct);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetUser(string lpName, IntPtr lpUserName, ref uint lpnLength);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetUniversalName(string lpLocalPath, uint dwInfoLevel, IntPtr lpBuffer, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetResourceParent(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetProviderName(uint dwNetType, IntPtr lpProviderName, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Error WNetGetResourceInformation(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer, ref IntPtr lplpSystem);

        private const int ErrorBufferSize = 1024;
        private const int EnumBufferSize = 16384;
        private const int BufferSize = 32;       

        public static Exception GetLastException(Error code)
        {
            // No error
            if (code == Error.NoError)
            {
                return null;
            }
            // Win32 error
            else if (code != Error.ExtendedError)
            {
                return new Win32Exception((int) code);
            }
                        
            // Extended error

            IntPtr errorPtr = IntPtr.Zero;
            IntPtr namePtr = IntPtr.Zero;

            try
            {   errorPtr = Marshal.AllocHGlobal(ErrorBufferSize);
                namePtr = Marshal.AllocHGlobal(ErrorBufferSize);
                uint errorCode = 0;

                var result = WNetGetLastError(ref errorCode, errorPtr, ErrorBufferSize, namePtr, ErrorBufferSize);
                if (result == Error.NoError)
                {
                    if (errorCode != (int)Error.NoError)
                    {
                        return new Win32Exception((int)errorCode, Marshal.PtrToStringUni(errorPtr));
                    }
                }
                else
                {
                    return new Win32Exception((int)result);
                }

                return null;
            }
            finally
            {
                if (errorPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(errorPtr);
                }

                if (namePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(namePtr);
                }
            }
        }

        public static IEnumerable<NetResource> EnumResources(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage)
        {
            var resource = new NetResource();
            IntPtr handle = IntPtr.Zero;
           
            try
            {
                uint bufferSize = EnumBufferSize;
                using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
                {

                    var result = WNetOpenEnum(dwScope, dwType, dwUsage, resource, out handle);
                    var ex = GetLastException(result);
                    if (ex != null)
                    {
                        throw ex;
                    }

                    uint count = 1;

                    do
                    {
                        result = WNetEnumResource(handle, ref count, buffer, ref bufferSize);

                        if (result == Error.NoError)
                        {
                            yield return buffer.PtrToStructure(resource);
                        }

                        if (result != Error.NoMoreItems)
                        {
                            ex = GetLastException(result);
                            if (ex != null)
                            {
                                throw ex;
                            }
                        }
                    }
                    while (result != Error.NoMoreItems);
                }
            }
            finally
            {
                // Close handle
                if (handle != IntPtr.Zero)
                {
                    WNetCloseEnum(handle);
                }
            }
        }

        public static void AddConnection(string remoteName, string localName, string password = null)
        {
            var result = WNetAddConnection(remoteName, password, localName);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
        }

        public static void AddConnection2(NetResource netResource, string password = null, string userName = null, Connect flags = Connect.UpdateProfile)
        {
            var result = WNetAddConnection2(netResource, password, userName, (uint)flags);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
        }

        public static NetInfoStruct GetNetworkInformation(string provider)
        {
            var info = new NetInfoStruct();
            var result = WNetGetNetworkInformation(provider, info);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
            return info;
        }

        public static void CancelConnection(string localOrRemoteName, bool force)
        {
            var result = WNetCancelConnection(localOrRemoteName, force);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
        }

        public static void CancelConnection2(string localOrRemoteName, Connect flags, bool force)
        {
            var result = WNetCancelConnection2(localOrRemoteName, (uint)flags, force);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
        }

        public static string GetConnection(string localName)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetConnection(localName, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr) bufferSize);
                    result = WNetGetConnection(localName, buffer, ref bufferSize);
                }

                // Return null if network is unavailable
                if (result == Error.NoNetwork || result == Error.ConnectionUnavailable)
                {
                    return null;
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return Marshal.PtrToStringUni(buffer);
            }
        }

        public static NetConnectionInfoStruct MultinetGetConnectionPerformance(NetResource resource)
        {
            var info = new NetConnectionInfoStruct();
            var result = MultinetGetConnectionPerformance(resource, info);
            var ex = GetLastException(result);
            if (ex != null)
            {
                throw ex;
            }
            return info;
        }

        public static string GetUser(string localOrRemoteName)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetUser(localOrRemoteName, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr)(bufferSize * sizeof(char)));
                    result = WNetGetUser(localOrRemoteName, buffer, ref bufferSize);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return Marshal.PtrToStringUni(buffer);
            }
        }

        public static UniversalNameInfo GetUniversalNameInfo(string localPath)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetUniversalName(localPath, (uint)InfoLevel.UniversalName, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr) bufferSize);
                    result = WNetGetUniversalName(localPath, (uint)InfoLevel.UniversalName, buffer, ref bufferSize);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return buffer.PtrToStructure<UniversalNameInfo>();
            }
        }

        public static RemoteNameInfo GetRemoteNameInfo(string localPath)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetUniversalName(localPath, (uint)InfoLevel.RemoteName, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr)bufferSize);
                    result = WNetGetUniversalName(localPath, (uint)InfoLevel.RemoteName, buffer, ref bufferSize);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return buffer.PtrToStructure<RemoteNameInfo>();
            }
        }

        public static NetResource GetResourceParent(NetResource netResource)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetResourceParent(netResource, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr) bufferSize);
                    result = WNetGetResourceParent(netResource, buffer, ref bufferSize);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return buffer.PtrToStructure<NetResource>();
            }
        }

        public string GetProviderName(uint type)
        {
            uint bufferSize = BufferSize;
            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetProviderName(type, buffer, ref bufferSize);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr)(bufferSize * sizeof(char)));
                    result = WNetGetProviderName(type, buffer, ref bufferSize);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return Marshal.PtrToStringUni(buffer);
            }
        }

        public static NetResource GetResourceInformation(NetResource netResource)
        {
            uint bufferSize = BufferSize;
            IntPtr pointer = IntPtr.Zero;

            using (var buffer = new HGlobalBuffer((IntPtr) bufferSize))
            {
                // Get buffer size
                var result = WNetGetResourceInformation(netResource, buffer, ref bufferSize, ref pointer);
                if (result == Error.MoreData)
                {
                    buffer.ReAlloc((IntPtr)bufferSize);
                    result = WNetGetResourceInformation(netResource, buffer, ref bufferSize, ref pointer);
                }

                var ex = GetLastException(result);
                if (ex != null)
                {
                    throw ex;
                }

                return buffer.PtrToStructure<NetResource>();
            }
        }
    }
}
