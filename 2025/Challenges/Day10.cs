namespace Advent;


public class Day10 : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day10.txt";


    // External utility methods.
    public void Run() {
        List<MachineSchematic> schematics = Files.LoadMachineSchematics(FILE_PATH);

        MachineStarter starterA = new(10, true);
        long totalMinSteps = starterA.ConfigureMachine(schematics);
        Console.WriteLine($"Total minimum steps (start-up): {totalMinSteps}");

        MachineStarter starterB = new(10, true);
        long totalJoltageSteps = starterB.ConfigureJoltage(schematics);
        Console.WriteLine($"Total minimum steps (joltage): {totalJoltageSteps}");
    }
}
