using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class PlayerUpdateModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var player = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Player");
            Debug.Assert(player != null);

            var update = player.Methods.FirstOrDefault(m => m.Name == "Update");
            Debug.Assert(update != null);

            update.InjectEnds(Instruction.Create(OpCodes.Call,
                player.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePlayerUpdate),
                    BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}