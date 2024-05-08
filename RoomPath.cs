using System.Numerics;

namespace ProceduralGenerationConsole
{
    internal class RoomPath
    {
        const int minRoomAmount = 2;
        const int maxRoomAmount = 8;
        private readonly List<int[,]> roomDefinitions;

        public RoomPath()
        {
            roomDefinitions = DefineRooms();
        }

        public int[,] Generate(int seed, int size)
        {
            Random random = new(seed);
            int[,] result = new int[size, size];

            int roomCount = random.Next(minRoomAmount, maxRoomAmount);
            List<Room> rooms = [];

            // Rooms
            for (int i = 0; i < roomCount; i++)
            {
                int roomType = random.Next(0, roomDefinitions.Count);
                (int x, int y) pos = (random.Next(0, size - roomDefinitions[roomType].GetLength(0)), random.Next(0, size - roomDefinitions[roomType].GetLength(1)));

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
                    }
                }
            }

            // Paths
            for (int i = 1; i < rooms.Count; i++)
            {
                List<Vector2>? path = new AStar(result).Calculate(new Vector2(rooms[i].GetEntrance().x, rooms[i].GetEntrance().y), new Vector2(rooms[i - 1].GetEntrance().x, rooms[i - 1].GetEntrance().y));
                if (path == null)
                {
                    continue;
                }
                foreach (var currentNode in path)
                {
                    result[(int)currentNode.X, (int)currentNode.Y] = 1;
                }
            }

            return result;
        }

        private List<int[,]> DefineRooms()
        {
            List<int[,]> result = [];

            result.Add(new int[,]{
                { 0, 2, 1},
                { 1, 1, 1},
                { 1, 1, 1}
            });

            result.Add(new int[,]{
                { 1, 1, 1, 1, 0, 0},
                { 2, 1, 1, 1, 1, 1},
                { 1, 1, 0, 0, 1, 1},
            });

            result.Add(new int[,]{
                { 1, 1, 1, 1, 2, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 0, 0, 0, 0},
            });

            result.Add(new int[,]{
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 0, 0, 0},
                { 1, 1, 1, 0, 0, 0},
                { 1, 2, 1, 0, 0, 0},
            });

            result.Add(new int[,]{
                { 0, 0, 0, 1, 2, 1},
                { 0, 0, 0, 1, 1, 1},
                { 0, 0, 0, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1},
            });

            result.Add(new int[,]{
                { 0, 1, 1},
                { 2, 1, 1},
                { 0, 0, 1}
            });

            result.Add(new int[,]{
                { 0, 1, 2, 0, 0},
                { 0, 1, 1, 1, 1},
                { 0, 1, 1, 1, 0},
                { 0, 1, 1, 1, 0},
                { 1, 1, 1, 0, 0},
                { 1, 1, 1, 1, 0},
                { 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1}
            });



            return result;
        }
    }

    internal class Room
    {
        public (int x, int y) pos;
        public int[,] roomDefinition;

        public Room((int x, int y) pos, int[,] roomDefinition)
        {
            this.pos = pos;
            this.roomDefinition = roomDefinition;
        }

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
