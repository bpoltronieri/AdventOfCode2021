using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Days.Day12Utils
{
    class CavePath
    {
        public string CurrentCave;
        private HashSet<string> SmallCavesVisited;
        private bool CanVisitSmallCaveTwice;

        // public string pathString; // for debugging to print completed paths

        public CavePath(CavePath parentPath) // deep copy of parentPath
        {
            CurrentCave = parentPath.CurrentCave;
            SmallCavesVisited = new HashSet<string>(parentPath.SmallCavesVisited);
            CanVisitSmallCaveTwice = parentPath.CanVisitSmallCaveTwice;
            // pathString = previousPath.pathString;
        }

        public CavePath(string cave, bool canVisitSmallCaveTwice)
        {
            CurrentCave = cave;
            SmallCavesVisited = new HashSet<string>();
            SmallCavesVisited.Add("start");
            CanVisitSmallCaveTwice = canVisitSmallCaveTwice;
            // pathString = "start,";
        }

        public void AddCaveToPath(string cave) // assumes you already checked CanVisitCave
        {
            CurrentCave = cave;

            if (SmallCavesVisited.Contains(cave))
                CanVisitSmallCaveTwice = false;
            else if (IsSmallCave(cave))
                SmallCavesVisited.Add(cave);

            // pathString += cave + ",";
        }

        public bool CanVisitCave(string cave)
        {
            return cave != "start"
                && (!IsSmallCave(cave) 
                    || !SmallCavesVisited.Contains(cave)
                    || CanVisitSmallCaveTwice);
        }

        private bool IsSmallCave(string cave)
        {
            return cave.All(c => char.IsLower(c));
        }
    }
}