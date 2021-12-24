using System;
using System.Collections.Generic;

namespace AoC2021.Days.Day24Utils
{
    public class ALU
    {
        private long w = 0;
        private long x = 0;
        private long y = 0;
        public long z = 0;
        private Queue<long> inputBuffer;

        public ALU(List<long> inputs)
        {
            inputBuffer = new Queue<long>(inputs);
        }

        public bool RunInstruction(string instruction)
        {
            var split = instruction.Split(' ');
            var operation = split[0];
            var variable = split[1];

            long result = -1;
            switch (operation)
            {
                case "inp":
                    if (inputBuffer.Count == 0) return true;
                    result = inputBuffer.Dequeue();
                    break;
                case "add":
                    result = GetValue(variable) + GetValue(split[2]);
                    break;
                case "mul":
                    result = GetValue(variable) * GetValue(split[2]);
                    break;
                case "div":
                    var divisor = GetValue(split[2]);
                    if (divisor == 0) return false; // crash
                    result = GetValue(variable) / divisor;
                    break;
                case "mod":
                    var value = GetValue(variable);
                    var modulo = GetValue(split[2]);
                    if (value < 0 || modulo <= 0) return false; // crash
                    result = value % modulo;
                    break;
                case "eql":
                    result = GetValue(variable) == GetValue(split[2]) ? 1 : 0;
                    break;
            }
            SetValue(variable, result);

            return true;
        }

        private long GetValue(string variable)
        {
            if (!char.IsLetter(variable[0])) // literal value
                return long.Parse(variable);

            switch (variable)
            {
                case "w": return w;
                case "x": return x;
                case "y": return y;
                case "z": return z;
                default: throw new InvalidOperationException();
            }
        }

        private void SetValue(string variable, long value)
        {
            switch (variable)
            {
                case "w": 
                    w = value;
                    break;
                case "x": 
                    x = value;
                    break;
                case "y": 
                    y = value;
                    break;
                case "z": 
                    z = value;
                    break;
                default: 
                    throw new InvalidOperationException();
            }
        }
    }
}