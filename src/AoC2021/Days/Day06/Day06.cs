using System;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day06 : IDay
    {
        private string[] input;

        public Day06(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            // Part One uses a List with an entry per fish, which is far too slow
            // and uses too much memory for Part Two. Part One should just use the 
            // same code as Part Two but thought I'd leave it in for comparison.
            var fishes = input[0].Split(',').Select(x => int.Parse(x)).ToList();
            for (var i = 0; i < 80; i++)
            {
                var nNewFishes = fishes.Count(x => x == 0);
                for (var j = 0; j < fishes.Count(); j++)
                {
                    if (fishes[j] == 0)
                        fishes[j] = 6;
                    else
                        fishes[j] -= 1;
                }
                var newFishes = Enumerable.Repeat(8, nNewFishes).ToList();
                fishes.AddRange(newFishes);
            }
            return fishes.Count().ToString();
        }

        public string PartTwo()
        {
            var fishes = input[0].Split(',').Select(x => int.Parse(x));
            var fishCounts = new long[9]; // number of fishes at cycles from 0 to 8
            for (var i = 0; i < 9; i++)
                fishCounts[i] = fishes.Count(x => x == i);

            for (var i = 0; i < 256; i++)
            {      
                var nNewFishes = fishCounts[0];
                // rotate array to the left:
                for (var j = 0; j < 8; j++)
                    fishCounts[j] = fishCounts[j+1];
                fishCounts[6] += nNewFishes; // parent fishes move back to 6
                fishCounts[8] = nNewFishes; // baby fishes
            }
            return fishCounts.Sum().ToString();
        }
    }
}