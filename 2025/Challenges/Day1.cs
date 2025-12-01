namespace Advent;


public class Day1() : IChallenge {
    // Constants.
    private const string FILE_PATH = "C:\\Users\\LSTAD\\Documents\\Azure\\AdventOfCode\\2025\\data\\Day1.txt";


    // External utility methods.
    public void Run() {
        var rotations = Files.LoadSafeRotations(FILE_PATH);

        SafeOpener opener = new();
        int totalZeroes = opener.CountZeroes(50, rotations);

        Console.WriteLine($"Total Zeroes: {totalZeroes}");
    }
}
