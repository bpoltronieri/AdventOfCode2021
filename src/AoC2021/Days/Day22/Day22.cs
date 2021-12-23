using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day22Utils;

namespace AoC2021.Days
{
    public class Day22 : IDay
    {
        private List<Instruction> input;

        public Day22(string file)
        {
            input = new List<Instruction>();
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var strInput = File.ReadAllLines(file);
            foreach (var line in strInput)
            {
                var on = line.Substring(0, 2) == "on";
                var intervals = line.Substring(on ? 3 : 4)
                                    .Split(',')
                                    .Select(s => s.Substring(2)
                                                  .Split("..")
                                                  .Select(s => int.Parse(s))
                                                  .ToArray() 
                                           )
                                    .ToArray();
                var volume = new Cuboid(intervals[0][0], intervals[0][1],
                                        intervals[1][0], intervals[1][1],
                                        intervals[2][0], intervals[2][1]);
                input.Add(new Instruction(on, volume));
            }
        }

        public string PartOne()
        {
            // brute-force for part one.
            // for each point in the initialisation procedure area,
            // go through instructions in reverse order to see the last time
            // that cube was switched on or off
            var nInstructions = input.Count();
            var nOnCubes = 0;
            for (var x = -50; x <= 50; x++)
                for (var y = -50; y <= 50; y++)
                    for (var z = -50; z <= 50; z++)
                    {
                        for (var i = 0; i < nInstructions; i++)
                        {
                            var instruction = input[nInstructions - 1 - i];
                            if (instruction.Region.ContainsPoint((x, y, z)))
                            {
                                if (instruction.On) nOnCubes++;
                                break;
                            }
                        }    
                    }
            return nOnCubes.ToString();
        }

        public string PartTwo()
        {
            var onCubes = new List<Cuboid>();
            // var (i, total) = (1, input.Count());
            foreach (var instruction in input)
            {
                if (instruction.On)
                    Unite(instruction.Region, onCubes);
                else
                    Subtract(instruction.Region, ref onCubes);
                // Console.WriteLine("Done " + i++ + " / " + total + " instructions.");
            }
            return onCubes.Sum(c => c.Volume()).ToString();
        }

        private void Unite(Cuboid newCuboid, List<Cuboid> cuboids)
        {
            // unites the given cuboid newCuboid with the cuboids in the given 
            // list, which are assumed to already be disjoint amongst themselves
            // the resulting list of cuboids should also be disjoint

            // first pass to get rid of any existing cuboids fully inside newCuboid
            cuboids.RemoveAll(c => newCuboid.Contains(c));

            // may need to split up newCuboid into sub-cuboids after removing overlapping regions
            var splitNewCuboid = new List<Cuboid>();
            splitNewCuboid.Add(newCuboid);
            foreach (var oldCuboid in cuboids)
            {
                var newSplits = new List<Cuboid>();
                foreach (var cuboid in splitNewCuboid)
                {
                    var subCuboids = cuboid.Subtract(oldCuboid);
                    if (subCuboids != null)
                        newSplits.AddRange(subCuboids);
                }
                splitNewCuboid = newSplits;
            }
            cuboids.AddRange(splitNewCuboid);
        }

        private void Subtract(Cuboid negativeCuboid, ref List<Cuboid> cuboids)
        {
            // subtracts the given cuboid negativeCuboid from the cuboids in the given list
            if (cuboids.Count() == 0) return;

            var subCuboids = new List<Cuboid>();
            foreach (var oldCuboid in cuboids)
            {
                List<Cuboid> splitCuboid = oldCuboid.Subtract(negativeCuboid);
                if (splitCuboid != null)
                    subCuboids.AddRange(splitCuboid);
            }
            cuboids = subCuboids;
        }

    }
}