using ProceduralGenerationConsole;

while (true)
{
    int[,] result = new RoomPath().Generate(Random.Shared.Next(), (30, 50));
    //int[,] result = new RandomWalk().Generate(Random.Shared.Next(), (30, 50));
    //int[,] result = new PerlinGenerator().Generate(Random.Shared.Next(), (30, 50));
    new DungeonResultDrawer().Draw(result);
    while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)) { /* Empty on purpose */ }
}