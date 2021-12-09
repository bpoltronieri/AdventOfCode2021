using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day09 : IDay
    {
        private int[][] heightMap;

        public Day09(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var str_input = File.ReadAllLines(file);
            heightMap = str_input.Select(x => x.ToCharArray()
                                               .Select(c => (int)char.GetNumericValue(c))
                                               .ToArray())
                                 .ToArray();

        }

        public string PartOne()
        {
            var answer = 0;
            for (var i = 0; i < heightMap.Length; i++)
            {
                var row = heightMap[i];
                for (var j = 0; j < row.Length; j++)
                {
                    if (IsLowPoint(i, j))
                        answer += row[j] + 1; // risk level
                }
            }
            return answer.ToString();
        }

        private bool IsLowPoint(int i, int j)
        {
            if (i > 0 && heightMap[i][j] >= heightMap[i-1][j])
                return false;

            if (j > 0 && heightMap[i][j] >= heightMap[i][j-1])
                return false;
            
            var row_len = heightMap.Count();
            if (i < row_len - 1 && heightMap[i][j] >= heightMap[i+1][j])
                return false;

            var col_len = heightMap[0].Count();
            if (j < col_len - 1 && heightMap[i][j] >= heightMap[i][j+1])
                return false;

            return true;
        }

        public string PartTwo()
        {
            var lowPointBasins = InitLowPointBasinsMap(); // maps low points to size of their basins
            var lowPoints = lowPointBasins.Keys.ToList(); // if foreach over keys directly, we can never modify the dictionary
            foreach (var lowPoint in lowPoints)
            {
                var ptsInBasin = new HashSet<Tuple<int,int>>();
                ptsInBasin.Add(lowPoint);    
                FlowFromPoint(lowPoint, ptsInBasin, lowPointBasins);
                lowPointBasins[lowPoint] = ptsInBasin.Count();
            }
            var sizes = lowPointBasins.Values.ToList();
            var product = 1;
            for (var i = 0; i < 3; i++)
            {
                var highest = sizes.Max();
                product *= highest;
                sizes.Remove(highest);
            }
            return product.ToString();
        }

        private void FlowFromPoint(Tuple<int, int> point, HashSet<Tuple<int, int>> ptsInBasin, Dictionary<Tuple<int, int>, int> lowPointBasins)
        {
            var i = point.Item1;
            var j = point.Item2;

            var ptsToVisit = new List<Tuple<int,int>>();
            
            if (i > 0 && FlowsDownTo(i-1, j, i, j))
                ptsToVisit.Add(new Tuple<int,int>(i-1, j)); // left

            if (j > 0 && FlowsDownTo(i, j-1, i, j))
                ptsToVisit.Add(new Tuple<int,int>(i, j-1)); // up

            var row_len = heightMap.Count();
            if (i < row_len - 1 && FlowsDownTo(i+1, j, i, j))
                ptsToVisit.Add(new Tuple<int,int>(i+1, j)); // right

            var col_len = heightMap[0].Count();
            if (j < col_len - 1 && FlowsDownTo(i, j+1, i, j))
                ptsToVisit.Add(new Tuple<int,int>(i, j+1)); // down

            foreach (var pt in ptsToVisit.Where(p => !ptsInBasin.Contains(p)))
            {
                ptsInBasin.Add(pt);
                FlowFromPoint(pt, ptsInBasin, lowPointBasins);
            }
        }

        private bool FlowsDownTo(int i1, int j1, int i2, int j2)
        {
            // whether (i1, j1) flows down to (i2, j2)
            return heightMap[i1][j1] != 9 && heightMap[i1][j1] > heightMap[i2][j2];
        }

        private Dictionary<Tuple<int,int>, int> InitLowPointBasinsMap()
        {
            var lowPoints = new Dictionary<Tuple<int,int>, int>();
            for (var i = 0; i < heightMap.Length; i++)
            {
                var row = heightMap[i];
                for (var j = 0; j < row.Length; j++)
                {
                    if (IsLowPoint(i, j))
                        lowPoints[new Tuple<int, int>(i,j)] = 1; // each low point is in its own basin
                }
            }
            return lowPoints;
        }
    }
}