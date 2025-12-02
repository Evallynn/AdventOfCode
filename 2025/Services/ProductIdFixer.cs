namespace Advent;


public record ProductIdRange(long Min, long Max);


public class ProductIdFixer {
    // External utility methods.
    public long TotalInvalidProductRanges(List<ProductIdRange> ranges) =>
        ranges.Sum(i => TotalInvalidProductRange(i));


    public long TotalInvalidProductRange(ProductIdRange range) {
        long total = 0;

        for (long i = range.Min; i <= range.Max; i++) {
            if (IsInvalidProductId(i))
                total += i;
        }

        return total;
    }

    // Class-internal utilities.
    private static bool IsInvalidProductId(long id) {
        string idStr = id.ToString();
        List<int> lengthDivisors = GetDivisors(idStr.Length);

        foreach (int divisor in lengthDivisors) {
            if (IsInvalidProductId(idStr, divisor))
                return true;
        }

        return false;
    }

    private static bool IsInvalidProductId(string id, int divisor) {
        string initialChunk = id[..divisor];

        for (int i = divisor; i < id.Length; i += divisor) {
            string chunk = id.Substring(i, divisor);

            if (initialChunk != chunk) {
                return false;
            }
        }

        return true;
    }

    private static List<int> GetDivisors(int input) {
        List<int> divisors = [];

        for (int i = 1; i < input; i++)
            if (input % i == 0)
                divisors.Add(i);

        return divisors;
    } 
}
