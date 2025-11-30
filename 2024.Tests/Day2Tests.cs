namespace Advent.Tests;


public class Day2Tests {
    [Theory]
    [InlineData(true, 7, 6, 4, 2, 1)]
    [InlineData(false, 1, 2, 7, 8, 9)]
    [InlineData(false, 9, 7, 6, 2, 1)]
    [InlineData(false, 1, 3, 2, 4, 5)]
    [InlineData(false, 8, 6, 4, 4, 1)]
    [InlineData(true, 1, 3, 6, 7, 9)]
    public void IsSafe_Successful_Undampened(bool expected, params int[] inputs) {
        // Arrange.
        SafetyEvaluator sut = new();

        // Act.
        bool result = sut.IsSafe([..inputs], false);

        // Assert.
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData(true, 7, 6, 4, 2, 1)]
    [InlineData(false, 1, 2, 7, 8, 9)]
    [InlineData(false, 9, 7, 6, 2, 1)]
    [InlineData(true, 1, 3, 2, 4, 5)]
    [InlineData(true, 8, 6, 4, 4, 1)]
    [InlineData(true, 1, 3, 6, 7, 9)]
    [InlineData(true, 1, 3, 6, 7, 15)]
    [InlineData(true, 1, 5, 6, 7, 9)]
    [InlineData(true, 17, 18, 23, 19, 21)]
    [InlineData(true, 20, 17, 18, 19, 21)]
    public void IsSafe_Successful_Dampened(bool expected, params int[] inputs) {
        // Arrange.
        SafetyEvaluator sut = new();

        // Act.
        bool result = sut.IsSafe([.. inputs], true);

        // Assert.
        Assert.Equal(expected, result);
    }
}
