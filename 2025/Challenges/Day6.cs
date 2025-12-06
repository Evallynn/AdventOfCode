namespace Advent;


internal class Day6 : IChallenge {
    // Constants.
    public const string FILE_PATH = "data/Day6.txt";


    // External utility methods.
    public void Run() {
        List<MathProblem> problems = Files.LoadCephalopodMath(FILE_PATH);
        MathSolver solver = new();

        long total = solver.Solve(problems);
        Console.WriteLine($"Math problem total: {total}");
    }
}
