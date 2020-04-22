using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    class Figure
    {
        public Pos[] Blocks { get; private set; }
        public int Area { get { return Blocks.Length; } }
        public Figure(IEnumerable<Pos> blocks)
        {
            if (blocks == null) throw new ArgumentNullException("blocks");
            this.Blocks = blocks.ToArray();
        }

        public Pos[] Neighbours()
        {
            List<Pos> neighbours = new List<Pos>();
            foreach (var block in Blocks)
            {
                var blockNeighbours = block.Neighbours();
                foreach (var neighbour in blockNeighbours)
                {
                    if (Blocks.Contains(neighbour)) continue;
                    neighbours.Add(neighbour);
                }
            }

            return neighbours.ToArray();
        }
        public bool Contains(Pos pos)
        {
            return Blocks.Contains(pos);
        }
        public bool IsColumnOriented()
        {
            if (Blocks.Length == 0) return false;
            var firstBlock = Blocks[0];
            return Blocks.All(b => b.Col == firstBlock.Col);
        }
        public bool IsRowOriented()
        {
            if (Blocks.Length == 0) return false;
            var firstBlock = Blocks[0];
            return Blocks.All(b => b.Row == firstBlock.Row);
        }

        public int Left()
        {
            return Blocks.Select(b => b.Col).Min();
        }
        public int Right()
        {
            return Blocks.Select(b => b.Col).Max();
        }
        public int Top()
        {
            return Blocks.Select(b => b.Row).Min();
        }
        public int Bottom()
        {
            return Blocks.Select(b => b.Row).Max();
        }
    }
}
