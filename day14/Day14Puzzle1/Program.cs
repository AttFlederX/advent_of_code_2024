var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var bathroom = new Bathroom(lines, 101, 103);
for (int i = 0; i < 100; i++)
{
    bathroom.MoveGuards();
}
var res = bathroom.SafetyFactor;

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");