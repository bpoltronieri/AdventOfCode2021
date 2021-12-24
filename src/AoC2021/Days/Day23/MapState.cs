using System;
using System.Collections.Generic;

namespace AoC2021.Days.Day23Utils
{
    struct MapState
    {
        public char[,] map;
        public int energyCost; // cost spent so far to reach this state

        public List<string> history; // map history, for debugging only

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
            var hallwayPoints = new (int,int)[7] { (1,1), (2,1), (4,1), (6,1), (8,1), (10,1), (11,1) };
            var roomXPosns = new int[4] {3, 5, 7, 9};
            var roomDepth = map.GetLength(1) - 2;

            foreach (var point in hallwayPoints)
                hash += map[point.Item1, point.Item2];

            for (var y = 2; y <= roomDepth; y++)
                foreach (var x in roomXPosns)
                    hash += map[x, y];

            return hash;
        }

        private void drawHashString(string hash)
        {
            var lines = new string[map.GetLength(1)];
            lines[0] = "#############";
            lines[1] = "#" + hash.Substring(0,2) + "."
                        + hash[2] + "." + hash[3] + "." 
                        + hash[4] + "." + hash.Substring(5,2) + "#";
            lines[2] = "###" + hash[7] + "#" + hash[8] + "#" + hash[9] + "#" + hash[10] + "###";

            for (var i = 0; i < map.GetLength(1) - 4; i++)
                lines[3+i] = "  #" + hash[11+4*i] + "#" + hash[12+4*i] + "#" + hash[13+4*i] + "#" + hash[14+4*i] + "#";

            lines[map.GetLength(1) - 1] = "  #########";
            
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
    }
}