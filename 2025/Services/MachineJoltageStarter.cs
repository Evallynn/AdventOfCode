namespace Advent;


public record Rref(
    double[,] Value,
    List<int> Pivots,
    List<int> Free
);


public class MachineJoltageStarter(bool _debug) : IMachineStarter {
    // Class constants.
    private const double ERROR_MARGIN = 1e-9;


    // External utility methods.
    public long Configure(List<MachineSchematic> schematics) {
        long total = 0;

        foreach (MachineSchematic schematic in schematics) {
            long best = SolveMachine(schematic.Transforms, schematic.Joltages)
                ?? throw new InvalidOperationException($"No non-negative integer solution for [{string.Join(',', schematic.Joltages)}].");

            if (_debug)
                Console.WriteLine($"Solution to [{string.Join(',', schematic.Joltages)}] found in {best} presses.");

            total += best;
        }

        return total;
    }


    // Class-internal utility methods.
    public static long? SolveMachine(List<int[]> buttons, List<short> target) {
        // Build a matrix which represents buttons mapped to joltages they impact.
        double[,] buttonEffectMatrix = new double[target.Count, buttons.Count];

        for (int y = 0; y < buttons.Count; y++)
            foreach (int x in buttons[y])
                buttonEffectMatrix[x, y] = 1.0d;


        // Reduce the set of equations down into their RREF form.
        Rref? rref = SolveRref(buttonEffectMatrix, [..target]);
        if (rref is null) return null;


        // Find the upper bound for each button. It can't exceed the smallest target joltage
        // remaining it affacts, given any button presses that are already fixed by our RREF.
        int[] maxPressesPerButton = new int[buttons.Count];

        for (int j = 0; j < buttons.Count; j++) {
            int upperBound = int.MaxValue;
            bool touchesAny = false;

            foreach (int i in buttons[j]) {
                upperBound = Math.Min(upperBound, target[i]);
                touchesAny = true;
            }

            maxPressesPerButton[j] = touchesAny ? upperBound : 0;
        }


        // Check if we have no free variables. If that's the case, there's only 
        // one solution so we can skip the heavy processing.
        if (rref.Free.Count == 0) {
            // Just read out the singular pivot value for each column.
            double[] presses = new double[buttons.Count];

            for (int i = 0; i < rref.Pivots.Count; i++) {
                int p = rref.Pivots[i];
                presses[p] = rref.Value[i, buttons.Count];
            }

            return CheckAndSumSolution(presses, maxPressesPerButton);
        }


        // Run depth first search against the remaining free values, within sensible bounds.
        return SearchFreeButtonPresses(
            depth: 0,
            rref: rref,
            maxPressesPerButton: maxPressesPerButton
        );
    }


    // TODO - Consider removing checks.
    private static long? CheckAndSumSolution(double[] presses, int[] upperBounds) {
        // Sum up all button press values, making sure we've not had rounding issues.
        long sum = 0;

        for (int j = 0; j < presses.Length; j++) {
            // Check for values that are somehow too far from an integer.
            if (Math.Abs(presses[j] - Math.Round(presses[j])) > ERROR_MARGIN) return null;

            // Check for absurd answers.
            long rounded = (long)Math.Round(presses[j]);
            if (rounded < 0 || rounded > upperBounds[j]) return null;

            // Add to the sum.
            sum += rounded;
        }

        return sum;
    }

    public static Rref? SolveRref(double[,] matrixToReduce, double[] target) {
        // Store the matric x/y for later.
        int width = matrixToReduce.GetLength(0);
        int height = matrixToReduce.GetLength(1);


        // Append the target to matrix we're reducing.
        double[,] comboMatrix = new double[width, height + 1];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++)
                comboMatrix[i, j] = matrixToReduce[i, j];
            comboMatrix[i, height] = target[i];
        }


        // Loop over the combined matrix looking for pivots - non-zero values.
        // These are equation parts that can be pinned to a single solution.
        List<int> pivotCols = [];
        int row = 0;

        for (int col = 0; col < height && row < width; col++) {
            // Look for a pivot row within this column.
            int pivotRow = -1;
            double bestAbs = ERROR_MARGIN;

            for (int r = row; r < width; r++) {
                double abs = Math.Abs(comboMatrix[r, col]);
                if (abs > bestAbs) {
                    bestAbs = abs;
                    pivotRow = r;
                }
            }


            // Carry on if there's no pivot in this column.
            if (pivotRow == -1)
                continue; 


            // If we found a pivot, swap it with the current row.
            if (pivotRow != row)
                for (int c = 0; c <= height; c++)
                    (comboMatrix[row, c], comboMatrix[pivotRow, c]) = (comboMatrix[pivotRow, c], comboMatrix[row, c]);


            // Store the new pivot value and double check it's valid.
            double pivotVal = comboMatrix[row, col];
            if (Math.Abs(pivotVal) < ERROR_MARGIN) continue;


            // Normalize the current row using the pivot.
            for (int c = 0; c <= height; c++)
                comboMatrix[row, c] /= pivotVal;


            // Eliminate the current column from all other rows.
            for (int r = 0; r < width; r++) {
                if (r == row) continue;

                double factor = comboMatrix[r, col];
                if (Math.Abs(factor) < ERROR_MARGIN) continue;

                for (int c = 0; c <= height; c++)
                    comboMatrix[r, c] -= factor * comboMatrix[row, c];
            }


            // Store the pivot column and move to the next row.
            pivotCols.Add(col);
            row++;
        }

        // Ensure the solution is consistent - if not, then there's no RREF solution.
        for (int r = 0; r < width; r++) {
            bool allZero = true;

            for (int c = 0; c < height; c++) {
                if (Math.Abs(comboMatrix[r, c]) > ERROR_MARGIN) {
                    allZero = false;
                    break;
                }
            }

            if (allZero && Math.Abs(comboMatrix[r, height]) > ERROR_MARGIN)
                return null;
        }


        // Identify columns for equations that can't be pinned to one solution (free columns).
        List<int> freeCols = [];
        HashSet<int> pivotSet = [..pivotCols];

        for (int c = 0; c < height; c++)
            if (!pivotSet.Contains(c))
                freeCols.Add(c);


        // Pass back the combined matric, pivot columns and free columns.
        return new(comboMatrix, pivotCols, freeCols);
    }

    private static long? SearchFreeButtonPresses(int depth, Rref rref, int[] maxPressesPerButton, double[]? pressesSoFar = null) {
        // All free buttons have assigned values...
        pressesSoFar ??= new double[maxPressesPerButton.Length];

        if (depth == rref.Free.Count) {
            int numButtons = maxPressesPerButton.Length;
            double[] fullPresses = new double[numButtons];
            Array.Copy(pressesSoFar, fullPresses, numButtons);

            // Compute the pivot button values using the RREF rows.
            for (int pivotRow = 0; pivotRow < rref.Pivots.Count; pivotRow++) {
                int pivotButton = rref.Pivots[pivotRow];
                double rhs = rref.Value[pivotRow, numButtons];
                double sum = 0.0;

                // Subtract the contribution from free buttons in this row.
                foreach (int freeButton in rref.Free) {
                    double coeff = rref.Value[pivotRow, freeButton];
                    if (Math.Abs(coeff) > ERROR_MARGIN) {
                        sum += coeff * fullPresses[freeButton];
                    }
                }

                fullPresses[pivotButton] = rhs - sum;
            }
            
            // Check if this full button assignment is valid and return its total presses (or null).
            return CheckAndSumSolution(fullPresses, maxPressesPerButton);
        }

        // We still have at least one free button to assign.
        int currentFreeButton = rref.Free[depth];
        int upperBound = maxPressesPerButton[currentFreeButton];

        long? bestTotalPresses = null;

        for (int presses = 0; presses <= upperBound; presses++) {
            pressesSoFar[currentFreeButton] = presses;

            long? candidate = SearchFreeButtonPresses(depth + 1, rref, maxPressesPerButton, pressesSoFar);

            if (candidate is long value) {
                if (bestTotalPresses is null || value < bestTotalPresses.Value)
                    bestTotalPresses = value;
            }
        }

        return bestTotalPresses;
    }
}