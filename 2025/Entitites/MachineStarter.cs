namespace Advent;


public class MachineStarter(int _maxDepth, long _maxNodes, bool _debug = false) {
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

        for (int i = 0; i < schematics.Count; i++) {
            if (_debug) Console.Write($"Processing {i}...    ");
            short[] state = new short[schematics[i].TargetState.Length];
            MachineNodeJoltage root = new(0, state);

            long? shortest = ConfigureSchematic(root, schematics[i].Transforms, [..schematics[i].Joltages]);
            if (shortest.HasValue) maxSteps.Add(shortest.Value);
        }

        return maxSteps.Sum();
    }


    // Class-internal utility methods.
    private long? ConfigureSchematic<T>(IMachineNode<T> root, List<int[]> transforms, T[] target) where T : notnull {
        // Process a queue of nodes, effectively doing a naive version of A*.
        PriorityQueue<IMachineNode<T>, int> nodesToProcess = new();
        nodesToProcess.Enqueue(root, root.GetHeuristic(target) ?? int.MaxValue);
        long totalNodesProcessed = 0;


        // Ensure we track states we've visited already to avoid duplicates.
        Dictionary<T[], int> visitedStates = new(new ArrayComparer<T>()) {
            { root.State, root.Depth }
        };


        // Keep looking until we hit the limit or run our of nodes to process.
        while (nodesToProcess.Count > 0 && totalNodesProcessed < _maxNodes) {
            // Pop the current node and check if we've reached the depth limit.
            IMachineNode<T> currNode = nodesToProcess.Dequeue();
            totalNodesProcessed++;
            if (currNode.Depth >= _maxDepth) continue;


            // Check if the current node is solved.
            if (currNode.IsSolved(target)) {
                if (_debug) {
                    Console.WriteLine($"Found solution [{string.Join(',', target)}] at depth {currNode.Depth}: {currNode.GetTransformPath()}.");
                }
                return currNode.Depth;
            }


            // Otherwise, create all the child nodes and add them to the queue.
            foreach (int[] transform in transforms) {
                // Create the child and get its heuristic.
                IMachineNode<T> child = currNode.AddChild(transform);
                int? heuristic = child.GetHeuristic(target);

                // Only process if we have a valid heuristic.
                if (heuristic.HasValue) {
                    if (visitedStates.ContainsKey(child.State)) {
                        if (visitedStates[child.State] > child.Depth)
                            Console.WriteLine("Found existing state that's a larger depth!");
                        continue;
                    }

                    // Add the node to the processing queue.
                    visitedStates.Add(child.State, child.Depth);
                    nodesToProcess.Enqueue(child, heuristic.Value);
                }
            }
        }

        // If we got here, then we failed to find a solution in the required depth or somehow ran out of nodes.
        if (totalNodesProcessed >= _maxNodes)
            Console.WriteLine($"Solution [{string.Join(',', target)}] hit max processed nodes of {_maxNodes}.");

        return null;
    } 
}
