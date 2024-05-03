using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            // create empty dicts for node weight and shortest route
            Dictionary<Node, double> shortestKnownNodeWeight = new Dictionary<Node, double>();
            Dictionary<Node, Node> shortestWeightPreviousNodes = new Dictionary<Node, Node>();

            // create sortedset for sorting weight
            // sort based on Node Id if weight is same
            SortedSet<(double, Node)> priorityQueue = new SortedSet<(double, Node)>(
                Comparer<(double, Node)>.Create((a, b) => 
                    a.Item1.CompareTo(b.Item1) != 0  ? a.Item1.CompareTo(b.Item1) : a.Item2.Id.CompareTo(b.Item2.Id)
                )
             );

            // if Node does not use Id property use the following queue instead
            /*
                 SortedSet<(double, Node)> priorityQueue = new SortedSet<(double, Node)>(
                    Comparer<(double, Node)>.Create((a, b) => 
                        a.Item1.CompareTo(b.Item1) != 0  ? a.Item1.CompareTo(b.Item1) : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode())
                    )
                 );
             */

            // set all nodes as empty
            foreach (Node node in graph.Nodes) 
            {
                shortestKnownNodeWeight[node] = double.PositiveInfinity;
                shortestWeightPreviousNodes[node] = null;
            }

            // add starting node calculations
            // make sure priority queue has at least 1 value for the loop
            // if nodes are not connected they will not be found
            shortestKnownNodeWeight[from] = 0;
            priorityQueue.Add((0, from));

            // loop through all nodes until to node is found
            while (priorityQueue.Count > 0)
            {
                // retrieve shortest unvisited Node and current shortest known weight to that Node
                (double currentWeight, Node currentNode) = priorityQueue.Min;

                // remove current Node from unvisited list
                priorityQueue.Remove(priorityQueue.Min);

                // return from loop if to node is found as neighbor of the last checked Node
                if (currentNode == to)
                {
                    break;
                }

                // check all paths to neighboring nodes
                foreach (Edge edge in graph.Edges.Where(e => e.From == currentNode))
                {
                    Node neighbor = edge.To;
                    double FromToCurrentNeighborWeight = currentWeight + edge.Weight;

                    // replace shortest weight with newly found weighted Node
                    if (FromToCurrentNeighborWeight < shortestKnownNodeWeight[neighbor])
                    {
                        priorityQueue.Remove((shortestKnownNodeWeight[neighbor], neighbor));
                        priorityQueue.Add((FromToCurrentNeighborWeight, neighbor));
                        shortestKnownNodeWeight[neighbor] = FromToCurrentNeighborWeight;
                        shortestWeightPreviousNodes[neighbor] = currentNode;
                    }
                }
            }


            // check if to node is found after looping
            bool toNodeIsFound = shortestWeightPreviousNodes.ContainsKey(to);

            if (toNodeIsFound)
            {
                // reverse shortestWeightPreviousNodes by making reversable list
                List<Node> shortestWeightNodePath = new List<Node>();

                Node currentPathNode = to;

                // loop through the nodes from end to start
                while (currentPathNode != null)
                {
                    shortestWeightNodePath.Add(currentPathNode);
                    currentPathNode = shortestWeightPreviousNodes[currentPathNode];
                }

                // reverse list so from is first node
                shortestWeightNodePath.Reverse();

                //Console.WriteLine(shortestWeightNodePath.Count);
                Console.WriteLine("To Node is found");
                return shortestWeightNodePath;
            }
            else
            {
                Console.WriteLine("To Node is not found");
                return Enumerable.Empty<Node>();
            }
        }
    }
}
