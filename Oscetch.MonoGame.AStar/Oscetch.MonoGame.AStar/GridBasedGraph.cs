using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.MonoGame.AStar
{
    internal class GridBasedGraph(Vector2 gridCellSize, Func<Vector2, Vector2, bool> isPassable) : IWeightedGraph
    {
        private readonly Vector2 _gridCellSize = gridCellSize;
        private readonly Func<Vector2, Vector2, bool> _isPassable = isPassable;

        public Vector2 FitPositionToGrid(Vector2 position) => position - new Vector2(position.X % _gridCellSize.X, position.Y % _gridCellSize.Y);

        public double Cost(Vector2 a, Vector2 b)
        {
            return 1;
        }

        public bool Intersects(Vector2 a, Vector2 b)
        {
            return a == b;
        }

        public IEnumerable<Vector2> PassableNeighbors(Vector2 id)
        {
            return GetNeighbors(id).Where(x => _isPassable(id, x));
        }

        private IEnumerable<Vector2> GetNeighbors(Vector2 id)
        {
            yield return id - _gridCellSize; // top left
            yield return id - new Vector2(_gridCellSize.X, 0); // left
            yield return id - new Vector2(_gridCellSize.X, -_gridCellSize.Y); // bottom left

            yield return id - new Vector2(0, _gridCellSize.Y); // top
            yield return id + new Vector2(0, _gridCellSize.Y); // bottom

            yield return id + new Vector2(_gridCellSize.X, -_gridCellSize.Y); // top right
            yield return id + new Vector2(_gridCellSize.X, 0); // right
            yield return id + _gridCellSize; // bottom right
        }
    }
}
