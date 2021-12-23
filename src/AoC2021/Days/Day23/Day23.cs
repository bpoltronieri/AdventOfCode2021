using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2021.Days.Day23Utils;

namespace AoC2021.Days
{
    public class Day23 : IDay
    {
        private char[,] inputMap;

        private Dictionary<char,int> energyCosts = new Dictionary<char, int>()
            { {'A', 1}, {'B', 10}, {'C', 100}, {'D', 1000} };
        
        private Dictionary<char,int> roomXPosns = new Dictionary<char, int>()
            { {'A', 3}, {'B', 5}, {'C', 7}, {'D', 9} };

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
            var visitedStates = new Dictionary<string, int>(); // maps MapState's hash string to lowest cost found to reach it.
            var ongoingPaths = new Stack<MapState>();
            ongoingPaths.Push(new MapState(CopyMap(inputMap), 0));
            var lowestCost = int.MaxValue;
            while (ongoingPaths.Count > 0)
            {   
                var currentState = ongoingPaths.Pop();

                var stateHash = currentState.HashString();
                if (visitedStates.ContainsKey(stateHash)
                    && visitedStates[stateHash] <= currentState.energyCost)
                    continue;
                else
                    visitedStates[currentState.HashString()] = currentState.energyCost;
                    
                // currentState.DrawMap();
                if (IsComplete(currentState.map))
                {
                    if (currentState.energyCost < lowestCost)
                    {
                        lowestCost = currentState.energyCost;
                        // currentState.DrawHistory();
                        ongoingPaths = new Stack<MapState>(ongoingPaths.Where(p => p.energyCost < lowestCost));
                    }
                    continue;
                }

                var done = false;
                var map = currentState.map;
                // try to move an amphipod from current room to its own room
                foreach (var room in GetMapRooms().Where(R => map[R.Item1, R.Item2] != '.' && !AmphipodInFinalPosition(map, R)))
                {
                    var amphipod = map[room.Item1, room.Item2];
                    // first try to go straight to amphipod's final position
                    var straightToRoom = AmphipodCanGoRoomToRoom(map, room);
                    if (straightToRoom != null)
                    {
                        var (ownRoom, steps) = straightToRoom.Value;
                        var newMap = CopyMap(map);
                        newMap[room.Item1, room.Item2] = '.';
                        newMap[ownRoom.Item1, ownRoom.Item2] = amphipod;
                        var newCost = currentState.energyCost + steps * energyCosts[amphipod];
                        if (newCost < lowestCost)
                            ongoingPaths.Push(new MapState(newMap, newCost, currentState.history)); 
                        done = true; // moving straight to room must be the best
                        break;
                    }
                }
                if (done) continue;

                // try to move an amphipod from hallway to its room
                foreach (var hall in GetMapHallways().Where(H => map[H.Item1, H.Item2] != '.'))
                {
                    var amphipod = map[hall.Item1, hall.Item2];
                    var toRoom = AmphipodCanGoHallToRoom(map, hall);
                    if (toRoom != null)
                    {
                        var (ownRoom, steps) = toRoom.Value;
                        var newMap = CopyMap(map);
                        newMap[hall.Item1, hall.Item2] = '.';
                        newMap[ownRoom.Item1, ownRoom.Item2] = amphipod;
                        var newCost = currentState.energyCost + steps * energyCosts[amphipod];
                        if (newCost < lowestCost)
                            ongoingPaths.Push(new MapState(newMap, newCost, currentState.history)); 
                        done = true; // moving straight to room must be the best
                        break;
                    }
                }
                if (done) continue;

                // otherwise try to move amphipod from room to hallway
                foreach (var room in GetMapRooms().Where(R => map[R.Item1, R.Item2] != '.' && !AmphipodInFinalPosition(map, R)))
                {
                    var amphipod = map[room.Item1, room.Item2];
                    foreach (var (hall, steps) in GetAmphipodPossibleHallwayStops(map, room))
                    {
                        var newMap = CopyMap(map);
                        newMap[room.Item1, room.Item2] = '.';
                        newMap[hall.Item1, hall.Item2] = amphipod;
                        var newCost = currentState.energyCost + steps * energyCosts[amphipod];

                        if (newCost < lowestCost)
                            ongoingPaths.Push(new MapState(newMap, newCost, currentState.history));
                    }
                }
            }

            return lowestCost.ToString();
        }

        public string PartTwo()
        {
            var answer = 0;
            return answer.ToString();
        }

        private IEnumerable<(int,int)> GetMapHallways()
        {
            for (var x = 1; x < 12; x++)
                yield return (x, 1);
        }

        private IEnumerable<(int,int)> GetMapRooms()
        {
            foreach (var x in roomXPosns.Values)
                for (var y = 2; y <= 3; y++)
                    yield return (x,y);
        }

        private ((int,int), int)? AmphipodCanGoHallToRoom(char[,] map, (int, int) hall)
        {
            var amphipod = map[hall.Item1, hall.Item2];
            var roomX = roomXPosns[amphipod];

            // check room has space for amphipod
            var roomY = -1; // how deep in the room the amphipod would go
            for (var y = 2; y <= 3; y++)
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
            var amphipod = map[room.Item1, room.Item2];
            if (IsRoom(room) && room.Item1 == roomXPosns[amphipod])
            {
                // check we don't have a different type of amphipod lower in the room
                for (var y = room.Item2 + 1; y <= 3; y++)
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
            return map[3,2] == 'A' && map[3,3] == 'A'
                && map[5,2] == 'B' && map[5,3] == 'B'
                && map[7,2] == 'C' && map[7,3] == 'C'
                && map[9,2] == 'D' && map[9,3] == 'D';
        }

        private bool IsRoom((int,int) position)
        {
            return roomXPosns.ContainsValue(position.Item1) 
                && (position.Item2 == 2 || position.Item2 == 3);
        }

        private bool IsOutsideRoomDoor((int,int) position)
        {
            return IsHallway(position) && roomXPosns.ContainsValue(position.Item1);
        }

        private bool IsHallway((int,int) position)
        {
            return position.Item2 == 1 && inputMap[position.Item1, position.Item2] == '.';
        }

    }
}