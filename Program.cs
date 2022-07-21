﻿using System;
using System.Collections.Generic;
using System.Linq;
using SeaFight.Armada;
using SeaFight.Players;

namespace SeaFight
{
    class Program
    {
        private const int GAMES_TO_PLAY = 1000;


        static void Main(string[] args) {
            var generator = new Random();

            //Ship ship = new Ship(new[]{
            //    new Pos(0,0 ),new Pos(0,1), new Pos(0,2)
            //});
            //Console.WriteLine(ship.TakeShot(new Pos(0, 0)));
            //Console.WriteLine(ship.TakeShot(new Pos(1, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 2)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 3)));
            //Console.WriteLine(ship.TakeShot(new Pos(0, 1)));
            //return;

            var players = new List<Player> { new Players.Amateur(generator) };
            var winRate = 1.0;

            while (winRate > 0.5) {
                players.Add(new Players.Beginner(generator));
                var amateurWins = 0;
                for (var gameNo = 0; gameNo < GAMES_TO_PLAY; gameNo++) {
                    var game = new Game(10, FleetLayout.Classic);
                    foreach (var player in players) {
                        game.RegisterPlayer(player);
                    }

                    try {
                        game.Play(false);
                    }
                    catch (GameOverException gox) {
                        if (gox.Winner.Id == 1) amateurWins++;
                    }
                }
                winRate = (double)amateurWins / GAMES_TO_PLAY;

                var monkeysQty = players.Count(p => p is Beginner);
                Console.WriteLine("{0:d} monkeys: {1:F2}%", monkeysQty, winRate * 100);
            }


            Console.ReadLine();
        }
    }
}