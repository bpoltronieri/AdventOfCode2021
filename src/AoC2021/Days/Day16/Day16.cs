using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2021.Days
{
    public class Day16 : IDay
    {
        private string input;

        private int versionSum;

        public Day16(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var hex_input = File.ReadAllLines(file)[0];
            var builder = new StringBuilder(hex_input.Length * 4);
            foreach (var c in hex_input)
                builder.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
            input = builder.ToString();
        }

        public string PartOne()
        {
            versionSum = 0;
            var i = 0;
            ProcessPacket(ref i);
            return versionSum.ToString();
        }

        public string PartTwo()
        {
            var i = 0;
            var answer = ProcessPacket(ref i);
            return answer.ToString();
        }

        private long ProcessPacket(ref int index)
        {
            var header = input.Substring(index, 6);
            var version = Convert.ToInt32(header.Substring(0, 3), 2);
            versionSum += version;
            var typeID = Convert.ToInt32(header.Substring(3,3), 2);
            index += 6;

            switch (typeID)
            {
                case 4: // literal value
                    var literalBuilder = new StringBuilder();
                    var done = false;
                    while (!done)
                    {
                        done = input[index] == '0';
                        var bits = input.Substring(index + 1, 4);
                        literalBuilder.Append(bits);
                        index += 5;
                    }
                    return Convert.ToInt64(literalBuilder.ToString(), 2);
                default: // operator value
                    var lengthTypeID = input[index++];
                    var values = new List<long>();
                    if (lengthTypeID == '0')
                    {
                        var subPacketsLength = Convert.ToInt32(input.Substring(index, 15), 2);
                        index += 15;
                        var subIndex = index;
                        while (subIndex < index + subPacketsLength)
                            values.Add(ProcessPacket(ref subIndex));
                        index = subIndex;
                    }
                    else if (lengthTypeID == '1')
                    {
                        var nSubPackets = Convert.ToInt32(input.Substring(index, 11), 2);
                        index += 11;
                        for (var i = 0; i < nSubPackets; i++)
                            values.Add(ProcessPacket(ref index));
                    }
                    else
                        throw new InvalidDataException();
                    return ApplyOperator(typeID, values);
            }
        }

        private long ApplyOperator(int typeID, List<long> values)
        {
            switch (typeID)
            {
                case 0:
                    return values.Sum();
                case 1:
                    long product = 1;
                    foreach (var v in values)
                        product *= v;
                    return product;
                case 2:
                    return values.Min();
                case 3:
                    return values.Max();
                case 5:
                    return values[0] > values[1] ? 1 : 0;
                case 6:
                    return values[0] < values[1] ? 1 : 0;
                case 7:
                    return values[0] == values[1] ? 1 : 0;
                default:
                    throw new InvalidDataException();
            }
        }
    }
}