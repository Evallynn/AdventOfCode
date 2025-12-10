namespace Advent;


public class Day10 : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day10.txt";


    // External utility methods.
    public void Run() {
        List<MachineSchematic> schematics = Files.LoadMachineSchematics(FILE_PATH);
        MachineStarter starter = new(10, false);

        long totalMinSteps = starter.ConfigureMachine(schematics);
        Console.WriteLine($"Total minimum steps (start-up): {totalMinSteps}");

        long totalJoltageSteps = starter.ConfigureJoltage(schematics);
        Console.WriteLine($"Total minimum steps (joltage): {totalJoltageSteps}");
    }
}
