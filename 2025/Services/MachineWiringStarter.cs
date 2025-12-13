namespace Advent;


public class MachineWiringStarter(int _maxDepth, long _maxNodes, bool _debug = false) : IMachineStarter {
    // External utility methods.
    public long Configure(List<MachineSchematic> schematics) {
        // We'll use basic Dijkstra for turning the machines on.
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
        // Dijkstra/A* is too inefficient for joltage. Use DFS over button presses instead of building a tree.
        List<long> smallestPresses = [];

        foreach (MachineSchematic schematic in schematics) {
            // It's more efficient to start at the end state and work towards zero
            short[] remaining = [..schematic.Joltages];
            int smallestPress = _maxDepth + 1;
            long nodesProcessed = 0;

            // Reorder buttons: those that hit big targets first.
            List<int[]> orderedTransforms = schematic.Transforms
                .OrderByDescending(t => t.Sum(idx => schematic.Joltages[idx]))
                .ToList();

            ConfigureJoltage(
                buttonIndex: 0,
                remaining: remaining,
                pressesSoFar: 0,
                transforms: orderedTransforms,
                maxButtonSize: orderedTransforms.Max(t => t.Length),
                maxPresses: _maxDepth,
                ref smallestPress,
                ref nodesProcessed
            );

            if (smallestPress == _maxDepth + 1) {
                Console.WriteLine("No solution found within bounds for a machine.");
            }
            else {
                if (_debug) Console.WriteLine($"Found [{string.Join(',', schematic.Joltages)}] in {smallestPress} presses.");
                smallestPresses.Add(smallestPress);
            }
        }

        return smallestPresses.Sum();
    }


    // Class-internal utility methods.
    private void ConfigureJoltage(int buttonIndex, short[] remaining, int pressesSoFar, List<int[]> transforms,
                                  int maxButtonSize, int maxPresses, ref int smallestPresses, ref long nodesProcessed) {
        // Global node budget, if you want to keep it
        nodesProcessed++;
        if (nodesProcessed > _maxNodes) return;

        // Too many presses already
        if (pressesSoFar >= smallestPresses || pressesSoFar > maxPresses)
            return;

        // If we've assigned counts to all buttons, check if we're done.
        if (buttonIndex == transforms.Count) {
            if (IsSolved(remaining) && pressesSoFar < smallestPresses)
                smallestPresses = pressesSoFar;

            return;
        }

        // Lower bound from remaining work; if we can't beat best, prune
        int lb = pressesSoFar + LowerBound(remaining, maxButtonSize);
        if (lb >= smallestPresses || lb > maxPresses)
            return;

        int[] button = transforms[buttonIndex];

        // Compute max times we can press this button without any counter going negative
        int maxUses = MaxUsesForButton(button, remaining);
        if (maxUses < 0) maxUses = 0;

        // Also can't exceed our total press budget
        maxUses = Math.Min(maxUses, maxPresses - pressesSoFar);

        // Try from maxUses down to 0.
        // Going high-to-low tends to find good solutions early -> tighter best -> more pruning.
        for (int uses = maxUses; uses >= 0; uses--) {
            // Apply 'uses' presses in-place
            PressButton(remaining, button, -uses); // subtract uses from remaining

            // If we overshot anywhere (remaining < 0), undo and skip
            if (!IsStillSolvable(remaining)) {
                PressButton(remaining, button, +uses); // undo
                continue;
            }

            this.ConfigureJoltage(
                buttonIndex + 1,
                remaining,
                pressesSoFar + uses,
                transforms,
                maxButtonSize,
                maxPresses,
                ref smallestPresses,
                ref nodesProcessed
            );

            // Undo before trying next 'uses'
            PressButton(remaining, button, +uses);
        }
    }

    private static void PressButton(short[] remaining, int[] button, int deltaUses) {
        if (deltaUses == 0) return;

        for (int i = 0; i < button.Length; i++)
            remaining[button[i]] += (short)deltaUses;
    }

    private static bool IsStillSolvable(short[] remaining) {
        for (int i = 0; i < remaining.Length; i++)
            if (remaining[i] < 0) return false;

        return true;
    }

    private static bool IsSolved(short[] remaining) {
        for (int i = 0; i < remaining.Length; i++)
            if (remaining[i] != 0) return false;

        return true;
    }

    // Same heuristic idea as for A*: max single remaining + total remaining / maxButtonSize
    private static int LowerBound(short[] remaining, int maxButtonSize) {
        int maxR = 0;
        int sumR = 0;

        for (int i = 0; i < remaining.Length; i++) {
            int r = remaining[i];
            if (r < 0) r = 0;
            if (r > maxR) maxR = r;
            sumR += r;
        }

        if (sumR == 0) return 0;

        int h1 = maxR;
        int h2 = (sumR + maxButtonSize - 1) / maxButtonSize;
        return Math.Max(h1, h2);
    }

    private static int MaxUsesForButton(int[] button, short[] remaining) {
        int max = int.MaxValue;

        foreach (int idx in button)
            max = Math.Min(max, remaining[idx]);

        return max == int.MaxValue ? 0 : max;
    }


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
