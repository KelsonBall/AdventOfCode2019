using FluentAssertions;
using Kelson.Advent.Day1;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day1Tests
    {
        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void Part1(int mass, int fuelRequired) => RocketEquation.FuelRequired(mass).Should().Be(fuelRequired);


        [Theory]
        [InlineData(14, 2)]
        [InlineData(1969, 966)]
        [InlineData(100756, 50346)]
        public void Part2(int mass, int totalRequired) => mass.TotalFuelRequired().Should().Be(totalRequired);
    }
}
