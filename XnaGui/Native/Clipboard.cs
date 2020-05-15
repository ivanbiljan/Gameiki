using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace XnaGui.Native {
    internal static class Clipboard {
        private const int AnsiTextFormat = 1;
        private const int UnicodeTextFormat = 13;

        public static void Copy(string text) {
            if (!NativeMethods.OpenClipboard(IntPtr.Zero)) {
                throw new Win32Exception("Cannot open the clipboard.");
            }
        }

        public static string GetText() {
            if (!NativeMethods.IsClipboardFormatAvailable(AnsiTextFormat)) {
                return string.Empty;
            }
            
            if (!NativeMethods.OpenClipboard(IntPtr.Zero)) {
                throw new Win32Exception("Cannot open the clipboard.");
            }

            var clipboardData = NativeMethods.GetClipboardData(AnsiTextFormat);
            if (clipboardData == IntPtr.Zero) {
                throw new Win32Exception("Failed to retrieve clipboard data.");
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