using ProceduralGenerationConsole;

while (true)
{
    //int[,] result = CombineResultSets(new RandomWalk().Generate(Random.Shared.Next(), 30), new RoomPath().Generate(Random.Shared.Next(), 30));
    int[,] result = new PerlinGenerator().Generate(Random.Shared.Next(), 30);
    new DungeonResultDrawer().Draw(result);
    while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)) { }
}

int[,] CombineResultSets(int[,] resultA, int[,] resultB)
{
    int[,] result = resultA;
    for (int i = 0; i < result.GetLength(0); i++)
    {
        for (int j = 0; j < result.GetLength(1); j++)
        {
            result[i, j] = Math.Max(result[i, j], resultB[i, j]);
        }
    }

    return result;
}