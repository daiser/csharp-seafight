using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Board;

namespace SeaFight.Armada
{
    class Ship: IReadOnlyList<Target>, IBlueprint, ITarget
    {
        private readonly List<Target> m_cells;

        public Point Location { get; }

        public int Size => m_cells.Count;

        public int Count => m_cells.Count;

        public Target this[int index]
        {
            get => m_cells[index];
            private set => m_cells[index] = value;
        }


        public Ship(IEnumerable<Point> placement) {
            m_cells = placement.Select(e => new Target(e.Col, e.Row)).ToList();
            if (Count < 1) throw new ArgumentException("Empty ship", nameof(placement));
            Location = m_cells[0];
        }


        private int IndexOf(Point cell) {
            for (var i = 0; i < Count; i++) {
                if (m_cells[i].Equals(cell)) return i;
            }
            return -1;
        }


        public ShotEffect TakeShot(Point at) {
            var idx = IndexOf(at);
            if (idx == -1) return ShotEffect.Miss;
            this[idx] = new Target(this[idx].Col, this[idx].Row);
            return this.All(e => !e.Value) ? ShotEffect.Kill : ShotEffect.Hit;
        }


        public IEnumerator<Target> GetEnumerator() { return m_cells.GetEnumerator(); }


        IEnumerator IEnumerable.GetEnumerator() { return m_cells.GetEnumerator(); }
    }
}