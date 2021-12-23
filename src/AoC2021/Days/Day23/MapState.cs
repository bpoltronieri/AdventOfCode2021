using System;
using System.Collections.Generic;

namespace AoC2021.Days.Day23Utils
{
    struct MapState : IComparable<MapState>
    {
        public char[,] map;
        public int energyCost; // cost spent so far to reach this state

        public List<string> history;

        public MapState(char[,] map, int energyCost)
        {
            this.map = map;
            this.energyCost = energyCost;
            history = new List<string>();
            history.Add(this.HashString());
        }

        public MapState(char[,] map, int energyCost, List<string> previousHistory)
        {
            this.map = map;
            this.energyCost = energyCost;
            history = new List<string>(previousHistory);
            history.Add(this.HashString());
        }

        public string HashString()
        {
            var hash = "";
            var pointsOfInterest = new (int,int)[15] 
                {
                    (1,1), (2,1), (4,1), (6,1), (8,1), (10,1), (11,1),
                    (3,2), (5,2), (7,2), (9,2), (3,3), (5,3), (7,3), (9,3)
                };
            foreach (var point in pointsOfInterest)
                hash += map[point.Item1, point.Item2];
            return hash;
        }

        private void drawHashString(string hash)
        {
            var lines = new string[5];
            lines[0] = "#############";
            lines[1] = "#" + hash.Substring(0,2) + "."
                        + hash[2] + "." + hash[3] + "." 
                        + hash[4] + "." + hash.Substring(5,2) + "#";
            lines[2] = "###" + hash[7] + "#" + hash[8] + "#" + hash[9] + "#" + hash[10] + "###";
            lines[3] = "  #" + hash[11] + "#" + hash[12] + "#" + hash[13] + "#" + hash[14] + "#";
            lines[4] = "  #########";
            
            foreach (var line in lines)
                Console.WriteLine(line);
            Console.WriteLine("");
        }

        public void DrawHistory()
        {
            foreach (var state in history)
                drawHashString(state);
        }

        public void DrawMap()
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                var line = "";
                for (var x = 0; x < map.GetLength(0); x++)
                    line += map[x,y];
                Console.WriteLine(line);
            }
        }

        public int CompareTo(MapState other)
        {
            return this.energyCost - other.energyCost;
        }
    }
}