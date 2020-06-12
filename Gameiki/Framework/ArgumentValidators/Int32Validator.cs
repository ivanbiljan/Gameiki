using Gameiki.Framework.Commands;

namespace Gameiki.Framework.ArgumentValidators {
    public sealed class Int32Validator : IArgument {
        public object Validate(string input) => int.Parse(input);
    }
}