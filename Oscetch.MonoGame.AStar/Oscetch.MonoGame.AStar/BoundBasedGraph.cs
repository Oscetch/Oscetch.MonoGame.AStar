using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Oscetch.MonoGame.AStar
{
    internal class BoundBasedGraph : IWeightedGraph
    {
        public Vector2 Origin { get; set; }

        private readonly Point _size;
        private readonly int _smallestStep;
        private readonly Predicate<Vector2> _isPassable;

        public BoundBasedGraph(Point size, Predicate<Vector2> isPassable)
        {
            _size = size;
            _isPassable = isPassable;
            _smallestStep = Math.Min(size.X, size.Y);
        }

        public double Cost(Vector2 a, Vector2 b)
        {
            var distance = Vector2.Distance(a, b);
            return distance / _smallestStep;
        }

        public bool Intersects(Vector2 position, Vector2 target)
        {
            return GetBoundsFromPosition(position).Contains(target);
        }

        public IEnumerable<Vector2> PassableNeighbors(Vector2 id)
        {
            foreach (var position in GetPossiblePositions(id))
            {
                if (_isPassable(position))
                {
                    yield return position;
                }
            } 
        }

        private IEnumerable<Vector2> GetPossiblePositions(Vector2 id)
        {
            var sizeVector = _size.ToVector2();

            yield return id - sizeVector; // top left
            yield return id - new Vector2(sizeVector.X, 0f); // left
            yield return id - new Vector2(sizeVector.X, -sizeVector.Y); // bottom left

            yield return id - new Vector2(0, sizeVector.Y); // top
            yield return id + new Vector2(0, sizeVector.Y); // bottom

            yield return id + sizeVector; // top right
            yield return id + new Vector2(sizeVector.X, 0f); // right
            yield return id + new Vector2(sizeVector.X, -sizeVector.Y); // bottom right
        }

        private Rectangle GetBoundsFromPosition(Vector2 position) => new(ToOriginPoint(position).ToPoint(), _size);
        private Vector2 ToOriginPoint(Vector2 position) => new(position.X + (_size.X * Origin.X), position.Y + (_size.Y * Origin.Y));
    }
}
