namespace Advent.Tests;


public class Day1Tests {
    [Fact]
    public void MultipleRotations_Solves() {
        // Arrange.



        // Act.


        // Assert.
        Assert.Fail("Not implemented");
    }

    [Theory]
    [InlineData(20, 30, false)]
    [InlineData(82, 68, false)]
    [InlineData(48, 98, true)]
    [InlineData(60, 10, true)]
    [InlineData(51, 999, false)]
    [InlineData(49, 999, true)]
    public void SingleRotation_GivesCorrectResult(int expected, int rotation, bool clockwise) {
        // Arrange.
        SafeOpener sut = new();


        // Act.
        int result = sut.Rotate(50, rotation, clockwise);


        // Assert.
        Assert.Equal(expected, result);
    }
}