using System;
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

    internal class Node : IComparable<Node>
    {
        public int ID { get; private set; }
        public double gCost { get; private set; } //Cost start node to current node.
        public double hCost { get; private set; } //Cost from current node to destination node.
        public double fCost { get { return gCost + hCost; } } //Total cost.
        public Node parent { get; private set; }

        public Node(int ID)
        {
           this.ID = ID;

           gCost = double.MaxValue;
           hCost = double.MaxValue;
        }

        public int CompareTo(Node other)
        {
            var c = fCost.CompareTo(other.fCost);

            if(c == 0)
            {
                c = hCost.CompareTo(other.hCost);   
            }

            return c;
        }

        public void SetParent(Node parent)
        {
            this.parent = parent;
        }

        public void SetGCost(double gCost)
        {
            this.gCost = gCost;
        }

        public void SetHCost(double hCost)
        {
            this.hCost = hCost;
        }
    }

    internal class Edge
    {
        public Node From, To;
        public double Weight;
    }
}