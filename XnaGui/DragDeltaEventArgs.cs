using System;
using Microsoft.Xna.Framework;

namespace XnaGui {
    /// <summary>
    /// Provides event data for the <see cref="DraggableObjectBase.DragDelta"/> event.
    /// </summary>
    public sealed class DragDeltaEventArgs : EventArgs {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragDeltaEventArgs"/> class with the specified properties.
        /// </summary>
        /// <param name="startPosition">The starting position.</param>
        /// <param name="finalPosition">The final position.</param>
        public DragDeltaEventArgs(Vector2 startPosition, Vector2 finalPosition) {
            StartPosition = startPosition;
            FinalPosition = finalPosition;
        }
        
        /// <summary>
        /// Gets the starting point.
        /// </summary>
        public Vector2 StartPosition { get; }
        
        /// <summary>
        /// Gets the final position.
        /// </summary>
        public Vector2 FinalPosition { get; }
    }
}