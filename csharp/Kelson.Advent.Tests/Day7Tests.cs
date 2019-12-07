using FluentAssertions;
using Kelson.Advent.Day7;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day7Tests
    {
        [Theory]
        [InlineData(new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 43210)]
        [InlineData(new int[] { 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0 }, 54321)]
        public void EvaluateCorrectMaxSetting(int[] program, int max)
        {
            var solver = new AmplifierSolver(program);
            solver.FindMaxOutput().Should().Be(max);
        }

        [Theory]
        [InlineData(new int[] { 9, 8, 7, 6, 5 }, new int[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 }, 139629729)]
        [InlineData(new int[] { 9, 7, 8, 5, 6 }, new int[] { 3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10 }, 18216)]        
        public void EvaluateCorrectMaxPhase2Setting(int[] combo, int[] program, int result)
        {
            var solver = new AmplifierSolver(program);

            solver.TryArangementWithFeedback(0, combo[0], combo[1], combo[2], combo[3], combo[4]).Should().Be(result);
        }
    }
}