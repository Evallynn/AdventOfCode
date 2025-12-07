using System.Text;

namespace Advent.Tests;


public class Day7Tests {
    [Fact]
    public void CheckManifolds_Classic() {
        // Arrange.
        string input = ".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        TachyonManifold manifolds = Files.LoadTachyonManifolds(reader);
        ManifoldReflector sut = new(true);


        // Act.
        int totalSplits = sut.ResolveClassic(manifolds);


        // Assert.
        Assert.Equal(21, totalSplits);
    }

    [Fact]
    public void CheckManifolds_Quantum() {
        // Arrange.
        string input = ".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        TachyonManifold manifolds = Files.LoadTachyonManifolds(reader);
        ManifoldReflector sut = new(true);


        // Act.
        long totalTimelines = sut.ResolveQuantum(manifolds);


        // Assert.
        Assert.Equal(40, totalTimelines);
    }
}