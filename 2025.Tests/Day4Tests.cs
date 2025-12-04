namespace Advent.Tests;


public class Day4Tests {
    [Fact]
    public void MoveRolls_OnePass() {
        // Arrange.
        PaperRollMover sut = new();

        bool[,] input = {
            { false, false, true, true, false, true, true, true, true, false },
            { true, true, true, false, true, false, true, false, true, true },
            { true, true, true, true, true, false, true, false, true, true },
            { true, false, true, true, true, true, false, false, true, false },
            { true, true, false, true, true, true, true, false, true, true },
            { false, true, true, true, true, true, true, true, false, true },
            { false, true, false, true, false, true, false, true, true, true },
            { true, false, true, true, true, false, true, true, true, true },
            { false, true, true, true, true, true, true, true, true, false },
            { true, false, true, false, true, true, true, false, true, false }
        };


        // Act.
        List<Vector2> rollsMoved = sut.MoveRolls(ref input, 3);


        // Assert.
        Assert.Equal(13, rollsMoved.Count);
    }

    [Fact]
    public void MoveRolls_Recursively() {
        // Arrange.
        PaperRollMover sut = new();

        bool[,] input = {
            { false, false, true, true, false, true, true, true, true, false },
            { true, true, true, false, true, false, true, false, true, true },
            { true, true, true, true, true, false, true, false, true, true },
            { true, false, true, true, true, true, false, false, true, false },
            { true, true, false, true, true, true, true, false, true, true },
            { false, true, true, true, true, true, true, true, false, true },
            { false, true, false, true, false, true, false, true, true, true },
            { true, false, true, true, true, false, true, true, true, true },
            { false, true, true, true, true, true, true, true, true, false },
            { true, false, true, false, true, true, true, false, true, false }
        };


        // Act.
        int rollsMoved = sut.MoveRollsRecursively(ref input, 3);


        // Assert.
        Assert.Equal(43, rollsMoved);
    }
}
