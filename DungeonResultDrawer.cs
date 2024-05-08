namespace ProceduralGenerationConsole
{
    internal class DungeonResultDrawer
    {
        public void Draw(int[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[i, j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    else if (data[i, j] == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else if (data[i, j] == 3)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else if (data[i, j] == 4)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else if (data[i, j] == 5)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }
    }
}
