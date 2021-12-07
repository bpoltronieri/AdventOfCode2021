using System;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day07 : IDay
    {
        private string[] input;

        public Day07(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        private int FuelCostConstant(int[] crabPositions, int targetPosition)
        {
            var cost = 0;
            foreach (var crab in crabPositions)
                cost += Math.Abs(crab - targetPosition);
            return cost;
        }

        public string PartOne()
        {
            var crabPositions = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
            Array.Sort(crabPositions);
            // median position minimises the sum of absolute deviations
            // see https://math.stackexchange.com/questions/113270/the-median-minimizes-the-sum-of-absolute-deviations-the-ell-1-norm
            var bestPosition = crabPositions[crabPositions.Length/2];
            return FuelCostConstant(crabPositions, bestPosition).ToString();
        }

        private int FuelCostGeometric(int[] crabPositions, int targetPosition)
        {
            var cost = 0;
            foreach (var crab in crabPositions)
            {
                var distance = Math.Abs(crab - targetPosition);
                cost += (distance * (distance + 1))/2;
            }
            return cost;
        }

        public string PartTwo()
        {
            var crabPositions = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
            // average position minimises the sum of squared distances
            // which isn't quite right here as we want to minimise sum(dist * (dist+1) / 2)
            // but it should be very close to the average and the below worked for my input
            var bestPositionHigh = (int)Math.Ceiling(crabPositions.Average());
            var bestPositionLow = (int)Math.Floor(crabPositions.Average());
            var costHigh = FuelCostGeometric(crabPositions, bestPositionHigh);
            var costLow = FuelCostGeometric(crabPositions, bestPositionLow);
            return Math.Min(costHigh, costLow).ToString();
        }
    }
}