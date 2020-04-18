using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gameiki.Patcher.Events;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class DoUpdateModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var main = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "Main");
            Debug.Assert(main != null);

            var doUpdate = main.Methods.FirstOrDefault(m => m.Name == "DoUpdate");
            Debug.Assert(doUpdate != null);
            
            doUpdate.InsertBefore(doUpdate.Body.Instructions[0],
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod("InvokePreUpdate",
                        BindingFlags.NonPublic | BindingFlags.Static))));
            doUpdate.InjectEnds(
                Instruction.Create(OpCodes.Call,
                    main.Module.ImportReference(typeof(Hooks).GetMethod("InvokePostUpdate",
                        BindingFlags.NonPublic | BindingFlags.Static))));
        }
    }
}