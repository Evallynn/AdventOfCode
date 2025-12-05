namespace Advent.Tests;


public class Day5Tests {
    [Fact]
    public void TotalFresh_Successful() {
        // Arrange.
        StockChecker sut = new();

        Ingredients input = new();
        input.AddRange(3, 5);
        input.AddRange(10, 14);
        input.AddRange(16, 20);
        input.AddRange(12, 18);
        input.AddStock(1);
        input.AddStock(5);
        input.AddStock(8);
        input.AddStock(11);
        input.AddStock(17);
        input.AddStock(32);


        // Act.
        long totalFresh = sut.TotalFreshIngredients(input);


        // Assert.
        Assert.Equal(3, totalFresh);
    }

    [Fact]
    public void TotalPossibleFresh_Successful() {
        // Arrange.
        StockChecker sut = new();

        Ingredients input = new();
        input.AddRange(3, 5);
        input.AddRange(10, 14);
        input.AddRange(16, 20);
        input.AddRange(12, 18);


        // Act.
        long totalFresh = sut.TotalPossibleFresh(input);


        // Assert.
        Assert.Equal(14, totalFresh);
    }

    [Fact]
    public void Check_NoOverlaps() {
        // Arrange.
        Ingredients input = Files.LoadCafeteriaStock(Day5.FILE_PATH);


        // Act.
        bool overlaps = false;

        foreach (FreshRange checkRange in input.FreshRanges) { 
            foreach (FreshRange currRange in input.FreshRanges) {
                if (checkRange == currRange) continue;

                if (checkRange.Min >= currRange.Min && checkRange.Min <= currRange.Max) {
                    overlaps = true;
                    Console.WriteLine($"Overlap between: {checkRange.Min}-{checkRange.Max} and {currRange.Min}-{currRange.Max} (MIN)");
                }

                if (checkRange.Max >= currRange.Min && checkRange.Max <= currRange.Max) {
                    overlaps = true;
                    Console.WriteLine($"Overlap between: {checkRange.Min}-{checkRange.Max} and {currRange.Min}-{currRange.Max} (MAX)");
                }
            }
        }


        // Assert.
        Assert.False(overlaps);
    }
}
