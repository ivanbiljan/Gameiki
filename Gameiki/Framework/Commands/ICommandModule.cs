using System.Collections.Generic;

namespace Gameiki.Framework.Commands {
    /// <summary>
    /// Defines a contract that describes a command module.
    /// </summary>
    public interface ICommandModule {
        void Register(ICommand command);
        void Run(ICommand command);
    }
}