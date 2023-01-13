using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            ResetNodes(graph);

            //Use from node to start
            var currentNode = from;

            //Set from node delta to 0
            currentNode.delta = 0;

            while (currentNode != to && currentNode!=null)
            {
                currentNode = SelectNode(graph);
                SetDeltas(currentNode, graph);
            }

            return BacktrackPath(graph, to, from);
        }

        public static void ResetNodes(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.delta = double.MaxValue;
                node.visited = false;
            }
        }

        /// <summary>
        /// Sets the new deltas for the neighbors of the current node
        /// </summary>
        public static void SetDeltas(Node currentNode, Graph graph)
        {
            currentNode.visited = true;
            var neighborsInfo = graph.Neighbors(currentNode);
            foreach (var neighbor in neighborsInfo)
            {
                if(neighbor.Neighbor.visited == false)
                neighbor.Neighbor.delta = currentNode.delta + neighbor.Weight;
            }
        }

        /// <summary>
        /// Select the next node to iterate from
        /// </summary>
        public static Node SelectNode(Graph graph)
        {
            return graph.Nodes.Where(n => !n.visited).OrderBy(n => n.delta).First();
        }

        /// <summary>
        /// Find the shortest path by backtracking the lowest deltas from end to start
        /// </summary>
        public static IEnumerable<Node> BacktrackPath(Graph graph, Node endNode, Node startNode)
        {
            List<Node> path = new List<Node>();
            var currentNode = endNode;
            path.Add(currentNode);
            while (currentNode != startNode)
            {
                currentNode = graph.Neighbors(currentNode).Select(ni => ni.Neighbor).OrderBy(n=>n.delta).First();
                path.Add(currentNode);
            }
            return path;
        }
    }
}
