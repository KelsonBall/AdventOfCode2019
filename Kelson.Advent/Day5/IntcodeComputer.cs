using System;

namespace Kelson.Advent.Day5
{
    public static class IntcodeComputer
    {
        public static void EvaluateProgram(Span<int> program, Sys? system = null)
        {
            system ??= new NoOpSystem();
            int ip = 0;
            int next = 0;
            var instruction = new Instruction(ip, program);
            while ((next = instruction.Evaluate(program, system)) != ip)
            {
                ip = next;
                instruction = new Instruction(ip, program);
            }
        }        
    }   
}
