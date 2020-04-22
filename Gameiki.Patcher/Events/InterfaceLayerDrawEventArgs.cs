using System;

namespace Gameiki.Patcher.Events {
    /// <summary>
    ///     Provides event data for the <see cref="Hooks.PreInterfaceLayerDraw" /> and
    ///     <see cref="Hooks.PostInterfaceLayerDraw" /> events.
    /// </summary>
    public sealed class InterfaceLayerDrawEventArgs : EventArgs {
        public InterfaceLayerDrawEventArgs(string name) {
            Name = name;
        }

        /// <summary>
        ///     Gets the name of the layer being drawn.
        /// </summary>
        public string Name { get; }
    }
}