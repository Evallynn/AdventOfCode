namespace Advent;


public interface IMachineNode<T> {
    public int Depth { get; }
    public T[] State { get; }

    public bool IsSolved(T[] target);
    public IMachineNode<T> AddChild(int[] transform);
    public string GetTransformPath();
}


public abstract class MachineNodeBase<T>(int _depth, T[] _state, int[]? transform = null) : IMachineNode<T> where T : IComparable<T>, IComparable {
    // Class properties.
    public int Depth { get; private init; } = _depth;
    public T[] State { get; private init; } = _state;

    protected List<MachineNodeBase<T>> Children { get; } = [];
    protected MachineNodeBase<T>? Parent = null;


    // Class-internal variables.
    private int[]? _transform = transform;


    // Abstract methods.
    public abstract IMachineNode<T> AddChild(int[] transform);


    // External utility methods.
    public bool IsSolved(T[] target) {
        if (target.Length != this.State.Length)
            throw new ArgumentException($"Target length ({target.Length}) must match state length ({this.State.Length}).");

        for (int i = 0; i < this.State.Length; i++)
            if (!target[i].Equals(this.State[i])) return false;

        return true;
    }

    public string GetTransformPath() {
        string transformStr = $"({string.Join(',', this._transform ?? [])})";
        MachineNodeBase<T>? node = this.Parent;

        while (node is not null) {
            if (node._transform is not null)
                transformStr = $"({string.Join(',', node._transform)}) -> {transformStr}";

            node = node.Parent;
        }

        return transformStr;
    }
}


public class MachineNodeBinary(int depth, bool[] state, int[]? transform = null) : MachineNodeBase<bool>(depth, state, transform) {
    // External utility methods.
    public override IMachineNode<bool> AddChild(int[] transform) {
        bool[] newState = ApplyTransform(transform, this.State);

        MachineNodeBinary newNode = new(this.Depth + 1, newState, transform) {
            Parent = this
        };

        this.Children.Add(newNode);
        return newNode;
    }


    // Class-internal utility methods.
    private static bool[] ApplyTransform(int[] transform, bool[] currState) {
        bool[] newState = new bool[currState.Length];
        currState.CopyTo(newState, 0);

        foreach (int i in transform)
            newState[i] = !currState[i];

        return newState;
    }
}


public class MachineNodeJoltage(int depth, int[] state, int[]? transform = null) : MachineNodeBase<int>(depth, state, transform) {
    // External utility methods.
    public override IMachineNode<int> AddChild(int[] transform) {
        int[] newState = ApplyTransform(transform, this.State);

        MachineNodeJoltage newNode = new(this.Depth + 1, newState, transform) {
            Parent = this
        };

        this.Children.Add(newNode);
        return newNode;
    }


    // Class-internal utility methods.
    private static int[] ApplyTransform(int[] transform, int[] currState) {
        int[] newState = new int[currState.Length];
        currState.CopyTo(newState, 0);

        foreach (int i in transform) newState[i]++;
        return newState;
    }
}