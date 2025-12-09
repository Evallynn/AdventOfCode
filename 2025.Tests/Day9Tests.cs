using System.Text;

namespace Advent.Tests;


public class Day9Tests {
    [Fact]
    public void Check_LargestRectangle_Any() {
        // Arrange.
        string input = "7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<Vector2> tiles = Files.LoadTiles(reader);
        FloorTiler sut = new(true);


        // Act.
        long result = sut.FindLargestRectangle(tiles, false);


        // Assert.
        Assert.Equal(50, result);
    }

    [Fact]
    public void Check_LargestRectangle_RedGreen() {
        // Arrange.
        string input = "7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<Vector2> tiles = Files.LoadTiles(reader);
        FloorTiler sut = new(true);


        // Act.
        long result = sut.FindLargestRectangle(tiles, true);


        // Assert.
        Assert.Equal(24, result);
    }
}
