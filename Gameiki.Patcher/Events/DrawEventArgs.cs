using System;
using JetBrains.Annotations;

namespace Gameiki.Patcher.Events {
    /// <summary>
    /// Provides data for <see cref="Hooks.PreDraw"/> and <see cref="Hooks.PostDraw"/> events.
    /// </summary>
    [PublicAPI]
    public sealed class DrawEventArgs : EventArgs {
        // TODO provide contextual information (if available/useful)
    }
}