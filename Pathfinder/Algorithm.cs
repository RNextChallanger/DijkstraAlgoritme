using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        /// <summary>
        /// Find the shortest path between two given nodes.
        /// </summary>
        /// <param name="graph">Graph object containing all the nodes and their edges.</param>
        /// <param name="from">Start node.</param>
        /// <param name="to">End node.</param>
        /// <returns>List of all nodes in the path between the two given nodes.</returns>
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            var openNodes = new List<Node>(); 
            var closedNodes = new List<Node>();
            var finished = false;
            Node node = null;

            // 0. Adding start node to the list of open nodes:
            openNodes.Add(from);

            // 1. Keep looping over all open nodes:
            while(openNodes.Count > 0)
            {
                // 2. Grabbing node with the lowest cost:
                node = openNodes.OrderBy(n => n).First();

                // 3. Check if we have reached the to node:
                if(node == to)
                {
                    finished = true;

                    break;
                }

                // 4. Checking path of all edges of current node:
                var edgeNodes = graph.Edges.Where(x => x.From == node);
                
                foreach (var edge in edgeNodes)
                {
                    var edgeNode = edge.To;

                    // Checking if node was already passed:
                    if(closedNodes.Contains(edgeNode))
                    {
                        continue;
                    }
                    
                    // Calculating new gCost:
                    var newGCost = node.gCost + edge.Weight;

                    // Checking if node is already in open nodes list:
                    if (openNodes.Contains(edgeNode))
                    {
                        var openNode = openNodes.Find(x => x.ID == edgeNode.ID);

                        // Comparing gCost:
                        if(openNode.gCost > newGCost)
                        {
                            openNode.SetGCost(newGCost);
                            openNode.SetParent(node);
                        }

                        continue;
                    }

                    // If node isnt in either list, just update its gcost and parent and add it to list of open nodes:
                    edgeNode.SetGCost(newGCost);
                    edgeNode.SetParent(node);

                    openNodes.Add(edgeNode);
                }

                // 5. Marking current node as vistied:
                closedNodes.Add(node);
                openNodes.Remove(node);
            }

            // 6. Checking if we actually found the shortest distance:
            if(!finished)
            {
                Console.WriteLine("Unable to find shortest distance....");

                return Enumerable.Empty<Node>();
            }

            // 7. Logging total distance:
            Console.WriteLine($"Total distance between start and end node: {node.gCost}.");

            // 8. Reconstructing the path:
            return RecreatePathFromFinalNode(node);
        }

        /// <summary>
        /// Reconstruct the path by looking at the parent tree of the final node.
        /// </summary>
        /// <param name="node">The final destination node from the algorithm.</param>
        /// <returns>List of all nodes in the path, starting with the end node and ending with the start node.</returns>
        public static IEnumerable<Node> RecreatePathFromFinalNode(Node node)
        {
            var path = new List<Node>();

            while (node.parent != null)
            {
                path.Add(node);

                node = node.parent;
            }

            path.Add(node);

            return path;
        }
    }
}
