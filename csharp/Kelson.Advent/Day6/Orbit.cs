using System.Collections.Generic;
using System.Linq;

namespace Kelson.Advent.Day6
{
    public class Orbit
    {        
        public Orbit Parent;
        public readonly string Key;

        private Dictionary<string, Orbit>? _descendants = null;
        public Dictionary<string, Orbit> Descendants => _descendants ?? (_descendants = EnumerateDescendants().ToDictionary(o => o.Key));

        public IEnumerable<Orbit> EnumerateDescendants()
        {            
            foreach (var child in Children)
            {
                yield return child;
                foreach (var desc in child.EnumerateDescendants())
                    yield return desc;
            }
        }

        public IEnumerable<Orbit> Parents()
        {
            var parent = this.Parent;
            while (parent != parent.Parent)
            {
                yield return parent;
                parent = parent.Parent;
            }
            yield return parent;
        }

        public readonly List<Orbit> Children = new List<Orbit>();

        private int? _depth = null;
        public int Depth => _depth ?? (int)(_depth = Parent == this ? 0 : 1 + Parent.Depth);

        public Orbit this[string key]
        {
            get => Descendants[key];            
        }

        public Orbit(string key, Orbit? parent = default) => (Key, Parent) = (key, parent ?? this);

        public static Orbit BuildOrbitMap(IEnumerable<string> lines)
        {
            var all_orbits = new Dictionary<string, Orbit>();

            Orbit getOrbit(string key, Orbit? parent = null)
            {
                if (all_orbits.TryGetValue(key, out Orbit value))
                {
                    value.Parent = parent ?? value.Parent;
                    return value;
                }
                else
                {
                    var orbit = new Orbit(key, parent);
                    all_orbits.Add(key, orbit);
                    return orbit;
                }
            }

            foreach (var relation in lines)
            {
                var elements = relation.Split(")");
                var parent = getOrbit(elements[0]);
                var child = getOrbit(elements[1], parent);
                parent.Children.Add(child);
            }

            return all_orbits["COM"];
        }

        public int TransfersBetween(string a, string b)
        {
            var parent_a = Descendants[a].Parent;
            var parent_b = Descendants[b].Parent;
            if (parent_a == parent_b)
                return 0;

            var set_a = parent_a.Parents().ToDictionary(o => o.Key);
            var set_b = parent_b.Parents().ToDictionary(o => o.Key);

            var common_ancestors = set_a.Keys.Intersect(set_b.Keys).Select(key => set_a[key]);
            var first_common_ancestor = common_ancestors.OrderByDescending(o => o.Depth).First();

            return (parent_a.Depth - first_common_ancestor.Depth) + (parent_b.Depth - first_common_ancestor.Depth);
        }

        //private int SumUpTo(int n) => n * (n + 1) / 2;
        //public int TotalOrbits() => Descendants.Values.Where(o => o.Children.Count == 0).Select(o => SumUpTo(o.Depth - 1)).Sum();

        public int TotalOrbits() => Descendants.Values.Select(o => o.Depth).Sum();

        public override string ToString() => $"({Key}:{Depth}->({string.Join(',', Children.Select(o => o.Key))})";
    }
}
