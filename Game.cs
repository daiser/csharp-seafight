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
        private Forms.LogForm log;
        public Game(int dim, FleetLayout fleetLayout)
        {
            Dim = dim;
            Armada = fleetLayout;
        }

        public void RegisterPlayer(ICompetitor player)
        {
            players.Add(player);
        }

        public GameStats Play()
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

                foreach (var rival in players)
                {
                    hitBoards.Add(new HitBoard(player, rival, Dim));
                }
            }

            List<ICompetitor> kia = new List<ICompetitor>();
            GameStats stats = new GameStats();
            using (log = new Forms.LogForm())
            {
                log.Show();
                while (stats.winner == null)
                {
                    var alive = players.Where(p => !kia.Contains(p));
                    if (alive.Count() == 1)
                    {
                        stats.winner = alive.First();
                        continue;
                    }
                    foreach (var activePlayer in alive)
                    {
                        while (true)
                        {
                            if (kia.Contains(activePlayer))
                            {
                                break;
                            }

                            var availableHitBoards = hitBoards.Where(h => h.Owner.Equals(activePlayer) && !kia.Contains(h.Rival));
                            if (availableHitBoards.Count() == 1)
                            {
                                break;
                            }

                            var shot = activePlayer.Shoot(availableHitBoards);
                            var rivalsFleet = fleets[shot.rival.Id];
                            var effect = rivalsFleet.TakeShot(shot.coords);
                            var hit = new Hit
                            {
                                attacker = activePlayer,
                                target = shot.rival,
                                coords = shot.coords,
                                effect = effect,
                            };
                            if (hit.attacker.Equals(hit.target))
                            {
                                log.Message(string.Format("Player '{0}' hit himself", hit.attacker));
                                log.Refresh();
                            }
                            DisplayHit(hit);
                            //Console.ReadKey();
                            //if (effect != ShotEffect.Miss)
                            //Console.WriteLine(hit);
                            stats.totalShots++;
                            System.Threading.Thread.Sleep(100);
                            foreach (var player in alive)
                            {
                                var hitboard = hitBoards.Where(h => h.Owner.Equals(player) && !kia.Contains(h.Rival));
                                player.UpdateHits(hitboard, hit);
                            }
                            if (!rivalsFleet.IsAlive())
                            {
                                kia.Add(shot.rival);
                                log.Message(string.Format("{0}: gg", hit.target));
                                log.Refresh();
                            }
                            if (effect == ShotEffect.Miss) break;
                        }
                    }
                }
                log.Close();
            }
            Console.SetCursorPosition(0, 12);

            return stats;
        }

        private void DisplayHit(Hit hit)
        {
            var offset = hit.target.Id - 1;
            var top = 0;
            var left = offset * (Dim + 3);

            Console.SetCursorPosition(left + hit.coords.Col, top + hit.coords.Row);
            char c = '.';
            switch (hit.effect)
            {
                case ShotEffect.Hit:
                case ShotEffect.Kill:
                    c = 'X';
                    break;
                default:
                    c = '-';
                    break;
            }
            Console.WriteLine(c);
        }
    }

    class GameStats
    {
        public long totalShots = 0;
        public ICompetitor winner = null;
    }
}
