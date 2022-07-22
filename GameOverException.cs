using System;
using SeaFight.Ai;

namespace SeaFight
{
    class GameOverException: Exception
    {
        public ICompetitor Winner { get; }

        public int TotalShots { get; }


        public GameOverException(ICompetitor winner, int totalShots = 0): base("GAME OVER!") {
            Winner = winner;
            TotalShots = totalShots;
        }
    }
}