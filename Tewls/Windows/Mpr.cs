using static Tewls.Windows.WNet;
using System.Runtime.InteropServices;
using System;

namespace Tewls.Windows
{
    public static class Mpr
    {
        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetOpenEnum(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage, NetResource p, out IntPtr lphEnum);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error WNetCloseEnum(IntPtr hEnum);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetEnumResource(IntPtr hEnum, ref uint lpcCount, IntPtr buffer, ref uint lpBufferSize);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetAddConnection(string lpRemoteName, string lpPassword, string lpLocalName);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetLastError(ref uint lpError, IntPtr lpErrorBuf, uint nErrorBufSize, IntPtr lpNameBuf, uint nNameBufSize);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUserName, uint dwFlags);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetNetworkInformation(string lpProvider, NetInfo lpNetInfoStruct);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetCancelConnection(string lpName, bool fForce);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetConnection(string lpLocalName, IntPtr lpRemoteName, ref uint lpnLength);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error MultinetGetConnectionPerformance(NetResource lpNetResource, NetConnectionInfo lpNetConnectInfoStruct);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetUser(string lpName, IntPtr lpUserName, ref uint lpnLength);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetUniversalName(string lpLocalPath, uint dwInfoLevel, IntPtr lpBuffer, ref uint lpBufferSize);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetResourceParent(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetProviderName(uint dwNetType, IntPtr lpProviderName, ref uint lpBufferSize);

        [DllImport(nameof(Mpr), CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetResourceInformation(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer, ref IntPtr lplpSystem);
    }
}
