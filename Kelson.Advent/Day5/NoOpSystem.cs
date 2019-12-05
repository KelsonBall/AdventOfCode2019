namespace Kelson.Advent.Day5
{
    public class NoOpSystem : Sys
    {
        public void Input(int value) { }

        public int Read() => default;

        public void Write(int value) { }
    }
}
