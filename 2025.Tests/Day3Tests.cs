namespace Advent.Tests;


public class Day3Tests {
    [Theory]
    [InlineData(2, 357)]
    [InlineData(12, 3121910778619)]
    public void CalculateMaxJolts_FullFile(int joltScale, long expected) {
        // Arrange.
        JoltsCalculator sut = new();

        List<string> input = [
            "987654321111111",
            "811111111111119",
            "234234234234278",
            "818181911112111"
        ];


        // Act.
        long maxJolts = sut.CalculateJolts(input, joltScale);


        // Assert.
        Assert.Equal(expected, maxJolts);
    }

    [Theory]
    [InlineData("987654321111111", 98, 2)]
    [InlineData("234234234234278", 78, 2)]
    [InlineData("811111111111119", 89, 2)]
    [InlineData("818181911112111", 92, 2)]
    [InlineData("987654321111111", 987654321111, 12)]
    [InlineData("234234234234278", 434234234278, 12)]
    [InlineData("811111111111119", 811111111119, 12)]
    [InlineData("818181911112111", 888911112111, 12)]
    public void CalculateMaxJolts_SingleLine(string input, long expected, int joltScale) {
        // Arrange.
        JoltsCalculator sut = new();


        // Act.
        long maxJolts = sut.CalculateJolts(input, joltScale);


        // Assert.
        Assert.Equal(expected, maxJolts);
    }
}
