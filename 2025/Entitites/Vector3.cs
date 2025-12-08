namespace Advent;


public struct Vector3(int x, int y, int z) {
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int Z { get; set; } = z;

    public override readonly string ToString() => $"[{X},{Y},{Z}]";
}
