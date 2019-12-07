using System;

namespace Kelson.Advent.Day5
{
    public class ProgramResult
    {
        public Sys System { get; set; }
        public ProgramState State { get; set; }
        public int ProgramPointer { get; set; }
        
        public ProgramResult Resume(Span<int> program) => IntcodeComputer.EvaluateProgram(program, System, ProgramPointer);
        
    }

    public static class IntcodeComputer
    {
        public static ProgramResult EvaluateProgram(Span<int> program, Sys? system = null, int? start_pointer = null)
        {
            system ??= new NoOpSystem();
            int ip = start_pointer ?? 0;
            while (true)
            {
                var instruction = new Instruction(ip, program);
                Console.WriteLine($"@{ip}::{instruction.ToString(program)}");
                var result = instruction.Evaluate(program, system);
                if (result.State == ProgramState.Running)
                    ip = result.NextProgramPointer;
                else
                    return new ProgramResult
                    {
                        State = result.State,
                        ProgramPointer = result.NextProgramPointer,
                        System = system
                    };
            }
        }        
    }   
}
