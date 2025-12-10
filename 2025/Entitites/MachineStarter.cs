namespace Advent;


public class MachineStarter(int _maxDepth, bool _debug = false) {
    // External utility methods.
    public long ConfigureMachine(List<MachineSchematic> schematics) {
        List<long> maxSteps = [];

        foreach (MachineSchematic schematic in schematics) {
            bool[] state = new bool[schematic.TargetState.Length];
            MachineNodeBinary root = new(0, state);

            long? shortest = ConfigureSchematic(root, schematic.Transforms, schematic.TargetState);
            if (shortest.HasValue) maxSteps.Add(shortest.Value);
        }

        return maxSteps.Sum();
    }


    public long ConfigureJoltage(List<MachineSchematic> schematics) {
        List<long> maxSteps = [];

        foreach (MachineSchematic schematic in schematics) {
            int[] state = new int[schematic.TargetState.Length];
            MachineNodeJoltage root = new(0, state);

            long? shortest = ConfigureSchematic(root, schematic.Transforms, [..schematic.Joltages]);
            if (shortest.HasValue) maxSteps.Add(shortest.Value);
        }

        return maxSteps.Sum();
    }


    // Class-internal utility methods.
    private long? ConfigureSchematic<T>(IMachineNode<T> root, List<int[]> transforms, T[] target) {
        // Process a queue of nodes, effectively doing a breadth-first search for solutions.
        Queue<IMachineNode<T>> nodesToProcess = [];
        nodesToProcess.Enqueue(root);

        while (nodesToProcess.Count > 0) {
            // Pop the current node and check if we've reached the depth limit.
            IMachineNode<T> currNode = nodesToProcess.Dequeue();
            if (currNode.Depth >= _maxDepth) {
                if (_debug) Console.WriteLine($"Solution [{string.Join(',', target)}] hit max depth of {_maxDepth} for path {currNode.GetTransformPath()}.");
                break;
            }

            // Check if the current node is solved.
            if (currNode.IsSolved(target)) {
                if (_debug) {
                    Console.WriteLine($"Found solution [{string.Join(',', target)}] at depth {currNode.Depth}: {currNode.GetTransformPath()}.");
                }
                return currNode.Depth;
            }

            // Otherwise, create all the child nodes and add them to the queue.
            foreach (var transform in transforms) {
                IMachineNode<T> child = currNode.AddChild(transform);
                nodesToProcess.Enqueue(child);
            }
        }

        // If we got here, then we failed to find a solution in the required depth or somehow ran out of nodes.
        return null;
    } 
}
