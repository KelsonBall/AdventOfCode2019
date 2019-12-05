using FluentAssertions;
using Kelson.Advent.Day2;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day2Tests
    {
        static readonly (int[] input, int[] output)[] TEST_CASES = 
        {
            ( new int[] { 1,0,0,0,99 }, new int[] { 2,0,0,0,99 } ),
            ( new int[] { 2,3,0,3,99 }, new int[] { 2,3,0,6,99 } ),
            ( new int[] { 2,4,4,5,99,0 }, new int[] { 2,4,4,5,99,9801 } ),
            ( new int[] { 1,1,1,4,99,5,6,0,99 }, new int[] { 30,1,1,4,2,5,6,0,99 } ),
            ( new int[] { 1,9,10,3,2,3,11,0,99,30,40,50 }, new int[] { 3500,9,10,70,2,3,11,0,99,30,40,50 } ),
        };

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Part1(int @case)
        {
            var (input, output) = TEST_CASES[@case - 1];
            IntcodeComputer.Evaluate(input).Should().BeEquivalentTo(output);
        }
    }
}
