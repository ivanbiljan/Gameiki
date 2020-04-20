﻿using System;
using JetBrains.Annotations;

namespace Gameiki.Patcher.Events {
    [PublicAPI]
    public static class Hooks {
        /// <summary>
        ///     Occurs after Terraria initializes.
        /// </summary>
        public static EventHandler GameInitialized;

        /// <summary>
        ///     Occurs after Terraria is done drawing.
        /// </summary>
        public static EventHandler<DrawEventArgs> PostDraw;

        /// <summary>
        ///     Occurs after Terraria updates.
        /// </summary>
        public static EventHandler PostUpdate;

        /// <summary>
        ///     Occurs before Terraria does any drawing.
        /// </summary>
        public static EventHandler<DrawEventArgs> PreDraw;

        /// <summary>
        ///     Occurs before Terraria updates.
        /// </summary>
        public static EventHandler PreUpdate;

        internal static void InvokeGameInitialized() {
            GameInitialized?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePostDraw() {
            PostDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePostUpdate() {
            PostUpdate?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePreDraw() {
            PreDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePreUpdate() {
            PreUpdate?.Invoke(null, EventArgs.Empty);
        }
    }
}