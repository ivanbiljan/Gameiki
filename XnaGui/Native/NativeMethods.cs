using System;
using System.Runtime.InteropServices;

namespace XnaGui.Native {
    // TODO: Linux implementations
    internal static class NativeMethods {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardData(uint format);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, out uint lpChar, uint flags);
    }
}