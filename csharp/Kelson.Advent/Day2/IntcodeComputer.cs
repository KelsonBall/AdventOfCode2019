using System;

namespace Kelson.Advent.Day2
{
    public static class IntcodeComputer
    {
        const int ADD_OP = 1;
        const int MULT_OP = 2;
        const int STOP_OP = 99;

        public static int[] Evaluate(int[] program, int index = 0) => program[index] switch 
        {
            STOP_OP => program,
            ADD_OP => Add(program, index),
            MULT_OP => Mult(program, index),
            _ => throw new InvalidOperationException()
        };

        private static int[] Add(int[] program, int index)
        {
            program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]];
            return Evaluate(program, index + 4);
        }

        private static int[] Mult(int[] program, int index)
        {
            program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]];
            return Evaluate(program, index + 4);
        }
    }
}
