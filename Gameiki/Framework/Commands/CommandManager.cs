// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using Terraria;
//
// namespace Gameiki.Framework.Commands {
//     /// <summary>
//     /// Represents the command manager.
//     /// </summary>
//     public sealed class CommandManager {
//         private static CommandManager _instance;
//         
//         private readonly IList<Command> _commands = new List<Command>();
//         
//         private CommandManager() {
//         }
//         
//         /// <summary>
//         ///     Parses parameters from the specified input string. Supports quotation marks.
//         /// </summary>
//         /// <param name="input">The input string.</param>
//         /// <returns>The parameters.</returns>
//         private static IEnumerable<string> ParseParameters(string input)
//         {
//             var parameters = new List<string>();
//             if (input == null)
//             {
//                 return parameters;
//             }
//
//             var stringBuilder = new StringBuilder();
//             var inQuotes = false;
//             foreach (var currentCharacter in input)
//             {
//                 switch (currentCharacter)
//                 {
//                     case ' ':
//                     case '\t':
//                     case '\n':
//                     {
//                         if (inQuotes)
//                         {
//                             stringBuilder.Append(currentCharacter);
//                         }
//                         else
//                         {
//                             if (stringBuilder.Length == 0)
//                             {
//                                 continue;
//                             }
//
//                             parameters.Add(stringBuilder.ToString());
//                             stringBuilder.Clear();
//                         }
//
//                         break;
//                     }
//
//                     case '"':
//                     {
//                         inQuotes = !inQuotes;
//                         if (inQuotes)
//                         {
//                             continue;
//                         }
//
//                         parameters.Add(stringBuilder.ToString());
//                         stringBuilder.Clear();
//                         break;
//                     }
//
//                     default:
//                         stringBuilder.Append(currentCharacter);
//                         break;
//                 }
//             }
//
//             if (stringBuilder.Length > 0)
//             {
//                 parameters.Add(stringBuilder.ToString());
//             }
//
//             return parameters;
//         }
//
//
//         /// <summary>
//         /// Gets the command manager instance.
//         /// </summary>
//         public static CommandManager Instance => _instance ?? (_instance = new CommandManager());
//
//         /// <summary>
//         /// Gets the commands.
//         /// </summary>
//         public IEnumerable<Command> Commands => _commands.AsEnumerable();
//
//         /// <summary>
//         /// Registers a command with the specified name and callback.
//         /// </summary>
//         /// <param name="name">The name, which must not be <c>null</c>.</param>
//         /// <param name="callback">The callback, which must not be <c>null</c>.</param>
//         /// <param name="description">An optional description of the command's functionality.</param>
//         /// <param name="helpText">The help text.</param>
//         /// <exception cref="ArgumentNullException"></exception>
//         public void Register(string name, Action<CommandContext> callback, string description = null, string helpText = null) {
//             if (name == null) {
//                 throw new ArgumentNullException(nameof(name));
//             }
//
//             if (callback == null) {
//                 throw new ArgumentNullException(nameof(callback));
//             }
//
//             //_commands.Add(new Command(new[] {name}, description, helpText, callback));
//         }
//
//         /// <summary>
//         /// Runs any command that matches the given input.
//         /// </summary>
//         /// <param name="input">The input string.</param>
//         public void Run(string input) {
//             if (string.IsNullOrWhiteSpace(input)) {
//                 return;
//             }
//
//             var indexOfSpace = input.IndexOf(' ');
//             indexOfSpace = indexOfSpace < 0 ? input.Length : indexOfSpace;
//
//             var commandName = input.Substring(0, indexOfSpace);
//             var command = _commands.FirstOrDefault(c => c.Aliases.Contains(commandName));
//             //command?.Callback(new CommandContext(commandName, Main.player[Main.myPlayer], ParseParameters(input.Substring(indexOfSpace))));
//         }
//     }
// }