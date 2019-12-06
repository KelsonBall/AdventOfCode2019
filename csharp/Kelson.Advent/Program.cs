using static System.Console;
using static System.Linq.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using Kelson.Advent.Day1;
using Day2Computer = Kelson.Advent.Day2.IntcodeComputer;
using Day5Computer = Kelson.Advent.Day5.IntcodeComputer;
using Kelson.Advent.Day3;
using Kelson.Advent.Day4;
using Kelson.Advent.Day5;
using Kelson.Advent.Day6;

namespace Kelson.Advent
{
    class Program
    {
        static readonly string TODAY = DateTime.UtcNow.Day.ToString();

        static void Main(string[] args)
        {
            int day = int.Parse(args.FirstOrDefault() ?? TODAY);
            switch (day)
            {
                case 1: Day1(); break;
                case 2: Day2(); break;
                case 3: Day3(); break;
                case 4: Day4(); break;
                case 5: Day5(); break;
                case 6: Day6(); break;
            }
        }

        static void Day1()
        {
            WriteLine("--- Day 1: The Tyranny of the Rocket Equation ---");
            var modules = "Day1/input.txt".ReadLines().Select(int.Parse).ToArray();

            WriteLine("Part 1 - Required fuel for modules:");
            var total = modules.ModuleFuelRequirement();
            WriteLine(total);

            WriteLine("Part 2 - Required fuel for modules and fuel:");
            total = modules.TotalModuleFuelRequirement();
            WriteLine(total);
        }

        static void Day2()
        {
            WriteLine("--- Day 2: 1202 Program Alarm ---");
            int[] program = "Day2/input.txt".ReadLines().Single().Split(",").Select(int.Parse).ToArray();
            
            WriteLine("Part 1 - 12, 02 program");
            var copy = program.ToArray();
            (copy[1], copy[2]) = (12, 2);
            int[] result = Day2Computer.Evaluate(copy);
            WriteLine(result[0]);

            WriteLine("Part 2 - Program that evals to 19690720");

            IEnumerable<(int noun, int verb)> inputs()
            {
                foreach (var noun in Range(0, 99))
                    foreach (var verb in Range(0, 99))
                        yield return (noun, verb);
            }

            int? evaluate(int noun, int verb)            
            {
                copy = program.ToArray();
                (copy[1], copy[2]) = (noun, verb);

                try { result = Day2Computer.Evaluate(copy); }
                catch (InvalidOperationException) { }

                if (result[0] == 19690720)
                    return noun * 100 + verb;
                else
                    return null;
            }

            var code = inputs().First(pair => evaluate(pair.noun, pair.verb) != null);
            WriteLine(code);
        }

        static void Day3()
        {
            WriteLine("--- Day 3: Crossed Wires ---");
            var lines = "Day3/input.txt".ReadLines().ToArray();

            WriteLine("Part 1 - Closest cross");
            var points = lines.Select(line => line.AsVectors().AsSetOfPoints()).ToArray();
            var (points_a, points_b) = (points[0], points[1]);
            var closest = points_a.Intersect(points_b).Select(point => point.ManhattenDistance()).OrderBy(i => i).First();
            WriteLine(closest);

            WriteLine("Part 2 - Least delay");
            var steps = lines.Select(line => line.AsVectors().AsMapOfMinimumSteps()).ToArray();
            var (steps_a, steps_b) = (steps[0], steps[1]);
            closest = steps_a.Keys
                .Intersect(steps_b.Keys)
                .Select(point => steps_a[point] + steps_b[point])
                .OrderBy(steps => steps)
                .First();
            WriteLine(closest);
        }

        static void Day4()
        {
            WriteLine("--- Day 4: Secure Container ---");
            WriteLine("Part 1 - increasing with adjacent");
            var passwords =
                Range(387638, 531485)
                    .Select(i => i.ToString())
                    .Where(RangeFilters.NoDigitDecreases)
                    .Where(RangeFilters.HasAdjacentDuplicate);                    
            WriteLine(passwords.Count());

            WriteLine("Part 2 - only 2 adjacent");
            passwords =
                Range(387638, 531485)
                    .Select(i => i.ToString())
                    .Where(RangeFilters.NoDigitDecreases)
                    .Where(RangeFilters.HasGroupOf2DigitsButNotMore);
            WriteLine(passwords.Count());            
        }

        static void Day5()
        {
            WriteLine("--- Day 5: Sunny with a Chance of Asteroids ---");
            var program = "Day5/input.txt".ReadLines().Single().Split(",").Select(int.Parse).ToArray();

            WriteLine("Part 1 - AC Diagnostic");
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(1);
            var copy = program.ToArray();            
            Day5Computer.EvaluateProgram(copy, system);
            WriteLine(string.Join(",", device.Buffer));

            WriteLine("Part 2 - Extend thermal radiators");
            (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(5);
            copy = program.ToArray();
            Day5Computer.EvaluateProgram(copy, system);
            WriteLine(string.Join(",", device.Buffer));
        }

        static void Day6()
        {
            WriteLine("--- Day 6: Universal Orbit Map ---");
            var map = "Day6/input.txt".ReadLines();
            var orbit = Orbit.BuildOrbitMap(map);
            var result = orbit.TotalOrbits();            
            WriteLine(result);

            var distance = orbit.TransfersBetween("YOU", "SAN");
            WriteLine(distance);
        }
    }    
}

