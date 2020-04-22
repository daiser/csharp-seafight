using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    public static class Ext
    {
        public static T PickRandom<T>(this IEnumerable<T> values, Random generator = null)
        {
            var rnd = generator ?? new Random();
            var count = rnd.Next(0, values.Count());
            foreach (T value in values)
            {
                count--;
                if (count < 0) return value;
            }
            throw new InvalidOperationException("Can't pick random value");
        }

        public static void AddIf<T>(this List<T> list, T value, bool condition)
        {
            if (condition)
            {
                list.Add(value);
            }
        }
    }
}
