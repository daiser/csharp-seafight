using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SeaFight
{
    class Ship: IReadOnlyList<Target>, IBlueprint, ITarget
    {
        private readonly List<Target> cells;

        public Pos Location { get; private set; }

        public int Size => cells.Count;

        public int Count => cells.Count;

        public Target this[int index]
        {
            get => cells[index];
            private set => cells[index] = value;
        }


        public Ship(IEnumerable<Pos> placement) {
            cells = placement.Select(e => new Target(e.Col, e.Row)).ToList();
            if (Count < 1) throw new ArgumentException("Empty ship", nameof(placement));
            Location = placement.First();
        }


        private int IndexOf(Pos cell) {
            for (int i = 0; i < Count; i++) {
                if (cells[i].Equals(cell)) return i;
            }
            return -1;
        }


        public ShotEffect TakeShot(Pos at) {
            var idx = IndexOf(at);
            if (idx == -1) return ShotEffect.Miss;
            this[idx] = new Target(this[idx].Col, this[idx].Row);
            return this.All(e => !e.Value) ? ShotEffect.Kill : ShotEffect.Hit;
        }


        public IEnumerator<Target> GetEnumerator() { return cells.GetEnumerator(); }


        IEnumerator IEnumerable.GetEnumerator() { return cells.GetEnumerator(); }
    }
}