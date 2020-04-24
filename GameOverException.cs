using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    class GameOverException : Exception
    {
        public ICompetitor Winner { get; private set; }
        public int TotalShots { get; private set; }
        public GameOverException(ICompetitor winner, int totalShots = 0) : base("GAME OVER!")
        {
            Winner = winner;
            TotalShots = totalShots;
        }
    }
}
