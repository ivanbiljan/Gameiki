using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Represents a command manager.
    /// </summary>
    public sealed class CommandManager {
        private readonly IList<Command> _commands = new List<Command>();

        private CommandManager() {
            
        }
        
        public static  CommandManager Instance { get; } = new CommandManager();

        public IEnumerable<Command> Commands => _commands.AsEnumerable();

        public void RegisterCommand(Regex syntaxRegex, Action<Match> callback, string helpText = null) {
            if (syntaxRegex == null) {
                throw new ArgumentNullException(nameof(syntaxRegex));
            }

            if (callback == null) {
                throw new ArgumentNullException(nameof(callback));
            }

            var command = new Command(syntaxRegex, input => callback.Invoke(syntaxRegex.Match(input)), helpText);
            _commands.Add(command);
        }

        public void RunCommand(string input) {
            for (var i = 0; i < _commands.Count; ++i) {
                var command = _commands[i];
                if (!command.Syntax.IsMatch(input)) {
                    continue;
                }
                
                command.Callback.Invoke(input);
            }
        }
    }
}