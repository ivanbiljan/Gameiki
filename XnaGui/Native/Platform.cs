using System;

namespace XnaGui.Native {
    internal static class Platform {
        /// <summary>
        ///     Gets a value indicating whether the current process is running in 64bit mode.
        /// </summary>
        public static bool Is64Bit => IntPtr.Size == 8;

        /// <summary>
        ///     Gets a value indicating whether the process is running on Linux.
        /// </summary>
        public static bool IsLinux {
            get {
                var platform = (int) Environment.OSVersion.Platform;
                return platform == 4 || platform == 6 || platform == 128;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the process is running Windows.
        /// </summary>
        public static bool IsWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}