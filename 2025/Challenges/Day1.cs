namespace Advent;


public class Day1() : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day1.txt";


    // External utility methods.
    public void Run() {
        var rotations = Files.LoadSafeRotations(FILE_PATH);

        SafeOpener opener = new();
        SafePassword password = opener.CountZeroes(50, rotations);

        Console.WriteLine($"Landed on zero: {password.LandedOnZero}");
        Console.WriteLine($"Passed zero: {password.PassedZero}");
    }
}
