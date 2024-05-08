
namespace ProceduralGenerationConsole
{
    internal class Walker
    {
        public Direction direction;
        public (int x, int y) position;

        public Walker(int i, (int x, int y) pos)
        {
            direction = (Direction)(i % 4);
            position = pos;
        }

        public Walker(int i, int pos)
        {
            direction = (Direction)(i % 4);
            position = (pos, pos);
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}