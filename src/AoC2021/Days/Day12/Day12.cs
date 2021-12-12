using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day12Utils;

namespace AoC2021.Days
{
    public class Day12 : IDay
    {
        private Dictionary<string, List<string>> caveLinks;

        public Day12(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            caveLinks = new Dictionary<string, List<string>>();
            foreach (var line in input)
            {
                var startEnd = line.Split('-');
                AddLinkBetween(startEnd[0], startEnd[1]);
            }
        }

        private void AddLinkBetween(string v1, string v2)
        {
            if (!caveLinks.ContainsKey(v1))
                caveLinks[v1] = new List<string>();
            if (!caveLinks.ContainsKey(v2))
                caveLinks[v2] = new List<string>();

            caveLinks[v1].Add(v2);
            caveLinks[v2].Add(v1);
        }

        public string PartOne()
        {
            return CountCavePaths(false).ToString();
        }

        public string PartTwo()
        {
            return CountCavePaths(true).ToString();
        }

        private int CountCavePaths(bool canVisitSmallCaveTwice)
        {
            var nPaths = 0;
            var ongoingPaths = new Queue<CavePath>();
            ongoingPaths.Enqueue(new CavePath("start", canVisitSmallCaveTwice));
            while (ongoingPaths.Count > 0)
            {   
                var currentPath = ongoingPaths.Dequeue();
                foreach (var cave in caveLinks[currentPath.CurrentCave])
                {
                    if (cave == "end")
                    {
                        nPaths += 1;
                        //Console.WriteLine(currentPath.pathString + "end");
                        continue;
                    }
                    else if (currentPath.CanVisitCave(cave))
                    {
                        var nextPath = new CavePath(currentPath);
                        nextPath.AddCaveToPath(cave);
                        ongoingPaths.Enqueue(nextPath);
                    }
                }
            }
            return nPaths;
        }
    }
}