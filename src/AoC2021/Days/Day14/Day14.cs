using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day14 : IDay
    {
        private string startPolymer;
        private Dictionary<string,char> insertions;

        public Day14(string file)
        {
            insertions = new Dictionary<string, char>();
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            startPolymer = input[0];
            for (var i = 2; i < input.Length; i++)
            {
                var insertion = input[i].Split(" -> ");
                insertions[insertion[0]] = insertion[1][0];
            }
        }

        public string PartOne()
        {
            // generate full polymer string for part one:
            var polymer = startPolymer.ToList();
            for (var step = 0; step < 10; step++)
            {
                var currentPolymer = new string(polymer.ToArray());
                for (var i = currentPolymer.Length - 2;  i >= 0; i--) // work backwards for easier insertion
                {
                    var pair = currentPolymer.Substring(i, 2);
                    if (insertions.ContainsKey(pair)) // think this is always true?
                        polymer.Insert(i+1, insertions[pair]);
                }
            }
            var counts = polymer.GroupBy(c => c).Select(g => g.Count());
            var answer = counts.Max() - counts.Min();
            return answer.ToString();
        }

        public string PartTwo()
        {
            // part two just keeps track of the number of occurences of different letters and different pairs:
            var letterOccs = InitLetterOccurences();
            var pairOccs = InitPairOccurences();
            for (var step = 0; step < 40; step++)
            {
                var pairChanges = new Dictionary<string, long>();
                foreach (var pair in pairOccs.Keys)
                {
                    var nOccs = pairOccs[pair];
                    var insert = insertions[pair];
                    AddToCount(insert, letterOccs, nOccs);
                    AddToCount(pair, pairChanges, -nOccs);
                    var pair1 = pair[0] + insert.ToString();
                    AddToCount(pair1, pairChanges, nOccs);
                    var pair2 = insert.ToString() + pair[1];
                    AddToCount(pair2, pairChanges, nOccs);
                }
                // apply pair changes:
                foreach (var pair in pairChanges.Keys)
                    AddToCount(pair, pairOccs, pairChanges[pair]);
            }
            var answer = letterOccs.Values.Max() - letterOccs.Values.Min();
            return answer.ToString();
        }

        private Dictionary<string, long> InitPairOccurences()
        {
            var pairOccs = new Dictionary<string, long>();
            for (int i = 0; i < startPolymer.Length - 1; i++)
            {
                var pair = startPolymer.Substring(i, 2);
                AddToCount(pair, pairOccs, 1);
            }
            return pairOccs;
        }

        private Dictionary<char,long> InitLetterOccurences()
        {
            var letterOccs = new Dictionary<char, long>();
            foreach (var c in startPolymer)
                AddToCount(c, letterOccs, 1);
            return letterOccs;
        }

        private void AddToCount<T>(T key, Dictionary<T, long> counts, long toAdd)
        {
            if (counts.ContainsKey(key))
                counts[key] += toAdd;
            else
                counts[key] = toAdd;
        }
    }
}