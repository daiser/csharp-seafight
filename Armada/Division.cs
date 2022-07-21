using System;

namespace SeaFight.Armada
{
    class Division
    {
        public ShipBlueprint Proto { get; }

        public int NumberOfShips { get; }


        public Division(in ShipBlueprint shipBlueprint, in int numberOfShips) {
            if (numberOfShips < 1) throw new ArgumentException("invalid number of ships", nameof(numberOfShips));
            Proto = shipBlueprint ?? throw new ArgumentNullException(nameof(shipBlueprint));
            NumberOfShips = numberOfShips;
        }
    }
}