namespace Advent;


public class Day5 : IChallenge {
    // Constants.
    public const string FILE_PATH = "data/Day5.txt";


    // External utility methods.
    public void Run() {
        Ingredients ingredients = Files.LoadCafeteriaStock(FILE_PATH);
        StockChecker checker = new StockChecker();

        long totalFresh = checker.TotalFreshIngredients(ingredients);
        Console.WriteLine($"Total fresh ingredients: {totalFresh}");

        long totalPossible = checker.TotalPossibleFresh(ingredients);
        Console.WriteLine($"Total possible fresh: {totalPossible}");
    }
}
