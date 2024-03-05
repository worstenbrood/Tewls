using static Tewls.Windows.WNet;
using System.Runtime.InteropServices;
using System;

namespace Tewls.Windows
{
    public static class Mpr
    {
        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetOpenEnum(ResourceScope dwScope, ResourceType dwType, ResourceUsage dwUsage, NetResource p, out IntPtr lphEnum);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        public static extern Error WNetCloseEnum(IntPtr hEnum);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetEnumResource(IntPtr hEnum, ref uint lpcCount, IntPtr buffer, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetAddConnection(string lpRemoteName, string lpPassword, string lpLocalName);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetLastError(ref uint lpError, IntPtr lpErrorBuf, uint nErrorBufSize, IntPtr lpNameBuf, uint nNameBufSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUserName, uint dwFlags);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetNetworkInformation(string lpProvider, NetInfoStruct lpNetInfoStruct);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetCancelConnection(string lpName, bool fForce);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetConnection(string lpLocalName, IntPtr lpRemoteName, ref uint lpnLength);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error MultinetGetConnectionPerformance(NetResource lpNetResource, NetConnectionInfoStruct lpNetConnectInfoStruct);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetUser(string lpName, IntPtr lpUserName, ref uint lpnLength);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetUniversalName(string lpLocalPath, uint dwInfoLevel, IntPtr lpBuffer, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetResourceParent(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetProviderName(uint dwNetType, IntPtr lpProviderName, ref uint lpBufferSize);

        [DllImport("Mpr.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Error WNetGetResourceInformation(NetResource lpNetResource, IntPtr lpBuffer, ref uint lpcbBuffer, ref IntPtr lplpSystem);
    }
}
