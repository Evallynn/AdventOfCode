namespace Advent;


public class MachineStarter(int _maxDepth, bool _debug = false) {
    // External utility methods.
    public long ConfigureMachine(List<MachineSchematic> schematics) {
        List<long> maxSteps = [];

        foreach (MachineSchematic schematic in schematics) {
            // Create the root node.
            bool[] state = new bool[schematic.TargetState.Length];
            MachineNode root = new(0, state);


            // Process a queue of nodes, effectively doing a breadth-first search for solutions.
            Queue<MachineNode> nodesToProcess = [];
            nodesToProcess.Enqueue(root);

            while (nodesToProcess.Count > 0) {
                // Pop the current node and check if we've reached the depth limit.
                MachineNode currNode = nodesToProcess.Dequeue();
                if (currNode.Depth >= _maxDepth) {
                    if (_debug) Console.WriteLine($"Solution {schematic} hit max depth of {_maxDepth} for path {currNode.GetTransformPath()}.");
                    break;
                }

                // Check if the current node is solved.
                if (currNode.IsSolved(schematic.TargetState)) {
                    if (_debug) {
                        Console.WriteLine($"Found solution {schematic} at depth {currNode.Depth}: {currNode.GetTransformPath()}.");
                    }
                    maxSteps.Add(currNode.Depth);
                    break;
                }

                // Otherwise, create all the child nodes and add them to the queue.
                foreach (var transform in schematic.Transforms) {
                    MachineNode child = currNode.AddChild(transform);
                    nodesToProcess.Enqueue(child);
                }
            }
        }

        return maxSteps.Sum();
    }


    public long ConfigureJoltage(List<MachineSchematic> schematics) {
        return 0;
    }
}
