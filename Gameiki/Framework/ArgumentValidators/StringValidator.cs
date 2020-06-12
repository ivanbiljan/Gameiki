using Gameiki.Framework.Commands;

namespace Gameiki.Framework.ArgumentValidators {
    public class StringValidator : IArgument {
        public object Validate(string input) => input;
    }
}