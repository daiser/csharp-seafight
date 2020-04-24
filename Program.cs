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
            //    new Pos(0,0 ),new Pos(0,1), new Pos(0,2)
            //});
            //Console.WriteLine(ship.TakeShot(new Pos(0, 0)));
            //Console.WriteLine(ship.TakeShot(new Pos(1, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 3)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 1)));
            //return;

            Console.SetWindowSize(120, 20);
            Console.SetBufferSize(120, 20);

            Game game = new Game(10, FleetLayout.classic);

            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            game.RegisterPlayer(new Players.Monkey(generator));
            //game.RegisterPlayer(new Players.Noob(generator));
            //game.RegisterPlayer(new Players.Beginner(generator));
            //game.RegisterPlayer(new Players.Amateur(generator));
            game.RegisterPlayer(new Players.Amateur(generator));

            try
            {
                game.Play();
            } catch(GameOverException gox)
            {
                Console.SetCursorPosition(0, 12);
                Console.WriteLine(string.Format("GAME OVER! Winner: {0}", gox.Winner));
                Console.WriteLine("Total shots: {0:d}", gox.TotalShots);
            }

            Console.ReadLine();
        }
    }
}
