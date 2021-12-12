using System.Collections.Generic;

namespace AoC2021.Days.Day12Utils
{
    class CavePath
    {
        public string currentCave;
        public HashSet<string> smallCavesVisited;

        public bool visitedSmallCaveTwice;

        // public string pathString;

        public CavePath(string cave, CavePath previousPath)
        {
            currentCave = cave;
            smallCavesVisited = new HashSet<string>(previousPath.smallCavesVisited);
            // pathString = previousPath.pathString;
            visitedSmallCaveTwice = previousPath.visitedSmallCaveTwice;
        }

        public CavePath(string cave)
        {
            currentCave = cave;
            smallCavesVisited = new HashSet<string>();
            smallCavesVisited.Add("start");
            visitedSmallCaveTwice = false;
            // pathString = "start,";
        }
    }
}