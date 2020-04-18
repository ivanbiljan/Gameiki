using System;
using JetBrains.Annotations;

namespace Gameiki.Patcher.Events {
    [PublicAPI]
    public static class Hooks {
        /// <summary>
        /// Occurs before Terraria does any drawing.
        /// </summary>
        public static EventHandler<DrawEventArgs> PreDraw;
        
        /// <summary>
        /// Occurs after Terraria is done drawing.
        /// </summary>
        public static EventHandler<DrawEventArgs> PostDraw;

        /// <summary>
        /// Occurs before Terraria updates.
        /// </summary>
        public static EventHandler PreUpdate;

        /// <summary>
        /// Occurs after Terraria updates.
        /// </summary>
        public static EventHandler PostUpdate;

        internal static void InvokePreDraw() {
            PreDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePostDraw() {
            PostDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePreUpdate() {
            PreUpdate?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePostUpdate() {
            PostUpdate?.Invoke(null, EventArgs.Empty);
        }
    }
}