using System.Numerics;

namespace ProceduralGenerationConsole
{
    internal class AStar
    {
        private Node[,] grid;
        private readonly List<int> avoidableObjectIds;

        public AStar(int[,] collidableObjects, List<int> collidableObjectIds)
        {
            avoidableObjectIds = collidableObjectIds;
            grid = GenerateGrid(collidableObjects);
        }

        public List<Vector2> GetValidSpotsInRange(Vector2 currentPosition, int range)
        {
            List<Vector2> validSpots = new();
            Node startGridPoint = GetPointOnGrid(currentPosition);
            for (int i = startGridPoint.gridPosition.x + range - 1; i >= startGridPoint.gridPosition.x - range; i--)
            {
                for (int j = startGridPoint.gridPosition.y - range; j <= startGridPoint.gridPosition.y + range; j++)
                {
                    Node currentNode = GetPointOnGrid(new Vector2(i, j));
                    if (currentNode != null && currentNode.canTraverse)
                        validSpots.Add(new Vector2(i, j));
                }
            }
            return validSpots;
        }

        public List<Vector2> Calculate(Vector2 startPosition, Vector2 endPosition, bool allowDiagonal = false)
        {
            ResetGrid();
            HashSet<Node> openList = [];
            HashSet<(int, int)> closedTable = [];

            Node? startGridPoint = GetPointOnGrid(startPosition);
            if (!startGridPoint.canTraverse)
            {
                int i = 1;
                while (!startGridPoint.canTraverse)
                {
                    Vector2 newPosition = startPosition + new Vector2(i, 0);
                    startGridPoint = GetPointOnGrid(newPosition);
                    if (startGridPoint != null && startGridPoint.canTraverse)
                        break;

                    newPosition = startPosition + new Vector2(-i, 0);
                    startGridPoint = GetPointOnGrid(newPosition);
                    if (startGridPoint != null && startGridPoint.canTraverse)
                        break;

                    i++;
                }
            }
            Node endGridPoint = GetPointOnGrid(endPosition);
            if (startGridPoint == null || endGridPoint == null)
                return null;

            if (!startGridPoint.canTraverse || !endGridPoint.canTraverse)
                return null;

            grid = GenerateWeights(endGridPoint);
            openList.Add(startGridPoint);

            while (openList.Count > 0)
            {
                Node currentNode = openList.OrderBy(x => x.fCost).First();

                if (currentNode.gridPosition == endGridPoint.gridPosition)
                {
                    List<Vector2> resultingPath = ReconstructPath(currentNode);
                    resultingPath.Add(endPosition);
                    resultingPath.RemoveAt(0);
                    resultingPath.Insert(0, startPosition);
                    return resultingPath;
                }

                openList.Remove(currentNode);
                closedTable.Add(currentNode.gridPosition);

                // Explore neighboring nodes
                for (int i = currentNode.gridPosition.x - 1; i <= currentNode.gridPosition.x + 1; i++)
                {
                    for (int j = currentNode.gridPosition.y - 1; j <= currentNode.gridPosition.y + 1; j++)
                    {
                        if (!allowDiagonal && currentNode.gridPosition.x != i && currentNode.gridPosition.y != j || !IsPositionWithinGridBounds(i, j) || !grid[i, j].canTraverse || closedTable.Contains((i, j)))
                            continue;

                        float gCost = currentNode.gCost + GetDistance(currentNode, grid[i, j]);

                        if (!openList.Contains(grid[i, j]) || gCost < grid[i, j].gCost)
                        {
                            grid[i, j].parent = currentNode;
                            grid[i, j].gCost = gCost;
                            grid[i, j].hCost = GetGridPointWeight(i, j);
                            grid[i, j].fCost = gCost + grid[i, j].hCost;

                            if (!openList.Contains(grid[i, j]))
                                openList.Add(grid[i, j]);
                        }
                    }
                }
            }

            return null;
        }

        private List<Vector2> ReconstructPath(Node gridPoint)
        {
            List<Vector2> resultPath = new();
            Node currentNode = gridPoint;
            while (currentNode != null)
            {
                resultPath.Add(new Vector2(currentNode.gridPosition.x, currentNode.gridPosition.y));
                currentNode = currentNode.parent;
            }
            resultPath.Reverse();
            return resultPath;
        }

        private void ResetGrid()
        {
            foreach (Node gridPoint in grid)
            {
                gridPoint.parent = null;
                gridPoint.weight = 0;
                gridPoint.hCost = 0;
                gridPoint.fCost = 0;
                gridPoint.gCost = 0;
            }
        }

        private float GetGridPointWeight(int x, int y)
        {
            return grid[x, y].weight;
        }

        private float GetDistance(Node node1, Node node2)
        {
            return Math.Abs(node1.gridPosition.x - node2.gridPosition.x) + Math.Abs(node1.gridPosition.y - node2.gridPosition.y);
        }

        private Node? GetPointOnGrid(Vector2 position)
        {
            return IsPositionWithinGridBounds((int)position.X, (int)position.Y) ? grid[(int)position.X, (int)position.Y] : null;
        }

        private bool IsPositionWithinGridBounds(int x, int y)
        {
            return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
        }

        private Node[,] GenerateWeights(Node startNode)
        {
            Node[,] result = new Node[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    int weight = Math.Abs(startNode.gridPosition.x - i) + Math.Abs(startNode.gridPosition.y - j);
                    result[i, j] = new Node(grid[i, j].canTraverse, weight, (i, j));
                }
            }
            return result;
        }

        private Node[,] GenerateGrid(int[,] collidableObjects)
        {
            Node[,] result = new Node[collidableObjects.GetLength(0), collidableObjects.GetLength(1)];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = new Node(!avoidableObjectIds.Contains(collidableObjects[i, j]), 0, (i, j));
                }
            }

            return result;
        }
    }

}