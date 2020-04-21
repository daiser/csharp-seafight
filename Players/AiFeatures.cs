using System;

namespace SeaFight.Players
{
    [Flags]
    enum AiFeatures
    {
        None = 0,
        RememberOwnShots = 0x01,
        RememberRivalShots = 0x02,
        DontShootYourself = 0x04,
    }
}
