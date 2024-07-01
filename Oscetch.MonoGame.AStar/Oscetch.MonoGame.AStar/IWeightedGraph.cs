using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Oscetch.MonoGame.AStar
{
    public interface IWeightedGraph
    {
        bool Intersects(Vector2 a, Vector2 b);
        double Cost(Vector2 a, Vector2 b);
        IEnumerable<Vector2> PassableNeighbors(Vector2 id);

    }
}
