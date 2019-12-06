using System;

namespace Kelson.Advent.Day5
{
    public enum Op
    {
        /// <summary>
        /// Add a b *
        /// </summary>
        Add = 1,
        
        /// <summary>
        /// Mult a b *
        /// </summary>
        Mult = 2,
        
        /// <summary>
        /// Set * << input
        /// </summary>
        Read = 3,
        
        /// <summary>
        /// Write a >> output
        /// </summary>
        Write = 4,

        /// <summary>
        /// Exit program
        /// </summary>
        Stop = 99,

        /// <summary>
        /// If the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing
        /// </summary>
        JumpIfTrue = 5,

        /// <summary>
        /// If the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing
        /// </summary>
        JumpIfFalse = 6,

        /// <summary>
        /// If the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0
        /// </summary>
        LessThan = 7,

        /// <summary>
        /// If the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0
        /// </summary>
        Equals = 8
    }

    public static class OpParameters
    {
        public static int ParamCount(this Op operand) => operand switch
        {
            Op.Stop => 0,
            Op.Add => 3,
            Op.Mult => 3,
            Op.Read => 1,
            Op.Write => 1,
            Op.JumpIfTrue => 2,
            Op.JumpIfFalse => 2,
            Op.LessThan => 3,
            Op.Equals => 3,
            _ => throw new InvalidOperationException($"Invalid operation: {operand}")
        };

        public static int[] AssignmentParams(this Op operand) => operand switch
        {
            Op.Stop => new int[] { },
            Op.Add => new int[] { 2 },
            Op.Mult => new int[] { 2 },
            Op.Read => new int[] { 0 },
            Op.Write => new int[] { },
            Op.JumpIfTrue => new int[] { },
            Op.JumpIfFalse => new int[] { },
            Op.LessThan => new int[] { 2 },
            Op.Equals => new int[] { 2 },
            _ => throw new InvalidOperationException($"Invalid operation: {operand}")
        };
    }
}
