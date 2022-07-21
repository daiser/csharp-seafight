using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaFight.Armada;

namespace SeaFight
{
    class Program
    {
        private const int GAMES_TO_PLAY = 1000;

        static void Main(string[] args)
        {
            Random generator = new Random();

            //Ship ship = new Ship(new[]{
            //    new Pos(0,0 ),new Pos(0,1), new Pos(0,2)
            //});
            //Console.WriteLine(ship.TakeShot(new Pos(0, 0)));
            //Console.WriteLine(ship.TakeShot(new Pos(1, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 3)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 1)));
            //return;

            List<Player> players = new List<Player> { new Players.Amateur(generator) };
            double winRate = 1.0;

            while (winRate > 0.5)
            {
                players.Add(new Players.Beginner(generator));
                int amateurWins = 0;
                for (int gameNo = 0; gameNo < GAMES_TO_PLAY; gameNo++)
                {
                    Game game = new Game(10, FleetLayout.Classic);
                    foreach (var player in players)
                    {
                        game.RegisterPlayer(player);
                    }

                    try
                    {
                        game.Play(false);
                    }
                    catch (GameOverException gox)
                    {
                        if (gox.Winner.Id == 1) amateurWins++;
                    }
                }
                winRate = (double)amateurWins / GAMES_TO_PLAY;

                int monkeysQty = players.Where(p => p is Players.Beginner).Count();
                Console.WriteLine("{0:d} monkeys: {1:F2}%", monkeysQty, winRate * 100);
            }


            Console.ReadLine();
        }
    }
}
