using System.Text;

namespace Advent.Tests;


public class Day10Tests {
    [Theory]
    [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 2)]
    [InlineData("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 3)]
    [InlineData("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 2)]
    public void Check_ConfigureMachine(string input, int expected) {
        // Arrange.
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<MachineSchematic> schematics = Files.LoadMachineSchematics(reader);
        MachineWiringStarter sut = new(6, 10000, true);


        // Act.
        long result = sut.Configure(schematics);


        // Assert.
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}", 10)]
    [InlineData("[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}", 12)]
    [InlineData("[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}", 11)]
    public void Check_ConfigureJoltage(string input, int expected) {
        // Arrange.
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<MachineSchematic> schematics = Files.LoadMachineSchematics(reader);
        MachineJoltageStarter sut = new(true);


        // Act.
        long result = sut.Configure(schematics);


        // Assert.
        Assert.Equal(expected, result);
    }
}
