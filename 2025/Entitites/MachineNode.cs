namespace Advent;


public class MachineNode(int _depth, bool[] _state, int[]? transform = null) {
    // Class properties.
    public int Depth { get; private init; } = _depth;
    public bool[] State { get; private init; } = _state;


    // Class-internal variables.
    private List<MachineNode> _children = [];
    private MachineNode? _parent = null;
    private int[]? _transform = transform;


    // External utility methods.
    public MachineNode AddChild(int[] transform) {
        bool[] newState = ApplyTransform(transform, this.State);

        MachineNode newNode = new(this.Depth + 1, newState, transform) {
            _parent = this
        };

        this._children.Add(newNode);
        return newNode;
    }

    public bool IsSolved(bool[] target) {
        if (target.Length != this.State.Length)
            throw new ArgumentException($"Target length ({target.Length}) must match state length ({this.State.Length}).");

        for (int i = 0; i < this.State.Length; i++)
            if (target[i] != this.State[i]) return false;

        return true;
    }

    public string GetTransformPath() {
        string transformStr = $"({string.Join(',', this._transform ?? [])})";
        MachineNode? node = this._parent;

        while (node is not null) {
            if (node._transform is not null)
                transformStr = $"({string.Join(',', node._transform)}) -> {transformStr}";

            node = node._parent;
        }

        return transformStr;
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
