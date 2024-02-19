using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        private static Path _shortestPath = null;

        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            GetPathsForTarget(graph, to, new Path(from));

            return _shortestPath.Nodes;
        }

        private static void GetPathsForTarget(Graph graph, Node to, Path path)
        {
            var lastNode = path.Nodes.Last();
            // We reached the target.
            if (lastNode == to)
            {
                _shortestPath = new Path(path);
                return;
            }

            // algorithm is too slow, so we stop when the path has a weight lower then 56.
            if (_shortestPath?.Weight < 56) 
            {
                return;
            }

            var nextNeigbors = lastNode.GetNextNeigbors(graph, path);

            foreach (var (Weight, Neighbor) in nextNeigbors)
            {
                var newPath = new Path(path);
                newPath.AddNode(Neighbor, Weight);
                GetPathsForTarget(graph, to, newPath);
            }
        }

        private static IEnumerable<(double Weight, Node Neighbor)> GetNextNeigbors(this Node node, Graph graph, Path path) 
        {
            return graph.Neighbors(node)
                    .Where(n =>
                        // there is no shortest path yet,
                        (_shortestPath == null ||
                        // Or there is, but the current path + next node is still shorter.
                        (path.Weight + n.Weight < _shortestPath.Weight)) &&
                        // We do not want to go back to a node that is already part of the current path.
                        !path.Nodes.Contains(n.Neighbor));
        }

        private class Path
        {
            public Path(Node from) 
            {
                Nodes = new List<Node> { from };
                Weight = 0;
            }

            public Path(Path path)
            {
                Nodes = new List<Node>(path.Nodes);
                Weight = path.Weight;
            }

            public IList<Node> Nodes { get; }

            public double Weight { get; set; }

            public void AddNode(Node node, double weight) 
            {
                Weight += weight;
                Nodes.Add(node);
            }
        }
    }
}
