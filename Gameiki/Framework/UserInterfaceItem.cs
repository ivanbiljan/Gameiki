using System;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;

namespace Gameiki.Framework {
    // TODO expand upon this
    public abstract class UserInterfaceItem : IDisposable {
        protected UserInterfaceItem() {
            
        }

        protected UserInterfaceItem(UserInterfaceItem parent, Vector2 position) {
            
        }
        
        public abstract AnchorPosition AnchorPosition { get; }

        public abstract Rectangle Boundaries { get;  }
        
        public int Padding { get; }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserInterfaceItem() {
            Dispose(false);
        }

        public abstract void Draw(object sender, DrawEventArgs args);

        public virtual void Initialize() {
            Hooks.PostDraw += Draw;
        }

        protected virtual void Dispose(bool disposing) {
            ReleaseUnmanagedResources();
            if (disposing) {
            }
        }

        private void ReleaseUnmanagedResources() {
            if (Hooks.PostDraw != null) {
                Hooks.PostDraw -= Draw;
            }
        }
    }
}