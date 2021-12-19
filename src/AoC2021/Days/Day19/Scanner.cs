using System;
using System.Collections.Generic;

namespace AoC2021.Days.Day19Utils
{
    class Scanner
    {
        // reasonably happy with this solution but it needs a better data structure for a 3D integer vector
        // I used 3-tuples but had to write functions for adding and multiplying them by scalars.
        public ValueTuple<int,int,int> GlobalPosition; // relative to scanner 0
        public List<ValueTuple<int,int,int>> Beacons; // positions relative to this scanner
        private ValueTuple<int,int,int> Forward; // unit vector in +-x, +-y, or +-z direction
        private ValueTuple<int,int,int> Up; // unit vector in +-x, +-y, or +-z direction
        private int RotatedUp = 0; // to keep track of how many times we've rotated the Up vector
        
        public Scanner()
        {
            GlobalPosition = new ValueTuple<int,int,int>(0, 0, 0);
            Beacons = new List<(int, int, int)>();
            Forward = new ValueTuple<int,int,int>(1,0,0);
            Up = new ValueTuple<int,int,int>(0,0,1);
        }

        public void AddBeacon(int x, int y, int z)
        {
            Beacons.Add((x, y, z));
        }

        public IEnumerable<ValueTuple<int, int, int>> BeaconGlobalPositions()
        {
            // returns beacons in global position, i.e. relative to scanner 0
            foreach (var beacon in Beacons)
            {
                var adjustedBeacon = ApplyOrientationToBeacon(beacon);
                yield return AddPoints(adjustedBeacon, GlobalPosition);
            }
        }
        private ValueTuple<int,int,int> ApplyOrientationToBeacon((int, int, int) beacon)
        {
            // applies current orientation to beacon position
            var x = MultiplyDirn(Forward, beacon.Item1);
            var y = MultiplyDirn(CrossProduct(Up, Forward), beacon.Item2);
            var z = MultiplyDirn(Up, beacon.Item3);
            return AddPoints(x, AddPoints(y, z));
        }

        public void RotateOrientation()
        {
            if (RotatedUp == 3)
            {
                RotateForward();
                RotatedUp = 0;
            }
            else // rotate Up
            {
                Up = CrossProduct(Forward, Up);
                RotatedUp += 1;
            }
        }

        private void RotateForward()
        {
            if (PositiveDirn(Forward))
                Forward = MultiplyDirn(Forward, -1);
            else if (Forward.Item1 != 0) // rotate from -x to y
            {
                Forward = new ValueTuple<int,int,int>(0, 1, 0);
                Up = new ValueTuple<int,int,int>(0,0,1);
            }
            else if (Forward.Item2 != 0) // rotate from -y to z
            {
                Forward = new ValueTuple<int,int,int>(0, 0, 1);
                Up = new ValueTuple<int,int,int>(1,0,0);
            }
            else if (Forward.Item3 != 0) // rotate from -z yo x
            {
                Forward = new ValueTuple<int,int,int>(1, 0, 0);
                Up = new ValueTuple<int,int,int>(0,0,1);
            }
        }

        internal void MoveBeaconTo(int i, (int, int, int) newPosition)
        {
            // moves the i'th beacon to given global positon by adjusting the scanner's position
            // of course that also moves all other beacons but that's the intention
            var adjustedBeacon = ApplyOrientationToBeacon(Beacons[i]);
            GlobalPosition = AddPoints(newPosition, MultiplyDirn(adjustedBeacon, -1));
        }

        // tuple operations:
        private ValueTuple<int, int, int> CrossProduct((int, int, int) v1, (int, int, int) v2)
        {
            var (x1, y1, z1) = (v1.Item1, v1.Item2, v1.Item3);
            var (x2, y2, z2) = (v2.Item1, v2.Item2, v2.Item3);

            var x3 = y1*z2 - z1*y2;
            var y3 = z1*x2 - x1*z2;
            var z3 = x1*y2 - y1*x2;
            return new ValueTuple<int,int,int>(x3, y3, z3);
        }

        private (int, int, int) AddPoints((int, int, int) p1, (int, int, int) p2)
        {
            return (p1.Item1 + p2.Item1, p1.Item2 + p2.Item2, p1.Item3 + p2.Item3);
        }

        private (int, int, int) MultiplyDirn((int, int, int) dirn, int factor)
        {
            return (dirn.Item1 * factor, dirn.Item2 * factor, dirn.Item3 * factor);
        }

        private bool PositiveDirn((int, int, int) dirn)
        {
            return dirn.Item1 + dirn.Item2 + dirn.Item3 > 0;
        }
    }
}