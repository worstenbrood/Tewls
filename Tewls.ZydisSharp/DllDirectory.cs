using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tewls.ZydisSharp
{
    [Flags]
    public enum LoadLibrarySearch : uint
    {
        ApplicationDir = 0x200,
        DefaultDirs = 0x1000,
        System32 = 0x800,
        UsersDirs = 0x400
    }

    internal class DllDirectory : IDisposable
    {
        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setdlldirectoryw
        /// </summary>
        /// <param name="lpPathName">Dll search path</param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetDllDirectory(string? lpPathName);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern nint AddDllDirectory(string newDirectory);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetDefaultDllDirectories(LoadLibrarySearch directoryFlags);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool RemoveDllDirectory(nint cookie);

        static DllDirectory()
        {
            if (!SetDefaultDllDirectories(LoadLibrarySearch.DefaultDirs | LoadLibrarySearch.ApplicationDir | LoadLibrarySearch.System32))
            {
                throw new Win32Exception();
            }
        }

        private readonly List<nint> _cookies = new ();

        private void TryAddDirectory(string path)
        {
            try
            {
                var cookie = AddDllDirectory(path);
                if (cookie == 0)
                {
                    throw new Win32Exception();
                }
                _cookies.Add(cookie);
            }
            catch
            {
                // Failover to SetDllDirectory
                if (!SetDllDirectory(path))
                {
                    throw new Win32Exception();
                }
            }
        }

        public DllDirectory(params string[] directories)
        {
            foreach (var directory in directories)
            {
                TryAddDirectory(directory);
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (_cookies.Count > 0)
            {
                _ = _cookies
                    .Select(RemoveDllDirectory)
                    .ToArray();
            }
            else
            {
                SetDllDirectory(null);
            }

            _disposed = true;
        }
    }
}
