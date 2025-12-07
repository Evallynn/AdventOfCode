namespace Advent;


internal class Day7 : IChallenge {
    // Constants.
    public const string FILE_PATH = "data/Day7.txt";


    // External utility methods.
    public void Run() {
        TachyonManifold manifolds = Files.LoadTachyonManifolds(FILE_PATH);
        ManifoldReflector reflector = new(false);

        int classicTotal = reflector.ResolveClassic(manifolds);
        Console.WriteLine($"Total reflections (classic): {classicTotal}");

        long quantumTotal = reflector.ResolveQuantum(manifolds);
        Console.WriteLine($"Total reflections (quantum): {quantumTotal}");
    }
}
