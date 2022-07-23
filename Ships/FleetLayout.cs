using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SeaFight.Ships
{
    public class FleetLayout: IEnumerable<int>
    {
        public static readonly FleetLayout Classic = new FleetLayout(4, 3, 3, 2, 2, 2, 1, 1, 1, 1);
        private readonly int[] m_sizes;


        public FleetLayout(params int[] sizes) { m_sizes = sizes; }


        public FleetLayout(IEnumerable<int> sizes) { m_sizes = sizes.ToArray(); }


        public IEnumerator<int> GetEnumerator() { return (IEnumerator<int>)m_sizes.GetEnumerator(); }


        IEnumerator IEnumerable.GetEnumerator() { return m_sizes.GetEnumerator(); }
    }
}