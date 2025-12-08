namespace Advent;


public readonly struct JunctionLinks(Vector3 boxPosition) {
    public Vector3 BoxPosition { get; } = boxPosition;
    public Dictionary<int, long> Distances { get; } = [];
}


public class JunctionBoxWirer {
    // External utility methods.
    public long ComputeNetworkSize(List<JunctionNetwork> networks, int totalToCompute) {
        networks.Sort((a, b) => b.Size.CompareTo(a.Size));

        networks.ForEach(i => {
            if (i.Size > 1) Console.WriteLine($"Size: {i.Size}");
        });

        long total = 1;
        for (int i = 0; i < totalToCompute && i < networks.Count; i++)
            total *= networks[i].Size;

        return total;
    }


    // TODO - Brute forcing for now. This should be pre-sorted and better optimised.
    public List<JunctionNetwork> WireNetworks(JunctionLinks[] links, int totalWires) {
        // Setup our view of the networks we have, and which network each node relates to.
        Dictionary<int, JunctionNetwork> networks = [];
        Dictionary<int, int> nodeNetwork = [];

        for (int i = 0; i < links.Length; i++) {
            networks.Add(i, new JunctionNetwork(i));
            nodeNetwork.Add(i, i);
        }


        // Create as many wirings as we've been asked to.
        for (int i = 0; i < totalWires; i++) {
            // Obtain the shortest path and map those nodes back to their corresponding networks.
            Vector2 shortestIds = FindShortestPath(links);
            int networkIdA = nodeNetwork[shortestIds.x];
            int networkIdB = nodeNetwork[shortestIds.y];

            // Determine if the two nodes are already in the same network.
            // Note that we can just ignore it if they're in the same network.
            if (networkIdB != networkIdA) {
                // Merge the networks if they aren't.
                Console.WriteLine($"Merging network {networkIdB} to {networkIdA}. Previous total: {networks.Count}");
                networks[networkIdA].Join(networks[networkIdB]);
                networks.Remove(networkIdB);

                // Remap the networks that contains the node.
                foreach (int nodeId in nodeNetwork.Keys)
                    if (nodeNetwork[nodeId] == networkIdB)
                        nodeNetwork[nodeId] = networkIdA;
            }

            // Remove the link so we don't reprocess it.
            links[shortestIds.x].Distances.Remove(shortestIds.y);

            // If we're ever left with only one network, finish.
            if (networks.Count <= 1) break;
        }


        // Pass back the resulting networks.
        return [..networks.Values];
    }

    public JunctionLinks[] CalculateLinks(List<Vector3> boxes) {
        // Create links from every point to every other point.
        JunctionLinks[] links = [..Enumerable.Range(0, boxes.Count).Select(i => new JunctionLinks(boxes[i]))];

        for (int i = 0; i < boxes.Count; i++) {
            for (int j = 0; j < boxes.Count; j++) {
                // Skip ourselves.
                if (i == j) continue;

                // Skip existing links - this cuts processing in half.
                if (links[i].Distances.ContainsKey(j) || links[j].Distances.ContainsKey(i))
                    continue;

                // Calculate the distance and store only one way. This also cut down loop processing later.
                links[i].Distances[j] = CalculateDistanceSq(boxes[i], boxes[j]);
            }
        }

        return links;
    }


    public static long CalculateDistanceSq(Vector3 a, Vector3 b) {
        // Note that we don't bother square rooting - it achieves the same effect for us and saves a lot of compute
        // and we get better accuracy because we can stick to longs over doubles.
        long xDiff = b.X - a.X;
        long yDiff = b.Y - a.Y;
        long zDiff = b.Z - a.Z;

        return (xDiff * xDiff) + (yDiff * yDiff) + (zDiff * zDiff);
    }


    // Class-internal utility methods.
    private static Vector2 FindShortestPath(JunctionLinks[] links) {
        long shortest = long.MaxValue;
        int shortestNetwork = -1;
        int shortestLink = -1;

        for (int i = 0; i < links.Length; i++) {
            foreach (int j in links[i].Distances.Keys) {
                if (links[i].Distances[j] < shortest) {
                    shortest = links[i].Distances[j];
                    shortestNetwork = i;
                    shortestLink = j;
                }
            }
        }

        Console.WriteLine($"Link between {links[shortestNetwork].BoxPosition} and {links[shortestLink].BoxPosition} of length {shortest}.");
        return new(shortestNetwork, shortestLink);
    }
}
