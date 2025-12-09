namespace Advent;


public class Day9 : IChallenge {
    // Constants.
    public const string FILE_PATH = "data/Day9.txt";


    // External utility methods.
    public void Run() {
        List<Vector2> tiles = Files.LoadTiles(FILE_PATH);
        FloorTiler tiler = new();

        long size = tiler.FindLargestRectangle(tiles, false);
        Console.WriteLine($"Largest rectangle (any): {size}");

        size = tiler.FindLargestRectangle(tiles, true);
        Console.WriteLine($"Largest rectangle (red/green only): {size}");
    }
}
