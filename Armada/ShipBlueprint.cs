using System;

namespace SeaFight.Armada
{
    [Obsolete("Use int size")]
    class ShipBlueprint: IBlueprint
    {
        public int Size { get; }


        public ShipBlueprint(in int size) { Size = size; }
    }
}