namespace Advent;


public class Day2() : IChallenge {
    // Class-internal variables.
    private SafetyEvaluator _evaluator = new();


    // Constants.
    private const string FILE_PATH = "data/Day2.txt";


    // External utility methods.
    public void Run() {
        var reports = Files.LoadReactorLevels(FILE_PATH);
        int totalSafeUndampened = 0;
        int totalSafeDampened = 0;

        foreach (List<int> report in reports) {
            if (this._evaluator.IsSafe(report, false))
                totalSafeUndampened++;

            if (this._evaluator.IsSafe(report, true))
                totalSafeDampened++;
        }

        Console.WriteLine($"Total safe (undampened): {totalSafeUndampened}");
        Console.WriteLine($"Total safe (dampened): {totalSafeDampened}");
    }
}
