namespace Advent;


public class ArrayComparer<T> : IEqualityComparer<T[]> where T : notnull {
    public bool Equals(T[]? x, T[]? y) {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        if (x.Length != y.Length) return false;

        for (int i = 0; i < x.Length; i++)
            if (!EqualityComparer<T>.Default.Equals(x[i], y[i])) return false;

        return true;
    }

    public int GetHashCode(T[] obj) {
        unchecked {
            int hash = 17;

            foreach (T item in obj)
                hash = hash * 31 + EqualityComparer<T>.Default.GetHashCode(item);
            return hash;
        }
    }
}
