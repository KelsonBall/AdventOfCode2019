using FluentAssertions;
using Kelson.Advent.Day6;
using System.Linq;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day6Tests    
    {
        static readonly string[] MAP_A = new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", };
        static readonly string[] MAP_B = new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN", };
        static readonly string[] MAP_C = new string[] { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "H)SAN", };

        [Fact]
        public void BuildPart1ExampleMap()
        {            

            var orbit = Orbit.BuildOrbitMap(MAP_A);

            orbit.Key.Should().Be("COM");
            orbit.Parent.Should().Be(orbit);
            orbit["B"].Children.Select(o => o.Key).Should().ContainInOrder("C", "G");
            orbit["L"].Parent.Should().Be(orbit["K"]);            
            orbit["L"].Depth.Should().Be(7);
            orbit["D"].Depth.Should().Be(3);            
            orbit.TotalOrbits().Should().Be(42);
        }

        [Fact]
        public void EnumerateParents()
        {
            var map_a = Orbit.BuildOrbitMap(MAP_A);
            map_a["L"].Parents().Select(o => o.Key).Should().ContainInOrder("K", "J", "E", "D", "C", "B", "COM");
            map_a["I"].Parents().Select(o => o.Key).Should().ContainInOrder("D", "C", "B", "COM");
        }

        [Fact]
        public void TransferCountShouldBeCorrect()
        {                        
            var orbit = Orbit.BuildOrbitMap(MAP_B);
            var distance = orbit.TransfersBetween("YOU", "SAN");
            distance.Should().Be(4);

            orbit = Orbit.BuildOrbitMap(MAP_C);
            distance = orbit.TransfersBetween("YOU", "SAN");
            distance.Should().Be(7);
        }
    }
}
