namespace Advent;


public record SafeRotation(int amount, bool clockwise) {
    public int NormalisedAmount => clockwise ? amount : -amount;
}
