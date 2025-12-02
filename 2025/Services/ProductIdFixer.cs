namespace Advent;


public record ProductIdRange(long Min, long Max);


public class ProductIdFixer {
    // External utility methods.
    public long TotalInvalidProductRanges(List<ProductIdRange> ranges) =>
        ranges.Sum(i => TotalInvalidProductRange(i));


    public long TotalInvalidProductRange(ProductIdRange range) {
        long total = 0;

        for (long i = range.Min; i <= range.Max; i++) {
            // Skip items with an odd number of digits, they can't be split.
            string idStr = i.ToString();
            if (idStr.Length % 2 != 0) continue;

            string firstHalf = idStr[..(idStr.Length / 2)];
            string secondHalf = idStr[(idStr.Length / 2)..];

            if (firstHalf == secondHalf)
                total += i;
        }

        return total;
    }
}
