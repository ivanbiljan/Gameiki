using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaGui {
    /// <summary>
    /// Represents the mouse manager.
    /// </summary>
    public static class MouseManager {
        private static MouseState _currentState;
        private static MouseState _previousState;

        /// <summary>
        /// Updates the mouse position.
        /// </summary>
        public static void Update() {
            _previousState = _currentState;
            _currentState = Mouse.GetState();
        }

        /// <summary>
        /// Gets the current mouse position.
        /// </summary>
        public static Vector2 MousePosition => new Vector2(_currentState.X, _currentState.Y);
        
        /// <summary>
        /// Indicates whether the left mouse button has been pressed.
        /// </summary>
        /// <returns><c>true</c> if the button has been pressed; otherwise, <c>false</c>.</returns>
        public static bool IsLeftButtonDown() =>
            _previousState.LeftButton == ButtonState.Released && _currentState.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// Indicates whether the right mouse button has been pressed.
        /// </summary>
        /// <returns><c>true</c> if the button has been pressed; otherwise, <c>false</c>.</returns>
        public static bool IsRightButtonDown() =>
            _previousState.RightButton == ButtonState.Released && _currentState.RightButton == ButtonState.Pressed;

        /// <summary>
        /// Indicates whether the left button has been released.
        /// </summary>
        /// <returns><c>true</c> if the button has been released; otherwise, <c>false</c>.</returns>
        public static bool IsLeftButtonUp() =>
            _previousState.LeftButton == ButtonState.Pressed && _currentState.LeftButton == ButtonState.Released;

        /// <summary>
        /// Indicates whether the right mouse button has been released.
        /// </summary>
        /// <returns><c>true</c> if the button has been pressed; otherwise, <c>false</c>.</returns>
        public static bool IsRightButtonUp() =>
            _previousState.RightButton == ButtonState.Pressed && _currentState.RightButton == ButtonState.Released;
    }
}