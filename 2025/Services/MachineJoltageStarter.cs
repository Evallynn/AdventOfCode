namespace Advent;


public record RrefResult(
    double[,] Rref,
    List<int> PivotCols,
    List<int> FreeCols
);


public class MachineJoltageStarter(bool _debug) : IMachineStarter {
    // Class constants.
    private const double ERROR_MARGIN = 1e-9;


    // External utility methods.
    public long Configure(List<MachineSchematic> schematics) {
        long total = 0;

        foreach (var schematic in schematics) {
            long best = SolveMachine(schematic.Transforms, schematic.Joltages)
                ?? throw new InvalidOperationException("No non-negative integer solution for a machine.");

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
        RrefResult? rref = SolveRref(buttonEffectMatrix, [..target]);
        if (rref is null) return null;


        // Find the upper bound for each button. It can't exceed the smallest target joltage
        // remaining it affacts, given any button presses that are already fixed by our RREF.
        int[] upperBounds = new int[buttons.Count];

        for (int j = 0; j < buttons.Count; j++) {
            int upperBound = int.MaxValue;
            bool touchesAny = false;

            foreach (int i in buttons[j]) {
                upperBound = Math.Min(upperBound, target[i]);
                touchesAny = true;
            }

            upperBounds[j] = touchesAny ? upperBound : 0;
        }


        // Check if we have no free variables. If that's the case, there's only 
        // one solution so we can skip the heavy processing.
        if (rref.FreeCols.Count == 0) {
            // Just read out the singular pivot value for each column.
            double[] presses = new double[buttons.Count];

            for (int i = 0; i < rref.PivotCols.Count; i++) {
                int p = rref.PivotCols[i];
                presses[p] = rref.Rref[i, buttons.Count];
            }

            return CheckAndSumSolution(presses, upperBounds);
        }


        // 5) Free variables: brute-force them within upper bounds
        long? best = null;

        double eps = 1e-9;

        void SearchFree(int freeIndex, double[] xPartial) {
            if (freeIndex == rref.FreeCols.Count) {
                // All free vars chosen: compute basic vars
                var x = new double[buttons.Count];
                Array.Copy(xPartial, x, buttons.Count);

                for (int i = 0; i < rref.PivotCols.Count; i++) {
                    int p = rref.PivotCols[i];
                    double rhs = rref.Rref[i, buttons.Count];
                    double sum = 0.0;

                    foreach (int fCol in rref.FreeCols) {
                        double coeff = rref.Rref[i, fCol];
                        if (Math.Abs(coeff) > eps)
                            sum += coeff * x[fCol];
                    }

                    x[p] = rhs - sum;
                }

                long? total = CheckAndSumSolution(x, upperBounds);
                if (total is long val) {
                    if (best is null || val < best.Value)
                        best = val;
                }
                return;
            }

            int col = rref.FreeCols[freeIndex];
            int ub = upperBounds[col];

            for (int v = 0; v <= ub; v++) {
                xPartial[col] = v;
                SearchFree(freeIndex + 1, xPartial);
            }
        }

        var xStart = new double[buttons.Count];
        for (int j = 0; j < buttons.Count; j++) xStart[j] = 0.0;

        SearchFree(0, xStart);

        return best;
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

    public static RrefResult? SolveRref(double[,] matrixToReduce, double[] target) {
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
}