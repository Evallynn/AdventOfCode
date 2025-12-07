namespace Advent;


public record FreshRange(long Min, long Max) {
    public bool Contains(long value) => value >= this.Min && value <= this.Max;
    public long Size => (this.Max - this.Min) + 1;
}

public class Ingredients {
    // Class properties.
    public IEnumerable<FreshRange> FreshRanges => this._freshRanges;
    public IEnumerable<long> Stock => this._stock;


    // Class-internal variables.
    private List<FreshRange> _freshRanges = [];
    private readonly List<long> _stock = [];


    // External utility methods.
    public void AddRange(long min, long max) {
        // Find any overlapping ranges and remove them, but account for them in what we add back in.
        List<int> modifiedRangeIds = [];
        long minMod = min;
        long maxMod = max;

        for (int i = 0; i < this._freshRanges.Count; i++) {
            FreshRange existing = this._freshRanges[i];
            bool containsMin = existing.Contains(min);
            bool containsMax = existing.Contains(max);

            // Do nothing, the range is entirely within another one.
            if (containsMin && containsMax) {
                return;
            }
            // The minimum is contained, extend the maximum.
            else if (containsMin) {
                minMod = existing.Min;
                modifiedRangeIds.Add(i);
            }
            // The maximum is contained, extend the minimum.
            else if (containsMax) {
                maxMod = existing.Max;
                modifiedRangeIds.Add(i);
            }
            // The new range fully contains the old range.
            else if (min <= existing.Min && max >= existing.Max) {
                minMod = min;
                maxMod = max;
                modifiedRangeIds.Add(i);
            }
        }


        // Remove overlapping ranges - note we decrement the ID by 'i' to account for the array shrinking as we remove items.
        for (int i = 0; i < modifiedRangeIds.Count; i++) {
            int id = modifiedRangeIds[i];
            this._freshRanges.RemoveAt(id - i);
        }


        // Add a new range.
        this._freshRanges.Add(new(minMod, maxMod));
    }

    public void AddStock(long id) => this._stock.Add(id);

    public bool IsFresh(long id) {
        foreach (FreshRange range in this.FreshRanges)
            if (id >= range.Min && id <= range.Max)
                return true;

        return false;
    }
}
