using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    internal static class Algorithm
    {
        public static IEnumerable<Node> ShortestPath(Graph graph, Node from, Node to)
        {
            List<Node> shortestPath = new List<Node>();
            List<Node> visitedNodes = new List<Node>();
            Node currentNode;
            // alle nodes begin lengte op infinity zetten, start node lengte op 0
            foreach (Node node in graph.Nodes)
            {
                node.Distance = double.MaxValue;
                node.Visited = false;
            }
            from.Distance = 0;

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                // currentnode is node die niet is bezocht en de kortste distance heeft
                currentNode = graph.Nodes.Where(q => q.Visited == false).OrderBy(n => n.Distance).FirstOrDefault();
                List<Edge> connectedEdges = graph.Edges.Where(q => q.From == currentNode).ToList();
                foreach (Edge edge in connectedEdges)
                {
                    // als huidige lengte groter is als lengte van nieuwe gevonden route wordt lengte geupdate
                    if(edge.To.Distance > edge.Weight + edge.From.Distance)
                    {
                        edge.To.Distance = edge.Weight + edge.From.Distance;
                        edge.To.PathVia = edge.From;
                    }
                }
                // node toegevoegd aan visited
                currentNode.Visited = true;
                visitedNodes.Add(currentNode);
            }

            // kortst mogelijk weg bepalen vanaf eind naar start met behulp van shortest path via
            currentNode = to;
            while (true)
            {
                if (currentNode == from)
                {
                    break;
                }
                shortestPath.Add(currentNode);
                currentNode = currentNode.PathVia;  
            }
            return shortestPath;
        }
    }
}
