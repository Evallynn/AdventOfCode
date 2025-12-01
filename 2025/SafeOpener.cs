namespace Advent;

public class SafeOpener {
    // Class-external utility methods.
    public int CountZeroes(int startPos, List<SafeRotation> rotations) {
        int totalZeroes = 0;
        int currRotation = startPos;

        foreach (SafeRotation rotation in rotations) {
            currRotation = this.Rotate(currRotation, rotation.amount, rotation.clockwise);
            if (currRotation == 0) totalZeroes++;
        }

        return totalZeroes;
    }


    public int Rotate(int startPos, int amount, bool clockwise) {
        return clockwise
            ? Modulo(startPos + amount, 100)
            : Modulo(startPos - amount, 100);
    }


    // Modulo in C# isn't actually modulo, it's remainder, so we need to do it ourselves
    // for negative numbers, properly.
    private static int Modulo(int dividend, int divisor) {
        int quotient = (int)Math.Floor((float)dividend / (float)divisor);
        return dividend - (quotient * divisor);
    }
}
