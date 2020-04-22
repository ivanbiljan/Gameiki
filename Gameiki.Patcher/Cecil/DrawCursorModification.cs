using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class DrawCursorModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var main = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Main");
            Debug.Assert(main != null);

            var drawCursorInterface = main.Methods.FirstOrDefault(m => m.Name == "DrawInterface_36_Cursor");
            Debug.Assert(drawCursorInterface != null);

            drawCursorInterface.InsertBefore(drawCursorInterface.Body.Instructions[0],
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePreCursorDraw),
                        BindingFlags.NonPublic | BindingFlags.Static))),
                Instruction.Create(OpCodes.Brfalse_S, drawCursorInterface.Body.Instructions[0]),
                Instruction.Create(OpCodes.Ret));
            drawCursorInterface.InjectEnds(Instruction.Create(OpCodes.Call,
                main.Module.ImportReference(typeof(Hooks).GetMethod(nameof(Hooks.InvokePostCursorDraw),
                    BindingFlags.NonPublic | BindingFlags.Static))));
            drawCursorInterface.ReplaceShortBranches();
        }
    }
}