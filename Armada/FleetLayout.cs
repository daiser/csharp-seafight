using System.Collections.Generic;
using System.Linq;

namespace SeaFight.Armada
{
    class FleetLayout
    {
        public static readonly FleetLayout Classic = MakeClassic(4);


        public static FleetLayout MakeClassic(in int maxSize) {
            var groups = new List<Division>();

            for (var size = maxSize; size > 0; size--) {
                groups.Add(new Division(new ShipBlueprint(size), maxSize - size + 1));
            }

            return new FleetLayout(groups);
        }


        public Division[] Content { get; private set; }


        public FleetLayout(params Division[] protos) { Content = protos.ToArray(); }


        public FleetLayout(IEnumerable<Division> protos) { Content = protos.ToArray(); }
    }
}