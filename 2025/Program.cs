using Advent;


Console.WriteLine("Choose challenge day:");
string? input = Console.ReadLine();

if (int.TryParse(input, out int day)) {
    IChallenge challenge = PickChallenge(day);
    challenge.Run();
}
else {
    Console.WriteLine("A valid integer must be entered.");
}


static IChallenge PickChallenge(int day) =>
    day switch {
        1 => new Day1(),
        2 => new Day2(),
        3 => new Day3(),
        4 => new Day4(),
        5 => new Day5(),
        6 => new Day6(),
        7 => new Day7(),
        8 => new Day8(),
        9 => new Day9(),
        _ => throw new ArgumentException($"Invalid day {day}.")
    };