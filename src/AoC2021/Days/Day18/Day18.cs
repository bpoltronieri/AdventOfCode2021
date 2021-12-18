using System.IO;
using AoC2021.Days.Day18Utils;

namespace AoC2021.Days
{
    public class Day18 : IDay
    {
        private string[] input;

        public Day18(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var sum = new SnailfishNumber(input[0], 0);
            // Console.WriteLine("Start: " + sum.ToString());
            for (var i = 1; i < input.Length; i++)
            {
                var next = new SnailfishNumber(input[i], 0);
                // Console.WriteLine("Adding: " + next.ToString());
                sum += next;
                sum.Reduce();
            }
            // Console.WriteLine("Sum: " + sum.ToString());
            return sum.Magnitude().ToString();
        }

        public string PartTwo()
        {
            var largest = int.MinValue;
            for (var i = 0; i < input.Length; i++)
                for (var j = 0; j < input.Length; j++)
                {
                    if (i == j) continue;
                    var n1 = new SnailfishNumber(input[i], 0);
                    var n2 = new SnailfishNumber(input[j], 0);

                    var sum = n1 + n2;
                    sum.Reduce();
                    var magnitude = sum.Magnitude();
                    if (magnitude > largest)
                        largest = magnitude;
                }
            return largest.ToString();
        }
    }
}