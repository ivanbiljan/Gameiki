using System;
using System.Globalization;
using System.Net;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaGui.Native;

namespace XnaGui {
    internal static class UserInputHandler {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static int _backspaceCooldown;
        private static int _typingCooldown;

        public static bool IsKeyPressed(Keys key) => _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);

        public static bool IsLongPress(Keys key) => _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyDown(key);

        public static bool IsCapsLockOn => NativeMethods.GetKeyState((ushort) Keys.CapsLock) != 0;

        public static void Update(GameTime gameTime) {
            if (_typingCooldown > 0) {
                --_typingCooldown;
            }
            
            if (_backspaceCooldown > 0) {
                --_backspaceCooldown;
            }
        }

        public static void GetTextInput(ref string text) {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            if (IsLongPress(Keys.Back)) {
                if (_backspaceCooldown > 0) {
                    --_backspaceCooldown;
                }
                else {
                    if (text.Length > 0) {
                        text = text.Remove(text.Length - 1, 1);
                    }

                    _backspaceCooldown = 15;
                    return;
                }
            }

            if (_typingCooldown > 0) {
                return;
            }

            var isShiftDown = IsLongPress(Keys.LeftShift) || IsLongPress(Keys.RightShift);
            var isAltDown = IsLongPress(Keys.LeftAlt) || IsLongPress(Keys.RightAlt);
            foreach (var key in _currentKeyboardState.GetPressedKeys()) {
                if (!IsAlphanumeric(key) && !isShiftDown && key != Keys.Space) {
                    continue;
                }

                if (key == Keys.LeftShift || key == Keys.RightShift) {
                    continue;
                }

                if (_previousKeyboardState.IsKeyDown(key)) {
                    continue;
                }

                switch (key) {
                    case Keys numKey when numKey >= Keys.D0 && numKey <= Keys.D9 && !isShiftDown && !isAltDown:
                        text += numKey.ToString().Substring(1);
                        break;
                    case Keys.Space:
                        text += " ";
                        break;
                    case Keys.Decimal:
                    case Keys.OemComma:
                        text += ",";
                        break;
                    case Keys.OemMinus:
                        text += "-";
                        break;
                    case Keys.OemPlus:
                        text += "+";
                        break;
                    case Keys.OemPeriod:
                        text += ".";
                        break;
                    default:
                        if (isAltDown) {
                            text += GetModifiedKey(key, Keys.RightAlt);
                        } else if (isShiftDown) {
                            text += GetModifiedKey(key, Keys.LeftShift);
                        }
                        else {
                            text += key.ToString().ToLower();
                        }
                        break;
                }
            }
            
            _typingCooldown = 1;
        }

        private static char GetModifiedKey(Keys key, Keys modifierKey = Keys.LeftShift) {
            // What's important to note is that XNA's Keys enum does not map to keychars, but rather to virutal key codes
            // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
            var keyCode = (uint) key & 0xFF;
            var pressedKeys = new byte[256];
            switch (modifierKey) {
                case Keys.LeftShift:
                case Keys.RightShift:
                    pressedKeys[0x10] = 0x80; // Represents shift. Mark it as pressed
                    break;
                case Keys.LeftAlt:
                case Keys.RightAlt:
                    pressedKeys[0xA5] = 0x80; // VK_MENU
                    break;
            }
            
            pressedKeys[(int) key] = 0x80; // The "key" argument
            if (NativeMethods.ToAscii(keyCode, keyCode, pressedKeys, out var modifiedKey, 1 << 28) <= 0) {
                throw new Exception("Cannot fetch modified key.");
            }

            return (char) modifiedKey;
        }

        private static bool IsAlphanumeric(Keys key) => key >= Keys.A && key <= Keys.Z || key >= Keys.D0 && key <= Keys.D9 ||
                                                        key == Keys.Decimal || key == Keys.OemComma || key == Keys.OemMinus ||
                                                        key == Keys.OemPlus || key == Keys.OemPeriod;
    }
}