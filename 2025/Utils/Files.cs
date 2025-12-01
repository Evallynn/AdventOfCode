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
            if (currLine == null) break;

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
}
