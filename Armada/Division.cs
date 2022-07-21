using System;

namespace SeaFight.Armada
{
    class Division
    {
        public int ShipSize { get; }

        public int NumberOfShips { get; }


        public Division(in int shipSize, in int numberOfShips) {
            if (numberOfShips < 1) throw new ArgumentException("invalid number of ships", nameof(numberOfShips));
            ShipSize = shipSize;
            NumberOfShips = numberOfShips;
        }
    }
}