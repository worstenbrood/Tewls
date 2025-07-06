namespace TewlKit.Core
{
    public interface IHook<TFunction>
    {
        string ModuleName { get; }
        TFunction Original { get; }
        string ProcName { get; }
    }
}