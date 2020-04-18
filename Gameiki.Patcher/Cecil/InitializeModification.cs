using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class InitializeModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var main = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Main");
            Debug.Assert(main != null);

            var initialize = main.Methods.FirstOrDefault(m => m.Name == "Initialize");
            Debug.Assert(initialize != null);

            initialize.InjectEnds(Instruction.Create(OpCodes.Call,
                main.Module.ImportReference(typeof(Hooks).GetMethod("InvokeGameInitialized",
                    BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}