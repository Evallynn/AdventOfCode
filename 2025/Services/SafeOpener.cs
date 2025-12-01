namespace Advent;


public struct SafePassword() {
    public int PassedZero { get; set; } = 0;
    public int LandedOnZero { get; set; } = 0;
}

public record RotationResult(int PassedZero, int Rotation);


public class SafeOpener(bool _debug = false) {
    // Class-external utility methods.
    public SafePassword CountZeroes(int startPos, List<SafeRotation> rotations) {
        SafePassword password = new();
        int currRotation = startPos;

        foreach (SafeRotation rotation in rotations) {
            RotationResult modulo = this.Rotate(currRotation, rotation.amount, rotation.clockwise);

            if (modulo.Rotation == 0) password.LandedOnZero++;
            password.PassedZero += modulo.PassedZero;

            if (modulo.PassedZero > 0)
                this.Log($"{(rotation.clockwise ? 'R' : 'L')}{rotation.amount} of {currRotation}->{modulo.Rotation} added {modulo.PassedZero}");

            currRotation = modulo.Rotation;
        }

        return password;
    }


    public RotationResult Rotate(int startPos, int amount, bool clockwise) {
        ModuloResult modulo = clockwise
            ? Maths.Modulo(startPos + amount, 100)
            : Maths.Modulo(startPos - amount, 100);

        int passedZero = CalculateTimesPassedZero(modulo.Quotient, modulo.Modulo, clockwise, startPos);
        return new(passedZero, modulo.Modulo);
    }


    // Class-internal utility methods.
    private void Log(string message) {
        if (_debug) Console.WriteLine(message);
    }

    private static int CalculateTimesPassedZero(int quotient, int modulo, bool clockwise, int startPos) {
        int passedZero = Math.Abs(quotient);

        if (!clockwise) {
            if (modulo == 0) passedZero++;                                // Account for anti-clockwise turns that land on zero.
            if (startPos == 0) passedZero = Math.Max(0, passedZero - 1);  // Account for anti-clockwise turns being one quotient too high (unless zero).
        }
            
        return passedZero;
    }
}
