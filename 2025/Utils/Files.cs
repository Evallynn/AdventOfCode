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

    public static bool[,] LoadPaperRolls(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadPaperRolls(stream);
        return result;
    }

    public static bool[,] LoadPaperRolls(StreamReader stream) {
        string[] fullFile = stream.ReadToEnd().Split("\r\n");
        bool[,] rolls = new bool[fullFile.Length, fullFile[0].Length];

        for (int y = 0; y < fullFile.Length; y++)
            for (int x = 0; x < fullFile[y].Length; x++) {
                if (fullFile[y][x] != '@' && fullFile[y][x] != '.')
                    throw new ArgumentException($"Dodgy character at {x},{y}: '{fullFile[y][x]}'");

                rolls[y, x] = fullFile[y][x] == '@';
            }

        return rolls;
    }

    public static Ingredients LoadCafeteriaStock(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadCafeteriaStock(stream);
        return result;
    }

    public static Ingredients LoadCafeteriaStock(StreamReader stream) {
        Ingredients ingredients = new();

        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine == null || currLine == "") continue;

            int hyphenIndex = currLine.IndexOf('-');

            if (hyphenIndex < 0) {
                ingredients.AddStock(long.Parse(currLine));
            }
            else {
                long rangeStart = long.Parse(currLine[..hyphenIndex]);
                long rangeEnd = long.Parse(currLine[(hyphenIndex + 1)..]);
                ingredients.AddRange(rangeStart, rangeEnd);
            }
        }

        return ingredients;
    }


    public static List<MathProblem> LoadCephalopodMath(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadCephalopodMath(stream);
        return result;
    }

    public static List<MathProblem> LoadCephalopodMath(StreamReader stream) {
        // FIXME - Absolutely certain there's a neater way to do this, but I don't have time right now.
        // Create a list we can return problems in.
        List<MathProblem> problems = [];


        // Read all of the lines in upfront, but then traverse them with x/y flipped.
        string[] lines = stream.ReadToEnd().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        List<string> parsedLines = [];
        OPERATION? operation = null;

        for (int y = 0; y < lines[0].Length; y++) {
            bool blankLine = true;
            string line = "";

            for (int x = 0; x < lines.Length; x++) {
                char currChar = lines[x][y];

                if (currChar == ' ') continue;

                if (char.IsDigit(currChar))
                    line += currChar;
                else if (currChar == '+')
                    operation = OPERATION.ADD;
                else if (currChar == '*')
                    operation = OPERATION.MULTIPLY;
                else
                    throw new FileLoadException($"Invalid character '{currChar}' found at [{x},{y}].");

                blankLine = false;
            }

            if (blankLine) {
                MathProblem problem = new();
                parsedLines.ForEach(i => problem.AddValue(int.Parse(i)));
                problem.Operation = operation!.Value;

                problems.Add(problem);
                parsedLines.Clear();
                operation = null;
            }
            else
                parsedLines.Add(line);
        }


        // Add the final line.
        MathProblem finalProblem = new();
        parsedLines.ForEach(i => finalProblem.AddValue(int.Parse(i)));
        finalProblem.Operation = operation!.Value;
        problems.Add(finalProblem);


        // Pass back the completed list.
        return problems;
    }
}
