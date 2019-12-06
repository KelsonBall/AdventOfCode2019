using System.Collections.Generic;
using System.IO;

namespace Kelson.Advent
{
    public static class Input
    {
        public static IEnumerable<string> ReadLines(this string filename) => File.ReadAllLines(filename);
    }
}
