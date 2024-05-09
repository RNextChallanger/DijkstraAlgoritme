namespace Pathfinder {

    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Algorithm {
        private static readonly Dictionary<Node, NodeData> NodeWeights = new Dictionary<Node, NodeData>();
        private static readonly HashSet<Node> VisitedNodes = new HashSet<Node>();

        /// <summary>
        /// Returns the shortest path using the 'Weighted Graph' Algorithm
        /// </summary>
        /// <param name="graph">A graph containing all the nodes</param>
        /// <param name="from">The starting node</param>
        /// <param name="to">The desired destination node</param>
        /// <returns>A list of nodes, containing both start and end node.</returns>
        /// <exception cref="InvalidOperationException">No route could be found to the <see cref="to"/> node</exception>
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to) {
            CalculateNodeWeights(graph, from);
            return CalculatePath(to);
        }
        
        /// <summary>
        /// Calculates the weights of all reachable nodes and add them to <see cref="NodeWeights"/>
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="from"></param>
        private static void CalculateNodeWeights(Graph graph, Node from) {
            
            // Add starting node
            NodeData currentNode = new NodeData(from);
            NodeWeights.Add(from, currentNode);
            
            // Scan all reachable neighbours from the current node. Then select a new node with the lowest TotalWeight.
            while (currentNode != null) {
                ScanNodeNeighbors(graph, currentNode);
                VisitedNodes.Add(currentNode.Node);
                currentNode = NodeWeights.Values.Where(x => !VisitedNodes.Contains(x.Node)).OrderBy(x => x.TotalWeight).FirstOrDefault();
            }
        }

        private static void ScanNodeNeighbors(Graph graph, NodeData from) {
            foreach ((double weight, Node neighbor) in graph.Neighbors(from.Node)) {
                if (VisitedNodes.Contains(from.Node)) continue;
                
                bool exists = NodeWeights.TryGetValue(neighbor, out NodeData current);

                // Node is not visited earlier, add to NodeWeights
                if (!exists) {
                    NodeWeights.Add(neighbor, new NodeData(neighbor) { TotalWeight = weight, PreviousNode = from.Node });
                    continue;
                } 
                
                // Node is already visited, update if new weight is lower
                double newWeight = weight + from.TotalWeight;

                if (current.TotalWeight < newWeight) continue;
                current.TotalWeight = newWeight;
                current.PreviousNode = from.Node;
            }
        }
        
        /// <summary>
        /// Calculates the shortest path based on the weights in <see cref="NodeWeights"/>
        /// </summary>
        /// <param name="to"></param>
        /// <returns>A list of nodes </returns>
        /// <exception cref="InvalidOperationException">Throws when the node is not in <see cref="NodeWeights"/>. This happens when there is no path to the destination node.</exception>
        private static IEnumerable<Node> CalculatePath(Node to) {
            List<NodeData> sortedNodeWeights = NodeWeights.Values.OrderBy(x => x.TotalWeight).ToList();
            
            NodeData bestNode = sortedNodeWeights.FirstOrDefault(x => x.Node == to);
            if (bestNode == null) throw new InvalidOperationException("No route to node 'to' was found");
            
            List<Node> shortestPath = new List<Node>();
            while (bestNode != null) {
                // Note we need to insert in the begin of the list, because we track the route backwards via the PreviousNode.
                shortestPath.Insert(0, bestNode.Node);
                bestNode = sortedNodeWeights.FirstOrDefault(x => x.Node == bestNode.PreviousNode);
            }

            return shortestPath;
        }
        
        /// <summary>
        ///     Data object that always contains the lowest possible <see cref="TotalWeight" /> to reach the
        ///     <see cref="Node" />. The <see cref="PreviousNode" /> describes how to get there.
        /// </summary>
        private sealed class NodeData {
            public Node Node { get; }
            public double TotalWeight { get; set; }
            public Node PreviousNode { get; set; }
            
            public NodeData(Node node) {
                this.Node = node;
            }

        }
    }
}
