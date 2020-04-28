using System.ComponentModel;

namespace Gameiki.Patcher.Events {
    /// <summary>
    /// Provides event data for the <see cref="Hooks.Chat"/> event.
    /// </summary>
    public sealed class ChatEventArgs : HandledEventArgs {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatEventArgs"/> class with the specified message.
        /// </summary>
        /// <param name="text">The message.</param>
        public ChatEventArgs(string text) {
            Text = text;
        }
        
        /// <summary>
        /// Gets the chat text.
        /// </summary>
        public string Text { get; }
    }
}