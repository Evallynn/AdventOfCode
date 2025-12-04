namespace Advent;


internal class Day4 : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day4.txt";


    // External utility methods.
    public void Run() {
        bool[,] input = Files.LoadPaperRolls(FILE_PATH);

        PaperRollMover mover = new();
        List<Vector2> movedRollsOnePass = mover.MoveRolls(ref input, 3);
        int movedRollsRecursive = mover.MoveRollsRecursively(ref input, 3);

        Console.WriteLine($"Moved rolls (single): {movedRollsOnePass.Count}");
        Console.WriteLine($"Moved rolls (recursive): {movedRollsRecursive}");
    }
}
