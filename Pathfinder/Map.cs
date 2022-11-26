using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;

namespace Pathfinder
{
    internal class Map
    {
        public Map(Bitmap bitmap, Graph graph, Node startingPosition, Node targetPosition, IReadOnlyDictionary<Node, Vec2> nodeCoordinates)
        {
            Bitmap = bitmap;
            Graph = graph;
            StartingPosition = startingPosition;
            TargetPosition = targetPosition;
            NodeCoordinates = nodeCoordinates;
        }

        public Bitmap Bitmap { get; }
        public Graph Graph { get; }
        public Node StartingPosition { get; }
        public Node TargetPosition { get; }
        public IReadOnlyDictionary<Node, Vec2> NodeCoordinates { get; }

        public Bitmap DrawPath(IEnumerable<Node> path)
        {
            var withPath = (Bitmap)Bitmap.Clone();

            var pointsToColor = path
                .Skip(1)
                .SkipLast(0) // skip last veranderd van 1 naar 0
                .Select(a => NodeCoordinates[a]);

            foreach (var point in pointsToColor)
            {
                withPath.SetPixel(point.X, point.Y, Color.Blue);
            }

            return withPath;
        }

        public static Map Read(Bitmap bitmap)
        {
            var nodeByCoord = new Dictionary<Vec2, Node>();

            var startColor = Color.FromArgb(0, 255, 0).ToArgb();
            var targetColor = Color.FromArgb(255, 0, 0).ToArgb();
            var wallColor = Color.Black.ToArgb();

            Node start = null, target = null;

            foreach (var y in Enumerable.Range(0, bitmap.Height))
            {
                foreach (var x in Enumerable.Range(0, bitmap.Width))
                {
                    var pixel = bitmap.GetPixel(x, y).ToArgb();
                    
                    if (pixel == wallColor)
                        continue;
                    
                    var node = new Node();
                    var position = new Vec2(x, y);

                    nodeByCoord[position] = node;
                    
                    if (pixel == startColor)
                        start = node;
                    else if (pixel == targetColor)
                        target = node;
                }
            }

            var edges = new List<Edge>();

            foreach (var kvp in nodeByCoord)
            {
                var coord = kvp.Key;
                var node = kvp.Value;

                foreach (var offset in NeighborOffsets)
                {
                    if (!nodeByCoord.TryGetValue(coord + offset, out var neighbor))
                        continue;

                    edges.Add(new Edge
                    {
                        From = node,
                        To = neighbor,
                        Weight = offset.Length
                    });
                }
            }

            return new Map(
                bitmap,
                new Graph(nodeByCoord.Values, edges),
                start,
                target,
                nodeByCoord.ToDictionary(a => a.Value, a => a.Key));
        }

        private static readonly List<Vec2> NeighborOffsets = new List<Vec2>
        {
            new Vec2(-1, -1), new Vec2( 0, -1), new Vec2( 1, -1),
            new Vec2(-1,  0),                       new Vec2( 1,  0),
            new Vec2(-1,  1), new Vec2( 0,  1), new Vec2( 1,  1)
        };
    }
}
