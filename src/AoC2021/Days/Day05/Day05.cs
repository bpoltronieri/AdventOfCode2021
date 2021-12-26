using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AoC2021.Days
{
    public class Day05 : IDay
    {
        private string[] input;

        public Day05(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            input = File.ReadAllLines(file);
        }

        private List<int[][]> ParseInputIntoLines()
        {
            return input.Select(x => x.Split(' ')
                                       .Where(x => x[0] != '-')
                                       .Select(y => y.Split(',')
                                                     .Select(z => int.Parse(z))
                                                     .ToArray()
                                              )
                                       .ToArray() 
                                )
                        .ToList();
        }

        private IEnumerable<Tuple<int,int>> LinePoints(int[][] line)
        {
            var start_x = line[0][0];
            var end_x = line[1][0];
            var step_x = Math.Sign(end_x - start_x);

            var start_y = line[0][1];
            var end_y = line[1][1];
            var step_y = Math.Sign(end_y - start_y);
                
            var n_steps = step_x != 0 
                        ? Math.Abs(end_x - start_x) + 1
                        : Math.Abs(end_y - start_y) + 1;

            for (var i = 0; i < n_steps; i++)
                yield return new Tuple<int,int>(start_x + i * step_x, start_y + i * step_y);
        }

        private void UpdateCoveredByPoint (Dictionary<Tuple<int, int>, int> covered, Tuple<int,int> position)
        {
            if (covered.ContainsKey(position))
                covered[position] += 1;
            else
                covered[position] = 1;
        }

        private void UpdateCoveredByLine (Dictionary<Tuple<int, int>, int> covered, int[][] line)
        {
            foreach (var p in LinePoints(line))
                UpdateCoveredByPoint(covered, p);
        }

        private bool LineIsHorzOrVert(int[][] x)
        {
            return x[0][0] == x[1][0] || x[0][1] == x[1][1];
        }

        public string PartOne()
        {
            var covered = new Dictionary<Tuple<int,int>, int>(); // maps x,y to number of times covered
            ParseInputIntoLines()
                .Where(x => LineIsHorzOrVert(x)) 
                .ToList()
                .ForEach(l => UpdateCoveredByLine(covered, l));
            return covered.Values.Count(x => x > 1).ToString();
        }

        public string PartTwo()
        {
            var covered = new Dictionary<Tuple<int,int>, int>(); // maps x,y to number of times covered
            ParseInputIntoLines().ForEach(l => UpdateCoveredByLine(covered, l));
            // CreateAndSaveGif(covered);
            return covered.Values.Count(x => x > 1).ToString();
        }

        // visualisation
        private void CreateAndSaveGif(Dictionary<Tuple<int,int>, int> covered)
        {
            var lines = ParseInputIntoLines();
            var minX = lines.Min(l => Math.Min(l[0][0], l[1][0]));
            var maxX = lines.Max(l => Math.Max(l[0][0], l[1][0]));
            var width = maxX - minX + 1;
            var minY = lines.Min(l => Math.Min(l[0][1], l[1][1]));
            var maxY = lines.Max(l => Math.Max(l[0][1], l[1][1]));
            var height = maxY - minY + 1;
            
            var frames = new List<Bitmap>();
            var bmp = new Bitmap(width, height);
            frames.Add(bmp);
            foreach (var line in lines)
            {
                bmp = new Bitmap(width, height);
                foreach (var p in LinePoints(line))
                {
                    if (covered[p] > 1)
                        bmp.SetPixel(p.Item1 - minX, p.Item2 - minY, Color.Red);
                    else
                        bmp.SetPixel(p.Item1 - minX, p.Item2 - minY, Color.Blue);
                }
                frames.Add(bmp);
            }

            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();
            foreach (System.Drawing.Bitmap bmpImage in frames)
            {
                var hbmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hbmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
            }
            using(FileStream fs = new FileStream("Visualisations/5_2.gif", FileMode.Create))
                gEnc.Save(fs);
        }
    }
}