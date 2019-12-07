using System;
using System.Linq;

namespace Kelson.Advent.Day5
{
    public enum ProgramState
    {
        Running,
        Halted,
        Suspended,        
    }

    public readonly struct OpResult
    {
        public readonly int NextProgramPointer;
        public readonly ProgramState State;

        public OpResult(int next) => (NextProgramPointer, State) = (next, ProgramState.Running);

        public OpResult(int next, ProgramState state) => (NextProgramPointer, State) = (next, state);

        public static implicit operator OpResult(int next) => new OpResult(next);
    }

    public readonly struct Instruction
    {
        public delegate OpResult Behavior(int pointer, Span<int> program, Sys system, int[] args);

        public readonly int InstructionPointer;
        public readonly Op Operation;
        public readonly Mode[] ParamModes;
        public readonly bool AnyImmediate => ParamModes.Any(m => m == Mode.Immediate);
        public readonly Behavior EvaluateOperation;

        public string ToString(Span<int> program) => $"{Operation}({Arguments(program)})";

        private string Arguments(Span<int> program)
        {
            int[] args = program.Slice(InstructionPointer + 1, ParamModes.Length).ToArray();
            string[] parameters = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                int arg = args[i];
                Mode mode = ParamModes[i];
                if (mode == Mode.Immediate)
                    parameters[i] = arg.ToString();
                else
                    parameters[i] = $"*{arg}: {program[arg]}";
            }
            return string.Join(", ", parameters);                
        }

        public Instruction(int instructionPointer, Span<int> program)
        {
            InstructionPointer = instructionPointer;
            (Operation, ParamModes) = DeconstructInstructionCode(program[instructionPointer]);
            EvaluateOperation = Operation switch
            {
                Op.Stop => Stop,
                Op.Add => Add,
                Op.Mult => Mult,
                Op.Read => Read,
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


        public OpResult Evaluate(Span<int> program, Sys system)
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

        private static OpResult Stop(int pointer, Span<int> program, Sys system, int[] args) => new OpResult(pointer, ProgramState.Halted);

        private static OpResult Add(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] + args[1];
            return pointer + args.Length + 1;
        }

        private static OpResult Mult(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] * args[1];
            return pointer + args.Length + 1;
        }

        private static OpResult Read(int pointer, Span<int> program, Sys system, int[] args)
        {
            if (system.CanRead())
            {
                program[args[0]] = system.Read();
                return pointer + args.Length + 1;
            }
            else
            {
                return new OpResult(pointer, ProgramState.Suspended);
            }
        }

        private static OpResult Write(int pointer, Span<int> program, Sys system, int[] args)
        {
            system.Write(args[0]);
            return pointer + args.Length + 1;
        }

        private static OpResult JumpIfTrue(int pointer, Span<int> program, Sys system, int[] args)
        {
            if (args[0] != 0)
                return args[1];
            else
                return pointer + args.Length + 1;
        }

        private static OpResult JumpIfFalse(int pointer, Span<int> program, Sys system, int[] args)
        {
            if (args[0] == 0)
                return args[1];
            else
                return pointer + args.Length + 1;
        }

        private static OpResult LessThan(int pointer, Span<int> program, Sys system, int[] args)
        {            
            program[args[2]] = args[0] < args[1] ? 1 : 0;            
            return pointer + args.Length + 1;
        }

        private static OpResult Equals(int pointer, Span<int> program, Sys system, int[] args)
        {
            program[args[2]] = args[0] == args[1] ? 1 : 0;
            return pointer + args.Length + 1;
        }
    }
}
