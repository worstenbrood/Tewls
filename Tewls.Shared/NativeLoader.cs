using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Tewls.Shared
{
    internal class Library() : HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        public bool IsLoaded(string library) => Contains(library);
    }

    public static class NativeLoader
    {
        private static readonly Library Library = new();

        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-loadlibraryw
        /// </summary>
        /// <param name="dllToLoad">Dll name</param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        private static extern nint LoadLibrary(string dllToLoad);

        /// <summary>
        /// Load dll and dependencies from the supplied path 
        /// </summary>
        /// <param name="path">Path to load dll from</param>
        /// <param name="dllName">Dll name</param>
        /// <exception cref="Win32Exception"></exception>
        private static void LoadDll(string path, string dllName)
        {
            // Set dll path so dependencies of the dll get loaded form the same folder
            using (new DllDirectory(path))
            {
                // Load the dll
                if (LoadLibrary(dllName) == 0)
                {
                    throw new Win32Exception();
                }
            }
        }

        /// <summary>
        /// Architecture -> subfolder mapping
        /// </summary>
        private static readonly Dictionary<Architecture, string> Directories = new()
        {
            { Architecture.X86, "win32" },
            { Architecture.X64, "win64" },
            { Architecture.Arm, "arm32" },
            { Architecture.Arm64, "arm64" }
        };

        /// <summary>
        /// Load the specified dll from the correct folder based on
        /// the process architecture.
        /// x86 => win32
        /// x64 => win64
        /// arm64 => arm64
        /// </summary>
        /// <typeparam name="T">Type of the class using the native dll</typeparam>
        /// <param name="library">Name of the dll</param>
        /// <exception cref="Win32Exception">kernel32:LoadLibrary exception</exception>
        public static void Load<T>(string library, string? extraFolder = null)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }

            // Check if already loaded
            if (Library.IsLoaded(library))
            {
                return;
            }

            var assemblyPath = Path.GetDirectoryName(new Uri(typeof(T).Assembly.Location).LocalPath) ?? 
                throw new InvalidOperationException("Unable to determine assembly path.");

            // fetch dll folder
            if (!Directories.TryGetValue(RuntimeInformation.ProcessArchitecture, out string? subfolder))
            {
                throw new ArgumentException($"Invalid architecture: {RuntimeInformation.ProcessArchitecture}");
            }

            // construct full path
            var fullPath = extraFolder != null ? Path.Combine(assemblyPath, extraFolder, subfolder) : 
                Path.Combine(assemblyPath, subfolder);

            // Load
            LoadDll(fullPath, library);

            // Mark as loaded
            Library.Add(library);
        }
    }
}
