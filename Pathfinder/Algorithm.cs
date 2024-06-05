using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        /// <summary>
        /// A comparer that handles duplicate keys by treating equal keys as greater.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys to compare.</typeparam>
        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                    return result;
            }
        }


        /// <summary>
        /// Finds the shortest path between two nodes in a graph using Dijkstra's algorithm.
        /// </summary>
        /// <param name="graph">The graph containing the nodes and edges.</param>
        /// <param name="from">The starting node.</param>
        /// <param name="to">The destination node.</param>
        /// <returns>A tuple containing the shortest path and the explored nodes.</returns>

        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            foreach (Node node in graph.Nodes)
            {
                node.Cost = double.MaxValue;
                node.Explored = false;
                node.Parent = null;
            }

            from.Cost = 0;

            SortedList<double, Node> queue = new SortedList<double, Node>(new DuplicateKeyComparer<double>());

            // Prime the queue with the start node
            queue.Add(0, from);

            while (queue.Count > 0)
            {
                var curNode = queue.FirstOrDefault();
                queue.RemoveAt(0);

                // If a node is explored we can skip it
                if (curNode.Value.Explored == true) { continue; }

                foreach (Edge edge in graph.Edges.Where(e => e.From == curNode.Value).OrderBy(e => e.Weight))
                {
                    double newCost = curNode.Key + edge.Weight;

                    if (newCost < edge.To.Cost)
                    {
                        // We found an shorter route
                        edge.To.Cost = newCost;
                        edge.To.Parent = curNode.Value;

                        queue.Add(edge.To.Cost, edge.To);
                    }

                    if (edge.To == to)
                    {
                        // We found the shortest path, we can stop now
                        queue = new SortedList<double, Node>();
                        break;
                    }
                }

                curNode.Value.Explored = true;
            }

            if (to.Parent == null)
            {
                // There is no valid route available
                return new List<Node>();
            }

            return LinkedNodesToList(to);
        }

        /// <summary>
        /// Converts a linked list of nodes to a list of nodes.
        /// </summary>
        /// <param name="node">The final node in the path.</param>
        /// <returns>A list of nodes representing the path.</returns>
        public static IEnumerable<Node> LinkedNodesToList(Node node)
        {
            var nodeList = new List<Node>();

            while (node != null)
            {
                // Add the current node to the list
                nodeList.Add(node);

                // Update the currentNode to the parrent of this node
                node = node.Parent;
            }

            nodeList.Reverse();
            return nodeList;
        }
    }
}
