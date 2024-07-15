using System.Drawing;

namespace Pathfinder
{
    internal class Program
    {
        private static void Main()
        {
            // var mapImage = new Bitmap("Map.png");
            var mapImage = new Bitmap("LargeMap.png");

            var map = Map.Read(mapImage);
            
            var path = Algorithm.ShortestPath(
                map.Graph, 
                map.StartingPosition, 
                map.TargetPosition);

            var result = map.DrawPath(path);

            new ShowResult(result).ShowDialog();
        }
    }
}
