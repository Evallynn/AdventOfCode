namespace Advent;


public class Day8 : IChallenge {
    // Constants.
    public const string FILE_PATH = "data/Day8.txt";


    // External utility methods.
    public void Run() {
        List<Vector3> boxes = Files.LoadJunctionBoxes(FILE_PATH);
        JunctionBoxWirer wirer = new();

        JunctionLinks[] links = wirer.CalculateLinks(boxes);
        List<JunctionNetwork> networks = wirer.WireNetworks(links, 10000);
        long total = wirer.ComputeNetworkSize(networks, 3);

        Console.WriteLine($"Largest three network total: {total}");
    }
}
