using FluentAssertions;
using Kelson.Advent.Day5;
using Xunit;

namespace Kelson.Advent.Tests
{
    public class Day5Tests
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
        public void Day2ProgramsStillValid(int @case)
        {
            var (input, output) = TEST_CASES[@case - 1];
            IntcodeComputer.EvaluateProgram(input);
            input.Should().BeEquivalentTo(output);
        }

        [Fact]
        public void ReadOperationSetsValue()
        {
            var input = new int[] { (int)Op.Read, 3, 99, 0 };
            var output = new int[] { (int)Op.Read, 3, 99, 12 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(12);
            IntcodeComputer.EvaluateProgram(input, system);
            input.Should().BeEquivalentTo(output);
        }

        [Fact]
        public void WriteOperationOutputsValue()
        {
            var input = new int[] { (int)Op.Write, 2, 99 };
            var output = new int[] { (int)Op.Write, 2, 99 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            IntcodeComputer.EvaluateProgram(input, system);
            input.Should().BeEquivalentTo(output);
            device.Read().Should().Be(99);
        }

        [Fact]       
        public void WriteOperationInImmediateModeOutputsValue()
        {
            var input = new int[] { 100 + (int)Op.Write, 5, 99 };
            var output = new int[] { 100 + (int)Op.Write, 5, 99 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            IntcodeComputer.EvaluateProgram(input, system);
            input.Should().BeEquivalentTo(output);
            device.Read().Should().Be(5);
        }

        [Fact]
        public void AddOperationInImmediateModeCorrectlySums()
        {
            var input = new int[] { 1100 + (int)Op.Add, 5, -2, 5, 99, 0 };
            var output = new int[] { 1100 + (int)Op.Add, 5, -2, 5, 99, 3 };            
            IntcodeComputer.EvaluateProgram(input);
            input.Should().BeEquivalentTo(output);            
        }

        [Fact]
        public void AddOperationInPartialImmediateModeCorrectlySums()
        {
            var input = new int[] { 1000 + (int)Op.Add, 4, -2, 5, 99, 0 };
            var output = new int[] { 1000 + (int)Op.Add, 4, -2, 5, 99, 97 };
            IntcodeComputer.EvaluateProgram(input);
            input.Should().BeEquivalentTo(output);
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(8, 1)]
        [InlineData(9, 0)]
        public void CheckEqualTo8InPositionMode(int input, int output)
        {
            int[] program = new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(4, 1)]
        [InlineData(8, 0)]
        [InlineData(9, 0)]
        public void CheckLessThanTo8InPositionMode(int input, int output)
        {
            int[] program = new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(8, 1)]
        [InlineData(9, 0)]
        public void CheckEqualTo8InImmediateMode(int input, int output)
        {
            int[] program = new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(4, 1)]
        [InlineData(8, 0)]
        [InlineData(9, 0)]
        public void CheckLessThanTo8InImmediateMode(int input, int output)
        {
            int[] program = new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(9, 1)]
        public void JumpTestInPositionMode(int input, int output)
        {
            int[] program = new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(9, 1)]
        public void JumpTestInImmediateMode(int input, int output)
        {
            int[] program = new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 };
            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }

        [Theory]
        [InlineData(4, 999)]
        [InlineData(8, 1000)]
        [InlineData(9, 1001)]
        public void CheckLtEqGt(int input, int output)
        {
            int[] program = new int[] 
            { 
                3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 
            };

            var (system, device) = QueueStoreSystem.CreateDuplux();
            device.Write(input);
            IntcodeComputer.EvaluateProgram(program, system);
            device.Read().Should().Be(output);
        }
    }
}
