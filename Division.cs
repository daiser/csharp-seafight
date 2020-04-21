using System;

namespace SeaFight
{
    class Division
    {
        public ShipBlueprint Proto { get; private set; }
        public int Number { get; private set; }

        public Division(in ShipBlueprint proto, in int number)
        {
            if (number < 1) throw new ArgumentException("invalid number of ships", "number");
            Proto = proto ?? throw new ArgumentNullException("proto");
            Number = number;
        }
    }
}
