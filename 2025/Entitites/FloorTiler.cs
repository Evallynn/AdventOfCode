namespace Advent;


public class FloorTiler(bool _debug = false) {
    // External utility methods.

    // The solution to part 2 is very similar to that in part 1,
    // but this time we just need to check if any other green tiles
    // or edges between green tiles are contained within our rectangle,
    // and ignore the rectangle if they are.
    public long FindLargestRectangle(List<Vector2> tiles, bool redGreenOnly) {
        // TODO Brute force with minor optimisation of not
        // doing the inverse of each pair, halving the total.
        long largest = long.MinValue;

        for (int i = 0; i < tiles.Count; i++) {
            for (int j = i + 1; j < tiles.Count; j++) {
                int xDiff = Math.Abs(tiles[j].x - tiles[i].x) + 1;
                int yDiff = Math.Abs(tiles[j].y - tiles[i].y) + 1;
                long currSize = (long)xDiff * (long)yDiff;

                if (currSize > largest) {
                    if (redGreenOnly && !IsOnlyRedGreen(i, j, ref tiles)) continue;
                    if (_debug) Console.WriteLine($"New largest: {tiles[i]}->{tiles[j]}");
                    largest = currSize;
                }
            }
        }

        return largest;
    }


    // Class-internal utility methods.
    private static bool IsOnlyRedGreen(int pointAIndex, int pointBIndex, ref List<Vector2> tiles) {
        Vector2 pointA = tiles[pointAIndex];
        Vector2 pointB = tiles[pointBIndex];

        int minX = Math.Min(pointA.x, pointB.x);
        int maxX = Math.Max(pointA.x, pointB.x);
        int minY = Math.Min(pointA.y, pointB.y);
        int maxY = Math.Max(pointA.y, pointB.y);

        for (int i = 0; i < tiles.Count; i++) {
            // Get the next tile from the current one so we can make an edge.
            Vector2 currTile = tiles[i];
            Vector2 nextTile = tiles.Last() == currTile ? tiles[0] : tiles[i + 1];


            // Catch diagonal connections in the input data, just in case it's bad.
            if (currTile.x != nextTile.x && currTile.y != nextTile.y)
                throw new ArgumentException($"Diagonal connection found: {currTile}->{nextTile}");


            // Ignore if either tile is one of our points.
            if (currTile == pointA || currTile == pointB || nextTile == pointA || nextTile == pointB) continue;


            // Check if any point on the edge are contained within our rectangle.
            if (currTile.x == nextTile.x) {
                // It's a vertical line. Start by ignoring it if the 'x' isn't within our rectangle.
                if (currTile.x <= minX || currTile.x >= maxX) continue;

                // If the 'x' is inside, check each 'y' to see if one false within the rectangle.
                int minEdgeY = Math.Min(currTile.y, nextTile.y);
                int maxEdgeY = Math.Max(currTile.y, nextTile.y);

                for (int y = minEdgeY; y <= maxEdgeY; y++)
                    if (y > minY && y < maxY) return false;
            }
            else {
                // It's a horizontal line. Start by ignoring it if the 'y' isn't within our rectangle.
                if (currTile.y <= minY || currTile.y >= maxY) continue;

                // If the 'y' is inside, check each 'x' to see if one false within the rectangle.
                int minEdgeX = Math.Min(currTile.x, nextTile.x);
                int maxEdgeX = Math.Max(currTile.x, nextTile.x);

                for (int x = minEdgeX; x <= maxEdgeX; x++)
                    if (x > minX && x < maxX) return false;
            }
        }

        return true;
    }
}
