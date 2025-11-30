namespace Advent;


public class SafetyEvaluator {
    // Public utility methods.
    public bool IsSafe(List<int> inputs, bool useDampening) {
        if (useDampening) {
            return IsSafeFixedSign(inputs, true, true) ||
                   IsSafeFixedSign(inputs, false, true);
        }
        else {
            int direction = inputs[1] - inputs[0];

            if (direction < 0)
                return IsSafeFixedSign(inputs, true, false);
            else if (direction > 0)
                return IsSafeFixedSign(inputs, false, false);
            else
                return false;
        }
    }


    // Class-internal utility methods.
    private bool IsSafeFixedSign(List<int> inputs, bool descending, bool useDampening) {
        int sign = descending ? -1 : 1;
        int maxCount = useDampening ? inputs.Count - 2 : inputs.Count - 1;

        for (int i = 0; i < maxCount; i++) {
            int currDiff = inputs[i + 1] - inputs[i];

            if (!IsValidStep(currDiff) || Math.Sign(currDiff) != sign) {
                if (useDampening) {
                    List<int> dampenedList = [];
                    dampenedList.AddRange(inputs);
                    dampenedList.RemoveAt(i);

                    if (!IsSafeFixedSign(dampenedList, descending, false)) {
                        dampenedList.Clear();
                        dampenedList.AddRange(inputs);
                        dampenedList.RemoveAt(i + 1);

                        return IsSafeFixedSign(dampenedList, descending, false);
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
            }
        }

        return true;
    }


    private static bool IsValidStep(int diff) {
        int absDiff = Math.Abs(diff);
        return absDiff > 0 && absDiff <= 3;
    }
}
