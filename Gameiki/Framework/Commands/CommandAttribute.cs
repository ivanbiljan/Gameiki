using System;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Describes a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute {
        internal CommandAttribute(string name) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Gets or sets the command description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        public string HelpText { get; set; }
    }
}