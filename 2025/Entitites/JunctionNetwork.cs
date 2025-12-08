namespace Advent;


public class JunctionNetwork {
    // Class properties.
    public int Size => this._links.Count;


    // Class-internal variables.
    private readonly Dictionary<int, List<int>> _links = [];


    // Constructors.
    public JunctionNetwork(int initialNode) =>
        this._links.Add(initialNode, []);


    // External utility methods.
    public void AddLink(int existing, int secondary) {
        if (!this._links.ContainsKey(existing))
            throw new ArgumentException($"Attempted to add link to node {existing} but it wasn't in the network.");

        this._links[existing].Add(secondary);

        if (!this._links.ContainsKey(secondary))
            this._links.Add(existing, []);

        this._links[secondary].Add(existing);
    }

    public void Join(JunctionNetwork network) {
        // Iterate over the network to be merged and add its links in one-by-one.
        foreach (int newLinkId in network._links.Keys) {
            if (!this._links.ContainsKey(newLinkId))
                this._links.Add(newLinkId, []);

            foreach (int newLinkTargetId in network._links[newLinkId])
                this.AddLink(newLinkId, newLinkTargetId);
        }
    }
}
