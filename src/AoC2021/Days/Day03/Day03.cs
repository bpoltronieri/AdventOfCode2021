using System.IO;

namespace AoC2021.Days
{
    public class Day03 : IDay
    {
        private string[] input;

        public Day03(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var answer = 0;

            foreach (var line in input)
            {
                // var command = line.Split(' ');
            }
            return (answer).ToString();
        }

        public string PartTwo()
        {
            var answer = 0;

            foreach (var line in input)
            {
                // var command = line.Split(' ');
            }
            return (answer).ToString();
        }
    }
}