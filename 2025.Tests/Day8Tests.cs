using System.Text;

namespace Advent.Tests;


public class Day8Tests {
    [Fact]
    public void CheckThreeShortestNetworks() {
        // Arrange.
        string input = "162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<Vector3> boxes = Files.LoadJunctionBoxes(reader);
        JunctionBoxWirer sut = new();


        // Act.
        JunctionLinks[] links = sut.CalculateLinks(boxes);
        List<JunctionNetwork> networks = sut.WireNetworks(links, 10);
        long result = sut.ComputeNetworkSize(networks, 3);


        // Assert.
        Assert.Equal(40, result);
    }
}
