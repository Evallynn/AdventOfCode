using System.Text;

namespace Advent.Tests;


public class Day6Tests {
    [Fact]
    public void CheckProblemTotal() {
        // Arrange.
        string input = "123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   +  ";
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);

        List<MathProblem> problems = Files.LoadCephalopodMath(reader);
        MathSolver sut = new();


        // Act.
        long result = sut.Solve(problems);


        // Assert.
        Assert.Equal(3263827, result);
    }
}
