namespace Advent;


public class PaperRollMover(bool _debug = false) {
    // External utility methods.
    public int MoveRollsRecursively(ref bool[,] rollGrid, int maxRolls) {
        int totalMoved = 0;
        List<Vector2> movedRolls;

        do {
            movedRolls = MoveRolls(ref rollGrid, maxRolls);
            totalMoved += movedRolls.Count;

            foreach (Vector2 rollToMove in movedRolls)
                rollGrid[rollToMove.y, rollToMove.x] = false;

        } while (movedRolls.Count > 0);

        return totalMoved;
    }

    public List<Vector2> MoveRolls(ref bool[,] rollGrid, int maxRolls) {
        List<Vector2> result = [];

        for (int y = 0; y < rollGrid.GetLength(0); y++) {
            for (int x = 0; x < rollGrid.GetLength(1); x++) {
                if (IsMovable(x, y, ref rollGrid, maxRolls))
                    result.Add(new Vector2(x, y));

                if (_debug) {
                    char c = GetDebugCharacter(x, y, ref rollGrid, maxRolls);
                    Console.Write(c);
                }
            }

            if (_debug) Console.WriteLine();
        }

        return result;
    }


    // Internal utility methods.
    private static bool IsInRange(int x, int y, ref bool[,] rollGrid) =>
        y >= 0 &&
        x >= 0 &&
        y < rollGrid.GetLength(0) &&
        x < rollGrid.GetLength(1);

    private static bool Get(int x, int y, ref bool[,] rollGrid) =>
        IsInRange(x, y, ref rollGrid) && rollGrid[y, x];

    private static bool GetTop(int x, int y, ref bool[,] rollGrid) =>
        Get(x, y - 1, ref rollGrid);

    private static bool GetBottom(int x, int y, ref bool[,] rollGrid) =>
        Get(x, y + 1, ref rollGrid);

    private static bool GetLeft(int x, int y, ref bool[,] rollGrid) =>
        Get(x - 1, y, ref rollGrid);

    private static bool GetRight(int x, int y, ref bool[,] rollGrid) =>
        Get(x + 1, y, ref rollGrid);

    private static bool GetTopLeft(int x, int y, ref bool[,] rollGrid) =>
        Get(x - 1, y - 1, ref rollGrid);

    private static bool GetTopRight(int x, int y, ref bool[,] rollGrid) =>
        Get(x + 1, y - 1, ref rollGrid);

    private static bool GetBottomLeft(int x, int y, ref bool[,] rollGrid) =>
        Get(x - 1, y + 1, ref rollGrid);

    private static bool GetBottomRight(int x, int y, ref bool[,] rollGrid) =>
        Get(x + 1, y + 1, ref rollGrid);

    private static int TotalNeighbours(int x, int y, ref bool[,] rollGrid) {
        int total = 0;

        if (GetTopLeft(x, y, ref rollGrid)) total++;
        if (GetTop(x, y, ref rollGrid)) total++;
        if (GetTopRight(x, y, ref rollGrid)) total++;
        if (GetLeft(x, y, ref rollGrid)) total++;
        if (GetRight(x, y, ref rollGrid)) total++;
        if (GetBottomLeft(x, y, ref rollGrid)) total++;
        if (GetBottom(x, y, ref rollGrid)) total++;
        if (GetBottomRight(x, y, ref rollGrid)) total++;

        return total;
    }

    private static bool IsMovable(int x, int y, ref bool[,] rollGrid, int maxRolls) =>
        Get(x, y, ref rollGrid) && TotalNeighbours(x, y, ref rollGrid) <= maxRolls;

    private static char GetDebugCharacter(int x, int y, ref bool[,] rollGrid, int maxRolls) {
        if (!Get(x, y, ref rollGrid)) return '.';
        return IsMovable(x, y, ref rollGrid, maxRolls) ? 'x' : '@';
    }
}
