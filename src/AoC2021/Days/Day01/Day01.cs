using System.IO;

namespace AoC2021.Days
{
    public class Day01 : IDay
    {
        private string[] input;

        public Day01(string file)
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
            for (var i = 1; i < input.Length; i++)
            {
                if (int.Parse(input[i]) > int.Parse(input[i-1]))
                    answer += 1;
            }
            return answer.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;

            var d0 = int.Parse(input[0]);
            var d1 = int.Parse(input[1]);
            var d2 = int.Parse(input[2]);
            var previousWindowSum = d0 + d1 + d2;

            for (var i = 1; i < input.Length - 2; i++)
            {
                var windowSum = previousWindowSum - int.Parse(input[i-1]) + int.Parse(input[i+2]);
                if (windowSum > previousWindowSum) 
                    answer += 1;
                previousWindowSum = windowSum;
            }
            
            return answer.ToString();
        }
    }
}