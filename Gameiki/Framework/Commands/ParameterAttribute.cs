using System;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Describes a command parameter.
    /// </summary>
    public sealed class ParameterAttribute : Attribute {
        internal ParameterAttribute(params string[] aliases) {
            Aliases = aliases;
        }
        
        /// <summary>
        /// Gets the aliases.
        /// </summary>
        public string[] Aliases { get; }
        
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the parameter is optional.
        /// </summary>
        public bool IsOptional { get; set; }
    }
}