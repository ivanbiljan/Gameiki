using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace Gameiki.Patcher.Events {
    // TODO determine whether Pre/PostCursorDraw are viable replacements to Pre/PostDraw

    [PublicAPI]
    public static class Hooks {
        /// <summary>
        /// Occurs when the player sends a message.
        /// </summary>
        public static EventHandler<ChatEventArgs> Chat;
        
        /// <summary>
        ///     Occurs after Terraria initializes.
        /// </summary>
        public static EventHandler GameInitialized;

        /// <summary>
        ///     Occurs after the player updates.
        /// </summary>
        public static EventHandler PlayerUpdate;

        /// <summary>
        ///     Occurs after the cursor is drawn.
        /// </summary>
        /// ///
        /// <remarks>Most elements (if not all) will have been drawn by the time this event occurs.</remarks>
        public static EventHandler PostCursorDraw;

        /// <summary>
        ///     Occurs after Terraria is done drawing, just before the DoDraw method exits.
        /// </summary>
        public static EventHandler<DrawEventArgs> PostDraw;

        /// <summary>
        ///     Occurs after Terraria updates.
        /// </summary>
        public static EventHandler PostUpdate;

        /// <summary>
        ///     Occurs before the cursor is drawn.
        /// </summary>
        /// <remarks>Most elements (if not all) will have been drawn by the time this event occurs.</remarks>
        public static EventHandler<HandledEventArgs> PreCursorDraw;

        /// <summary>
        ///     Occurs before Terraria does any drawing, before any calls inside the DoDraw method.
        /// </summary>
        public static EventHandler<DrawEventArgs> PreDraw;

        /// <summary>
        ///     Occurs before Terraria updates.
        /// </summary>
        public static EventHandler PreUpdate;

        /// <summary>
        ///     Occurs after an interface layer is drawn.
        /// </summary>
        public static event EventHandler<InterfaceLayerDrawEventArgs> PostInterfaceLayerDraw;

        /// <summary>
        ///     Occurs before an interface layer is drawn.
        /// </summary>
        public static event EventHandler<InterfaceLayerDrawEventArgs> PreInterfaceLayerDraw;

        /// <summary>
        ///     Occurs when the player gets his stats reset.
        /// </summary>
        public static event EventHandler ResetEffects;

        internal static bool InvokeChat(string text) {
            var args = new ChatEventArgs(text);
            Chat?.Invoke(null, args);
            return args.Handled;
        }

        internal static void InvokeGameInitialized() {
            GameInitialized?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePlayerUpdate() {
            PlayerUpdate?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePostCursorDraw() {
            PostCursorDraw?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokePostDraw() {
            PostDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePostInterfaceLayerDraw(string name) {
            PostInterfaceLayerDraw?.Invoke(null, new InterfaceLayerDrawEventArgs(name));
        }

        internal static void InvokePostUpdate() {
            PostUpdate?.Invoke(null, EventArgs.Empty);
        }

        internal static bool InvokePreCursorDraw() {
            var handledEventArgs = new HandledEventArgs();
            PreCursorDraw?.Invoke(null, handledEventArgs);
            return handledEventArgs.Handled;
        }

        internal static void InvokePreDraw() {
            PreDraw?.Invoke(null, new DrawEventArgs());
        }

        internal static void InvokePreInterfaceLayerDraw(string name) {
            PreInterfaceLayerDraw?.Invoke(null, new InterfaceLayerDrawEventArgs(name));
        }

        internal static void InvokePreUpdate() {
            PreUpdate?.Invoke(null, EventArgs.Empty);
        }

        internal static void InvokeResetEffects() {
            ResetEffects?.Invoke(null, EventArgs.Empty);
        }
    }
}