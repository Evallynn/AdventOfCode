namespace Advent;


internal class Day3 : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day3.txt";


    // External utility methods.
    public void Run() {
        var rawJolts = Files.LoadJoltsFile(FILE_PATH);
        JoltsCalculator calculator = new();

        long maxJolts2 = calculator.CalculateJolts(rawJolts, 2);
        Console.WriteLine($"Maximum jolts (2): {maxJolts2}");

        long maxJolts12 = calculator.CalculateJolts(rawJolts, 12);
        Console.WriteLine($"Maximum jolts (12): {maxJolts12}");
    }
}