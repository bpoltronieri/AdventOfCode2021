using System.IO;

namespace AoC2021.Days
{
    public class Day10 : IDay
    {
        private string[] input;

        public Day10(string file)
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
            for (var i = 0; i < input.Length; i++)
            {
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;
            for (var i = 0; i < input.Length; i++)
            {
            }
            return answer.ToString();
        }
    }
}