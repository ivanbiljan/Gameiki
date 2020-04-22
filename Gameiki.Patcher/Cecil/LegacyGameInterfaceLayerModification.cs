using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace Gameiki.Patcher.Cecil {
    internal sealed class LegacyGameInterfaceLayerModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var legacyInterfaceLayer = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "LegacyGameInterfaceLayer");
            Debug.Assert(legacyInterfaceLayer != null);

            var ctor = legacyInterfaceLayer.Methods.FirstOrDefault(m => m.Name == ".ctor");
            Debug.Assert(ctor != null);

            var drawSelf = legacyInterfaceLayer.Methods.FirstOrDefault(m => m.Name == "DrawSelf");
            Debug.Assert(drawSelf != null);

            var nameField = new FieldDefinition("_name", FieldAttributes.Private,
                legacyInterfaceLayer.Module.ImportReference(typeof(string)));
            legacyInterfaceLayer.Fields.Add(nameField);

            ctor.InjectEnds(Instruction.Create(OpCodes.Ldarg_0), Instruction.Create(OpCodes.Ldarg_1),
                Instruction.Create(OpCodes.Stfld, nameField));

            drawSelf.InsertBefore(drawSelf.Body.Instructions[0],
                Instruction.Create(OpCodes.Ldarg_0), Instruction.Create(OpCodes.Ldfld, nameField),
                Instruction.Create(OpCodes.Call,
                    legacyInterfaceLayer.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePreInterfaceLayerDraw),
                        BindingFlags.NonPublic | BindingFlags.Static))));
            drawSelf.InjectEnds(Instruction.Create(OpCodes.Ldarg_0), Instruction.Create(OpCodes.Ldfld, nameField),
                Instruction.Create(OpCodes.Call,
                    legacyInterfaceLayer.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePostInterfaceLayerDraw),
                        BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}