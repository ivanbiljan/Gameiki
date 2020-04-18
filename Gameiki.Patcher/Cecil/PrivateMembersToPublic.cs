using Mono.Cecil;

namespace Gameiki.Patcher.Cecil {
    internal sealed class PrivateMembersToPublic : IModification {
        public void Execute(AssemblyDefinition assemblyDefinition) {
            foreach (var type in assemblyDefinition.MainModule.Types) {
                MakeTypePublic(type);
            }

            void MakeTypePublic(TypeDefinition typeDefinition) {
                foreach (var @event in typeDefinition.Events) {
                    @event.AddMethod.IsPrivate = false;
                    @event.AddMethod.IsPublic = true;

                    @event.RemoveMethod.IsPrivate = false;
                    @event.RemoveMethod.IsPublic = true;
                }

                foreach (var field in typeDefinition.Fields) {
                    field.IsPrivate = false;
                    field.IsPublic = true;
                }

                foreach (var property in typeDefinition.Properties) {
                    if (property.GetMethod != null) {
                        property.GetMethod.IsPrivate = false;
                        property.GetMethod.IsPublic = true;
                    }
                    
                    if (property.SetMethod != null) {
                        property.SetMethod.IsPrivate = false;
                        property.SetMethod.IsPublic = true;
                    }
                }

                foreach (var method in typeDefinition.Methods) {
                    method.IsPrivate = false;
                    method.IsPublic = true;
                }

                foreach (var nestedType in typeDefinition.NestedTypes) {
                    MakeTypePublic(nestedType);
                }
            }
        }
    }
}