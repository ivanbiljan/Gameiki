using System.Linq;
using Gameiki.Patcher.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Cecil {
    // As requested - originally part of TerrariaPatcher
    internal sealed class SteamInitializeModification : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            var socialApi = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.Name == "SocialAPI");
            var initializeSteam = socialApi.Methods.FirstOrDefault(m => m.Name == "Initialize");
            var shutdown = socialApi.Methods.FirstOrDefault(m => m.Name == "Shutdown");

            initializeSteam.InsertBefore(initializeSteam.Body.Instructions[0], Instruction.Create(OpCodes.Ret));
            shutdown.InsertBefore(shutdown.Body.Instructions[0], Instruction.Create(OpCodes.Ret));
            
            var notificationsTracker = assemblyDefinition.MainModule.Types.FirstOrDefault(t => t.FullName == "Terraria.UI.InGameNotificationsTracker");
            var initializeTracker = notificationsTracker.Methods.FirstOrDefault(m => m.Name == "Initialize");
            var processor = initializeTracker.Body.GetILProcessor();
            processor.InsertBefore(initializeTracker.Body.Instructions[0], Instruction.Create(OpCodes.Ret));
        }
    }
}