namespace Advent;


public class Day2() : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day2.txt";


    // External utility methods.
    public void Run() {
        var ranges = Files.LoadProductIds(FILE_PATH);
        ProductIdFixer fixer = new();

        long totalInvalid = fixer.TotalInvalidProductRanges(ranges);

        Console.WriteLine($"Total invalid: {totalInvalid}");
    }
}
