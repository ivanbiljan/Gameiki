using System;
using System.Runtime.InteropServices;

namespace XnaGui.Native {
    internal static class Platform {

        public static bool Is64Bit => IntPtr.Size == 8;
        
        public static bool IsWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;

        public static bool IsLinux {
            get {
                var platform = (int) Environment.OSVersion.Platform;
                return platform == 4 || platform == 6 || platform == 128;
            }
        }
    }
}