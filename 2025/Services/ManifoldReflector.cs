namespace Advent;


// TODO - I could factor these into one thing; that will invariably not happen as I CBA.
public class ManifoldReflector(bool _debug = false) {
    // External utility methods.
    public int ResolveClassic(TachyonManifold manifolds) {
        int totalSplits = 0;
        bool[] currBeams = manifolds.StartBeamsClassic;

        for (int row = 0; row < manifolds.Height; row++) {
            ClassicManifoldResult result = manifolds.ProcessClassic(row, currBeams);
            totalSplits += result.TotalSplits;
            currBeams = result.NewLocations;

            if (_debug) {
                for (int col = 0; col < manifolds.Width; col++) {
                    if (manifolds.IsManifold(row, col)) Console.Write('^');
                    else if (currBeams[col]) Console.Write('|');
                    else Console.Write('.');
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        return totalSplits;
    }


    public long ResolveQuantum(TachyonManifold manifolds) {
        int totalSplits = 0;
        long[] currTimelines = manifolds.StartBeamsQuantum;

        for (int row = 0; row < manifolds.Height; row++) {
            QuantumManifoldResult result = manifolds.ProcessQuantum(row, currTimelines);
            totalSplits += result.TotalSplits;
            currTimelines = result.NewTimelines;

            if (_debug) {
                for (int col = 0; col < manifolds.Width; col++) {
                    if (manifolds.IsManifold(row, col)) Console.Write('^');
                    else if (currTimelines[col] > 0) Console.Write($"{currTimelines[col]}");
                    else Console.Write('.');
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        return currTimelines.Sum();
    }
}
