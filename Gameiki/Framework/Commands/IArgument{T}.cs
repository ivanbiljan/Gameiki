namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Defines a contract that describes a command argument.
    /// </summary>
    /// <typeparam name="T">The type of argument.</typeparam>
    public interface IArgument {
        /// <summary>
        /// Validates the argument based on the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed argument.</returns>
        object Validate(string input);
    }
}