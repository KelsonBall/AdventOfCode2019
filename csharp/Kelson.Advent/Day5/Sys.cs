namespace Kelson.Advent.Day5
{
    public interface Sys
    {
        bool CanRead();
        void Write(int value);
        int Read();
    }
}
