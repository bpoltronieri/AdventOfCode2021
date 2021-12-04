using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day04 : IDay
    {
        private string[] input;

        public Day04(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        private (List<int>, List<int[,]>) ParseInput(string[] input)
        {
            var numbers = input[0].Split(',').Select(x => int.Parse(x)).ToList();
            var grids = new List<int[,]>();
            var j = 2;
            while (j < input.Count())
            {
                var grid = new int[5,5];
                for (var i = 0; i < 5; i++)
                {
                    var row = input[j+i].Split(' ')
                                        .Where(x => x != "")
                                        .Select(x => int.Parse(x.Trim()))
                                        .ToArray();
                    for (var k = 0; k < 5; k++)
                        grid[i,k] = row[k];
                }
                grids.Add(grid);
                j += 6;
            }
            return (numbers, grids);
        }

        private int GridScore(int[,] grid, int n)
        {
            var sum = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (grid[i,j] != -1)
                        sum += grid[i,j];
                }
            return sum * n;
        }

        private bool GridIsDone(int[,] grid)
        {
            var indices = new int[] {0, 1, 2, 3, 4};
            // check all rows and columns in one loop:
            for (var i = 0; i < 5; i++)
            {
                var row = indices.Select(x => grid[i, x]);
                if (row.All(x => x == -1))
                    return true;
                var col = indices.Select(x => grid[x, i]);
                if (col.All(x => x == -1))
                    return true;
            }
            return false;
        }

        private void RemoveNFromGrid(int[,] grid, int n)
        {
            // "remove" the number n from grid by overwriting with -1
            for (var i = 0; i < 5; i++)
                for (var j = 0; j < 5; j++)
                {
                    if (grid[i,j] == n)
                        grid[i,j] = -1;
                }
        }

        public string PartOne()
        {
            var (numbers, grids) = ParseInput(input);
            
            var answer = 0;
            var done = false;
            foreach (var n in numbers)
            {
                foreach (var grid in grids)
                {
                    RemoveNFromGrid(grid, n);
                    if (GridIsDone(grid))
                    {
                        answer = GridScore(grid, n);
                        done = true;
                        break;
                    }
                }
                if (done) break;
            }
            return (answer).ToString();
        }

        public string PartTwo()
        {
            var (numbers, grids) = ParseInput(input);
            
            var answer = 0;
            var done = false;
            foreach (var n in numbers)
            {
                for (var i = 0; i < grids.Count; i++)
                {
                    var grid = grids[i];
                    RemoveNFromGrid(grid, n);
                    if (GridIsDone(grid))
                    {
                        if (grids.Count == 1) // last grid to finish
                        {
                            answer = GridScore(grid, n);
                            done = true;
                            break;
                        }
                        else
                        {
                            grids.RemoveAt(i);
                            i -= 1;
                        }
                    }
                }
                if (done) break;
            }
            return (answer).ToString();
        }
    }
}