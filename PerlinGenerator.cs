namespace ProceduralGenerationConsole
{
    internal class PerlinGenerator
    {
        private const double minValue = 0.001d;
        private const float perlinSize = 24;

        public int[,] Generate(int seed, int size)
        {
            int[,] result = new int[size, size];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    double d = new FastNoise(seed).GetPerlin(i * perlinSize, j * perlinSize);
                    //if (d <= minValue * 300)
                    //{
                    //    result[i, j] = 2;
                    //}
                    if (d <= minValue)
                    {
                        result[i, j] = 1;
                    }
                }
            }

            return result;
        }
    }
}
