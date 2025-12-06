namespace Advent;


public enum OPERATION {
    ADD,
    MULTIPLY
}


public class MathProblem() {
    // Class properties.
    public OPERATION Operation { get; set; } = OPERATION.ADD;


    // Internal variables.
    private List<int> _values = [];


    // External utility methods.
    public void AddValue(int value) => this._values.Add(value);

    public long Compute() =>
        this.Operation switch {
            OPERATION.ADD => this.Sum(),
            OPERATION.MULTIPLY => this.Multiply(),
            _ => throw new InvalidOperationException($"Unknown operation {this.Operation}.")
        };


    // Class-internal utility methods.
    private long Sum() {
        long sum = 0;
        this._values.ForEach(i => sum += i);
        return sum;
    }

    private long Multiply() {
        long sum = 1;
        this._values.ForEach(i => sum *= i);
        return sum;
    }
}
