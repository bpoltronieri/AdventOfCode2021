using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day08 : IDay
    {
        private string[] input;

        public Day08(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var nEasyDigits = 0; // number of times 1, 4, 7, or 8 appear in output values
            foreach (var line in input)
            {
                var outputDigits = line.Split(" | ")[1].Split(' ');
                nEasyDigits += outputDigits.Count(digit => digit.Length == 2 || digit.Length == 4 || digit.Length == 3 || digit.Length == 7);
            }
            return nEasyDigits.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;
            foreach (var line in input)
            {
                var intputsAndOutput = line.Split(" | ");
                var uniquePatterns = intputsAndOutput[0].Split(' ');
                var outputDigits = intputsAndOutput[1].Split(' ');
                answer += OutputValue(outputDigits, uniquePatterns);
            }
            return answer.ToString();
        }

        private int OutputValue(string[] outputDigits, string[] uniquePatterns)
        {
            var segWirePossibilities = new Dictionary<char, HashSet<char>>();
            // initialise map from segment to possible wires:
            for (var i = 0; i < 7; i++)
            {
                var current_char = (char)((int)'a' + i);
                segWirePossibilities[current_char] = new HashSet<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'};
            }

            // useful sets:
            var twoThreeFiveIntersect = uniquePatterns
                .Where(p => p.Length == 5)
                .Select(p => p.ToCharArray().ToHashSet())
                .Aggregate((a,b) => a.Intersect(b).ToHashSet());

            var zeroSixNineIntersect = uniquePatterns
                .Where(p => p.Length == 6)
                .Select(p => p.ToCharArray().ToHashSet())
                .Aggregate((a,b) => a.Intersect(b).ToHashSet());

            var oneChars = uniquePatterns
                .Where(p => p.Length == 2)
                .First()
                .ToCharArray()
                .ToHashSet();

            var sevenChars = uniquePatterns
                .Where(p => p.Length == 3)
                .First()
                .ToCharArray()
                .ToHashSet();
            
            // d is shared by 2,3,5 but not by 0,6,9:
            segWirePossibilities['d'].IntersectWith(twoThreeFiveIntersect);
            segWirePossibilities['d'].ExceptWith(zeroSixNineIntersect);
            if (segWirePossibilities['d'].Count != 1)
                throw new InvalidDataException();
            
            // f is shared by 0,6,9, but not by 2,3,5 and is in 1
            segWirePossibilities['f'].IntersectWith(zeroSixNineIntersect);
            segWirePossibilities['f'].ExceptWith(twoThreeFiveIntersect);
            segWirePossibilities['f'].IntersectWith(oneChars);
            if (segWirePossibilities['f'].Count != 1)
                throw new InvalidDataException();

            // c is in 1 but isn't f
            segWirePossibilities['c'].IntersectWith(oneChars);
            segWirePossibilities['c'].ExceptWith(segWirePossibilities['f']);
            if (segWirePossibilities['c'].Count != 1)
                throw new InvalidDataException();

            // b is in 0,6,9 but not 2,3,5 and isn't f
            segWirePossibilities['b'].IntersectWith(zeroSixNineIntersect);
            segWirePossibilities['b'].ExceptWith(twoThreeFiveIntersect);
            segWirePossibilities['b'].ExceptWith(segWirePossibilities['f']);
            if (segWirePossibilities['b'].Count != 1)
                throw new InvalidDataException();

            // a is in 2,3,5,0,6,9 and 7
            segWirePossibilities['a'].IntersectWith(twoThreeFiveIntersect);
            segWirePossibilities['a'].IntersectWith(zeroSixNineIntersect);
            segWirePossibilities['a'].IntersectWith(sevenChars);
            if (segWirePossibilities['a'].Count != 1)
                throw new InvalidDataException();

            // g is in 2,3,5,0,6,9 and isn't a
            segWirePossibilities['g'].IntersectWith(twoThreeFiveIntersect);
            segWirePossibilities['g'].IntersectWith(zeroSixNineIntersect);
            segWirePossibilities['g'].ExceptWith(segWirePossibilities['a']);
            if (segWirePossibilities['g'].Count != 1)
                throw new InvalidDataException();

            // e is the last remaining one:
            foreach (var wire in segWirePossibilities.Keys.Where(k => k != 'e'))
                segWirePossibilities['e'].ExceptWith(segWirePossibilities[wire]);
            if (segWirePossibilities['e'].Count != 1)
                throw new InvalidDataException();

            if (!Solved(segWirePossibilities))
                throw new InvalidDataException();

            // find set of wires for each digit 0-9
            var digitSegments = InitDigitSegmentsMap(); // map from digit to segments
            var digitWires = new Dictionary<int, HashSet<char>>();
            foreach (var digit in digitSegments.Keys)
            {
                var wires = new HashSet<char>();
                foreach (var seg in digitSegments[digit])
                    wires.Add(segWirePossibilities[seg].First());
                digitWires[digit] = wires;
            }

            // translate output digits:
            var value_s = "";
            foreach (var d in outputDigits)
            {
                var dChars = d.ToCharArray().ToHashSet();
                var digit = digitWires.Keys.Where(k => dChars.SetEquals(digitWires[k])).First();
                value_s += digit.ToString();
            }

            return int.Parse(value_s);
        }

        private Dictionary<int, HashSet<char>> InitDigitSegmentsMap()
        {
            // fills in dictionary mapping digits 0-9 to hashset of the segments needed
            var map = new Dictionary<int, HashSet<char>>();
            map[0] = (new char[] {'a', 'b', 'c', 'e', 'f', 'g'}).ToHashSet();
            map[1] = (new char[] {'c', 'f'}).ToHashSet();
            map[2] = (new char[] {'a', 'c', 'd', 'e', 'g'}).ToHashSet();
            map[3] = (new char[] {'a', 'c', 'd', 'f', 'g'}).ToHashSet();
            map[4] = (new char[] {'b', 'c', 'd', 'f'}).ToHashSet();
            map[5] = (new char[] {'a', 'b', 'd', 'f', 'g'}).ToHashSet();
            map[6] = (new char[] {'a', 'b', 'd', 'e', 'f', 'g'}).ToHashSet();
            map[7] = (new char[] {'a', 'c', 'f'}).ToHashSet();
            map[8] = (new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g'}).ToHashSet();
            map[9] = (new char[] {'a', 'b', 'c', 'd', 'f', 'g'}).ToHashSet();
            return map;
        }

        private bool Solved(Dictionary<char, HashSet<char>> wireSegmentPossibilities)
        {
            return wireSegmentPossibilities.Values.All(p => p.Count == 1)
                    && wireSegmentPossibilities.Values.Distinct().Count() == wireSegmentPossibilities.Values.Count();
        }
    }
}