using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            IComparer<TKey> Members;

            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                    return result;
            }
        }

        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            foreach (Node node in graph.Nodes)
            {
                node.Cost = double.MaxValue;
                node.Explored = false;
            }

            from.Cost = 0;
            from.Explored = false;

            SortedList<double, Node> queue = new SortedList<double, Node>(new DuplicateKeyComparer<double>());

            queue.Add(0, from);

            while (queue.Count > 0)
            {
                var curNode = queue.FirstOrDefault();
                queue.RemoveAt(0);

                // if the node is explored we can skip it
                if (curNode.Value.Explored == true) { continue; }

                foreach (Edge edge in graph.Edges.Where(e => e.From == curNode.Value))
                {
                    // Update de cost in de target node
                    double newCost = curNode.Key + edge.Weight;

                    if (newCost < edge.To.Cost)
                    {
                        // We found an shorter route
                        edge.To.Cost = newCost;
                        edge.To.Parent = curNode.Value;

                        // Voeg de target node toe aan de queue
                        queue.Add(edge.To.Cost, edge.To);
                    }
                }

                // We zijn nu klaar, markeer de current node als explored
                curNode.Value.Explored = true;
            }

            var result = to.Parent;

            return LinkedParentToList(result);

            return Enumerable.Empty<Node>();
        }

        public static IEnumerable<Node> LinkedParentToList(Node node)
        {
            var nodeList = new List<Node>();
            // Add the initial node
            nodeList.Add(node);

            var currentNode = node;

            while (currentNode.Parent != null)
            {
                // Add the current node to the list
                nodeList.Add(currentNode);

                // Update the currentNode to the parrent of this node
                currentNode = currentNode.Parent;
            }

            // Now add the final node
            nodeList.Add(currentNode);
            return nodeList;
        }
    }
}
