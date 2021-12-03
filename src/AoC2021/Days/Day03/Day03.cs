using System;
using System.IO;
using System.Linq;

namespace AoC2021.Days
{
    public class Day03 : IDay
    {
        private string[] input;

        public Day03(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        public string PartOne()
        {
            var gamma = "";
            var epsilon = "";

            var n_bits = input[0].Count();
            for (var i = 0; i < n_bits; i++)
            {
                var ordered = input.GroupBy(x => x[i])
                                 .OrderByDescending(g => g.Count())
                                 .Select(g => g.Key);
                var g_bit = ordered.First(); // most common bit value
                var e_bit = ordered.Last(); // least common bit value

                gamma += g_bit;
                epsilon += e_bit;
            }

            var gamma_val = Convert.ToInt32(gamma, 2);
            var epsilon_val = Convert.ToInt32(epsilon, 2);
            return (gamma_val * epsilon_val).ToString();
        }

        public string PartTwo()
        {
            var scrubber_filtered = input.ToList();
            var oxygen_filtered = input.ToList();

            var n_bits = input[0].Count();
            for (var i = 0; i < n_bits && (scrubber_filtered.Count() > 1 || oxygen_filtered.Count() > 1); i++)
            {
                if (scrubber_filtered.Count() > 1)
                {
                    var least_bit = '0';
                    var count_0 = scrubber_filtered.Count(x => x[i] == '0');
                    if (2 * count_0 > scrubber_filtered.Count())
                        least_bit = '1'; 

                    scrubber_filtered = scrubber_filtered.Where(x => x[i] == least_bit).ToList();
                }
                if (oxygen_filtered.Count() > 1)
                {
                    var most_bit = '1';
                    var count_1 = oxygen_filtered.Count(x => x[i] == '1');
                    if (2 * count_1 < oxygen_filtered.Count())
                        most_bit = '0'; 

                    oxygen_filtered = oxygen_filtered.Where(x => x[i] == most_bit).ToList();
                }
            }

            var scrubber_val = Convert.ToInt32(scrubber_filtered.First(), 2);
            var oxygen_val = Convert.ToInt32(oxygen_filtered.First(), 2);
            return (scrubber_val * oxygen_val).ToString();
        }
    }
}