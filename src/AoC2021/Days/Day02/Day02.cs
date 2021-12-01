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
            var answer = 0;
            for (var i = 0; i < input.Length; i++)
            {
                // int.Parse(input[i])
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;

            for (var i = 0; i < input.Length; i++)
            {
                // int.Parse(input[i-1])
            }

            return answer.ToString();
        }
    }
}