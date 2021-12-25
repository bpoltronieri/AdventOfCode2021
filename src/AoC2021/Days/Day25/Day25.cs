using System.IO;

namespace AoC2021.Days
{
    public class Day25 : IDay
    {
        private char[,] inputMap;

        public Day25(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            inputMap = new char[input[0].Length, input.Length];
            for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                    inputMap[x,y] = input[y][x];
        }

        public string PartOne()
        {
            var moved = true;
            var map = CopyMap(inputMap);
            var nSteps = 0;
            while (moved)
            {
                moved = false;
                // east facing first
                var newMap = CopyMap(map);
                for (var x = 0; x < map.GetLength(0); x++)
                    for (var y  = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x,y] == '>' && map[(x+1) % map.GetLength(0), y] == '.')
                        {
                            newMap[x,y] = '.';
                            newMap[(x+1) % map.GetLength(0), y] = '>';
                            moved = true;
                        }
                    }
                map = newMap;

                // south facing
                newMap = CopyMap(map);
                for (var x = 0; x < map.GetLength(0); x++)
                    for (var y  = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x,y] == 'v' && map[x, (y+1) % map.GetLength(1)] == '.')
                        {
                            newMap[x,y] = '.';
                            newMap[x, (y+1) % map.GetLength(1)] = 'v';
                            moved = true;
                        }
                    }
                map = newMap;
                
                nSteps += 1;
            }
            return nSteps.ToString();
        }

        private char[,] CopyMap(char[,] map)
        {
            return (char[,])map.Clone();
        }

        public string PartTwo()
        {
            // no part two on day 25!
            var answer = 0;
            return answer.ToString();
        }
    }
}