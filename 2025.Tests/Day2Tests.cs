namespace Advent.Tests;


public class Day2Tests {
    [Fact]
    public void ProductRange_Successful() {
        // Arrange.
        ProductIdFixer sut = new();

        List<ProductIdRange> input = [
            new(11, 22),
            new(95, 115),
            new(998, 1012),
            new(1188511880, 1188511890),
            new(222220, 222224),
            new(1698522, 1698528),
            new(446443, 446449),
            new(38593856, 38593862),
            new(565653, 565659),
            new(824824821, 824824827),
            new(2121212118, 2121212124)
        ];


        // Act.
        long actual = sut.TotalInvalidProductRanges(input);


        // Assert.
        Assert.Equal(4174379265, actual);
    }
}
