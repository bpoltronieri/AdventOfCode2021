using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day13Utils;

namespace AoC2021.Days
{
    public class Day13 : IDay
    {
        private char[,] startPaper;
        private List<Tuple<char, int>> folds;

        public Day13(string file)
        {
            folds = new List<Tuple<char, int>>();
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var str_input = File.ReadAllLines(file);
         
            // parse input into dots set and folds list:
            var startDots = new HashSet<Tuple<int,int>>();
            var i = 0;
            var line = str_input[i];
            while (line != "")
            {
                var XY = line.Split(',').Select(v => int.Parse(v)).ToList();
                startDots.Add(new Tuple<int, int>(XY[0], XY[1]));
                i += 1;
                line = str_input[i];
            }
            i += 1;
            while (i < str_input.Length)
            {
                var fold = str_input[i].Split('=');
                var xOry = fold[0][11];
                var index = int.Parse(fold[1]);
                folds.Add(new Tuple<char, int>(xOry, index));
                i += 1;
            }

            // convert dots set into paper 2D array
            // note there might be rows/cols beyond the last dot
            // so we have to check the fold positions to figure out 
            // the true size of the paper
            var maxDotX = startDots.Select(D => D.Item1).Max();
            var maxFoldX = folds.Where(f => f.Item1 == 'x')
                                .Max(f => f.Item2);
            var width = Math.Max(maxDotX + 1, 2*maxFoldX + 1);
            
            var maxDotY = startDots.Select(D => D.Item2).Max();
            var maxFoldY = folds.Where(f => f.Item1 == 'y')
                                .Max(f => f.Item2);
            var height = Math.Max(maxDotY + 1, 2*maxFoldY + 1);

            startPaper = new char[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (startDots.Contains(new Tuple<int, int>(x, y)))
                        startPaper[x,y] = '#';
                    else
                        startPaper[x,y] = '.';
                }
        }

        public string PartOne()
        {
            var paper = new TransparentPaper(startPaper);
            var firstFold = folds[0];

            if (firstFold.Item1 == 'x')
                paper.FoldLeft(firstFold.Item2);
            else
                paper.FoldUp(firstFold.Item2);

            return paper.CountDots().ToString();
        }

        public string PartTwo()
        {
            var paper = new TransparentPaper(startPaper);
            foreach (var fold in folds)
            {
                if (fold.Item1 == 'x')
                    paper.FoldLeft(fold.Item2);
                else
                    paper.FoldUp(fold.Item2);
            }
            paper.Draw();
            return paper.CountDots().ToString(); // answer is actually the drawing above, but return number of dots for the unit test
        }
    }
}