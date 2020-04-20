using JetBrains.Annotations;
using Mono.Cecil;

namespace Gameiki.Patcher.Cecil {
    /// <summary>
    ///     Specifies a contract that all Terraria assembly modifications must implement.
    /// </summary>
    public interface IModification {
        /// <summary>
        ///     Executes the modification.
        /// </summary>
        /// <param name="assemblyDefinition">The assembly definition, which must not be <c>null</c>.</param>
        void Execute([NotNull] AssemblyDefinition assemblyDefinition);
    }
}