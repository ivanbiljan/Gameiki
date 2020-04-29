using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class HurtModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var player = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Player");
            Debug.Assert(player != null);

            var hurt = player.Methods.FirstOrDefault(m => m.Name == "Hurt");
            Debug.Assert(hurt != null);

            hurt.InsertBefore(hurt.Body.Instructions[0],
                Instruction.Create(OpCodes.Call,
                    player.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePreHurt),
                        BindingFlags.NonPublic | BindingFlags.Static))), Instruction.Create(OpCodes.Brfalse_S, hurt.Body.Instructions[0]),
                Instruction.Create(OpCodes.Ldc_R8, 0.0D),
                Instruction.Create(OpCodes.Ret));
            hurt.InjectEnds(Instruction.Create(OpCodes.Call,
                player.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePostHurt),
                    BindingFlags.NonPublic | BindingFlags.Static))));
            hurt.ReplaceShortBranches();
        }
    }
}