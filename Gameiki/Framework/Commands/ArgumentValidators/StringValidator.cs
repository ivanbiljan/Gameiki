using Gameiki.Framework.Commands;

namespace Gameiki.Framework.Commands.ArgumentValidators {
    public class StringValidator : IArgument {
        public object Validate(string input) => input;
    }
}