using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Board;

namespace SeaFight
{
    class Figure
    {
        public Pos[] Blocks { get; }

        public int Area => Blocks.Length;


        public Figure(IEnumerable<Pos> blocks) {
            if (blocks == null) throw new ArgumentNullException("blocks");
            this.Blocks = blocks.ToArray();
        }


        public Pos[] Neighbours() {
            return (from block in Blocks from neighbour in block.Neighbours() where !Blocks.Contains(neighbour) select neighbour).ToArray();
        }


        public bool Contains(Pos pos) { return Blocks.Contains(pos); }


        public bool IsColumnOriented() {
            if (Blocks.Length == 0) return false;
            var firstBlock = Blocks[0];
            return Blocks.All(b => b.Col == firstBlock.Col);
        }


        public bool IsRowOriented() {
            if (Blocks.Length == 0) return false;
            var firstBlock = Blocks[0];
            return Blocks.All(b => b.Row == firstBlock.Row);
        }


        public int Left() { return Blocks.Select(b => b.Col).Min(); }


        public int Right() { return Blocks.Select(b => b.Col).Max(); }


        public int Top() { return Blocks.Select(b => b.Row).Min(); }


        public int Bottom() { return Blocks.Select(b => b.Row).Max(); }
    }
}