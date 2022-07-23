using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SeaFight.Ai;
using SeaFight.Boards;
using SeaFight.Ships;

namespace SeaFight
{
    internal class Game
    {
        private const string HIT = "H";
        private const string KILL = "K";
        private const string MISS = "-";
        private readonly Dictionary<int, Fleet> m_fleets = new Dictionary<int, Fleet>();

        private readonly Dictionary<int, Player> m_players = new Dictionary<int, Player>();


        public Game(int dim, FleetLayout fleetLayout) {
            Dim = dim;
            Armada = fleetLayout;
        }


        private FleetLayout Armada { get; }

        private int Dim { get; }


        public void Play(bool visualize = true) {
            if (m_players.Count < 2) throw new InvalidOperationException("Not enough players");

            Console.CursorVisible = false;
            m_fleets.Clear();

            foreach (var player in m_players.Values) m_fleets[player.Id] = player.StartGame(Dim, Armada);

            var alive = new List<int>();
            alive.AddRange(m_players.Select(p => p.Value.Id));
            var totalShots = 0;
            while (true) // Infinite game loop
                foreach (var activePlayer in m_players)
                    while (true) {
                        Debug.WriteLine("Active=" + activePlayer);
                        Debug.WriteLine(alive.Count);
                        if (alive.Count == 1) throw new GameOverException(activePlayer.Value, totalShots);

                        var shot = activePlayer.Value.Shoot(alive.Select(id => m_players[id]));
                        var rivalsFleet = m_fleets[shot.Victim.Id];
                        var hit = new Hit {
                            Attacker = activePlayer.Value, Victim = shot.Victim, Target = shot.Target, Result = rivalsFleet.TakeShot(shot)
                        };

                        Debug.WriteLine(hit);
                        totalShots++;
                        foreach (var player in m_players.Values) player.UpdateHits(hit);
                        if (visualize)
                        {
                            Thread.Sleep(50);
                            activePlayer.Value.PrintHitBoard(shot.Victim, shot.Victim.Id * Dim + 5, activePlayer.Key * Dim + 5, ReprCell);
                        }
                        if (hit.Result == CellState.Kill) {
                            if (hit.Result == CellState.Kill && !rivalsFleet.IsAlive)
                            {
                                alive.Remove(hit.Victim.Id);
                                Debug.WriteLine("{0}: GG", hit.Victim);
                            }
                        }
                        if (hit.Result == CellState.Miss) break;
                    }
        }


        public void RegisterPlayer(Player player) { m_players[player.Id] = player; }


        private string ReprCell(CellState state) {
            switch (state) {
                case CellState.Hit:
                    return "*";
                case CellState.Kill:
                    return "X";
                case CellState.Miss:
                    return "-";
                case CellState.None:
                default:
                    return " ";
            }
        }
    }
}