using System.IO;

namespace AoC2021.Days
{
    public class Day24 : IDay
    {
        private string[] input;

        public Day24(string file)
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