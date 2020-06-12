using System;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Defines a default value for a command parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class DefaultAttribute : Attribute {
        internal DefaultAttribute(object value) {
            Value = value;
        }
        
        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value { get; }
    }
}