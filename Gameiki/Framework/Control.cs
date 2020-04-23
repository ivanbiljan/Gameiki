using System;
using System.Collections.Generic;
using System.Security.Policy;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Gameiki.Framework {
    /// <summary>
    ///     Represents the base class for GUI controls.
    /// </summary>
    public abstract class Control : IDisposable {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private bool _wasHovered;

        /// <summary>
        ///     Occurs when the control is clicked.
        /// </summary>
        public EventHandler MouseClick;

        /// <summary>
        ///     Occurs when the control is pressed.
        /// </summary>
        public EventHandler MouseDown;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Control" /> class with the specified dimensions.
        /// </summary>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper right corner.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected Control(int x, int y, int width, int height) {
            Position = new Vector2(x, y);
            Dimensions = new Vector2(width, height);
        }

        /// <summary>
        ///     Gets the child controls.
        /// </summary>
        public List<Control> Children { get; } = new List<Control>();

        /// <summary>
        ///     Gets the dimensions.
        /// </summary>
        public Vector2 Dimensions { get; private set; }

        /// <summary>
        ///     Gets the padding.
        /// </summary>
        public int Padding { get; }

        /// <summary>
        ///     Gets the parent control.
        /// </summary>
        public Control Parent { get; }

        /// <summary>
        ///     Gets the position relative to the control's parent.
        /// </summary>
        public Vector2 Position { get; private set; }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Control() {
            Dispose(false);
        }

        public virtual void Initialize() {
            Hooks.PostCursorDraw += OnPostCursorDraw;
            Hooks.PostUpdate += OnPostUpdate;
        }

        /// <summary>
        ///     Occurs when the control is first hovered over.
        /// </summary>
        public event EventHandler MouseEnter;

        /// <summary>
        ///     Occurs when the cursor leaves the control.
        /// </summary>
        public event EventHandler MouseLeave;

        protected virtual void Dispose(bool disposing) {
            ReleaseUnmanagedResources();
            if (disposing) {
            }
        }

        protected bool IsHoveredOver() =>
            Main.mouseX >= Position.X && Main.mouseX <= Position.X + Dimensions.X && Main.mouseY >= Position.Y &&
            Main.mouseY <= Position.Y + Dimensions.Y;

        protected bool IsMouseClick() =>
            _currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed;

        protected bool IsMouseDown() => _currentMouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        ///     Handles how the control is drawn.
        /// </summary>
        /// <param name="sender">The sender, which is <c>null</c>.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnPostCursorDraw(object sender, EventArgs args) {
            foreach (var child in Children) {
                child.OnPostCursorDraw(sender, args);
            }
        }

        /// <summary>
        ///     Executes when the game is updated.
        /// </summary>
        /// <param name="sender">The sender, which is <c>null</c>.</param>
        /// <param name="args">The event data.</param>
        protected virtual void OnPostUpdate(object sender, EventArgs args) {
            if (!IsHoveredOver()) {
                if (!_wasHovered) {
                    return;
                }

                MouseLeave?.Invoke(null, EventArgs.Empty);
                _wasHovered = false;
                return;
            }
            
            if (!_wasHovered) {
                MouseEnter?.Invoke(null, EventArgs.Empty);
                _wasHovered = true;
            }
            //
            // if (IsMouseDown()) {
            //     MouseDown?.Invoke(null, EventArgs.Empty);
            // }
            // else if (IsMouseClick()) {
            //     MouseClick?.Invoke(null, EventArgs.Empty);
            // }

            if (Main.mouseLeft && Main.mouseLeftRelease) {
                MouseClick?.Invoke(null, EventArgs.Empty);
            }

            // Do the above for all child controls as well
            foreach (var child in Children) {
                child.OnPostUpdate(sender, args);
            }
        }

        public void SetPosition(float x, float y) {
            Position = new Vector2(x, y);
        }

        private void ReleaseUnmanagedResources() {
            Hooks.PostCursorDraw -= OnPostCursorDraw;
            Hooks.PostUpdate -= OnPostUpdate;
        }
    }
}