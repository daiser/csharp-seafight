using System;
using SeaFight.Ai;

namespace SeaFight
{
    class GameOverException: Exception
    {
        public Player Winner { get; }

        public int TotalShots { get; }


        public GameOverException(Player winner, int totalShots = 0): base("GAME OVER!") {
            Winner = winner;
            TotalShots = totalShots;
        }
    }
}