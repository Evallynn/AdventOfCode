namespace Advent;


public class Day10 : IChallenge {
    // Constants.
    private const string FILE_PATH = "data/Day10.txt";


    // External utility methods.
    public void Run() {
        List<MachineSchematic> schematics = Files.LoadMachineSchematics(FILE_PATH);

        IMachineStarter starterA = new MachineWiringStarter(10, 250000, false);
        long totalMinSteps = starterA.Configure(schematics);
        Console.WriteLine($"Total minimum steps (start-up): {totalMinSteps}");

        IMachineStarter starterB = new MachineJoltageStarter(true);
        long totalJoltageSteps = starterB.Configure(schematics);
        Console.WriteLine($"Total minimum steps (joltage): {totalJoltageSteps}");
    }
}
