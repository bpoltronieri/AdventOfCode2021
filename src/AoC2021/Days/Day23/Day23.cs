using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using AoC2021.Days.Day23Utils;

namespace AoC2021.Days
{
    public class Day23 : IDay
    {
        private char[,] inputMap;
        private int roomDepth = 3;

        private Dictionary<char,int> energyCosts = new Dictionary<char, int>()
            { {'A', 1}, {'B', 10}, {'C', 100}, {'D', 1000} };
        
        private Dictionary<char,int> roomXPosns = new Dictionary<char, int>()
            { {'A', 3}, {'B', 5}, {'C', 7}, {'D', 9} };

        private MapState lowestState; // for visualisation

        public Day23(string file)
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

        public string PartOne()
        {
            var startMap = CopyMap(inputMap);
            var lowestCost = CheapestCompletionCost(startMap);
            return lowestCost.ToString();
        }

        public string PartTwo()
        {
            var startMap = UnfoldInputMap();
            var lowestCost = CheapestCompletionCost(startMap);
            // CreateAndSaveGif(lowestState);
            return lowestCost.ToString();
        }

        private char[,] UnfoldInputMap()
        {
            roomDepth = 5;
            var startMap = new char[inputMap.GetLength(0), inputMap.GetLength(1) + 2];
            var extraLines = new string[2] {"  #D#C#B#A#  ", "  #D#B#A#C#  "};
            for (var y = 0; y < 3; y++)
                for (var x = 0; x < inputMap.GetLength(0); x++)
                    startMap[x,y] = inputMap[x,y];

            for (var y = 3; y < 5; y++)
                for (var x = 0; x < inputMap.GetLength(0); x++)
                    startMap[x,y] = extraLines[y-3][x];

            for (var y = 5; y < inputMap.GetLength(1) + 2; y++)
                for (var x = 0; x < inputMap.GetLength(0); x++)
                    startMap[x,y] = inputMap[x,y-2];

            return startMap;
        }

        private int CheapestCompletionCost(char[,] startMap)
        {
            var visitedStates = new Dictionary<string, int>(); // maps MapState's hash string to lowest cost found to reach it.
            var ongoingPaths = new Stack<MapState>();
            ongoingPaths.Push(new MapState(CopyMap(startMap), 0));
            var lowestCost = int.MaxValue;
            while (ongoingPaths.Count > 0)
            {   
                var currentState = ongoingPaths.Pop();
                var stateHash = currentState.HashString();
                if (visitedStates.ContainsKey(stateHash) && visitedStates[stateHash] <= currentState.energyCost)
                    continue;
                else
                    visitedStates[currentState.HashString()] = currentState.energyCost;
                    
                // currentState.DrawMap();
                if (IsComplete(currentState.map))
                {
                    lowestCost = currentState.energyCost; // must be lower than lowest or would have been pruned by visistedStates
                    lowestState = currentState;
                    // currentState.DrawHistory();
                    ongoingPaths = new Stack<MapState>(ongoingPaths.Where(p => p.energyCost < lowestCost));
                    continue;
                }

                var done = false;
                var map = currentState.map;
                // try to move an amphipod from current room to its own room
                foreach (var room in GetMapRooms().Where(R => map[R.Item1, R.Item2] != '.' && !AmphipodInFinalPosition(map, R)))
                {
                    var straightToRoom = AmphipodCanGoRoomToRoom(map, room);
                    if (straightToRoom != null)
                    {
                        var (ownRoom, steps) = straightToRoom.Value;
                        MakeMove(ongoingPaths, lowestCost, currentState, room, ownRoom, steps);
                        done = true; // moving straight to room must be the best
                        break;
                    }
                }
                if (done) continue;

                // try to move an amphipod from hallway to its room
                foreach (var hall in GetMapHallways().Where(H => map[H.Item1, H.Item2] != '.'))
                {
                    var toRoom = AmphipodCanGoHallToRoom(map, hall);
                    if (toRoom != null)
                    {
                        var (ownRoom, steps) = toRoom.Value;
                        MakeMove(ongoingPaths, lowestCost, currentState, hall, ownRoom, steps);
                        done = true; // moving straight to room must be the best
                        break;
                    }
                }
                if (done) continue;

                // otherwise try to move amphipod from room to hallway
                foreach (var room in GetMapRooms().Where(R => map[R.Item1, R.Item2] != '.' && !AmphipodInFinalPosition(map, R)))
                    foreach (var (hall, steps) in GetAmphipodPossibleHallwayStops(map, room))
                        MakeMove(ongoingPaths, lowestCost, currentState, room, hall, steps);
            }
            return lowestCost;
        }

        private void MakeMove(Stack<MapState> ongoingPaths, int lowestCost, MapState currentState, (int, int) from, (int, int) to, int steps)
        {
            // makes a copy of the current map state and makes the given move from 'from' to 'to'
            // adds it to the stack of map states given that it doesn't cost more than the lowest cost so far
            var amphipod = currentState.map[from.Item1, from.Item2];
            var newCost = currentState.energyCost + steps * energyCosts[amphipod];
            if (newCost >= lowestCost) return;

            var newMap = CopyMap(currentState.map);
            newMap[from.Item1, from.Item2] = '.';
            newMap[to.Item1, to.Item2] = amphipod;
            ongoingPaths.Push(new MapState(newMap, newCost, currentState.history)); 
        }

        private IEnumerable<(int,int)> GetMapHallways()
        {
            for (var x = 1; x < 12; x++)
                yield return (x, 1);
        }

        private IEnumerable<(int,int)> GetMapRooms()
        {
            foreach (var x in roomXPosns.Values)
                for (var y = 2; y <= roomDepth; y++)
                    yield return (x,y);
        }

        // the next few functions could probably be simplified by a simple CanGetFromAToB function...
        private ((int,int), int)? AmphipodCanGoHallToRoom(char[,] map, (int, int) hall)
        {
            var amphipod = map[hall.Item1, hall.Item2];
            var roomX = roomXPosns[amphipod];

            // check room has space for amphipod
            var roomY = -1; // how deep in the room the amphipod would go
            for (var y = 2; y <= roomDepth; y++)
            {
                if (map[roomX, y] != amphipod && map[roomX, y] != '.')
                    return null;
                if (map[roomX, y] == '.')
                    roomY = y;
            }

            // check there's nothing blocking the way to the room
            var xStep = hall.Item1 > roomX ? -1 : 1;
            for (var x = hall.Item1 + xStep; x != roomX; x += xStep)
            {
                if (map[x, hall.Item2] != '.') // something in the way
                    return null;
            }

            var nSteps = Math.Abs(roomX - hall.Item1) + roomY - hall.Item2;
            return ((roomX, roomY), nSteps);
        }

        private ((int,int), int)? AmphipodCanGoRoomToRoom(char[,] map, (int, int) room)
        {
            var amphipod = map[room.Item1, room.Item2];
            // can we get out of current room:
            for (var y = room.Item2 - 1; y >= 1; y--)
            {
                if (map[room.Item1, y] != '.')
                    return null;
            }
            map[room.Item1, 1] = amphipod; // temporary so as to not confuse AmphipodCanGoHallToRoom
            var hallToRoom = AmphipodCanGoHallToRoom(map, (room.Item1, 1));
            map[room.Item1, 1] = '.';
            if (hallToRoom == null)
                return null;

            var nSteps = room.Item2 - 1 + hallToRoom.Value.Item2;
            return (hallToRoom.Value.Item1, nSteps);
        }

        private IEnumerable<((int,int), int)> GetAmphipodPossibleHallwayStops(char[,] map, (int, int) room)
        {
            // assumes amphipod is currently in a room
            // get out of current room:
            var stuckInRoom = false;
            for (var y = room.Item2 - 1; y >= 1; y--)
            {
                if (map[room.Item1, y] != '.')
                {
                    stuckInRoom = true;
                    break;
                }
            }

            if (!stuckInRoom)
            {
                // then try different hallway stops:
                foreach (var hall in GetMapHallways().Where(p => !IsOutsideRoomDoor(p) && map[p.Item1, p.Item2] == '.'))
                {
                    var xStep = hall.Item1 > room.Item1 ? 1 : -1;
                    var blocked = false;
                    for (var x = room.Item1; x != hall.Item1; x += xStep)
                    {
                        if (map[x + xStep, 1] != '.') // something in the way
                        {
                            blocked = true;
                            break;
                        }
                    }
                    if (!blocked)
                    {
                        var nSteps = room.Item2 - 1 + Math.Abs(hall.Item1 - room.Item1);
                        yield return (hall, nSteps);
                    }
                    
                }
            }
        }

        private bool AmphipodInFinalPosition(char[,] map, (int, int) room)
        {
            // whether given amphipod is already in its final position
            var amphipod = map[room.Item1, room.Item2];
            if (IsRoom(room) && room.Item1 == roomXPosns[amphipod])
            {
                // check we don't have a different type of amphipod lower in the room
                for (var y = room.Item2 + 1; y <= roomDepth; y++)
                {
                    if (map[room.Item1, y] != amphipod)
                        return false;
                }
                return true;
            }
            return false;
        }

        private char[,] CopyMap(char[,] map)
        {
            return (char[,])map.Clone();
        }

        private bool IsComplete(char[,] map)
        {
            var complete = true;
            foreach (var p in roomXPosns)
            {
                for (var y = 2; y <= roomDepth && complete; y++)
                    if (map[p.Value, y] != p.Key)
                        complete = false;
                if (!complete) break;
            }
            return complete;
        }

        private bool IsRoom((int,int) position)
        {
            return roomXPosns.ContainsValue(position.Item1) 
                && (position.Item2 >= 2 || position.Item2 <= roomDepth);
        }

        private bool IsOutsideRoomDoor((int,int) position)
        {
            return IsHallway(position) && roomXPosns.ContainsValue(position.Item1);
        }

        private bool IsHallway((int,int) position)
        {
            return position.Item2 == 1 
                && position.Item2 > 0 && position.Item2 < 12;
        }

        // visualisation
        private Bitmap CreateBitmap(string[] map)
        {
            var fontSize = 16.0f;
            var textHeight = (float)(fontSize * 1.333);
            var fontFamily = new FontFamily("Consolas");
            var font = new Font(fontFamily, fontSize, System.Drawing.FontStyle.Regular);
            var brush = new SolidBrush(Color.White);

            var bmp = new Bitmap((int)Math.Ceiling(map[0].Length * fontSize * 0.8), (int)Math.Ceiling(map.Length * textHeight));
            Graphics bmpGraphics = Graphics.FromImage(bmp);
            bmpGraphics.Clear(Color.Black);
            for (var y = 0; y < map.Length; y++)
            {
                bmpGraphics.DrawString(map[y], font, brush, new PointF(0, y * textHeight));
            }
            // bmp = new Bitmap(bmp, new System.Drawing.Size(bmp.Width * 3, bmp.Height * 3)); // to scale up
            return bmp;
        }

        private void CreateAndSaveGif(MapState state)
        {
            var bitmapList = new List<Bitmap>();
            foreach (var hashString in state.history)
            {
                var map = state.HashStringToStringMap(hashString);
                bitmapList.Add(CreateBitmap(map));
                bitmapList.Add(bitmapList.Last());
                bitmapList.Add(bitmapList.Last()); // triple each frame to slow down gif...
            }
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
            using(FileStream fs = new FileStream("Visualisations/23_2.gif", FileMode.Create))
                gEnc.Save(fs);
        }

    }
}