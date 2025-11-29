using System.Text;

namespace Advent.Tests;


public class Day1Tests {
    [Fact]
    public void OrdersList() {
        // Arrange.
        ListPair expected = new();
        expected.First.AddRange([1, 2, 3, 3, 3, 4]);
        expected.Second.AddRange([3, 3, 3, 4, 5, 9]);
        string input = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";


        // Act.
        using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        using StreamReader reader = new(stream);
        ListPair actual = Files.LoadOrdered(reader);


        // Assert.
        for (int i = 0; i < expected.First.Count; i++) {
            Assert.Equal(actual.First[i], expected.First[i]);
            Assert.Equal(actual.Second[i], expected.Second[i]);
        }
    }

    [Fact]
    public void CalculatesDistance() {
        // Arrange.
        ListPair input = new();
        input.First.AddRange([1, 2, 3, 3, 3, 4]);
        input.Second.AddRange([3, 3, 3, 4, 5, 9]);

        // Act.
        ListEvaluator evaluator = new(input);

        // Assert.
        Assert.Equal(evaluator.Distance, 11);
    }

    [Fact]
    public void CalculatesSimilarity() {
        // Arrange.
        ListPair input = new();
        input.First.AddRange([3, 4, 2, 1, 3, 3]);
        input.Second.AddRange([4, 3, 5, 3, 9, 3]);

        // Act.
        ListEvaluator evaluator = new(input);

        // Assert.
        Assert.Equal(evaluator.Similarity, 31);
    }
}