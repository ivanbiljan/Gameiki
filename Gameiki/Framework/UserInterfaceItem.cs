using System;
using Gameiki.Patcher.Events;
using Microsoft.Xna.Framework;

namespace Gameiki.Framework {
    public abstract class UserInterfaceItem : IDisposable {
        protected UserInterfaceItem() {
            
        }
        
        public abstract AnchorPosition AnchorPosition { get; protected set; }
        
        public abstract Rectangle Boundaries { get; protected set; }
        
        public abstract void Draw(object sender, DrawEventArgs args);

        public virtual void Initialize() {
            Hooks.PostDraw += Draw;
        }

        private void ReleaseUnmanagedResources() {
            if (Hooks.PostDraw != null) {
                Hooks.PostDraw -= Draw;
            }
        }

        protected virtual void Dispose(bool disposing) {
            ReleaseUnmanagedResources();
            if (disposing) {
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserInterfaceItem() {
            Dispose(false);
        }
    }
}