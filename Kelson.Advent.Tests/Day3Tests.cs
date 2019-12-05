using FluentAssertions;
using Kelson.Advent.Day3;
using System.Linq;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day3Tests
    {
        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        public void Part1Test(string pathA, string pathB, int expectedDistance)
        {            
            var points_a = pathA.AsVectors().AsSetOfPoints();
            var points_b = pathB.AsVectors().AsSetOfPoints();

            var intersections = points_a.Intersect(points_b);

            var closest = intersections.Select(point => point.ManhattenDistance()).OrderBy(i => i).First();

            closest.Should().Be(expectedDistance);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 30)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        public void Part2Test(string pathA, string pathB, int expectedSteps)
        {
            var steps_a = pathA.AsVectors().AsMapOfMinimumSteps();
            var steps_b = pathB.AsVectors().AsMapOfMinimumSteps();

            var closest = steps_a.Keys
                .Intersect(steps_b.Keys)
                .Select(point => steps_a[point] + steps_b[point])
                .OrderBy(steps => steps)
                .First();

            closest.Should().Be(expectedSteps);
        }
    }
}
