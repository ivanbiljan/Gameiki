﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XnaGui {
    public abstract class Control {
        private readonly Vector2 _origin;
        private Vector2 _position;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Control" /> class with the specified dimensions and parent.
        /// </summary>
        /// <param name="parent">The parent control, if any.</param>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper right corner.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected Control(Control parent, int x, int y, int width, int height) {
            _origin = parent?.Position ?? new Vector2(0, 0);
            _position = new Vector2(x, y);
            
            Parent = parent;
            Dimensions = new Vector2(width, height);
            BoundBox = new Rectangle((int) Position.X, (int) Position.Y, width, height);
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
        public int Padding { get; set; }

        /// <summary>
        ///     Gets the parent control.
        /// </summary>
        public Control Parent { get; }
        
        /// <summary>
        /// Gets the control's boundaries.
        /// </summary>
        public Rectangle BoundBox { get; }

        /// <summary>
        ///     Gets the position relative to the control's parent.
        /// </summary>
        public Vector2 Position {
            get => new Vector2(_origin.X + _position.X + Padding, _origin.Y + _position.Y + Padding);
            set {
                if (value.X < 0 || value.Y < 0) {
                    return;
                }

                _position = value;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Control() {
            Dispose(false);
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        /// <param name="disposing"><c>true</c> if disposing managed resources; otherwise, <c>false</c>.</param>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
            }

            foreach (var child in Children) {
                child.Dispose(disposing);
            }
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        /// <param name="gameTime">The current time snapshot.</param>
        public virtual void Update(GameTime gameTime) {
            foreach (var child in Children) {
                child.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="gameTime">The current time snapshot.</param>
        /// <param name="spriteBatch">The sprite batch used to draw the control.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (var child in Children) {
                child.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public Color ForegroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// Indicates whether the control is hovered over.
        /// </summary>
        /// <returns><c>true</c> if the control is hovered over; otherwise, <c>false</c>.</returns>
        public bool IsHoveredOver() => BoundBox.Contains((int) MouseManager.MousePosition.X, (int) MouseManager.MousePosition.Y);
    }
}