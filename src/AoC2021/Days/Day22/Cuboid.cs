using System;
using System.Collections.Generic;

namespace AoC2021.Days.Day22Utils
{
    struct Cuboid
    {
        public (int,int) xInterval;
        public (int,int) yInterval;
        public (int,int) zInterval;

        public Cuboid(int xLow, int xHigh, int yLow, int yHigh, int zLow, int zHigh)
        {
            xInterval = (xLow, xHigh);
            yInterval = (yLow, yHigh);
            zInterval = (zLow, zHigh);
        }

        public long Volume()
        {
            return (long)(xInterval.Item2 - xInterval.Item1 + 1)
                    * (long)(yInterval.Item2 - yInterval.Item1 + 1)
                    * (long)(zInterval.Item2 - zInterval.Item1 + 1);
        }

        public bool ContainsPoint((int,int,int) point)
        {
            return InInterval(point.Item1, xInterval)
                && InInterval(point.Item2, yInterval)
                && InInterval(point.Item3, zInterval);
        }

        public bool Contains(Cuboid other)
        {
            return IntervalInInterval(other.xInterval, xInterval)
                && IntervalInInterval(other.yInterval, yInterval)
                && IntervalInInterval(other.zInterval, zInterval);
        }

        public List<Cuboid> Subtract(Cuboid negativeCuboid)
        {
            // subtracts the given cuboid from the current one.
            // Can require splitting the current one into sub-cuboids.
            if (negativeCuboid.Contains(this))
                return null;

            var subCuboids = new List<Cuboid>();
            subCuboids.Add(this);

            if (negativeCuboid.IntersectWith(this) == null)
                return subCuboids;

            // slice up current cuboid by all the planes of negativeCuboid
            // could optimise by reducing max number of sub-cuboids to 6, currently 27
            if (InInterval(negativeCuboid.xInterval.Item1, xInterval))
                SliceByPlane(ref subCuboids, 'x', negativeCuboid.xInterval.Item1);
            if (InInterval(negativeCuboid.xInterval.Item2, xInterval))
                SliceByPlane(ref subCuboids, 'x', negativeCuboid.xInterval.Item2 + 1);
            if (InInterval(negativeCuboid.yInterval.Item1, yInterval))
                SliceByPlane(ref subCuboids, 'y', negativeCuboid.yInterval.Item1);
            if (InInterval(negativeCuboid.yInterval.Item2, yInterval))
                SliceByPlane(ref subCuboids, 'y', negativeCuboid.yInterval.Item2 + 1);
            if (InInterval(negativeCuboid.zInterval.Item1, zInterval))
                SliceByPlane(ref subCuboids, 'z', negativeCuboid.zInterval.Item1);
            if (InInterval(negativeCuboid.zInterval.Item2, zInterval))
                SliceByPlane(ref subCuboids, 'z', negativeCuboid.zInterval.Item2 + 1);

            subCuboids.RemoveAll(c => negativeCuboid.Contains(c));
            return subCuboids;
        }

        private void SliceByPlane(ref List<Cuboid> cuboids, char axis, int value)
        {
            // slices the given cuboids by the plane with the given axis as its normal
            // positioned at the start  of the cube at the given value
            var subCuboids = new List<Cuboid>();
            foreach (var cuboid in cuboids)
            {
                var interval = cuboid.GetInterval(axis);
                if (!StrictlyInInterval(value, (interval.Item1, interval.Item2 + 1)))
                {
                    subCuboids.Add(cuboid);
                    continue;
                }

                var sub1 = cuboid;
                sub1.SetInterval((interval.Item1, value - 1), axis);
                subCuboids.Add(sub1);
                var sub2 = cuboid;
                sub2.SetInterval((value, interval.Item2), axis);
                subCuboids.Add(sub2);
            }
            cuboids = subCuboids;
        }

        private (int,int) GetInterval(char axis)
        {
            switch (axis)
            {
                case 'x':
                    return xInterval;
                case 'y':
                    return yInterval;
                case 'z':
                    return zInterval;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void SetInterval((int,int) interval, char axis)
        {
            switch (axis)
            {
                case 'x':
                    xInterval = interval;
                    break;
                case 'y':
                    yInterval = interval;
                    break;
                case 'z':
                    zInterval = interval;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private Cuboid? IntersectWith(Cuboid other)
        {
            var xLow = Math.Max(this.xInterval.Item1, other.xInterval.Item1);
            var xHigh = Math.Min(this.xInterval.Item2, other.xInterval.Item2);
            if (xLow > xHigh) return null;

            var yLow = Math.Max(this.yInterval.Item1, other.yInterval.Item1);
            var yHigh = Math.Min(this.yInterval.Item2, other.yInterval.Item2);
            if (yLow > yHigh) return null;

            var zLow = Math.Max(this.zInterval.Item1, other.zInterval.Item1);
            var zHigh = Math.Min(this.zInterval.Item2, other.zInterval.Item2);
            if (zLow > zHigh) return null;

            return new Cuboid(xLow, xHigh, yLow, yHigh, zLow, zHigh);
        }

        // interval utilities
        private bool InInterval(int value, (int, int) interval)
        {
            return value >= interval.Item1 && value <= interval.Item2;
        }
        private bool StrictlyInInterval(int value, (int, int) interval)
        {
            return value > interval.Item1 && value < interval.Item2;
        }
        private bool IntervalInInterval((int, int) interval1, (int, int) interval2)
        {
            // whether interval1 is contained within interval2
            return interval1.Item1 >= interval2.Item1
                && interval1.Item2 <= interval2.Item2;
        }
    }
}