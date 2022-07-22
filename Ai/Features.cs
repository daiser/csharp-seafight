using System;

namespace SeaFight.Ai
{
    [Flags]
    enum Features
    {
        None = 0,
        RememberOwnShots = 0x01,
        RememberRivalShots = 0x02,
        DontShootYourself = 0x04,
    }
}