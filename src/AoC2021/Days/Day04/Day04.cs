using System;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day04 : IDay
    {
        private string[] input;

        public Day04(string file)
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

            }
            return (answer).ToString();
        }

        public string PartTwo()
        {
            var answer = 0;
            for (var i = 1; i < input.Length; i++)
            {

            }
            return (answer).ToString();
        }
    }
}