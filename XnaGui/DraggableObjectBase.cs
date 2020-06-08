using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using XnaGui.Input;

namespace XnaGui {
    /// <summary>
    /// Represents the base class for controls that can be dragged across the screen or resized.
    /// </summary>
    [PublicAPI]
    public abstract class DraggableObjectBase : Control {
        private bool _isBeingDragged;

        /// <summary>
        /// Occurs when the control is dragged (i.e, the mouse is moved).
        /// </summary>
        public event EventHandler<DragDeltaEventArgs> DragDelta;
        
        protected DraggableObjectBase(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
            MouseManager.Move += OnMouseMoveInternal;
        }

        private void OnMouseMoveInternal(object sender, DragDeltaEventArgs e) {
            if (!_isBeingDragged) {
                return;
            }

            OnDrag(sender, e);
        }

        protected virtual void OnDrag(object sender, DragDeltaEventArgs e) {
            DragDelta?.Invoke(this, e);
        }

        protected override void OnLeftButtonDown(object sender, EventArgs args) {
            _isBeingDragged = true;
            base.OnLeftButtonDown(sender, args);
        }

        protected override void OnLeftButtonUp(object sender, EventArgs args) {
            _isBeingDragged = false;
            base.OnLeftButtonUp(sender, args);
        }
    }
}