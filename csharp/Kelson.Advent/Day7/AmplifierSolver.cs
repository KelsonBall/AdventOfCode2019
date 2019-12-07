using System;
using static Kelson.Advent.Input;
using static System.Linq.Enumerable;

namespace Kelson.Advent.Day7
{
    using Day5;
    using System.Collections.Generic;
    using System.Linq;

    public class AmplifierSolver
    {        

        private readonly int[] program;

        public AmplifierSolver(Span<int> program)
        {
            this.program = program.ToArray();
        }

        public int FindMaxOutput() => allCombos().Select(c => TryArangement(0, c.a, c.b, c.c, c.d, c.e)).Max();

        public int TryArangement(int input, int a, int b, int c, int d, int e)
        {
            Log("");
            Log($">> Trying {a}, {b}, {c}, {d}, {e}");
            int a_result = RunControllerSoftware(a, input);
            Log($"A>> {a_result}");
            int b_result = RunControllerSoftware(b, a_result);
            Log($"B>> {b_result}");
            int c_result = RunControllerSoftware(c, b_result);
            Log($"C>> {c_result}");
            int d_result = RunControllerSoftware(d, c_result);
            Log($"D>> {d_result}");
            int e_result = RunControllerSoftware(e, d_result);
            Log($"E>> {e_result}");
            return e_result;
        }
        
        public int FindMaxWithFeedback() => allCombos(5).Select(c => TryArangementWithFeedback(0, c.a, c.b, c.c, c.d, c.e)).Max();

        public int TryArangementWithFeedback(int input, params int[] amp_settings)
        {
            var memories = Range(0, 5).Select(i => program.ToArray()).ToArray();
            var systems = Range(0, 5).Select(i => QueueStoreSystem.CreateDuplux()).ToArray();
            foreach (var index in Range(0, 5))
                systems[index].device.Write(amp_settings[index]);

            var results = Range(0, 5).Select(i => new ProgramResult { ProgramPointer = 0, State = ProgramState.Running, System = systems[i].system }).ToArray();
            int amp = 0;
            int loops = 0;
            var last = RunControllerSoftwareWithFeedback(memories[amp], results[amp], systems[amp].device, 0);
            results[amp] = last;
            int[] outputs = new int[] { systems[amp].device.Read(), 0, 0, 0, 0 };
            while (!results.All(r => r.State == ProgramState.Halted))
            {                
                int last_output = outputs[amp];
                amp += 1;
                if (amp > 4)
                    (amp, loops) = (0, loops + 1);

                last = RunControllerSoftwareWithFeedback(memories[amp], results[amp], systems[amp].device, last_output);
                results[amp] = last;
                outputs[amp] = systems[amp].device.Read();
            }
            Log($">> feedback cycle completed in {loops} loops");
            return outputs[4];
        }


        private ProgramResult RunControllerSoftwareWithFeedback(int[] memory, ProgramResult state, Sys device, int input)
        {
            device.Write(input);
            return state.Resume(memory);            
        }

        private int RunControllerSoftware(int amp, int input)
        {
            var memory = program.ToArray();
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(amp);
            device.Write(input);
            IntcodeComputer.EvaluateProgram(memory, system);
            return device.Read();
        }

        public static IEnumerable<(int a, int b, int c, int d, int e)> allCombos(int start = 0, int count = 5)
        {
            foreach (var a in Range(start, count))
            {
                foreach (var b in Range(start, count))
                {
                    if (a == b)
                        continue;
                    foreach (var c in Range(start, count))
                    {
                        if (c == a || c == b)
                            continue;
                        foreach (var d in Range(start, count))
                        {
                            if (d == c || d == b || d == a)
                                continue;
                            foreach (var e in Range(start, count))
                            {
                                if (e == d || e == c || e == b || e == a)
                                    continue;
                                yield return (a, b, c, d, e);
                            }
                        }
                    }
                }
            }
        }
    }
}