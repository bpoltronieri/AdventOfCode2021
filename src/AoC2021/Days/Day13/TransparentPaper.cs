using System;
using System.Linq;
using System.Text;

namespace AoC2021.Days.Day13Utils
{
    class TransparentPaper
    {
        private char[,] paper;
        private int width; // becomes smaller after folding left
        private int height; // becomes smaller after folding up

        public TransparentPaper(char[,] startPaper)
        {
            paper = (char[,]) startPaper.Clone();
            width = paper.GetLength(0);
            height = paper.GetLength(1);
        }

        public int CountDots()
        {
            var nDots = 0;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (paper[x,y] == '#')
                        nDots += 1;
                }
            return nDots;
        }

        public void FoldUp(int fold_y)
        {
            if (2*fold_y != (height - 1)) // fold not exactly down the middle
                throw new NotImplementedException(); // instructions imply this won't happen
            
            for (var y = 0; y < fold_y; y++)
                for (var x = 0; x < width; x++)
                {
                    if (paper[x, height - 1 - y] == '#')
                        paper[x, y] = '#';
                }

            height = fold_y;
        }

        public void FoldLeft(int fold_x)
        {
            if (2*fold_x != (width - 1)) // fold not exactly down the middle
                throw new NotImplementedException(); // instructions imply this won't happen?
            
            for (var x = 0; x < fold_x; x++)
                for (var y = 0; y < height; y++)
                {
                    if (paper[width - 1 - x, y] == '#')
                        paper[x,y] = '#';
                }

            width = fold_x;
        }

        public void Draw()
        {
            for (var y = 0; y < height; y++)
            {
                var lineBuilder = new StringBuilder(width);
                for (var x = 0; x < width; x++)
                    lineBuilder.Append(paper[x,y]);
                Console.WriteLine(lineBuilder.ToString());
            }
        }

    }
}