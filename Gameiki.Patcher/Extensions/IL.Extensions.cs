using System;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Extensions {
    /// <summary>
    ///     Defines extension methods for various Cecil types.
    /// </summary>
    public static class ILExtensions {
        /// <summary>
        ///     Injects the specified instruction range after the provided target instruction.
        /// </summary>
        /// <param name="methodDefinition">The method definition, which must not be <c>null</c>.</param>
        /// <param name="target">The target instruction, which must not be <c>null</c>.</param>
        /// <param name="instructions">The instruction pattern, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void InsertAfter([NotNull] this MethodDefinition methodDefinition, [NotNull] Instruction target,
            [NotNull] params Instruction[] instructions) {
            if (methodDefinition == null) {
                throw new ArgumentNullException(nameof(methodDefinition));
            }

            if (target == null) {
                throw new ArgumentNullException(nameof(target));
            }

            if (instructions == null) {
                throw new ArgumentNullException(nameof(instructions));
            }

            var processor = methodDefinition.Body.GetILProcessor();
            foreach (var instruction in instructions.Reverse()) {
                processor.InsertAfter(target, instruction);
            }
        }

        public static void InsertBefore([NotNull] this MethodDefinition methodDefinition, [NotNull] Instruction target,
            [NotNull] params Instruction[] instructions) {
            if (methodDefinition == null) {
                throw new ArgumentNullException(nameof(methodDefinition));
            }

            if (target == null) {
                throw new ArgumentNullException(nameof(target));
            }

            if (instructions == null) {
                throw new ArgumentNullException(nameof(instructions));
            }

            var processor = methodDefinition.Body.GetILProcessor();
            foreach (var instruction in instructions) {
                processor.InsertBefore(target, instruction);
            }
        }

        /// <summary>
        ///     Scans the method and injects the specified instruction range before any return calls.
        /// </summary>
        /// <param name="methodDefinition">The method definition, which must not be <c>null</c>.</param>
        /// <param name="instructions">The instruction pattern, which must not be <c>null</c>.</param>
        public static void InjectEnds([NotNull] this MethodDefinition methodDefinition, [NotNull] params Instruction[] instructions) {
            if (methodDefinition == null) {
                throw new ArgumentNullException(nameof(methodDefinition));
            }

            if (instructions == null) {
                throw new ArgumentNullException(nameof(instructions));
            }

            var retInstructions = methodDefinition.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToArray();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < retInstructions.Length; ++i) {
                InsertAfter(methodDefinition, retInstructions[i].Previous, instructions);
            }
        }
    }
}