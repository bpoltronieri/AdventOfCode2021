using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day15Utils;

namespace AoC2021.Days
{
    public class Day15 : IDay
    {
        private int[][] riskMap;

        public Day15(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            riskMap = File.ReadAllLines(file)
                          .Select(x => x.ToCharArray()
                                        .Select(c => (int)char.GetNumericValue(c))
                                        .ToArray())
                          .ToArray();
        }

        public string PartOne()
        {
            var answer = GetShortestPath(1);
            return answer.ToString();
        }

        public string PartTwo()
        {
            var answer = GetShortestPath(5);
            return answer.ToString();
        }

        private int GetShortestPath(int tiling)
        {
            var endX = (riskMap[0].Count() * tiling) - 1;
            var endY = (riskMap.Count() * tiling) - 1;

            // once again accidentally implemented Dijkstra's
            var positionRiskCache = new Dictionary<Tuple<int,int>, int>(); // maps position to lowest risk path found (so far) to get there
            var pathList = new List<RiskyCavePath>(); // would be better to use a sorted list or priority queue
            var startPath = new RiskyCavePath(0, 0, 0);
            pathList.Add(startPath);
            
            while (pathList.Count > 0)
            {
                var currentPath = pathList.Min(); // path with minimum risk. See RiskyCavePath.CompareTo
                pathList.Remove(currentPath);
                foreach (var neighbour in GetNeighbours(currentPath.X, currentPath.Y, endX, endY))
                {
                    var (newX, newY) = (neighbour.Item1, neighbour.Item2);
                    var newRisk = currentPath.Risk + RiskValue(newX, newY);

                    if (positionRiskCache.ContainsKey(neighbour) && positionRiskCache[neighbour] <= newRisk) // already found a quicker way here
                        continue;

                    positionRiskCache[neighbour] = newRisk;

                    if (newX != endX || newY != endY)
                        pathList.Add(new RiskyCavePath(newRisk, newX, newY));
                    else
                        break; // reached end point and must be shortest path becauses of our choice of currentPath
                }
            }
            return positionRiskCache[new Tuple<int,int>(endX, endY)];
        }

        private int RiskValue(int x, int y)
        {
            var baseXLen = riskMap[0].Count();
            var baseYLen = riskMap.Count();

            var baseX = x % baseXLen;
            var baseY = y % baseYLen;
            
            var baseRisk = riskMap[baseY][baseX];

            var increment = (x / baseXLen) + (y / baseYLen);
            
            return ((baseRisk + increment - 1) % 9) + 1;
        }

        private IEnumerable<Tuple<int, int>> GetNeighbours(int x, int y, int endX, int endY)
        {
            if (x > 0) yield return new Tuple<int, int>(x-1, y);
            if (y > 0) yield return new Tuple<int, int>(x, y-1);
            if (x < endX) yield return new Tuple<int, int>(x+1, y);
            if (y < endY) yield return new Tuple<int, int>(x, y+1);
        }
    }
}