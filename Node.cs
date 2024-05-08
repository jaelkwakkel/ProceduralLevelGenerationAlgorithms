namespace ProceduralGenerationConsole
{
    public class Node
    {
        internal bool canTraverse;
        internal int weight;
        internal (int x, int y) gridPosition;
        internal Node? parent;

        internal float hCost;
        internal float gCost;
        internal float fCost;

        public Node(bool canTraverse)
        {
            this.canTraverse = canTraverse;
        }

        public Node(bool canTraverse, int weight, (int x, int y) gridPosition)
        {
            this.canTraverse = canTraverse;
            this.weight = weight;
            this.gridPosition = gridPosition;
        }
    }
}