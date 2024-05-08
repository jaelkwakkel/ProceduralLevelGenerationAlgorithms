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

        public int[,] Generate(int seed, int size)
        {
            Random random = new(seed);
            int[,] result = new int[size, size];

            List<Walker> walkers = [];
            for (int i = 0; i < walkerCount; i++)
            {
                Walker walker = new(i, size / 2);
                walkers.Add(walker);
                SetRandomWalkerDirection(random, walker);
            }
            int amountOfTiles = 0;
            int steps = 0;
            while (((float)amountOfTiles / (size * size)) < fillPercentage && steps < maxSteps)
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
                    if (random.NextDouble() < roomChance)
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
                    }
                    else
                    {
                        if (result[walkers[i].position.x, walkers[i].position.y] == 0)
                        {
                            amountOfTiles++;
                            result[walkers[i].position.x, walkers[i].position.y] = 1;
                        }
                    }

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
                            else
                            {
                                //walkers[i].position.y = 0;
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
                            else
                            {
                                //walkers[i].position.y = result.GetLength(1) - 1;
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
                            else
                            {
                                //walkers[i].position.x = 0;
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
                            else
                            {
                                //walkers[i].position.x = result.GetLength(0) - 1;
                            }
                            break;
                        default:
                            throw new ArgumentException("No direction!");
                    }

                    if (random.NextDouble() < turnChance)
                    {
                        SetRandomWalkerDirection(random, walkers[i]);
                    }
                }
            }

            // Find dead ends and add an item tag
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

            result[walkers[0].position.x, walkers[0].position.y] = 4;
            result[size / 2, size / 2] = 5;

            return result;
        }

        private static void SetRandomWalkerDirection(Random random, Walker walker)
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
