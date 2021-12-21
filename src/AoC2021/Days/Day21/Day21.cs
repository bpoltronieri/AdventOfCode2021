using System.Collections.Generic;
using System.Linq;
using System.IO;
using AoC2021.Days.Day21Utils;
using System;

namespace AoC2021.Days
{
    public class Day21 : IDay
    {
        private int player1Start; // starting square of player 1
        private int player2Start; // starting square of player 2

        public Day21(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            player1Start = int.Parse(input[0].Substring(28));
            player2Start = int.Parse(input[1].Substring(28));
        }

        public string PartOne()
        {
            var nRolls = 0;
            var loserScore = 0;

            var player1Score = 0;
            var player2Score = 0;
            foreach (var squares in PlayerSquares())
            {
                nRolls += 3;
                player1Score += squares.Item1;
                if (player1Score >= 1000)
                {
                    loserScore = player2Score;
                    break;
                }
                nRolls += 3;
                player2Score += squares.Item2;
                if (player2Score >= 1000)
                {
                    loserScore = player1Score;
                    break;
                }
            }

            return (nRolls * loserScore).ToString();
        }

        private IEnumerable<(int,int)> PlayerSquares()
        {
            // squares that players land on
            var square1 = player1Start;
            var square2 = player2Start;
            var n = 1;
            while (true)
            {
                square1 = (square1 + 8*n - 3) % 10 + 1;
                square2 = (square2 + 8*n - 4) % 10 + 1;
                yield return (square1, square2);
                n += 1;
            }
        }

        public string PartTwo()
        {
            var cache = new Dictionary<GameState, (long, long)>(); // maps game state to eventual number of wins for each player
            var startState = new GameState(player1Start, player2Start, 0, 0, true);
            var (player1Wins, player2Wins) = GetWinsFromGameState(startState, cache);
            return Math.Max(player1Wins, player2Wins).ToString();
        }

        // In part two, each turn, we are interested in the sum of 3 rolls of the
        // quantum die. there are 27 possible outcomes but only 7 distinct sums
        private Dictionary<int, int> quantumDieSums = new Dictionary<int, int>() { {3, 1}, {4, 3}, {5, 6}, {6, 7}, {7, 6}, {8, 3}, {9, 1} };

        private (long, long) GetWinsFromGameState(GameState game, Dictionary<GameState, (long, long)> cache)
        {
            // given a game state, returns the number of eventual wins for each player
            if (cache.ContainsKey(game))
                return cache[game];

            (long, long) totalWins = (0, 0);
            foreach (var rollSum in quantumDieSums.Keys)
            {
                GameState newGameState;
                if (game.Is1Turn)
                {
                    var newPlayer1Square = (game.Player1Square + rollSum - 1) % 10 + 1;
                    var newPlayer1Score = game.Player1Score + newPlayer1Square;
                    newGameState = new GameState(newPlayer1Square, game.Player2Square,
                                                 newPlayer1Score, game.Player2Score,
                                                 false);
                }
                else
                {
                    var newPlayer2Square = (game.Player2Square + rollSum - 1) % 10 + 1;
                    var newPlayer2Score = game.Player2Score + newPlayer2Square;
                    newGameState = new GameState(game.Player1Square, newPlayer2Square,
                                                 game.Player1Score, newPlayer2Score,
                                                 true);
                }

                (long, long) wins;
                if (newGameState.GameOver())
                    wins = newGameState.Winner() == 1 ? (1, 0) : (0, 1);
                else
                    wins = GetWinsFromGameState(newGameState, cache);

                totalWins.Item1 += quantumDieSums[rollSum] * wins.Item1;
                totalWins.Item2 += quantumDieSums[rollSum] * wins.Item2;
            }

            cache[game] = totalWins;
            return totalWins;
        }

    }
}