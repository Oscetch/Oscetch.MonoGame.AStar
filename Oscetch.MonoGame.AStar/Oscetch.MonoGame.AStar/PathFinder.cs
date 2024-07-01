﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Oscetch.MonoGame.AStar
{
    public class PathFinder
    {
        private readonly IWeightedGraph _graph;

        public PathFinder(IWeightedGraph graph)
        {
            _graph = graph;
        }

        public List<Vector2> Find(Vector2 start, Vector2 end)
        {
            var cameFrom = new Dictionary<Vector2, Vector2>();
            var costSoFar = new Dictionary<Vector2, double>();
            var frontier = new PriorityQueue<Vector2, double>();

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while(frontier.Count > 0)
            {
                var current = frontier.Dequeue();


                if (_graph.Intersects(current, end)) break;

                foreach (var nextLocation in _graph.PassableNeighbors(current))
                {
                    var newCost = costSoFar[current] + _graph.Cost(current, nextLocation);
                    if (!costSoFar.ContainsKey(nextLocation) || newCost < costSoFar[nextLocation])
                    {
                        costSoFar[nextLocation] = newCost;
                        frontier.Enqueue(nextLocation, newCost + Heuristic(nextLocation, end));
                        cameFrom[nextLocation] = current;
                    }
                }
            }

            var path = new List<Vector2>();
            var location = end;
            while (location != start)
            {
                path.Add(location);
                location = cameFrom[location];
            }
            path.Reverse();
            return path;
        }

        private static double Heuristic(Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}