# Oscetch.MonoGame.AStar
Small path finding project. Based of https://github.com/manbeardgames/monogame-astar-pathfinding

## Usage

```csharp
// assuming the world has walls and stuff in it.
var world = List<Rectangle>();
var npc = new Rectangle(50, 50, 10, 10);
var target = new Vector2(1000, 1000);
var graph = new BoundBasedGraph(npc.Size, testPoint => world.All(wall => !wall.Contains(testPoint)));
var pathFinder = new PathFinder(graph);

var path = pathFinder.Find(npc.Location.ToVector2(), target);
```
