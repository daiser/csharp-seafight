using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    class Game
    {
        public int Dim { get; private set; }
        public FleetLayout Armada { get; private set; }
        private readonly List<ICompetitor> players = new List<ICompetitor>();
        private readonly List<HitBoard> hitBoards = new List<HitBoard>();
        private readonly Dictionary<int, Fleet> fleets = new Dictionary<int, Fleet>();
        public Game(int dim, FleetLayout fleetLayout)
        {
            Dim = dim;
            Armada = fleetLayout;
        }

        public void RegisterPlayer(ICompetitor player)
        {
            players.Add(player);
        }

        public void Play(bool visialize = true)
        {
            if (players.Count < 2) throw new InvalidOperationException("Not enough players");

            Console.CursorVisible = false;
            fleets.Clear();
            hitBoards.Clear();

            foreach (var player in players)
            {
                Board board = new Board(Dim);
                var fleet = player.PlaceFleet(Armada, board);
                if (fleet == null) throw new InvalidOperationException(string.Format("Player #{0:d} failed to place fleet", player.Id));
                fleets.Add(player.Id, fleet);

                //fleets.Add(player.Id, Fleet.fixed_0);

                foreach (var rival in players)
                {
                    hitBoards.Add(new HitBoard(player, rival, Dim));
                }
            }

            List<ICompetitor> kia = new List<ICompetitor>();
            int totalShots = 0;
            while (true) // Infinite game loop
            {
                foreach (var activePlayer in players)
                {
                    while (!kia.Contains(activePlayer))
                    {
                        Debug.WriteLine("Active=" + activePlayer);
                        var availableHitBoards = hitBoards.Where(h => h.Owner.Equals(activePlayer) && !kia.Contains(h.Rival));
                        if (availableHitBoards.Count() == 1)
                        {
                            throw new GameOverException(activePlayer, totalShots);
                        }

                        var shot = activePlayer.Shoot(availableHitBoards);
                        var rivalsFleet = fleets[shot.Rival.Id];
                        var effect = rivalsFleet.TakeShot(shot.Coords);
                        var hit = new Hit
                        {
                            Attacker = activePlayer,
                            Target = shot.Rival,
                            Coords = shot.Coords,
                            Effect = effect,
                        };
                        Debug.WriteLine(hit);
                        //if (effect != ShotEffect.Miss)
                        //Console.WriteLine(hit);
                        totalShots++;
                        //Console.ReadKey();
                        if (visialize)
                        {
                            System.Threading.Thread.Sleep(100);
                            DisplayHit(hit);
                        }
                        foreach (var player in players)
                        {
                            var hitboard = hitBoards.Where(h => h.Owner.Equals(player) && !kia.Contains(h.Rival));
                            player.UpdateHits(hitboard, hit);
                        }
                        if (effect == ShotEffect.Kill && !rivalsFleet.IsAlive())
                        {
                            kia.Add(shot.Rival);
                            Debug.WriteLine("{0}: GG", hit.Target);
                        }
                        if (effect == ShotEffect.Miss) break;
                    }
                }
            }
        }

        private const string HIT = "H";
        private const string MISS = "-";
        private const string KILL = "K";

        private void DisplayHit(Hit hit)
        {
            var offset = hit.Target.Id - 1;
            var top = 0;
            var left = offset * (Dim + 5);

            Console.SetCursorPosition(left + hit.Coords.Col, top + hit.Coords.Row);
            switch (hit.Effect)
            {
                case ShotEffect.Hit:
                    Console.Write(HIT);
                    break;
                case ShotEffect.Kill:
                    Console.Write(KILL);
                    break;
                default:
                    Console.Write(MISS);
                    break;
            }
        }
    }
}
