namespace Advent;


public class Day1() : IChallenge {
    // Constants.
    private const string FILE_PATH = "C:\\Users\\LSTAD\\Documents\\Azure\\AdventOfCode\\2024\\data\\Day1.txt";


    // External utility methods.
    public void Run() {
        ListPair lists = Files.LoadOrdered(FILE_PATH);
        ListEvaluator evaluator = new(lists);

        Console.WriteLine($"Distance: {evaluator.Distance}");
        Console.WriteLine($"Similarity: {evaluator.Similarity}");
    }
}
