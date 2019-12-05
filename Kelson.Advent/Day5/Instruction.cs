﻿using System;
using System.Linq;

namespace Kelson.Advent.Day5
{
    public readonly struct Instruction
    {
        public delegate int Behavior(int pointer, Span<int> program, Sys system, int[] args);

        public readonly int InstructionPointer;
        public readonly Op Operation;
        public readonly Mode[] ParamModes;
        public readonly bool AnyImmediate => ParamModes.Any(m => m == Mode.Immediate);
        public readonly Behavior EvaluateOperation;

        public Instruction(int instructionPointer, Span<int> program)
        {
            InstructionPointer = instructionPointer;
            (Operation, ParamModes) = DeconstructInstructionCode(program[instructionPointer]);
            EvaluateOperation = Operation switch
            {
                Op.Stop => Stop,
                Op.Add => Add,
                Op.Mult => Mult,
                Op.Read => Store,
                Op.Write => Write,
                Op.JumpIfTrue => JumpIfTrue,
                Op.JumpIfFalse => JumpIfFalse,
                Op.LessThan => LessThan,
                Op.Equals => Equals,
                _ => throw new InvalidOperationException($"Invalid operation: {Operation}")
            };
        }

        static (Op, Mode[]) DeconstructInstructionCode(int code)
        {
            int mode_flags = code / 100;
            Op operation = (Op)(code - mode_flags * 100);
            var modes = new Mode[operation.ParamCount()];
            for (int i = 0; i < modes.Length; i++)
            {
                modes[i] = (Mode)(mode_flags & 1);
                mode_flags /= 10;
            }

            foreach (var force_immediate in operation.AssignmentParams())
                modes[force_immediate] = Mode.Immediate;

            return (operation, modes);
        }


        public int Evaluate(Span<int> program, Sys system)
        {
            var args = program.Slice(InstructionPointer + 1, ParamModes.Length).ToArray();
            for (int i = 0; i < ParamModes.Length; i++)
                args[i] = ParamModes[i] switch
                {
                    Mode.Immediate => program[InstructionPointer + 1 + i],
                    Mode.Position => program[program[InstructionPointer + 1 + i]]
                };

            return EvaluateOperation(InstructionPointer, program, system, args);
        }

        private static int Stop(int pointer, Span<int> program, Sys system, int[] args) => pointer;

        private static int Add(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] + args[1];
            return pointer + args.Length + 1;
        }

        private static int Mult(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] * args[1];
            return pointer + args.Length + 1;
        }

        private static int Store(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[0]] = system.Read();
            return pointer + args.Length + 1;
        }

        private static int Write(int pointer, Span<int> program, Sys system, int[] args)
        {
            system.Write(args[0]);
            return pointer + args.Length + 1;
        }

        private static int JumpIfTrue(int pointer, Span<int> program, Sys system, int[] args)
        {
            if (args[0] != 0)
                return args[1];
            else
                return pointer + args.Length + 1;
        }

        private static int JumpIfFalse(int pointer, Span<int> program, Sys system, int[] args)
        {
            if (args[0] == 0)
                return args[1];
            else
                return pointer + args.Length + 1;
        }

        private static int LessThan(int pointer, Span<int> program, Sys system, int[] args)
        {            
            program[args[2]] = args[0] < args[1] ? 1 : 0;            
            return pointer + args.Length + 1;
        }

        private static int Equals(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] == args[1] ? 1 : 0;
            return pointer + args.Length + 1;
        }
    }
}
