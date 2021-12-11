using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day11 : IDay
    {
        private int[][] energyMapConst; // stays fixed as the input map
        private int[][] energyMap; // to be cloned from input then modified by the two parts

        public Day11(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var str_input = File.ReadAllLines(file);
            energyMapConst = str_input.Select(x => x.ToCharArray()
                                                    .Select(c => (int)char.GetNumericValue(c))
                                                    .ToArray())
                                      .ToArray();
        }

        private int[][] CopyEnergyMapConst()
        {
            return energyMapConst.Select(x => x.ToArray())
                                 .ToArray();
        }

        public string PartOne()
        {
            energyMap = CopyEnergyMapConst();
            
            var nFlashes = 0;
            for (var step = 0; step < 100; step++)
            {
                IncrementAllOctopodes();
                PropagateFlashes();
                nFlashes += CountAndResetFlashes();
            }
            return nFlashes.ToString();
        }

        private void IncrementAllOctopodes()
        {
            for (var i = 0; i < energyMap.Count(); i++)
                for (var j = 0; j < energyMap[i].Count(); j++)
                    energyMap[i][j] += 1;
            return;
        }

        private void PropagateFlashes()
        {
            var toFlash = new Queue<Tuple<int,int>>();
            // init toFlash:
            for (var i = 0; i < energyMap.Count(); i++)
                for (var j = 0; j < energyMap[i].Count(); j++)
                {
                    if (energyMap[i][j] == 10)
                        toFlash.Enqueue(new Tuple<int,int>(i, j));
                }

            while (toFlash.Count() > 0)
            {
                var flashing = toFlash.Dequeue();
                foreach (var neighbour in GetNeighbours(flashing))
                {
                    var i = neighbour.Item1;
                    var j = neighbour.Item2;
                    energyMap[i][j] += 1;
                    if (energyMap[i][j] == 10)
                        toFlash.Enqueue(new Tuple<int,int>(i, j));
                }
            }
        }

        private IEnumerable<Tuple<int, int>> GetNeighbours(Tuple<int, int> current)
        {
            var rowLen = energyMap.Count();
            var colLen = energyMap[0].Count();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var i = current.Item1 + x;
                    var j = current.Item2 + y;
                    if (i >= 0 && i < rowLen && j >= 0 && j < colLen)
                        yield return new Tuple<int, int>(i, j);
                }
        }

        private int CountAndResetFlashes()
        {
            var nFlashes = 0;
            for (var i = 0; i < energyMap.Count(); i++)
                for (var j = 0; j < energyMap[i].Count(); j++)
                {
                    if (energyMap[i][j] > 9)
                    {
                        nFlashes += 1;
                        energyMap[i][j] = 0;
                    }
                }
            return nFlashes;
        }

        public string PartTwo()
        {
            energyMap = CopyEnergyMapConst();
            
            var step = 0;
            while (!AllFlashed())
            {
                IncrementAllOctopodes();
                PropagateFlashes();
                CountAndResetFlashes();
                step += 1;
            }
            return step.ToString();
        }

        private bool AllFlashed()
        {
            return energyMap.All(x => x.All(y => y == 0));
        }
    }
}