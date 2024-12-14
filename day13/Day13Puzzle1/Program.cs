var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

List<ClawMachine> clawMachines = [];
while (lines.Length > 0)
{
    clawMachines.Add(new ClawMachine(lines.Take(3).ToArray()));
    lines = lines.Skip(4).ToArray();
}
var res = clawMachines.Select(cm => cm.CalculateCheapestWin()).Sum();

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");