using System.Collections.Generic;
using System.Linq;

namespace Kelson.Advent.Day4
{
    public static class RangeFilters
    {

        public static bool NoDigitDecreases(string number)
        {
            var previous = number[0];
            foreach (var c in number.Skip(1))
            {
                if (c < previous)
                    return false;
                previous = c;
            }
            return true;
        }

        public static bool HasAdjacentDuplicate(string number)
        {
            var previous = number[0];
            foreach (var c in number.Skip(1))
            {
                if (c == previous)
                    return true;
                previous = c;
            }
            return false;
        }

        public static IEnumerable<List<char>> GroupsOfDigits(string number)
        {
            IEnumerable<char> digitsFromIndex(int index)
            {
                var c = number[index];
                for (int i = index; i < number.Length; i++)
                    if (number[i] == c)
                        yield return c;
            }

            for (int j = 0; j < number.Length;)
            {
                var group = digitsFromIndex(j).ToList();
                j += group.Count;
                yield return group;
            }
        }

        public static bool HasGroupOf2DigitsButNotMore(string number) => GroupsOfDigits(number).Where(group => group.Count > 1).Any(group => group.Count == 2);
    }
}
