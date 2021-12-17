using System;
using System.IO;

namespace AoC2021.Days
{
    public class Day17 : IDay
    {
        private int tx0; // target low x
        private int tx1; // target high x
        private int ty0; // target low y

        private int ty1; // target high y

        public Day17(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var strInput = File.ReadAllLines(file)[0];
            var data = strInput.Substring(13).Split(", ");
            var X = data[0].Substring(2).Split("..");
            var Y = data[1].Substring(2).Split("..");
            tx0 = int.Parse(X[0]);
            tx1 = int.Parse(X[1]);
            ty0 = int.Parse(Y[0]);
            ty1 = int.Parse(Y[1]);
        }

        public string PartOne()
        {
            // by symmetry, all projectiles shot upwards will come back to y=0 at after 
            // (2vy0 + 1) steps, where vy0 is the starting Y velocity. Then we want the 
            // largest vy0 such that we won't step right over the target in the next step.
            // Y velocity at that point is vy0 - (2vy0 + 1) = -vy0 - 1, so we want
            // -vy0 - 1 = ty0, so vy0 = -1 - ty0
            var answer = MaxDist(-1 - ty0);
            return answer.ToString();
        }

        private int MaxDist(int v)
        {
            // returns max distant reached in a direction given the 
            // starting velocity in that direction. simple geometric sum
            return v*(v+1)/2;
        }

        public string PartTwo()
        {
            var vx0 = FindMinVX();
            var vy0 = ty0;

            var nPaths = 0;
            for (var vx = vx0; vx <= tx1; vx++)
                for (var vy = vy0; vy <= -1 - ty0; vy++)
                {
                    if (PathCrossesTarget(vx, vy))
                        nPaths += 1;
                }
            return nPaths.ToString();
        }

        private bool PathCrossesTarget(int vx0, int vy0)
        {
            // checks if we cross target at any step given starting velocity (vx0, vy0)
            // dumb simulation
            var vx = vx0;
            var vy = vy0;
            var x = 0;
            var y = 0;
            while (x <= tx1 && y >= ty0)
            {
                if (OnTarget(x, y)) 
                    return true;
                x += vx;
                y += vy;
                vx = Math.Max(0, vx - 1);
                vy -= 1;
            }
            return false;
        }

        private bool OnTarget(int x, int y)
        {
            return x >= tx0 && x <= tx1 && y >= ty0 && y <= ty1;
        }

        private int FindMinVX()
        {
            // finds smallest starting X velocity that won't lose speed before reaching the target 
            // could do a binary chop or something but values are pretty small here
            var x = 1;
            while (MaxDist(x) < tx0) 
                x+= 1;
            return x;
        }
    }
}