namespace Advent;


public struct ClassicManifoldResult(int width) {
    public bool[] NewLocations { get; } = new bool[width];
    public int TotalSplits { get; set; } = 0;
}

public struct QuantumManifoldResult(int width) {
    public long[] NewTimelines { get; } = new long[width];
    public int TotalSplits { get; set; } = 0;
}


public class TachyonManifold(int width) {
    // Class properties.
    public int Width { get; } = width;
    public int Height => _manifoldLocations.Count;
    public bool[] StartBeamsClassic => this._startIndices;

    public long[] StartBeamsQuantum {
        get {
            long[] startBeams = new long[this.Width];
            for (int i = 0; i < this.Width; i++)
                startBeams[i] = this._startIndices[i] ? 1 : 0;
            return startBeams;
        }
    }


    // Class-internal variables.
    private bool[] _startIndices = new bool[width];
    private List<bool[]> _manifoldLocations = [];


    // External utility methods.
    public void SetStarts(bool[] indices) {
        if (indices.Length != this.Width)
            throw new ArgumentException($"Start array must be of length {this.Width}.");

        this._startIndices = indices;
    }

    public void AddStart(int index) {
        if (index >= this.Width || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), $"Start index was out of range - positive numbers under {this.Width} only.");

        this._startIndices[index] = true;
    }

    public void AddRow(bool[] newRow) {
        if (newRow.Length != this.Width)
            throw new ArgumentException($"Rows must be of length {this.Width}.", nameof(newRow));

        this._manifoldLocations.Add(newRow);
    }


    public bool IsManifold(int row, int index) => this._manifoldLocations[row][index];


    // Classic simply propagates beams onwards - any in the same location merge.
    public ClassicManifoldResult ProcessClassic(int manifoldIndex, bool[] tachyonLocations) {
        if (tachyonLocations.Length != this.Width)
            throw new ArgumentException($"Tachyon locations must be of length {this.Width}.", nameof(tachyonLocations));

        if (manifoldIndex >= this.Width || manifoldIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(manifoldIndex), $"Index was out of range - positive numbers under {this.Width} only.");

        ClassicManifoldResult result = new(this.Width);

        for (int i = 0; i < this.Width; i++) {
            // If there was a beam, we need to process it.
            if (tachyonLocations[i]) {
                // Check for a mirror on the current row.
                if (this._manifoldLocations[manifoldIndex][i]) {
                    // Split the beam.
                    if (i != 0) result.NewLocations[i - 1] = true;
                    if (i != this.Width - 1) result.NewLocations[i + 1] = true;
                    result.TotalSplits++;
                }
                else {
                    // Continue the beam.
                    result.NewLocations[i] = true;
                }
            }
        }

        return result;
    }


    // Quantum keeps track of all beam paths, tracking how many beams co-exist in a given location.
    public QuantumManifoldResult ProcessQuantum(int manifoldIndex, long[] tachyonTimelines) {
        if (tachyonTimelines.Length != this.Width)
            throw new ArgumentException($"Tachyon locations must be of length {this.Width}.", nameof(tachyonTimelines));

        if (manifoldIndex >= this.Width || manifoldIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(manifoldIndex), $"Index was out of range - positive numbers under {this.Width} only.");

        QuantumManifoldResult result = new(this.Width);

        for (int i = 0; i < this.Width; i++) {
            // If there was a beam, we need to process it.
            if (tachyonTimelines[i] > 0) {
                // Check for a mirror on the current row.
                if (this._manifoldLocations[manifoldIndex][i]) {
                    // Split the beam.
                    if (i != 0) result.NewTimelines[i - 1] += tachyonTimelines[i];
                    if (i != this.Width - 1) result.NewTimelines[i + 1] += tachyonTimelines[i];
                    result.TotalSplits++;
                }
                else {
                    // Continue the beam.
                    result.NewTimelines[i] += tachyonTimelines[i];
                }
            }
        }

        return result;
    }
}
