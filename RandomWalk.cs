namespace ProceduralGenerationConsole
{
    internal class RandomWalk
    {
        const int walkerCount = 2;
        const float turnChance = 0.3f;
        const float fillPercentage = 0.25f;
        const int maxSteps = 1000000;
        const float roomChance = 0.003f;

        const float walkerSplitChance = 0.05f;
        const float walkerDestroyChance = 0.1f;

        public int[,] Generate(int seed, (int x, int y) size)
        {
            System.Random random = new(seed);
            int[,] result = new int[size.x, size.y];
            int amountOfTiles = 0;
            int steps = 0;

            List<Walker> walkers = CreateWalkers(size, random);
            while (((float)amountOfTiles / (size.x * size.y)) < fillPercentage && steps < maxSteps)
            {
                for (int i = walkers.Count - 1; i >= 0; i--)
                {
                    steps++;
                    if (random.NextDouble() < walkerSplitChance)
                    {
                        Walker newWalker = new(random.Next(0, 3), walkers[i].position);
                        walkers.Add(newWalker);
                    }
                    if (random.NextDouble() < walkerDestroyChance && walkers.Count > 1)
                    {
                        walkers.RemoveAt(i);
                        continue;
                    }
                    amountOfTiles = random.NextDouble() < roomChance
                        ? CreateRoom(result, amountOfTiles, walkers, i)
                        : CreateHallway(result, amountOfTiles, walkers, i);

                    TakeStep(random, result, walkers, i);

                    if (random.NextDouble() < turnChance)
                    {
                        SetRandomWalkerDirection(random, walkers[i]);
                    }
                }
            }

            // Find dead ends and add an item tag
            TagDeadEnd(random, result);

            result[walkers[0].position.x, walkers[0].position.y] = 4;
            result[size.x / 2, size.y / 2] = 5;

            return result;
        }

        private void TagDeadEnd(Random random, int[,] result)
        {
            for (int x = 0; x < result.GetLength(0); x++)
            {
                for (int y = 0; y < result.GetLength(1); y++)
                {
                    if (result[x, y] == 0)
                    {
                        continue;
                    }
                    int neighBours = 0;
                    for (int i = Math.Max(x - 1, 0); i < Math.Min(result.GetLength(0) - 1, x + 1); i++)
                    {
                        for (int j = Math.Max(y - 1, 0); j < Math.Min(result.GetLength(1) - 1, y + 1); j++)
                        {
                            if (result[i, j] != 0)
                            {
                                neighBours++;
                            }
                        }
                    }
                    if (neighBours < 2 && random.NextDouble() < 0.5)
                    {
                        result[x, y] = 3;
                    }
                }
            }
        }

        private void TakeStep(Random random, int[,] result, List<Walker> walkers, int i)
        {
            switch (walkers[i].direction)
            {
                case Walker.Direction.Up:
                    if (walkers[i].position.y + 2 < result.GetLength(1))
                    {
                        if (result[walkers[i].position.x, walkers[i].position.y + 2] != 0)
                        {
                            SetRandomWalkerDirection(random, walkers[i]);
                        }
                        walkers[i].position.y++;
                    }
                    break;
                case Walker.Direction.Down:
                    if (walkers[i].position.y - 1 > 0)
                    {
                        if (result[walkers[i].position.x, walkers[i].position.y - 2] != 0)
                        {
                            SetRandomWalkerDirection(random, walkers[i]);
                        }
                        walkers[i].position.y--;
                    }

                    break;
                case Walker.Direction.Left:
                    if (walkers[i].position.x - 1 > 0)
                    {
                        if (result[walkers[i].position.x - 2, walkers[i].position.y] != 0)
                        {
                            SetRandomWalkerDirection(random, walkers[i]);
                        }
                        walkers[i].position.x--;
                    }
                    break;
                case Walker.Direction.Right:
                    if (walkers[i].position.x + 2 < result.GetLength(0))
                    {
                        if (result[walkers[i].position.x + 2, walkers[i].position.y] != 0)
                        {
                            SetRandomWalkerDirection(random, walkers[i]);
                        }
                        walkers[i].position.x++;
                    }
                    break;
                default:
                    throw new ArgumentException("No direction!");
            }
        }

        private int CreateHallway(int[,] result, int amountOfTiles, List<Walker> walkers, int i)
        {
            if (result[walkers[i].position.x, walkers[i].position.y] == 0)
            {
                amountOfTiles++;
                result[walkers[i].position.x, walkers[i].position.y] = 1;
            }

            return amountOfTiles;
        }

        private int CreateRoom(int[,] result, int amountOfTiles, List<Walker> walkers, int i)
        {
            for (int x = walkers[i].position.x - 2; x <= walkers[i].position.x + 2; x++)
            {
                for (int y = walkers[i].position.y - 2; y <= walkers[i].position.y + 2; y++)
                {
                    if (x >= result.GetLength(0) || x < 0 || y >= result.GetLength(1) || y < 0)
                    {
                        continue;
                    }
                    if (result[x, y] != 2)
                    {
                        if (result[x, y] != 1)
                        {
                            amountOfTiles++;
                        }
                        result[x, y] = 2;
                    }
                }
            }
            result[walkers[i].position.x, walkers[i].position.y] = 3;
            return amountOfTiles;
        }

        private List<Walker> CreateWalkers((int x, int y) size, Random random)
        {
            List<Walker> walkers = new();
            for (int i = 0; i < walkerCount; i++)
            {
                Walker walker = new(i, (size.x / 2, size.y / 2));
                walkers.Add(walker);
                SetRandomWalkerDirection(random, walker);
            }

            return walkers;
        }

        private static void SetRandomWalkerDirection(System.Random random, Walker walker)
        {
            walker.direction = random.Next(0, 4) switch
            {
                0 => Walker.Direction.Up,
                1 => Walker.Direction.Down,
                2 => Walker.Direction.Left,
                3 => Walker.Direction.Right,
                _ => throw new ArgumentException("Incorrect number random direction!"),
            };
        }
    }
}
