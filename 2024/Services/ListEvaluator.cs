namespace Advent;


public class ListEvaluator {
    // Class properties.
    public int? Distance { get; private set; }
    public int? Similarity { get; private set; }


    // Constructors.
    public ListEvaluator(ListPair input) => Calculate(input);


    // Internal utility methods.
    private void Calculate(ListPair lists) {
        if (lists.First.Count != lists.Second.Count)
            throw new ArgumentException($"List sizes didn't match. Received {lists.First.Count} and {lists.Second.Count}.");

        this.Distance = 0;
        this.Similarity = 0;

        for (int i = 0; i < lists.First.Count; i++) {
            this.Distance += Math.Abs(lists.Second[i] - lists.First[i]);
            this.Similarity += lists.First[i] * lists.Second.Count(x => x == lists.First[i]);
        }
    }
}
