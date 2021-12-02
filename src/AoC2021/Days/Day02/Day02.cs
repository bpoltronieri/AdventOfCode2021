using System.IO;

namespace AoC2021.Days
{
    public class Day02 : IDay
    {
        private string[] input;

        public Day02(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var horzn = 0;
            var depth = 0;

            foreach (var line in input)
            {
                var command = line.Split(' ');
                var dirn = command[0];
                var dist = int.Parse(command[1]);
                switch (dirn)
                {
                    case "down":
                        depth += dist;
                        break;
                    case "up":
                        depth -= dist;
                        break;
                    case "forward":
                        horzn += dist;
                        break;
                }
                
            }
            return (horzn * depth).ToString();
        }

        public string PartTwo()
        {
            var horzn = 0;
            var depth = 0;
            var aim = 0;

            foreach (var line in input)
            {
                var command = line.Split(' ');
                var dirn = command[0];
                var dist = int.Parse(command[1]);
                switch (dirn)
                {
                    case "down":
                        aim += dist;
                        break;
                    case "up":
                        aim -= dist;
                        break;
                    case "forward":
                        horzn += dist;
                        depth += aim * dist;
                        break;
                }
                
            }
            return (horzn * depth).ToString();
        }
    }
}