namespace Advent;

public record Vector2(int x, int y) {
    public override string ToString() => $"[{x},{y}]";
}