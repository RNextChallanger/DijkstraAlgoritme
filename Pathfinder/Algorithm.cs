using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            var queue = new List<Node>();
            from.Distance = 0d;
            queue.Add(from);
            var startToEndDistance = double.PositiveInfinity;

            while (queue.Any())
            {
                queue = queue.OrderBy(x => x.Distance).ToList();
                var current = queue.First();
                queue.Remove(current);

                current.Visited = true;

                if (current.Equals(to))
                {
                    startToEndDistance = current.Distance;
                    Console.WriteLine($"Distance shortest path: {current.Distance}");
                    break;
                }

                foreach (var (Weight, Neighbor) in graph.Neighbors(current))
                {
                    if (Neighbor.Visited)
                    {
                        continue;
                    }

                    if (current.Distance + Weight < Neighbor.Distance && current.Distance + Weight < startToEndDistance)
                    {
                        Neighbor.Distance = current.Distance + Weight;
                        Neighbor.Parent = current;

                        if (!queue.Contains(Neighbor))
                        {
                            queue.Add(Neighbor);
                        }
                    }
                }
            }

            return ReconstructPath(from, to);
        }

        private static IEnumerable<Node> ReconstructPath(Node from, Node to)
        {
            var path = new List<Node>();
            var current = to;
            while (current != from)
            {
                if (current.Parent != null)
                {
                    path.Add(current);
                    current = current.Parent;
                }
                else
                {
                    break; // If there is no current.Parent then reached node 'from', exit the loop
                }
            }

            path.Add(from);
            path.Reverse();
            return path;
        }
    }
}
