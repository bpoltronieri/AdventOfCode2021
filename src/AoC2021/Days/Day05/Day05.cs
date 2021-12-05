using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day05 : IDay
    {
        private string[] input;

        public Day05(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        private List<int[][]> ParseInputIntoLines()
        {
            return input.Select(x => x.Split(' ')
                                       .Where(x => x[0] != '-')
                                       .Select(y => y.Split(',')
                                                     .Select(z => int.Parse(z))
                                                     .ToArray()
                                              )
                                       .ToArray() 
                                )
                        .ToList();
        }

        private void UpdateCovered(Dictionary<Tuple<int, int>, int> covered, int[][] line)
        {
            var start_x = line[0][0];
            var end_x = line[1][0];
            var step_x = Math.Sign(end_x - start_x);

            var start_y = line[0][1];
            var end_y = line[1][1];
            var step_y = Math.Sign(end_y - start_y);
                
            var n_steps = step_x != 0 
                        ? Math.Abs(end_x - start_x) + 1
                        : Math.Abs(end_y - start_y) + 1;

            for (var i = 0; i < n_steps; i++)
            {
                var position = new Tuple<int,int>(start_x + i * step_x, start_y + i * step_y);
                if (covered.ContainsKey(position))
                    covered[position] += 1;
                else
                    covered[position] = 1;
            }

        return;
        }

        private bool LineIsHorzOrVert(int[][] x)
        {
            return x[0][0] == x[1][0] || x[0][1] == x[1][1];
        }

        public string PartOne()
        {
            var covered = new Dictionary<Tuple<int,int>, int>(); // maps x,y to number of times covered
            ParseInputIntoLines()
                .Where(x => LineIsHorzOrVert(x)) 
                .ToList()
                .ForEach(l => UpdateCovered(covered, l));
            return covered.Values.Count(x => x > 1).ToString();
        }

        public string PartTwo()
        {
            var covered = new Dictionary<Tuple<int,int>, int>(); // maps x,y to number of times covered
            ParseInputIntoLines().ForEach(l => UpdateCovered(covered, l));
            return covered.Values.Count(x => x > 1).ToString();
        }
    }
}