using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day19Utils;

namespace AoC2021.Days
{
    public class Day19 : IDay
    {
        private List<Scanner> Scanners;
        private HashSet<Tuple<Scanner, Scanner>> NoOverlapCache; // cache pairs of scanners which don't overlap to avoid repeating work

        private bool ScannersMapped = false;

        public Day19(string file)
        {
            Scanners = new List<Scanner>();
            NoOverlapCache = new HashSet<Tuple<Scanner, Scanner>>();
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            Scanner currentScanner = null;
            foreach (var line in input)
            {
                if (line.Length == 0) 
                    continue;
                else if (line.Contains("scanner"))
                {
                    currentScanner = new Scanner();
                    Scanners.Add(currentScanner);
                }
                else
                {
                    var beacon = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    currentScanner.AddBeacon(beacon[0], beacon[1], beacon[2]);
                }
            }
        }

        public string PartOne()
        {
            FixScannerPositions();
            var allBeacons = new HashSet<ValueTuple<int,int,int>>();
            foreach (var scanner in Scanners)
                allBeacons.UnionWith(scanner.BeaconGlobalPositions());
            return allBeacons.Count().ToString();
        }

        private void FixScannerPositions()
        {
            if (ScannersMapped) return; // in case Part One was already called

            var fixedScanners = new List<Scanner>();
            var unfixedScanners = Scanners.ToList();
            // 0 is fixed, all others need to be fixed relative to 0:
            fixedScanners.Add(Scanners[0]);
            unfixedScanners.Remove(Scanners[0]); 

            while (unfixedScanners.Count() > 0)
            {
                Scanner matchingScanner = null;
                foreach (var currentScanner in unfixedScanners)
                {
                    foreach (var fixedScanner in fixedScanners)
                    {
                        if (ScannersOverlap(fixedScanner, currentScanner))
                        {
                            matchingScanner = currentScanner;
                            break;
                        }
                    }
                    if (matchingScanner != null) break;
                }
                if (matchingScanner != null)
                {
                    fixedScanners.Add(matchingScanner);
                    unfixedScanners.Remove(matchingScanner);
                }
                else
                    throw new InvalidOperationException();
            }
            
            ScannersMapped = true;
        }

        private bool ScannersOverlap(Scanner fixedScanner, Scanner newScanner)
        {
            // after looking at solutions, a faster approach here would be to convert
            // each scanner's N beacons into a list of N(N-1)/2 vectors between beacons,
            // which is translation independent. In my approach I have to try different
            // positions of newScanner relative to fixedScanner.
            // as a quick test to rule out overlap, we could also use N(N-1)/2 manhattan 
            // distances between beacons, which is both translation and orientation independent

            var scannerPair = new Tuple<Scanner, Scanner>(fixedScanner, newScanner);
            if (NoOverlapCache.Contains(scannerPair))
                return false;

            var newScannerPosition = newScanner.GlobalPosition; // so we can reset it if needed
            for (int i = 0; i < 24; i++) // 24 possible orientations
            {
                for (var j = 0; j < newScanner.Beacons.Count(); j++)
                {
                    // try placing j'th beacon of newScanner on each beacon of fixedScanner and see if we get 12 or more overlaps
                    foreach (var fixedBeacon in fixedScanner.BeaconGlobalPositions())
                    {
                        newScanner.MoveBeaconTo(j, fixedBeacon);
                        var sharedBeacons = fixedScanner.BeaconGlobalPositions().ToHashSet();
                        sharedBeacons.IntersectWith(newScanner.BeaconGlobalPositions().ToHashSet());
                        if (sharedBeacons.Count() >= 12)
                            return true;
                    }
                }
                newScanner.RotateOrientation();
            }
            newScanner.GlobalPosition = newScannerPosition;
            NoOverlapCache.Add(scannerPair);
            return false;
        }

        public string PartTwo()
        {
            FixScannerPositions();
            var maxManhattanDist = int.MinValue;
            foreach (var scanner1 in Scanners)
                foreach (var scanner2 in Scanners)
                {
                    var dist = ManhattanDist(scanner1.GlobalPosition, scanner2.GlobalPosition);
                    if (dist > maxManhattanDist)
                        maxManhattanDist = dist;
                }
            return maxManhattanDist.ToString();
        }

        private int ManhattanDist((int, int, int) p1, (int, int, int) p2)
        {
            return Math.Abs(p1.Item1 - p2.Item1)
                 + Math.Abs(p1.Item2 - p2.Item2)
                 + Math.Abs(p1.Item3 - p2.Item3);
        }
    }
}