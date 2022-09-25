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

            var colsCount = s.Select(c => c.col).Distinct().Count();
            var rowsCount = s.Select(c => c.row).Distinct().Count();

            if (colsCount != 1 && rowsCount != 1) return Shape.None;
            switch (colsCount) {
                case 1 when rowsCount == 1:
                    return Shape.Point;
                case 1:
                    return Shape.Vertical;
                default:
                    return Shape.Horizontal;
            }
        }


        public static T PickRandom<T>(this IEnumerable<T> values, Random generator = null) {
            var v = values as T[] ?? values.ToArray();
            Debug.WriteLine("{0}, cnt={1:d}", values, v.Length);

            if (v.Length == 0) throw new InvalidOperationException("Can't pick random value");

            var rng = generator ?? new Random();
            return v[rng.Next(0, v.Length)];
        }


        public static (int col, int row) Shift(this (int col, int row) point, (int col, int row) vector) {
            return (point.col + vector.col, point.row + vector.row);
        }
    }
}