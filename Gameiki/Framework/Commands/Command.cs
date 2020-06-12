using System;
using System.Windows.Forms;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Represents a command.
    /// </summary>
    public sealed class Command {
        internal readonly Action<string> Callback;
        
        /// <summary>
        /// Gets the aliases.
        /// </summary>
        public string[] Aliases { get; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// Gets the help text, i.e syntax examples and such.
        /// </summary>
        public string HelpText { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class with the specified properties.
        /// </summary>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <param name="helpText">The help text.</param>
        /// <param name="callback">The callback method.</param>
        public Command(string[] aliases, string description, string helpText, Action<string> callback) {
            Aliases = aliases ?? new string[0];
            Description = description;
            HelpText = helpText;
            Callback = callback;
        }
    }
}