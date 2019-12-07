namespace Kelson.Advent.Day5
{
    public class NoOpSystem : Sys
    {
        public bool CanRead() => false;

        public void Input(int value) { }

        public int Read() => default;

        public void Write(int value) { }
    }
}
