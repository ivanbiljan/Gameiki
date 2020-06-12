using System.Collections.Generic;
using Terraria;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Represents a command context.
    /// </summary>
    public sealed class CommandContext {
        internal CommandContext(string name, Player player, IEnumerable<string> arguments) {
            CommandName = name;
            Player = player;
            Arguments = arguments;
        }
        
        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IEnumerable<string> Arguments { get; }
        
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string CommandName { get; }
        
        /// <summary>
        /// Gets the player.
        /// </summary>
        public Player Player { get; }
    }
}