using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class DrawModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var main = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Main");
            Debug.Assert(main != null);

            var doDraw = main.Methods.FirstOrDefault(m => m.Name == "DoDraw");
            Debug.Assert(doDraw != null);
            
            var doDrawTargetInstruction = doDraw.Body.Instructions.Single(i =>
                i.OpCode == OpCodes.Callvirt && i.Operand is MethodReference methodReference &&
                methodReference.DeclaringType.FullName == "Microsoft.Xna.Framework.Graphics.GraphicsDevice" &&
                methodReference.Name == "Clear");
            doDraw.InsertAfter(doDrawTargetInstruction,
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod("InvokePreDraw",
                        BindingFlags.NonPublic | BindingFlags.Static))));
            doDraw.InjectEnds(
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod("InvokePostDraw",
                        BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}