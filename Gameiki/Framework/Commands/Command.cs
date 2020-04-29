using System;
using System.Text.RegularExpressions;

namespace Gameiki.Framework.Commands {
    public sealed class Command {
        internal Action<string> Callback { get; }
        
        public Command(Regex syntaxRegex, Action<string> callback, string helpText = null) {
            Syntax = syntaxRegex ?? throw new ArgumentNullException(nameof(syntaxRegex));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            HelpText = helpText;
        }

        public Regex Syntax { get; }
        
        public string HelpText { get; }
    }
}