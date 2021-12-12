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
            var nPaths = 0;
            var ongoingPaths = new Queue<CavePath>();
            // initialise starting paths
            caveLinks["start"].ForEach(cave => ongoingPaths.Enqueue(new CavePath(cave)));

            while (ongoingPaths.Count > 0)
            {   
                var currentPath = ongoingPaths.Dequeue();
                foreach (var cave in caveLinks[currentPath.currentCave])
                {
                    if (cave == "end")
                    {
                        nPaths += 1;
                        continue;
                    }
                    else if (!currentPath.smallCavesVisited.Contains(cave))
                    {
                        var nextPath = new CavePath(cave, currentPath);
                        if (IsSmallCave(currentPath.currentCave))
                            nextPath.smallCavesVisited.Add(currentPath.currentCave);
                        ongoingPaths.Enqueue(nextPath);
                    }
                }
            }

            return nPaths.ToString();
        }

        private bool IsSmallCave(string cave)
        {
            return cave.All(c => char.IsLower(c));
        }

        public string PartTwo()
        {
            var nPaths = 0;
            var ongoingPaths = new Queue<CavePath>();
            // initialise starting paths
            caveLinks["start"].ForEach(cave => ongoingPaths.Enqueue(new CavePath(cave)));
            foreach (var path in ongoingPaths)
            {
                // path.pathString += path.currentCave + ",";
                if (IsSmallCave(path.currentCave))
                    path.smallCavesVisited.Add(path.currentCave);
            }

            while (ongoingPaths.Count > 0)
            {   
                var currentPath = ongoingPaths.Dequeue();
                foreach (var cave in caveLinks[currentPath.currentCave])
                {
                    if (cave == "start")
                        continue; // can't visit start twice
                    if (cave == "end")
                    {
                        //Console.WriteLine(currentPath.pathString + "end");
                        nPaths += 1;
                        continue; // can't visit end twice
                    }
                    else if (!(currentPath.smallCavesVisited.Contains(cave) && currentPath.visitedSmallCaveTwice)) 
                    {
                        var nextPath = new CavePath(cave, currentPath);
                        //nextPath.pathString += cave + ",";
                        if (IsSmallCave(cave))
                        {
                            if (nextPath.smallCavesVisited.Contains(cave))
                                nextPath.visitedSmallCaveTwice = true;
                            else
                                nextPath.smallCavesVisited.Add(cave);
                        }
                        ongoingPaths.Enqueue(nextPath);
                    }
                }
            }

            return nPaths.ToString();
        }
    }
}