namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Defines a contract that describes a command.
    /// </summary>
    public interface ICommand {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// Gets the help text.
        /// </summary>
        string HelpText { get; }
    }
}