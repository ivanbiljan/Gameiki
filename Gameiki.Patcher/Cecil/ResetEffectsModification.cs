using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class ResetEffectsModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var player = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Player");
            Debug.Assert(player != null);

            var resetEffects = player.Methods.FirstOrDefault(m => m.Name == "ResetEffects");
            Debug.Assert(resetEffects != null);

            resetEffects.InjectEnds(Instruction.Create(OpCodes.Call,
                player.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokeResetEffects),
                    BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}