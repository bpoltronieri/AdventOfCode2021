using System;

namespace AoC2021.Days.Day15Utils
{
    struct RiskyCavePath : IComparable<RiskyCavePath>
    {
        public int Risk { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public RiskyCavePath(int risk, int x, int y)
        {
            Risk = risk;
            X = x;
            Y = y;
        }

        public int CompareTo(RiskyCavePath a)
        {
            return this.Risk - a.Risk;
        }
    }
}