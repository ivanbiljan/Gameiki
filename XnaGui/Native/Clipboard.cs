using System;
using System.Runtime.InteropServices;

namespace XnaGui.Native {
    internal static class Clipboard {
        private const int AnsiTextFormat = 1;
        private const int UnicodeTextFormat = 13;

        public static void Copy(string text) {
            
        }

        public static string GetText() {
            if (!NativeMethods.IsClipboardFormatAvailable(AnsiTextFormat)) {
                return string.Empty;
            }
            
            if (!NativeMethods.OpenClipboard(IntPtr.Zero)) {
                return string.Empty;
            }

            var clipboardData = NativeMethods.GetClipboardData(AnsiTextFormat);
            if (clipboardData == IntPtr.Zero) {
                return string.Empty;
            }

            var clipTextPtr = NativeMethods.GlobalLock(clipboardData);
            if (clipTextPtr == IntPtr.Zero) {
                return string.Empty;
            }
            
            var clipboardText = Marshal.PtrToStringAnsi(clipTextPtr);
            NativeMethods.CloseClipboard();
            return clipboardText;
        }
    }
}