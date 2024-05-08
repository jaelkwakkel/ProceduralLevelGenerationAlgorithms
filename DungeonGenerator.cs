internal class DungeonGenerator
{
    const int minRooms = 3;
    const int maxRooms = 8;

    internal int[,] Generate(int seed, int size)
    {
        Random random = new(seed);
        int[,] result = new int[size, size];

        int roomAmount = random.Next(minRooms, maxRooms + 1);
        List<(int, int)> rooms = [];
        for (int roomCount = 0; roomCount < roomAmount; roomCount++)
        {
            (int x, int y) position;
            position.x = random.Next(0, size);
            position.y = random.Next(0, size);
            rooms.Add(position);
            for (int i = position.x - 2; i <= position.x + 2; i++)
            {
                for (int j = position.y - 2; j <= position.y + 2; j++)
                {
                    if (i >= result.GetLength(0) || i < 0 || j >= result.GetLength(1) || j < 0)
                    {
                        continue;
                    }
                    result[i, j] = 2;

                }
            }
        }



        return result;
    }
}