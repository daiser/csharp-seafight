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

        public ICompetitor Play()
        {
            if (players.Count < 2) throw new InvalidOperationException("Not enough players");

            fleets.Clear();
            hitBoards.Clear();

            foreach (var player in players)
            {
                Board board = new Board(Dim);
                var fleet = player.PlaceFleet(Armada, board);
                if (fleet == null) throw new InvalidOperationException(string.Format("Player #{0:d} failed to place fleet", player.Id));
                fleets.Add(player.Id, fleet);

                foreach (var rival in players)
                {
                    if (player == rival) continue;
                    hitBoards.Add(new HitBoard(player, rival, Dim));
                }
            }


            List<ICompetitor> kia = new List<ICompetitor>();
            ICompetitor winner = null;
            while (winner == null)
            {
                var alive = players.Where(p => !kia.Contains(p));
                if (alive.Count() == 1)
                {
                    winner = alive.First();
                    continue;
                }
                foreach (var activePlayer in alive)
                {
                    Debug.WriteLine("{0}'s turn", activePlayer);
                    while (true)
                    {
                        if (kia.Contains(activePlayer))
                        {
                            Debug.WriteLine("{0}'s is dead. Next!", activePlayer);
                            continue;
                        }

                        var hbs = hitBoards.Where(h => h.Owner.Equals(activePlayer) && !kia.Contains(h.Rival));
                        if (hbs.Count() == 0) break;

                        var shot = activePlayer.Shoot(hbs);
                        var rivalsFleet = fleets[shot.rival.Id];
                        var effect = rivalsFleet.TakeShot(shot.coords);
                        var hit = new Hit
                        {
                            attacker = activePlayer,
                            target = shot.rival,
                            coords = shot.coords,
                            effect = effect,
                        };
                        //DisplayHit(hit);
                        //Console.ReadKey();
                        //if (effect != ShotEffect.Miss)
                        Console.WriteLine(hit);
                        foreach (var player in alive)
                        {
                            var hitboard = hitBoards.Where(h => h.Owner.Equals(player) && !kia.Contains(h.Rival));
                            player.UpdateHits(hitboard, hit);
                        }
                        if (!rivalsFleet.IsAlive())
                        {
                            kia.Add(shot.rival);
                        }
                        if (effect == ShotEffect.Miss) break;
                    }
                }
            }

            return winner;
        }

        private void DisplayHit(Hit hit)
        {
            var offset = hit.target.Id - 1;
            var top = 0;
            var left = offset * (Dim + 2);

            Console.SetCursorPosition(left + hit.coords.Col, top + hit.coords.Row);
            Console.Write('*');
        }
    }
}
