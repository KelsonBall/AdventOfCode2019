using System.Collections.Generic;
using System.Linq;

namespace Kelson.Advent.Day1
{
    public static class RocketEquation
    {
        public static int FuelRequired(int mass) => mass / 3 - 2;

        public static int ModuleFuelRequirement(this IEnumerable<int> moduleMasses) => moduleMasses.Select(FuelRequired).Sum();

        public static int TotalFuelRequired(this int mass)
        {
            int fuel_required = FuelRequired(mass);
            if (fuel_required <= 0)
                return 0;
            else
                return fuel_required + TotalFuelRequired(fuel_required);
        }

        public static int TotalModuleFuelRequirement(this IEnumerable<int> moduleMasses) => moduleMasses.Select(TotalFuelRequired).Sum();
    }
}
