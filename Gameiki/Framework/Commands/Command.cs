using System;
using System.Text.RegularExpressions;

namespace Gameiki.Framework.Commands {
    public sealed class Command {
        public Command(Regex syntaxRegex, Action<string> callback, string helpText = null) {
            Syntax = syntaxRegex ?? throw new ArgumentNullException(nameof(syntaxRegex));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            HelpText = helpText;
        }

        internal Action<string> Callback { get; }

        public string HelpText { get; }

        public Regex Syntax { get; }
    }
}