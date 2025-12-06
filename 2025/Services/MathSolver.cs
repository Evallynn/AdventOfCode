namespace Advent;


public class MathSolver {
    // External utility methods.
    public long Solve(List<MathProblem> problems) =>
        problems.Sum(i => i.Compute());
}
