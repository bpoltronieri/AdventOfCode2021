using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System;
using System.Windows;

namespace AoC2021.Days
{
    public class Day25 : IDay
    {
        private char[,] inputMap;

        public Day25(string file)
        {
            LoadInput(file);
        }

        private void LoadInput(string file)
        {
            var input = File.ReadAllLines(file);
            inputMap = new char[input[0].Length, input.Length];
            for (var y = 0; y < input.Length; y++)
                for (var x = 0; x < input[y].Length; x++)
                    inputMap[x,y] = input[y][x];
        }

        private char[,] CopyMap(char[,] map)
        {
            return (char[,])map.Clone();
        }

        public string PartOne()
        {
            var moved = true;
            var map = CopyMap(inputMap);
            var nSteps = 0;
            // var bitmapList = new List<Bitmap>(); // for visualisation
            while (moved)
            {
                // bitmapList.Add(CreateBitmap(map));
                moved = false;
                // east facing first
                var newMap = CopyMap(map);
                for (var x = 0; x < map.GetLength(0); x++)
                    for (var y  = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x,y] == '>' && map[(x+1) % map.GetLength(0), y] == '.')
                        {
                            newMap[x,y] = '.';
                            newMap[(x+1) % map.GetLength(0), y] = '>';
                            moved = true;
                        }
                    }
                map = newMap;

                // south facing
                newMap = CopyMap(map);
                for (var x = 0; x < map.GetLength(0); x++)
                    for (var y  = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x,y] == 'v' && map[x, (y+1) % map.GetLength(1)] == '.')
                        {
                            newMap[x,y] = '.';
                            newMap[x, (y+1) % map.GetLength(1)] = 'v';
                            moved = true;
                        }
                    }
                map = newMap;
                
                nSteps += 1;
            }
            // CreateAndSaveGif(bitmapList);
            return nSteps.ToString();
        }

        public string PartTwo()
        {
            // no part two on day 25!
            var answer = 0;
            return answer.ToString();
        }

        // visualisation
        private Dictionary<char, Color> colours = new Dictionary<char, Color>() {{'.', Color.White}, {'>', Color.Red}, {'v', Color.Magenta}};
        private Bitmap CreateBitmap(char[,] map)
        {
            var bmp = new Bitmap(map.GetLength(0), map.GetLength(1));
            for (var x = 0; x < map.GetLength(0); x++)
                for (var y = 0; y < map.GetLength(1); y++)
                    bmp.SetPixel(x, y, colours[map[x,y]]);
            // bmp = new Bitmap(bmp, new System.Drawing.Size(bmp.Width * 3, bmp.Height * 3)); // to scale up
            return bmp;
        }
        private void CreateAndSaveGif(List<Bitmap> bitmapList)
        {
            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();

            foreach (System.Drawing.Bitmap bmpImage in bitmapList)
            {
                var bmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
            }
            using(FileStream fs = new FileStream("Visualisations/25_1.gif", FileMode.Create))
                gEnc.Save(fs);
        }
    }
}