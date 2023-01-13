using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal class Graph
    {
        public IReadOnlyList<Node> Nodes { get; }
        public IReadOnlyList<Edge> Edges { get; }

        public Graph(IEnumerable<Node> nodes, IEnumerable<Edge> edges)
        {
            Nodes = nodes.ToList();
            Edges = edges.ToList();

            List<(double, Node)> GetNeighborList(Node node)
            {
                if (!_neighborMapping.TryGetValue(node, out var list))
                {
                    list = new List<(double, Node)>();
                    _neighborMapping.Add(node, list);
                }

                return list;
            }

            foreach (var edge in Edges)
            {
                var fromList = GetNeighborList(edge.From);
                fromList.Add((edge.Weight, edge.To));

                var toList = GetNeighborList(edge.To);
                toList.Add((edge.Weight, edge.From));
            }
        }

        private readonly Dictionary<Node, List<(double, Node)>> _neighborMapping =
            new Dictionary<Node, List<(double, Node)>>();

        public IEnumerable<(double Weight, Node Neighbor)> Neighbors(Node node)
        {
            return _neighborMapping[node];
        }
    }

    internal class Node
    {
        public bool visited;
        public double delta;
    }

    internal class Edge
    {
        public Node From, To;
        public double Weight;
    }
}