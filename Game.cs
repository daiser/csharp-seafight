using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SeaFight.Armada;
using SeaFight.Board;
using SeaFight.Players;

namespace SeaFight
{
    class Game
    {
        private int Dim { get; }

        private FleetLayout Armada { get; }

        private readonly List<ICompetitor> m_players = new List<ICompetitor>();
        private readonly List<HitBoard> m_hitBoards = new List<HitBoard>();
        private readonly Dictionary<int, Fleet> m_fleets = new Dictionary<int, Fleet>();


        public Game(int dim, FleetLayout fleetLayout) {
            Dim = dim;
            Armada = fleetLayout;
        }


        public void RegisterPlayer(ICompetitor player) { m_players.Add(player); }


        public void Play(bool visualize = true) {
            if (m_players.Count < 2) throw new InvalidOperationException("Not enough players");

            Console.CursorVisible = false;
            m_fleets.Clear();
            m_hitBoards.Clear();

            foreach (var player in m_players) {
                var board = new Board.Board(Dim);
                var fleet = player.PlaceFleet(Armada, board);
                if (fleet == null) throw new InvalidOperationException($"Player #{player.Id:d} failed to place fleet");
                m_fleets.Add(player.Id, fleet);

                //fleets.Add(player.Id, Fleet.fixed_0);

                foreach (var rival in m_players) {
                    m_hitBoards.Add(new HitBoard(player, rival, Dim));
                }
            }

            var kia = new List<ICompetitor>();
            var totalShots = 0;
            while (true) // Infinite game loop
            {
                foreach (var activePlayer in m_players) {
                    while (!kia.Contains(activePlayer)) {
                        Debug.WriteLine("Active=" + activePlayer);
                        var availableHitBoards = m_hitBoards.Where(h => h.Owner.Equals(activePlayer) && !kia.Contains(h.Rival));
                        if (availableHitBoards.Count() == 1) {
                            throw new GameOverException(activePlayer, totalShots);
                        }

                        var shot = activePlayer.Shoot(availableHitBoards);
                        var rivalsFleet = m_fleets[shot.Victim.Id];
                        var effect = rivalsFleet.TakeShot(shot.Target);
                        var hit = new Hit {
                            Attacker = activePlayer, Victim = shot.Victim, Target = shot.Target, Effect = effect,
                        };
                        Debug.WriteLine(hit);
                        //if (effect != ShotEffect.Miss)
                        //Console.WriteLine(hit);
                        totalShots++;
                        //Console.ReadKey();
                        if (visualize) {
                            Thread.Sleep(100);
                            DisplayHit(hit);
                        }
                        foreach (var player in m_players) {
                            var hitboard = m_hitBoards.Where(h => h.Owner.Equals(player) && !kia.Contains(h.Rival));
                            player.UpdateHits(hitboard, hit);
                        }
                        if (effect == ShotEffect.Kill && !rivalsFleet.IsAlive()) {
                            kia.Add(shot.Victim);
                            Debug.WriteLine("{0}: GG", hit.Victim);
                        }
                        if (effect == ShotEffect.Miss) break;
                    }
                }
            }
        }


        private const string HIT = "H";
        private const string MISS = "-";
        private const string KILL = "K";


        private void DisplayHit(Hit hit) {
            var offset = hit.Victim.Id - 1;
            var top = 0;
            var left = offset * (Dim + 5);

            Console.SetCursorPosition(left + hit.Target.Col, top + hit.Target.Row);
            switch (hit.Effect) {
                case ShotEffect.Hit:
                    Console.Write(HIT);
                    break;
                case ShotEffect.Kill:
                    Console.Write(KILL);
                    break;
                case ShotEffect.Miss:
                default:
                    Console.Write(MISS);
                    break;
            }
        }
    }
}