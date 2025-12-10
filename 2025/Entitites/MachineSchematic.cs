namespace Advent;


public class MachineSchematic(bool[] _targetState) {
    // Class properties.
    public List<int> Joltages { get; } = [];
    public bool[] TargetState { get; } = _targetState;
    public List<int[]> Transforms { get; } = [];


    // External utility methods.
    public void AddJoltage(int joltage) => this.Joltages.Add(joltage);

    public void AddSwitch(params int[] affectedValues) {
        if (affectedValues.Any(i => i < 0 || i >= this.TargetState.Length))
            throw new ArgumentException($"Switch values must be within range of total target states. Received [{string.Join(',', affectedValues)}] but expected 0-{this.TargetState.Length - 1}.");
    
        this.Transforms.Add(affectedValues);
    }

    public override string ToString() {
        string str = "[";

        foreach (bool state in this.TargetState)
            str += state ? "#" : '.';

        return str + "]";
    }
}
