namespace Advent;


public class Files {
    //***
    //*** External utility methods.
    //***
    public static List<SafeRotation> LoadSafeRotations(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadSafeRotations(stream);
        return result;
    }

    public static List<SafeRotation> LoadSafeRotations(StreamReader stream) {
        List<SafeRotation> rotations = [];
        int lineNum = 0;

        while (!stream.EndOfStream) {
            lineNum++;
            string? currLine = stream.ReadLine();
            if (currLine == null) continue;

            bool clockwise = currLine.ToUpper()[0] switch {
                'L' => false,
                'R' => true,
                _ => throw new ArgumentException($"Invalid rotation at line {lineNum}.")
            };

            int amount = int.Parse(currLine[1..]);

            rotations.Add(new(amount, clockwise));
        }

        return rotations;
    }

    public static List<ProductIdRange> LoadProductIds(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadProductIds(stream);
        return result;
    }

    public static List<ProductIdRange> LoadProductIds(StreamReader stream) {
        List<ProductIdRange> ranges = [];

        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine == null) continue;

            string[] splitLine = currLine.Split(',');

            foreach (string rangeStr in splitLine) {
                int hyphenPos = rangeStr.IndexOf('-');
                if (hyphenPos < 0)
                    throw new ArgumentException($"Missing hyphen in range {rangeStr}.");

                long rangeStart = long.Parse(rangeStr[..hyphenPos]);
                long rangeEnd = long.Parse(rangeStr[(hyphenPos + 1)..]);

                ranges.Add(new(rangeStart, rangeEnd));
            }
        }

        return ranges;
    }

    public static List<string> LoadJoltsFile(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadJoltsFile(stream);
        return result;
    }

    public static List<string> LoadJoltsFile(StreamReader stream) {
        List<string> jolts = [];

        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine == null) continue;
            jolts.Add(currLine);
        }

        return jolts;
    }
}
