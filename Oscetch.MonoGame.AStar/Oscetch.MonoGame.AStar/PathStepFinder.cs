using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Oscetch.MonoGame.AStar
{
    public class PathStepFinder(IWeightedGraph graph)
    {
        private readonly IWeightedGraph _graph = graph;

        public double MaxMovementCost { get; set; } = 100d;

        private Dictionary<Vector2, Vector2> _cameFrom = new();
        private Dictionary<Vector2, double> _costSoFar = new();
        private PriorityQueue<Vector2, double> _frontier = new();
        private Vector2 _start;
        private Vector2 _end;
        private Vector2 _currentLocation;
        private bool _isFinished;

        public bool IsStarted { get; private set; }

        public void Start(Vector2 start, Vector2 end)
        {
            _cameFrom = [];
            _costSoFar = [];
            _frontier = new PriorityQueue<Vector2, double>();
            _start = start;
            _end = end;
            _frontier.Enqueue(start, 0);
            _cameFrom[start] = start;
            _costSoFar[start] = 0;
            _isFinished = false;
            IsStarted = true;
        }

        public List<Vector2> Run()
        {
            if (_isFinished) return Path();

            var current = _frontier.Dequeue();
            _currentLocation = current;

            if (_graph.Intersects(current, _end))
            {
                _cameFrom[_end] = current;
                _isFinished = true;
                return Path();
            }

            foreach (var nextLocation in _graph.PassableNeighbors(current))
            {
                var newCost = _costSoFar[current] + _graph.Cost(current, nextLocation);
                if (newCost > MaxMovementCost)
                {
                    continue;
                }
                if (!_costSoFar.ContainsKey(nextLocation) || newCost < _costSoFar[nextLocation])
                {
                    _costSoFar[nextLocation] = newCost;
                    _frontier.Enqueue(nextLocation, newCost + Heuristic(nextLocation, _end));
                    _cameFrom[nextLocation] = current;
                }
            }

            return Path();
        }

        private List<Vector2> Path()
        {
            var path = new List<Vector2>();
            var location = _currentLocation;
            while (location != _start)
            {
                path.Add(location);
                location = _cameFrom[location];
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
