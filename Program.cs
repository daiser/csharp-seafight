using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    class Program
    {
        static void Main(string[] args)
        {
            Random generator = new Random();

            //Ship ship = new Ship(new[]{
            //    new Cell(0,0 ),new Cell(0,1), new Cell(0,2)
            //});
            //Console.WriteLine(ship.TakeShot(new Cell(0, 0)));
            //Console.WriteLine(ship.TakeShot(new Cell(1, 2)));
            //Console.WriteLine(ship.TakeShot(new Cell(0, 2)));
            //Console.WriteLine(ship.TakeShot(new Cell(0, 3)));
            //Console.WriteLine(ship.TakeShot(new Cell(0, 1)));
            //return;

            Game game = new Game(10, FleetLayout.classic);

            //game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Baby(generator));
            game.RegisterPlayer(new Players.Kid(generator));

            var stats = game.Play();
            Console.WriteLine(string.Format("GAME OVER! Winner: {0}", stats.winner));
            Console.WriteLine("Total shots: {0:d}", stats.totalShots);
        }
    }
}
