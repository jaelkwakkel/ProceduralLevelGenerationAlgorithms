using System.Numerics;

namespace ProceduralGenerationConsole
{
    internal class RoomPath
    {
        const int minRoomAmount = 2;
        const int maxRoomAmount = 8;
        const int minDistanceFromPreviousRoom = 5;
        const int maxDistanceFromPreviousRoom = 15;
        private List<int[,]> roomDefinitions;

        public RoomPath()
        {
            roomDefinitions = DefineRooms();
        }

        public int[,] Generate(int seed, (int x, int y) size)
        {
            roomDefinitions = DefineRooms();
            System.Random random = new(seed);
            int[,] result = new int[size.x, size.y];

            int roomCount = random.Next(minRoomAmount, maxRoomAmount);
            List<Room> rooms = CreateRooms(size, random, result, roomCount);

            CreatePaths(result, rooms);

            return result;
        }

        private static void CreatePaths(int[,] result, List<Room> rooms)
        {
            // Paths
            for (int i = 1; i < rooms.Count; i++)
            {
                List<Vector2> path = new AStar(result, new List<int> { 2 }).Calculate(new Vector2(rooms[i].GetEntrance().x, rooms[i].GetEntrance().y), new Vector2(rooms[i - 1].GetEntrance().x, rooms[i - 1].GetEntrance().y));
                if (path == null)
                {
                    // If astar path not found, create a direct path
                    int startX = rooms[i].GetEntrance().x;
                    int startY = rooms[i].GetEntrance().y;
                    int endX = rooms[i - 1].GetEntrance().x;
                    int endY = rooms[i - 1].GetEntrance().y;

                    while (startX != endX || startY != endY)
                    {
                        result[startX, startY] = 1;
                        if (startX < endX)
                            startX++;
                        else if (startX > endX)
                            startX--;
                        else if (startY < endY)
                            startY++;
                        else if (startY > endY)
                            startY--;
                    }
                }
                else
                {
                    foreach (var currentNode in path)
                    {
                        result[(int)currentNode.X, (int)currentNode.Y] = 1;
                    }
                }
            }
        }

        private List<Room> CreateRooms((int x, int y) size, System.Random random, int[,] result, int roomCount)
        {
            List<Room> rooms = [];

            // Rooms
            for (int i = 0; i < roomCount; i++)
            {
                int roomType = random.Next(0, roomDefinitions.Count);
                (int x, int y) pos;
                if (i == 0)
                {
                    roomType = 0;
                    // Always place first room in center
                    pos = (size.x / 2, size.y / 2);
                }
                else
                {
                    int newXPos = rooms[i - 1].pos.x + random.Next(minDistanceFromPreviousRoom, maxDistanceFromPreviousRoom) * (random.NextDouble() < 0.5d ? -1 : 1);
                    int newYPos = rooms[i - 1].pos.y + random.Next(minDistanceFromPreviousRoom, maxDistanceFromPreviousRoom) * (random.NextDouble() < 0.5d ? -1 : 1);
                    int clampedXPos = Math.Clamp(newXPos, 2, size.x - roomDefinitions[roomType].GetLength(0) - 2);
                    int clampedYPos = Math.Clamp(newYPos, 2, size.y - roomDefinitions[roomType].GetLength(1) - 2);
                    pos = (clampedXPos, clampedYPos);
                }

                Room newRoom = new(pos, roomDefinitions[roomType]);

                rooms.Add(newRoom);

                for (int newRoomX = newRoom.pos.x; newRoomX < newRoom.pos.x + newRoom.GetSize().x; newRoomX++)
                {
                    for (int newRoomY = newRoom.pos.y; newRoomY < newRoom.pos.y + newRoom.GetSize().y; newRoomY++)
                    {
                        if (newRoom.roomDefinition[newRoomX - newRoom.pos.x, newRoomY - newRoom.pos.y] == 1)
                        {
                            result[newRoomX, newRoomY] = 2;
                        }
                        else if (newRoom.roomDefinition[newRoomX - newRoom.pos.x, newRoomY - newRoom.pos.y] == 2)
                        {
                            result[newRoomX, newRoomY] = 1;
                        }
                        else if (newRoom.roomDefinition[newRoomX - newRoom.pos.x, newRoomY - newRoom.pos.y] == 4)
                        {
                            result[newRoomX, newRoomY] = 4;
                        }
                    }
                }
            }

            return rooms;
        }

        private static List<int[,]> DefineRooms()
        {
            List<int[,]> result = [
            new int[,]{
                { 0, 0, 2, 0, 0},
                { 0, 1, 1, 1, 0},
                { 0, 1, 4, 1, 0},
                { 0, 1, 1, 1, 0},
                { 0, 0, 0, 0, 0},
            },
                new int[,]{
                { 0, 0, 2, 0, 0},
                { 0, 0, 1, 0, 0},
                { 1, 1, 1, 1, 0},
                { 0, 0, 1, 1, 0},
                { 0, 0, 0, 0, 0},
            },
                new int[,]{
                { 1, 1, 1, 1, 0, 0},
                { 2, 1, 1, 1, 1, 1},
                { 1, 1, 0, 0, 1, 1},
            },
                new int[,]{
                { 1, 1, 1, 1, 2, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 0, 0, 0, 0},
            },
                new int[,]{
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 0, 0, 0},
                { 1, 1, 1, 0, 0, 0},
                { 1, 2, 1, 0, 0, 0},
            },
                new int[,]{
                { 0, 0, 0, 1, 2, 1},
                { 0, 0, 0, 1, 1, 1},
                { 0, 0, 0, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
            },
                new int[,]{
                { 0, 1, 1},
                { 2, 1, 1},
                { 0, 0, 1}
            },
                new int[,]{
                { 0, 1, 2, 0, 0},
                { 0, 1, 1, 1, 1},
                { 0, 1, 1, 1, 0},
                { 0, 1, 1, 1, 0},
                { 1, 1, 1, 0, 0},
                { 1, 1, 1, 1, 0},
                { 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1}
            }
        ];

            return result;
        }
    }

    internal class Room((int x, int y) pos, int[,] roomDefinition)
    {
        public (int x, int y) pos = pos;
        public int[,] roomDefinition = roomDefinition;

        public (int x, int y) GetEntrance()
        {
            for (int i = 0; i < roomDefinition.GetLength(0); i++)
            {
                for (int j = 0; j < roomDefinition.GetLength(1); j++)
                {
                    if (roomDefinition[i, j] == 2)
                    {
                        return (i + pos.x, j + pos.y);
                    }
                }
            }

            throw new ArgumentException("Room has no entrance");
        }

        public (int x, int y) GetSize()
        {
            return (roomDefinition.GetLength(0), roomDefinition.GetLength(1));
        }
    }
}
