using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AoC2021.Days
{
    public class Day20 : IDay
    {
        private string enhancement;
        private string[] inputImage;

        public Day20(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            enhancement = input[0];
            // whole algorithm will assume fully light squares become dark and 
            // fully dark squares become light. so better check that condition now.
            if (enhancement[0] != '#' || enhancement[enhancement.Length - 1] != '.')
                throw new InvalidDataException();

            inputImage = new string[input.Length - 2];
            Array.Copy(input, 2, inputImage, 0, input.Length - 2);
        }

        public string PartOne()
        {
            // need set to alternately track light and dark points.
            // we start off with infinitely many dark points, then after
            // one iteration we have inifinitely many light points, and so on.
            // therefore it's better to track the finite set of points each time.
            var trackedPoints = InitLightPointsFromInput();
            for (var i = 0; i < 2; i++) // two iterations of enhancement algorithm
                trackedPoints = ApplyEnhancementAlgorithm(trackedPoints, i);
        
            // after two iterations, trackedPoints is once again tracking light points
            var answer = trackedPoints.Count();
            return answer.ToString();
        }

        public string PartTwo()
        {
            // same as part one but 50 iterations of enhancement algorithm
            var trackedPoints = InitLightPointsFromInput();
            for (var i = 0; i < 50; i++)
                trackedPoints = ApplyEnhancementAlgorithm(trackedPoints, i);
            var answer = trackedPoints.Count();
            return answer.ToString();
        }

        private HashSet<(int, int)> ApplyEnhancementAlgorithm(HashSet<(int, int)> oldPoints, int iteration)
        {
            // if iteration is even, oldPoints are light and newPoints will be dark, else vice-versa
            var newPoints = new HashSet<(int,int)>();
            var newTracking = iteration % 2 == 0 ? '.' : '#';

            // expand grid of points to examine by 1 to include all points that have neighbours of different lightness
            var minX = oldPoints.Min(p => p.Item1) - 1;
            var maxX = oldPoints.Max(p => p.Item1) + 1;
            var minY = oldPoints.Min(p => p.Item2) - 1;
            var maxY = oldPoints.Max(p => p.Item2) + 1;

            for (var x = minX; x <= maxX; x++)
                for (var y = minY; y <= maxY; y++)
                {
                    if (NextValue((x, y), oldPoints, iteration) == newTracking)
                        newPoints.Add((x,y));
                }
                
            return newPoints;
        }

        private char NextValue((int, int) p, HashSet<(int, int)> trackedPoints, int iteration)
        {
            var binaryStr = "";
            foreach (var pixel in GetSquarePixels(p))
            {
                var oldValue = GetValue(pixel, trackedPoints, iteration);
                if (oldValue == '#')
                    binaryStr += '1';
                else
                    binaryStr += '0';
            }
            var index = Convert.ToInt32(binaryStr, 2);
            return enhancement[index];
        }

        private char GetValue((int, int) pixel, HashSet<(int, int)> trackedPoints, int iteration)
        {
            // if iteration is even, trackedPoints are ### else they are ...
            var values = new char[2] {'#', '.'};
            var tracked = trackedPoints.Contains(pixel);
            if (tracked)
                return values[iteration % 2];
            else
                return values[(iteration + 1) % 2];
        }

        private HashSet<(int,int)> InitLightPointsFromInput()
        {
            var set = new HashSet<(int,int)>();
            for (var y = 0; y < inputImage.Length; y++)
                for (var x = 0; x < inputImage[0].Length; x++)
                {
                    if (inputImage[y][x] == '#')
                        set.Add((x,y));
                }
            return set;
        }

        private IEnumerable<(int, int)> GetSquarePixels((int, int) current)
        {
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                    yield return (current.Item1 + x, current.Item2 + y);
        }

    }
}