using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class ChatModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var main = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Main");
            Debug.Assert(main != null);

            // Chatting is disabled unless the communication is strictly client -> server
            // Thus, we need to remove the instructions that prevent singleplayer input
            // This is done by finding the proper instruction range and the corresponding indices
            var doUpdateToggleChat = main.Methods.FirstOrDefault(m => m.Name == "DoUpdate_Enter_ToggleChat");
            var startInstructionIndex = doUpdateToggleChat.ScanInstructionPattern(OpCodes.Ldsfld, OpCodes.Ldc_I4_1, OpCodes.Bne_Un);
            doUpdateToggleChat.RemoveInstructionRange(startInstructionIndex, 3);

            // Finally, inject a hook to allow plugins to handle the text
            var doUpdateHandleChat = main.Methods.FirstOrDefault(m => m.Name == "DoUpdate_HandleChat");
            var doUpdateTargetInstructionIndex = doUpdateHandleChat.ScanInstructionPattern(OpCodes.Ldsfld,
                OpCodes.Newobj,
                OpCodes.Stloc_2, OpCodes.Ldsfld, OpCodes.Ldloc_2, OpCodes.Callvirt);
            var doUpdateTargetInstruction = doUpdateHandleChat.Body.Instructions[doUpdateTargetInstructionIndex];
            doUpdateHandleChat.InsertBefore(doUpdateTargetInstruction,
                Instruction.Create(OpCodes.Ldsfld, main.Fields.FirstOrDefault(f => f.Name == "chatText")),
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokeChat),
                        BindingFlags.NonPublic | BindingFlags.Static))),
                Instruction.Create(OpCodes.Brtrue_S, doUpdateTargetInstruction.Previous.Operand as Instruction));
            doUpdateHandleChat.ReplaceShortBranches();
        }
    }
}