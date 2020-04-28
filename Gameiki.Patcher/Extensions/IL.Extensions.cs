using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Gameiki.Patcher.Extensions {
    /// <summary>
    ///     Defines extension methods for various Cecil types.
    /// </summary>
    public static class ILExtensions {
        private static readonly Dictionary<OpCode, OpCode> BranchMap = new Dictionary<OpCode, OpCode> {
            {OpCodes.Beq_S, OpCodes.Beq},
            {OpCodes.Bge_S, OpCodes.Bge},
            {OpCodes.Bge_Un_S, OpCodes.Bge_Un},
            {OpCodes.Bgt_S, OpCodes.Bgt},
            {OpCodes.Bgt_Un_S, OpCodes.Bgt_Un},
            {OpCodes.Ble_S, OpCodes.Ble},
            {OpCodes.Ble_Un_S, OpCodes.Ble_Un},
            {OpCodes.Blt_S, OpCodes.Blt},
            {OpCodes.Blt_Un_S, OpCodes.Blt_Un},
            {OpCodes.Bne_Un_S, OpCodes.Bne_Un},
            {OpCodes.Br_S, OpCodes.Br},
            {OpCodes.Brfalse_S, OpCodes.Brfalse},
            {OpCodes.Brtrue_S, OpCodes.Brtrue},
            {OpCodes.Leave_S, OpCodes.Leave}
        };

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

            for (var i = methodDefinition.Body.Instructions.Count - 1; i >= 0; --i) {
                var currentInstruction = methodDefinition.Body.Instructions[i];
                if (currentInstruction.OpCode != OpCodes.Ret) {
                    continue;
                }

                /*
                 * Scopes are a bit tricky to deal with in IL when branching is incorporated
                 * consider the following example:
                 * if (expression) {
                 *     ret call
                 * } else if (expression2) {
                 *     ret call
                 * } else {
                 * }
                 * ret call
                 *
                 * The else statement will attempt to branch to the final ret call.
                 * If we were to insert more instructions they would fall into the scope of that particular else block
                 * As a result, we have to cancel out the ret call and inject the logic we wish to execute, after which comes the final ret instruction
                 */
                currentInstruction.OpCode = OpCodes.Nop;
                InsertAfter(methodDefinition, currentInstruction, Instruction.Create(OpCodes.Ret));
                foreach (var instruction in instructions.Reverse()) {
                    InsertAfter(methodDefinition, currentInstruction, instruction);
                }
            }
        }

        /// <summary>
        ///     Replaces short form branching instructions with their corresponding long equivalent.
        /// </summary>
        /// <param name="methodDefinition">The method definition.</param>
        public static void ReplaceShortBranches(this MethodDefinition methodDefinition) {
            foreach (var instruction in methodDefinition.Body.Instructions.Where(
                i => i.OpCode.OperandType == OperandType.ShortInlineBrTarget)) {
                instruction.OpCode = BranchMap[instruction.OpCode];
            }
        }

        /// <summary>
        ///     Removes the specified instruction range starting from the specified index.
        /// </summary>
        /// <param name="methodDefinition">The method definition.</param>
        /// <param name="startIndex">The index of the first instruction to remove.</param>
        /// <param name="numberOfInstructions">The number of instructions to remove.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="startIndex" /> is out of range.</exception>
        public static void RemoveInstructionRange(this MethodDefinition methodDefinition, int startIndex,
            int numberOfInstructions) {
            if (startIndex < 0 || startIndex > methodDefinition.Body.Instructions.Count - 1) {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            for (var i = startIndex; i < startIndex + numberOfInstructions; ++i) {
                methodDefinition.Body.Instructions.RemoveAt(startIndex);
            }
        }

        /// <summary>
        ///     Scans the method definition for the specified instruction pattern and returns the index
        ///     of the first instruction in the pattern.
        /// </summary>
        /// <param name="methodDefinition">The method definition.</param>
        /// <param name="opCodes">The instructions, which must not be <c>null</c>.</param>
        /// <returns>The index of the first instruction in the pattern, or -1 if the pattern has no match.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="opCodes" /> is <c>null</c>.</exception>
        public static int ScanInstructionPattern(this MethodDefinition methodDefinition,
            [NotNull] params OpCode[] opCodes) {
            if (opCodes == null) {
                throw new ArgumentNullException(nameof(opCodes));
            }

            for (var i = 0; i < methodDefinition.Body.Instructions.Count; ++i) {
                var instruction = methodDefinition.Body.Instructions[i];
                if (instruction.OpCode != opCodes[0]) {
                    continue;
                }

                if (opCodes.TakeWhile((oc, ix) => methodDefinition.Body.Instructions[i + ix].OpCode == oc)
                    .SequenceEqual(opCodes)) {
                    return i;
                }
            }

            return -1;
        }
    }
}