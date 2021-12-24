using System.IO;

namespace AoC2021.Days
{
    public class Day25 : IDay
    {
        private string[] input;

        public Day25(string file)
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
            return answer.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;
            return answer.ToString();
        }
    }
}