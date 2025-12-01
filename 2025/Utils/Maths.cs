namespace Advent;

public record ModuloResult(int Quotient, int Modulo);


public static class Maths {
    // Modulo in C# isn't actually modulo, it's remainder, so we need to do it ourselves
    // for negative numbers, properly.
    public static ModuloResult Modulo(int dividend, int divisor) {
        int quotient = (int)Math.Floor((float)dividend / (float)divisor);

        return new(
            quotient,
            dividend - (quotient * divisor)
        );
    }
}
