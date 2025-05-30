﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tewls.Windows.NetApi.Structures;

namespace Tewls.Windows.NetApi
{
    public class NetShare : NetBase
    {
        public static IEnumerable<TStruct> Enum<TStruct>(string servername = null)
            where TStruct : class, IInfo<ShareLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();

                uint entriesRead = 0;
                uint totalEntries = 0;

                var result = Netapi32.NetShareEnum(servername, info.GetLevel(), ref buffer.Buffer, PrefMaxLength, ref entriesRead, ref totalEntries, IntPtr.Zero);
                if (result != Error.Success)
                {
                    throw new Win32Exception();
                }

                foreach(var structure in buffer.EnumStructure<TStruct>(totalEntries))
                {
                    yield return structure;
                }
            }
        }

        public static TStruct GetInfo<TStruct>(string serverName, string netName)
           where TStruct : class, IInfo<ShareLevel>, new()
        {
            using (var buffer = new NetBuffer())
            {
                var info = new TStruct();
                var result = Netapi32.NetShareGetInfo(serverName, netName, info.GetLevel(), ref buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }

                return buffer.PtrToStructure(info);
            }
        }

        public static void SetInfo<TStruct>(string serverName, string netName, TStruct info)
            where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = Netapi32.NetShareSetInfo(serverName, netName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Add<TStruct>(string serverName, TStruct info)
           where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                uint paramIndex = 0;

                var result = Netapi32.NetShareAdd(serverName, info.GetLevel(), buffer.Buffer, ref paramIndex);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }

        public static void Del(string serverName, string netName)
        {
            var result = Netapi32.NetShareDel(serverName, netName, 0);
            if (result != Error.Success)
            {
                throw new Win32Exception((int)result);
            }
        }

        public static void Del<TStruct>(string serverName, TStruct info)
            where TStruct : class, IInfo<ShareLevel>
        {
            using (var buffer = new NetBuffer<TStruct>(info))
            {
                var result = Netapi32.NetShareDelEx(serverName, info.GetLevel(), buffer.Buffer);
                if (result != Error.Success)
                {
                    throw new Win32Exception((int)result);
                }
            }
        }
    }
}
