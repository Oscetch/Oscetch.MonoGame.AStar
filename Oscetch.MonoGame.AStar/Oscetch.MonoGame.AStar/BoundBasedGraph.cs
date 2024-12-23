using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.MonoGame.AStar
{
    public class BoundBasedGraph(Point size, Vector2 origin, int smallestStep, Func<Vector2, Vector2, bool> isPassable) : IWeightedGraph
    {
        private readonly Point _size = size;
        private readonly Vector2 _origin = origin;
        private readonly int _smallestStep = smallestStep;
        private readonly Func<Vector2, Vector2, bool> _isPassable = isPassable;

        public BoundBasedGraph(Point size, Func<Vector2, Vector2, bool> isPassable) : this(size, Math.Min(size.X, size.Y), isPassable) { }
        public BoundBasedGraph(Point size, int smallestStep, Func<Vector2, Vector2, bool> isPassable) : this(size, size.ToVector2() / 2f, smallestStep, isPassable) { }

        public double Cost(Vector2 a, Vector2 b)
        {
            var distance = Vector2.Distance(a, b);
            return distance / _smallestStep;
        }

        public bool Intersects(Vector2 position, Vector2 target)
        {
            var withoutOriginPosition = (position - _origin).ToPoint();
            var bounds = new Rectangle(withoutOriginPosition, _size);
            return bounds.Contains(target);
        }

        public IEnumerable<Vector2> PassableNeighbors(Vector2 id)
        {
            return GetPossiblePositions(id).Where(x => _isPassable(id, x));
        }

        private IEnumerable<Vector2> GetPossiblePositions(Vector2 id)
        {
            var sizeVector = _size.ToVector2();

            yield return id - new Vector2(_smallestStep); // top left
            yield return id - new Vector2(_smallestStep, 0); // left
            yield return id - new Vector2(_smallestStep, -_smallestStep); // bottom left

            yield return id - new Vector2(0, _smallestStep); // top
            yield return id + new Vector2(0, _smallestStep); // bottom

            yield return id + sizeVector; // top right
            yield return id + new Vector2(_smallestStep, 0); // right
            yield return id + new Vector2(_smallestStep, -_smallestStep); // bottom right
        }
    }
}
