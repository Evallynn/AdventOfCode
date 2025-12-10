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

    public static TachyonManifold LoadTachyonManifolds(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadTachyonManifolds(stream);
        return result;
    }

    public static TachyonManifold LoadTachyonManifolds(StreamReader stream) {
        // Pull the first line to establish start positions and line width.
        string firstLine = stream.ReadLine()!;
        List<bool> startPos = [];

        foreach (char c in firstLine) {
            if (c == '.') startPos.Add(false);
            else if (c == 'S') startPos.Add(true);
        }

        TachyonManifold manifolds = new(startPos.Count);
        manifolds.SetStarts([.. startPos]);


        // Process the remaining lines to add in the reflectors.
        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine is null) continue;

            bool[] manifoldLocations = new bool[startPos.Count];
            int manifoldCount = 0;

            foreach (char c in currLine) {
                if (c == '.') {
                    manifoldLocations[manifoldCount] = false;
                    manifoldCount++;
                }
                else if (c == '^') {
                    manifoldLocations[manifoldCount] = true;
                    manifoldCount++;
                }
            }

            manifolds.AddRow(manifoldLocations);
        }


        // Pass back the result.
        return manifolds;
    }

    public static List<Vector3> LoadJunctionBoxes(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadJunctionBoxes(stream);
        return result;
    }

    public static List<Vector3> LoadJunctionBoxes(StreamReader stream) {
        List<Vector3> boxes = [];

        while (!stream.EndOfStream) {
            string currLine = stream.ReadLine() ?? "";
            string[] splitLine = currLine.Split(',');

            if (splitLine.Length != 3)
                throw new FileLoadException($"Unable to split the following line into three integer positions: '{currLine}'");

            boxes.Add(new(
                int.Parse(splitLine[0]),
                int.Parse(splitLine[1]),
                int.Parse(splitLine[2])
            ));
        }

        return boxes;
    }

    public static List<Vector2> LoadTiles(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadTiles(stream);
        return result;
    }

    public static List<Vector2> LoadTiles(StreamReader stream) {
        List<Vector2> tiles = [];

        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine is null) continue;

            string[] splitLine = currLine.Split(',');
            if (splitLine.Length != 2)
                throw new FileLoadException($"Line could not be split into a Vector2: '{currLine}'.");

            Vector2 newPos = new(
                int.Parse(splitLine[0]),
                int.Parse(splitLine[1])
            );

            tiles.Add(newPos);
        }

        return tiles;
    }

    public static List<MachineSchematic> LoadMachineSchematics(string path) {
        using StreamReader stream = File.OpenText(path);
        var result = LoadMachineSchematics(stream);
        return result;
    }

    public static List<MachineSchematic> LoadMachineSchematics(StreamReader stream) {
        List<MachineSchematic> schematics = [];

        while (!stream.EndOfStream) {
            // Skip empty lines.
            string? currLine = stream.ReadLine();
            if (currLine is null) continue;


            // Read in the raw component strings.
            string targetStateStr = currLine[(currLine.IndexOf('[') + 1)..currLine.LastIndexOf(']')];
            string switchesStr = currLine[currLine.IndexOf('(')..(currLine.LastIndexOf(')') + 1)];
            string joltagesStr = currLine[(currLine.IndexOf('{') + 1)..currLine.LastIndexOf('}')];


            // Process the target states and use them to create the initial schematic.
            bool[] targetState = new bool[targetStateStr.Length];
            for (int i = 0; i < targetStateStr.Length; i++)
                targetState[i] = targetStateStr[i] == '#';

            MachineSchematic schematic = new(targetState);


            // Process the switches.
            foreach (string switchStr in switchesStr.Split( )) {
                string[] affectedValuesStr = switchStr.TrimStart('(').TrimEnd(')').Split(",");
                int[] affectedValues = new int[affectedValuesStr.Length];

                for (int i = 0; i < affectedValuesStr.Length; i++)
                    affectedValues[i] = int.Parse(affectedValuesStr[i]);

                schematic.AddSwitch(affectedValues);
            }


            // Process the joltages.
            foreach (string joltageStr in joltagesStr.Split(','))
                schematic.AddJoltage(int.Parse(joltageStr));


            // Store the completed schematic.
            schematics.Add(schematic);
        }

        return schematics;
    }
}
