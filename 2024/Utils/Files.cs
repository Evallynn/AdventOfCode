namespace Advent;


public static class Files {
    //***
    //*** External utility methods.
    //***
    public static ListPair LoadOrdered(string path) {
        using StreamReader stream = File.OpenText(path);
        ListPair result = LoadOrdered(stream);
        return result;
    }

    public static ListPair LoadOrdered(StreamReader stream) {
        ListPair lists = new();

        while (!stream.EndOfStream) {
            string? currLine = stream.ReadLine();
            if (currLine == null) break;

            string[] splitLine = currLine.Split("   ");
            int firstItem = int.Parse(splitLine[0].TrimEnd());
            int secondItem = int.Parse(splitLine[1].TrimStart());

            Insert(ref lists.First, firstItem);
            Insert(ref lists.Second, secondItem);
        }

        return lists;
    }


    //***
    //*** Internal utility methods.
    //***
    private static void Insert(ref List<int> target, int value) {
        for (int i = 0; i < target.Count; i++) {
            if (value < target[i]) {
                target.Insert(i, value);
                return;
            }
        }

        target.Add(value);
    }
}