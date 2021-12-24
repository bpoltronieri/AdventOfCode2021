using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day24Utils;

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
            // solved this one by hand by examining the input code
            var highest = 99893999291967;

            if (!ValidModelNumber(highest))
                throw new InvalidDataException();

            return highest.ToString();
        }

        public string PartTwo()
        {
            // solved this one by hand by examining the input code
            var lowest = 34171911181211;

            if (!ValidModelNumber(lowest))
                throw new InvalidDataException();

            return lowest.ToString();
        }

        private bool ValidModelNumber(long modelNumber)
        {
            var inputs = modelNumber.ToString().Select(c => long.Parse(c.ToString())).ToList();
            if (inputs.Contains(0)) return false;

            var alu = new ALU(inputs);
            foreach (var instruction in input)
            {
                if (!alu.RunInstruction(instruction))
                    return false; // crashed
            }
            if (alu.z != DecompiledALU(inputs))
                throw new InvalidProgramException();
            
            return alu.z == 0;
        }

        private long DecompiledALU(List<long> digits)
        {
            long z = 0;

            var xAdds = new int[14] {12, 11, 10, 10, -16, 14, 12, -4, 15, -7, -8, -4, -15, -8};
            var yAdds = new int[14] {6,  12,  5, 10,   7,  0,  4, 12, 14, 13, 10, 11,   9,  9};

            for (var i = 0; i < 14; i++)
            {
                var increaseZ = z % 26 + xAdds[i] != digits[i];

                if (i == 4 || i == 7 || i >= 9) // happens 7 times
                    z = z / 26;

                if (increaseZ) // so should only let this happen 7 times
                    z = 26*z + digits[i] + yAdds[i];
            }

            return z;
        }


    }
}