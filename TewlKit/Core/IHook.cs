using System;

namespace TewlKit.Core
{
    public interface IHook
    {
        string ModuleName { get; }
        string ProcName { get; }
    }
}