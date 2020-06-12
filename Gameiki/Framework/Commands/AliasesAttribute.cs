// using System;
// using JetBrains.Annotations;
//
// namespace Gameiki.Framework.Commands {
//     /// <summary>
//     /// Defines aliases for a command parameter.
//     /// </summary>
//     [PublicAPI]
//     public sealed class AliasesAttribute : Attribute {
//         /// <summary>
//         /// Initializes a new instance of the <see cref="AliasesAttribute"/> class with the specified aliases.
//         /// </summary>
//         /// <param name="aliases">The aliases.</param>
//         public AliasesAttribute(params string[] aliases) {
//             Aliases = aliases;
//         }
//         
//         /// <summary>
//         /// Gets the aliases.
//         /// </summary>
//         public string[] Aliases { get; }
//     }
// }