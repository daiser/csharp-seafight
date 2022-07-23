using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SeaFight
{
    public static class Ext
    {
        public static Shape DetectShape(this IEnumerable<(int col, int row)> shape) {
            var s = shape as (int col, int row)[] ?? shape.ToArray();

            var cols = s.Select(c => c.col).Distinct().OrderBy(v => v).ToArray();
            var rows = s.Select(c => c.row).Distinct().OrderBy(v => v).ToArray();

            if (cols.Length == 1 && rows.Length == 1) return Shape.Point;
            if (cols.Length > 1 && rows.Length > 1) return Shape.None;

            if (cols.Length == 1) return Shape.Vertical;
            return Shape.Horizontal;
        }


        public static T PickRandom<T>(this IEnumerable<T> values, Random generator = null) {
            var v = values as T[] ?? values.ToArray();
            Debug.WriteLine("{0}, cnt={1:d}", values, v.Length);

            if (v.Length == 0) throw new InvalidOperationException("Can't pick random value");

            var rnd = generator ?? new Random();
            return v[rnd.Next(0, v.Length)];
        }


        public static (int col, int row) Shift(this (int col, int row) point, (int col, int row) vector) {
            return (point.col + vector.col, point.row + vector.row);
        }
    }
}