using System.Linq;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    // As requested - originally part of TerrariaPatcher
    internal sealed class SteamInitializeModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var socialApi = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "SocialAPI");
            var initialize = socialApi.Methods.FirstOrDefault(m => m.Name == "Initialize");
            var shutdown = socialApi.Methods.FirstOrDefault(m => m.Name == "Shutdown");

            initialize.InsertBefore(initialize.Body.Instructions[0], Instruction.Create(OpCodes.Ret));
            shutdown.InsertBefore(shutdown.Body.Instructions[0], Instruction.Create(OpCodes.Ret));
        }
    }
}