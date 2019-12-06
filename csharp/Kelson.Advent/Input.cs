using System.Collections.Generic;
using System.IO;

namespace Kelson.Advent
{
    public static class Input
    {
        public static IEnumerable<string> ReadLines(this string filename)
        {
            using var stream = File.OpenRead(filename);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line != null)
                    yield return line;
                else
                    yield break;
            }
        }

        public static async IAsyncEnumerable<string> ReadLinesAsync(this string filename)
        {
            using var stream = File.OpenRead(filename);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (line != null)
                    yield return line;
                else
                    yield break;
            }
        }
    }
}
