namespace Advent;


public record HighDigit(int Digit, int Position);

public class JoltsCalculator {
    // External utility methods.
    public long CalculateJolts(List<string> rawJolts, int joltScale) =>
        rawJolts.Sum(i => CalculateJolts(i, joltScale));


    public long CalculateJolts(string rawJolts, int joltScale) {
        List<int> intJolts = ToInts(rawJolts);
        List<HighDigit> joltDigits = [];

        for (int i = 0; i < joltScale; i++) {
            int startPos = joltDigits.Count <= 0 ? 0 : joltDigits[^1].Position + 1;
            int endPos = (intJolts.Count + 1) - (joltScale - i);

            HighDigit currHighest = GetHighestDigitAndPosition(intJolts, startPos, endPos);
            joltDigits.Add(currHighest);
        }

        return AssembleMaxJolts(joltDigits);
    }


    // Class-internal utility methods.
    private static List<int> ToInts(string input) {
        List<int> output = [];

        foreach (char digit in input)
            output.Add(int.Parse(digit.ToString()));

        return output;
    }

    private static HighDigit GetHighestDigitAndPosition(List<int> input, int startPos, int maxPos) {
        int highest = 0;
        int position = -1;

        for (int i = startPos; i < maxPos; i++) {
            if (input[i] > highest) {
                highest = input[i];
                position = i;
            }

            if (highest == 9)
                return new HighDigit(highest, position);
        }

        return new HighDigit(highest, position);
    }

    private static long AssembleMaxJolts(List<HighDigit> joltDigits) {
        string maxJoltStr = "";

        foreach (HighDigit joltDigit in joltDigits)
            maxJoltStr += joltDigit.Digit.ToString();

        return long.Parse(maxJoltStr);
    }
}
