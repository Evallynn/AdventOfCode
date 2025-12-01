namespace Advent.Tests;


public class Day1Tests {
    [Fact]
    public void MultipleRotations_CountsZeroes() {
        // Arrange.
        SafeOpener sut = new();

        List<SafeRotation> rotations = [
            new(68, false),
            new(30, false),
            new(48, true),
            new(5, false),
            new(60, true),
            new(55, false),
            new(1, false),
            new(99, false),
            new(14, true),
            new(82, false)
        ];


        // Act.
        SafePassword password = sut.CountZeroes(50, rotations);


        // Assert.
        Assert.Equal(3, password.LandedOnZero);
        Assert.Equal(6, password.PassedZero);
    }

    [Theory]
    [InlineData(20, 30, false)]
    [InlineData(82, 68, false)]
    [InlineData(48, 98, true)]
    [InlineData(60, 10, true)]
    [InlineData(51, 999, false)]
    [InlineData(49, 999, true)]
    public void SingleRotation_GivesCorrectRotation(int expected, int rotation, bool clockwise) {
        // Arrange.
        SafeOpener sut = new();


        // Act.
        RotationResult result = sut.Rotate(50, rotation, clockwise);


        // Assert.
        Assert.Equal(expected, result.Rotation);
    }


    [Theory]
    [InlineData(0, 49, false, 50)]
    [InlineData(0, 49, true, 50)]
    [InlineData(1, 50, false, 50)]
    [InlineData(1, 50, true, 50)]
    [InlineData(1, 51, false, 50)]
    [InlineData(9, 949, false, 50)]
    [InlineData(9, 949, true, 50)]
    [InlineData(10, 950, false, 50)]
    [InlineData(10, 950, true, 50)]
    [InlineData(10, 951, false, 50)]
    [InlineData(0, 49, false, 0)]
    [InlineData(0, 49, true, 0)]
    [InlineData(0, 50, false, 0)]
    [InlineData(0, 50, true, 0)]
    [InlineData(0, 51, false, 0)]
    [InlineData(1, 100, false, 0)]
    [InlineData(1, 100, true, 0)]
    [InlineData(1, 101, false, 0)]
    [InlineData(9, 949, false, 0)]
    [InlineData(9, 949, true, 0)]
    [InlineData(9, 950, false, 0)]
    [InlineData(9, 950, true, 0)]
    [InlineData(9, 951, false, 0)]
    public void SingleRotation_GivesCorrectPassedZeroCount(int expected, int rotation, bool clockwise, int startPoint) {
        // Arrange.
        SafeOpener sut = new();


        // Act.
        RotationResult result = sut.Rotate(startPoint, rotation, clockwise);


        // Assert.
        Assert.Equal(expected, result.PassedZero);
    }
}