using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using Set = System.Collections.Generic.HashSet<(int x, int y)>;
using StepMap = System.Collections.Generic.Dictionary<(int x, int y), int>;

namespace Kelson.Advent.Day3
{   
    public static class WireGrid
    {
        /// <summary>
        /// Manhatten ("Taxi cab") distance: sum of component magnitudes
        /// </summary>
        public static int ManhattenDistance(this (int x, int y) vector) => Abs(vector.x) + Abs(vector.y);

        /// <summary>
        /// Normalize a basis vector.
        /// </summary>        
        public static (int x, int y) Normalize(this (int x, int y) vector) => (vector.x.CompareTo(0), vector.y.CompareTo(0));        

        /// <summary>
        /// Map a direction code as a direction vector
        /// </summary>        
        public static (int x, int y) ParseDirection(this string code) => code[0] switch
        {
            'R' => (int.Parse(code.Substring(1)), 0),
            'L' => (-int.Parse(code.Substring(1)), 0),
            'U' => (0, int.Parse(code.Substring(1))),
            'D' => (0, -int.Parse(code.Substring(1))),
            _ => throw new InvalidOperationException()
        };

        /// <summary>
        /// Enumerate a path string as direction vectors
        /// </summary>        
        public static IEnumerable<(int x, int y)> AsVectors(this string line) => line.Split(",").Select(ParseDirection);        

        public static IEnumerable<(int x, int y)> AsPoints(this IEnumerable<(int x, int y)> directions)
        {
            (int x, int y) = (0, 0);
            foreach (var vector in directions)
            {
                (int dx, int dy) = vector.Normalize();
                for (int count = 0; count < vector.ManhattenDistance(); count++)
                {
                    (x, y) = (x + dx, y + dy);
                    yield return (x, y);                    
                }
            }
        }

        /// <summary>
        /// Follow a path and populate a set with each of the traversed points
        /// </summary>
        public static Set AsSetOfPoints(this IEnumerable<(int x, int y)> directions) => 
            directions
                .AsPoints()
                .ToHashSet();

        public static StepMap AsMapOfMinimumSteps(this IEnumerable<(int x, int y)> directions) =>
            directions
                .AsPoints()
                .SelectWithIndex((point, index) => (point, index + 1))
                .GroupBy(ps => ps.point)
                .ToDictionary(
                    pg => pg.Key,
                    pg => pg.Min(pi => pi.Item2));
            

        public static IEnumerable<V> SelectWithIndex<U, V>(this IEnumerable<U> source, Func<U, int, V> map)
        {
            int index = 0;
            foreach (var item in source)
                yield return map(item, index++);
        }
    }
}
