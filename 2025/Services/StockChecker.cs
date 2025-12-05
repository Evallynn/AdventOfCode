namespace Advent;


public class StockChecker {
    // External utility methods.
    public long TotalFreshIngredients(Ingredients ingredients) =>
        ingredients.Stock.Count(i => ingredients.IsFresh(i));


    public long TotalPossibleFresh(Ingredients ingredients) =>
        ingredients.FreshRanges.Sum(i => i.Size);
}
